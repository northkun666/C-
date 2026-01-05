using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using DotSpatial.Topology;

namespace 集成
{
    public partial class gis软件 : Form
    {
        public Map Map1 => map1;

        // 当前操作的形状类型： "Point", "line", "polygon", "hikingPath"
        string shapeType = "";

        #region "类级变量（Class level variables）"
        // 徒步路径绘制完成的布尔变量
        bool hikingpathPathFinished = true; // 默认为 true，只有开始绘制时才设为 false
        #endregion

        #region 点形状文件类级变量
        FeatureSet pointF = new FeatureSet(FeatureType.Point);
        int pointID = 0;
        bool pointmouseClick = false;
        #endregion

        #region 折线形状文件类级变量
        MapLineLayer lineLayer = default(MapLineLayer);
        FeatureSet lineF = new FeatureSet(FeatureType.Line);
        int lineID = 0;
        bool firstClick = true; // 默认为 true
        bool linemouseClick = false;
        #endregion

        #region 多边形形状文件类级变量
        FeatureSet polygonF = new FeatureSet(FeatureType.Polygon);
        int polygonID = 0;
        bool polygonmouseClick = false;
        #endregion

        public class PathPoint
        {
            public double X;
            public double Y;
            public double Distance;
            public double Elevation;
        }

        public gis软件()
        {
            InitializeComponent();
            // 绑定菜单栏事件 (防止 Designer 中未绑定的情况)
            BindMenuEvents();
        }

        // 手动绑定那些在 Designer 中可能丢失的事件
        private void BindMenuEvents()
        {
            this.放大ToolStripMenuItem.Click += new EventHandler(k放大_Click);
            this.缩小ToolStripMenuItem.Click += new EventHandler(k缩小_Click);
            this.平移ToolStripMenuItem.Click += new EventHandler(k平移_Click);
            this.还原ToolStripMenuItem.Click += new EventHandler(k缩放至图层_Click);
            this.取消操作ToolStripMenuItem.Click += new EventHandler(k取消_Click);

            // 确保 MouseUp 事件已绑定（用于栅格查询）
            this.map1.MouseUp += new MouseEventHandler(map1_MouseUp);
        }

        // ★★★ 核心修复：重置工具状态方法 ★★★
        // 在切换任何功能之前调用此方法，防止状态残留
        private void ResetToolState()
        {
            map1.Cursor = Cursors.Arrow;
            map1.FunctionMode = FunctionMode.None;
            shapeType = ""; // 清空形状类型

            // 重置所有点击标志
            pointmouseClick = false;
            linemouseClick = false;
            polygonmouseClick = false;

            // 重置徒步路径状态
            hikingpathPathFinished = true;

            // 重置首点判断
            firstClick = true;

            // 取消栅格查询勾选 (可选，视需求而定)
            // chbRasterValue.Checked = false;
        }

        /// <summary>
        /// 提取高程逻辑
        /// </summary>
        public List<PathPoint> ExtractElevation(double startX, double startY, double endX, double endY, IMapRasterLayer raster)
        {
            double curX = startX;
            double curY = startY;
            double curElevation = 0;
            List<PathPoint> pathPointList = new List<PathPoint>();
            int numberofpoints = 100;
            double constXdif = ((endX - startX) / numberofpoints);
            double constYdif = ((endY - startY) / numberofpoints);
            for (int i = 0; i <= numberofpoints; i++)
            {
                PathPoint newPathPoint = new PathPoint();
                if ((i == 0))
                {
                    curX = startX;
                    curY = startY;
                }
                else
                {
                    curX = curX + constXdif;
                    curY = curY + constYdif;
                }
                Coordinate coordinate = new Coordinate(curX, curY);
                // 添加边界检查，防止越界崩溃
                if (raster.DataSet.Bounds != null)
                {
                    RcIndex rowColumn = raster.DataSet.Bounds.ProjToCell(coordinate);
                    if (rowColumn.Row >= 0 && rowColumn.Row < raster.DataSet.NumRows &&
                        rowColumn.Column >= 0 && rowColumn.Column < raster.DataSet.NumColumns)
                    {
                        curElevation = raster.DataSet.Value[rowColumn.Row, rowColumn.Column];
                    }
                    else
                    {
                        curElevation = 0; // 或者设为 NoDataValue
                    }
                }

                newPathPoint.X = curX;
                newPathPoint.Y = curY;
                newPathPoint.Elevation = curElevation;
                pathPointList.Add(newPathPoint);
            }
            return pathPointList;
        }

        #region 基础地图操作 (放大/缩小/平移)
        // 每次点击工具前，都先 ResetToolState()
        private void k放大_Click(object sender, EventArgs e)
        {
            ResetToolState();
            map1.ZoomIn();
        }

        private void k缩小_Click(object sender, EventArgs e)
        {
            ResetToolState();
            map1.ZoomOut();
        }

        private void k平移_Click(object sender, EventArgs e)
        {
            ResetToolState();
            map1.FunctionMode = FunctionMode.Pan;
        }

        private void k选择_Click(object sender, EventArgs e)
        {
            ResetToolState();
            map1.FunctionMode = FunctionMode.Select;
        }

        private void k缩放至图层_Click(object sender, EventArgs e)
        {
            // 这个操作不需要重置状态，因为它是一次性的
            map1.ZoomToMaxExtent();
        }

        private void k取消_Click(object sender, EventArgs e)
        {
            ResetToolState(); // 仅仅重置状态，恢复默认箭头
        }
        #endregion

        #region 文件操作
        private void 矢量图形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map1.AddLayer();
        }

        private void 栅格图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map1.AddRasterLayer();
        }

        private void 退出ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region 鼠标绘图核心逻辑 (已重构)
        private void map1_MouseDown(object sender, MouseEventArgs e)
        {
            // 如果处于漫游或选择模式，不执行绘图逻辑
            if (map1.FunctionMode != FunctionMode.None) return;

            // 获取地图坐标
            Coordinate coord = map1.PixelToProj(e.Location);

            // 根据当前的形状类型执行对应逻辑
            switch (shapeType)
            {
                case "Point":
                    if (e.Button == MouseButtons.Left && pointmouseClick)
                    {
                        DotSpatial.Topology.Point point = new DotSpatial.Topology.Point(coord);
                        IFeature currentFeature = pointF.AddFeature(point);
                        pointID++;
                        currentFeature.DataRow["PointID"] = pointID;
                        map1.ResetBuffer();
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        map1.Cursor = Cursors.Default;
                        pointmouseClick = false;
                        shapeType = ""; // 退出编辑模式
                    }
                    break;

                case "line":
                    if (e.Button == MouseButtons.Left && linemouseClick)
                    {
                        if (firstClick)
                        {
                            List<Coordinate> lineArray = new List<Coordinate>();
                            LineString lineGeometry = new LineString(lineArray);
                            IFeature lineFeature = lineF.AddFeature(lineGeometry);
                            lineFeature.Coordinates.Add(coord);
                            lineID++;
                            lineFeature.DataRow["LineID"] = lineID;
                            firstClick = false;
                        }
                        else
                        {
                            if (lineF.Features.Count > 0)
                            {
                                IFeature existingFeature = lineF.Features[lineF.Features.Count - 1];
                                existingFeature.Coordinates.Add(coord);
                                if (existingFeature.Coordinates.Count >= 2)
                                {
                                    lineF.InitializeVertices();
                                    map1.ResetBuffer();
                                }
                            }
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        // 右键结束当前这条线的绘制，准备画下一条，但不退出模式
                        firstClick = true;
                        map1.ResetBuffer();
                    }
                    break;

                case "polygon":
                    if (e.Button == MouseButtons.Left && polygonmouseClick)
                    {
                        if (firstClick)
                        {
                            List<Coordinate> polygonArray = new List<Coordinate>();
                            LinearRing polygonGeometry = new LinearRing(polygonArray);
                            IFeature polygonFeature = polygonF.AddFeature(polygonGeometry);
                            polygonFeature.Coordinates.Add(coord);
                            polygonID++;
                            polygonFeature.DataRow["PolygonID"] = polygonID;
                            firstClick = false;
                        }
                        else
                        {
                            if (polygonF.Features.Count > 0)
                            {
                                IFeature existingFeature = polygonF.Features[polygonF.Features.Count - 1];
                                existingFeature.Coordinates.Add(coord);
                                if (existingFeature.Coordinates.Count >= 3)
                                {
                                    polygonF.InitializeVertices();
                                    map1.ResetBuffer();
                                }
                            }
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        firstClick = true;
                    }
                    break;

                case "hikingPath":
                    // ★★★ 原来的徒步路径逻辑移到这里 ★★★
                    if (hikingpathPathFinished) return;

                    if (e.Button == MouseButtons.Left)
                    {
                        if (firstClick)
                        {
                            List<Coordinate> lineArray = new List<Coordinate>();
                            LineString lineGeometry = new LineString(lineArray);
                            IFeature lineFeature = lineF.AddFeature(lineGeometry);
                            lineFeature.Coordinates.Add(coord);
                            lineID++;
                            lineFeature.DataRow["ID"] = lineID;
                            firstClick = false;
                        }
                        else
                        {
                            if (lineF.Features.Count > 0)
                            {
                                IFeature existingFeature = lineF.Features[lineF.Features.Count - 1];
                                existingFeature.Coordinates.Add(coord);
                                if (existingFeature.Coordinates.Count >= 2)
                                {
                                    lineF.InitializeVertices();
                                    map1.ResetBuffer();
                                }
                            }
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        // 徒步路径特有逻辑：右键直接保存并退出
                        firstClick = true;
                        map1.ResetBuffer();
                        try
                        {
                            lineF.SaveAs("c:\\hiking_path.shp", true); // 修改了保存路径，避免权限问题
                            MessageBox.Show("线shapefile已保存。");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("保存失败: " + ex.Message);
                        }

                        map1.Cursor = Cursors.Arrow;
                        hikingpathPathFinished = true;
                        shapeType = ""; // 退出模式
                    }
                    break;
            }
        }
        #endregion

        #region 工具栏菜单点击事件
        private void 添加点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetToolState(); // 先重置
            map1.Cursor = Cursors.Cross;
            shapeType = "Point";

            pointF = new FeatureSet(FeatureType.Point); // 建议每次重新初始化或清空，视需求而定
            pointF.Projection = map1.Projection;
            if (!pointF.DataTable.Columns.Contains("PointID"))
                pointF.DataTable.Columns.Add(new DataColumn("PointID"));

            MapPointLayer pointLayer = (MapPointLayer)map1.Layers.Add(pointF);
            PointSymbolizer symbol = new PointSymbolizer(Color.Red, DotSpatial.Symbology.PointShape.Ellipse, 3);
            pointLayer.Symbolizer = symbol;
            pointLayer.LegendText = "point";

            pointmouseClick = true;
        }

        private void 添加线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetToolState();
            map1.Cursor = Cursors.Cross;
            shapeType = "line";

            lineF = new FeatureSet(FeatureType.Line);
            lineF.Projection = map1.Projection;
            if (!lineF.DataTable.Columns.Contains("LineID"))
                lineF.DataTable.Columns.Add(new DataColumn("LineID"));

            lineLayer = (MapLineLayer)map1.Layers.Add(lineF);
            LineSymbolizer symbol = new LineSymbolizer(Color.Blue, 2);
            lineLayer.Symbolizer = symbol;
            lineLayer.LegendText = "line";

            firstClick = true;
            linemouseClick = true;
        }

        private void 添加面ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ResetToolState();
            map1.Cursor = Cursors.Cross;
            shapeType = "polygon";

            polygonF = new FeatureSet(FeatureType.Polygon);
            polygonF.Projection = map1.Projection;
            if (!polygonF.DataTable.Columns.Contains("PolygonID"))
                polygonF.DataTable.Columns.Add(new DataColumn("PolygonID"));

            MapPolygonLayer polygonLayer = (MapPolygonLayer)map1.Layers.Add(polygonF);
            PolygonSymbolizer symbol = new PolygonSymbolizer(Color.Green);
            polygonLayer.Symbolizer = symbol;
            polygonLayer.LegendText = "polygon";

            firstClick = true;
            polygonmouseClick = true;
        }

        private void 绘制徒步路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 移除旧路径 (可选)
            // map1.Layers.Clear(); // 慎用 Clear，会清除所有图层，最好只清除特定的

            ResetToolState();
            map1.Cursor = Cursors.Cross;
            shapeType = "hikingPath"; // 关键：设置专用类型
            hikingpathPathFinished = false;

            lineF = new FeatureSet(FeatureType.Line);
            lineF.Projection = map1.Projection;
            if (!lineF.DataTable.Columns.Contains("ID"))
                lineF.DataTable.Columns.Add(new DataColumn("ID"));

            lineLayer = (MapLineLayer)map1.Layers.Add(lineF);
            LineSymbolizer symbol = new LineSymbolizer(Color.Blue, 2);
            lineLayer.Symbolizer = symbol;
            lineLayer.LegendText = "徒步路径（Hiking path）";

            firstClick = true;
        }
        #endregion

        #region 保存操作
        private void 保存点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFeatureSet(pointF, "point.shp");
        }

        private void 保存线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ★★★ 修复：原来这里写成了 polygonF，改成 lineF ★★★
            SaveFeatureSet(lineF, "line.shp");
        }

        private void 保存面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFeatureSet(polygonF, "polygon.shp");
        }

        // 封装保存方法
        private void SaveFeatureSet(FeatureSet fs, string defaultName)
        {
            if (fs == null || fs.Features.Count == 0)
            {
                MessageBox.Show("没有可保存的要素。");
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = defaultName;
            sfd.Filter = "Shapefiles (*.shp)|*.shp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fs.SaveAs(sfd.FileName, true);
                MessageBox.Show("保存成功！");
            }
            ResetToolState(); // 保存后通常重置光标
        }
        #endregion

        #region 其他功能
        private void 查看属性表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 graphForm = new Form2(this);
            graphForm.Show();
        }

        private void 快捷查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map1.Layers.Count > 0 && map1.Layers.SelectedLayer is MapPolygonLayer stateLayer)
            {
                属性表.DataSource = stateLayer.DataSet.DataTable;
            }
            else if (map1.Layers.Count > 0 && map1.Layers[0] is MapPolygonLayer layer0)
            {
                // 如果没选中，默认取第一个
                属性表.DataSource = layer0.DataSet.DataTable;
            }
            else
            {
                MessageBox.Show("请选择一个面图层进行查看。");
            }
        }

        private void 计算徒步路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (map1.GetRasterLayers().Count() == 0)
                {
                    MessageBox.Show("请添加栅格图层（DEM）。");
                    return;
                }
                IMapRasterLayer rasterLayer = map1.GetRasterLayers()[0];

                if (map1.GetLineLayers().Count() == 0)
                {
                    MessageBox.Show("请绘制徒步路径。");
                    return;
                }
                // 获取最近绘制的一条线
                IMapLineLayer pathLayer = map1.GetLineLayers().Last();
                IFeatureSet featureSet = pathLayer.DataSet;

                if (featureSet.Features.Count == 0) return;

                IList<Coordinate> coordinateList = featureSet.Features[0].Coordinates;
                List<PathPoint> fullPathList = new List<PathPoint>();

                for (int i = 0; i < coordinateList.Count - 1; i++)
                {
                    Coordinate startCoord = coordinateList[i];
                    Coordinate endCoord = coordinateList[i + 1];
                    List<PathPoint> segmentPointList = ExtractElevation(startCoord.X, startCoord.Y, endCoord.X, endCoord.Y, rasterLayer);
                    fullPathList.AddRange(segmentPointList);
                }

                double distanceFromStart = 0;
                for (int i = 1; i < fullPathList.Count; i++)
                {
                    double x1 = fullPathList[i - 1].X;
                    double y1 = fullPathList[i - 1].Y;
                    double x2 = fullPathList[i].X;
                    double y2 = fullPathList[i].Y;
                    double distance12 = Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
                    distanceFromStart += distance12;
                    fullPathList[i].Distance = distanceFromStart;
                }
                Form3 graphForm = new Form3(fullPathList);
                graphForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("计算错误: " + ex.Message);
            }
        }

        private void 投影探索工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProjection projForm = new FormProjection();
            projForm.Show();
        }

        private void 山体阴影ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map1.Layers.Count > 0 && map1.Layers[0] is IMapRasterLayer layer)
            {
                layer.Symbolizer.ShadedRelief.ElevationFactor = 1;
                layer.Symbolizer.ShadedRelief.IsUsed = true;
                layer.WriteBitmap();
            }
            else MessageBox.Show("请确保第一层是栅格图层。");
        }

        private void 改变颜色方案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map1.Layers.Count > 0 && map1.Layers[0] is IMapRasterLayer layer)
            {
                ColorScheme scheme = new ColorScheme();
                ColorCategory category1 = new ColorCategory(2500, 3000, Color.Red, Color.Yellow);
                category1.LegendText = "High Elevation";
                scheme.AddCategory(category1);

                ColorCategory category2 = new ColorCategory(1000, 2500, Color.Blue, Color.Green);
                category2.LegendText = "Low Elevation";
                scheme.AddCategory(category2);

                layer.Symbolizer.Scheme = scheme;
                layer.WriteBitmap();
            }
        }

        private void 栅格乘法x2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map1.Layers.Count > 0 && map1.Layers[0] is IMapRasterLayer layer)
            {
                IRaster demRaster = layer.DataSet;
                IRaster newRaster = Raster.CreateRaster("multiply.bgd", null, demRaster.NumColumns, demRaster.NumRows, 1, demRaster.DataType, new string[0]);
                newRaster.Bounds = demRaster.Bounds.Copy();
                newRaster.NoDataValue = demRaster.NoDataValue;
                newRaster.Projection = demRaster.Projection;

                for (int r = 0; r < demRaster.NumRows; r++)
                {
                    for (int c = 0; c < demRaster.NumColumns; c++)
                    {
                        if (demRaster.Value[r, c] != demRaster.NoDataValue)
                            newRaster.Value[r, c] = demRaster.Value[r, c] * 2;
                    }
                }
                newRaster.Save();
                map1.Layers.Add(newRaster);
            }
        }

        private void 栅格重分类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map1.Layers.SelectedLayer is IMapRasterLayer layer)
            {
                IRaster demRaster = layer.DataSet;
                IRaster newRaster = Raster.CreateRaster("reclassify.bgd", null, demRaster.NumColumns, demRaster.NumRows, 1, demRaster.DataType, new string[0]);
                newRaster.Bounds = demRaster.Bounds.Copy();
                newRaster.NoDataValue = demRaster.NoDataValue;
                newRaster.Projection = demRaster.Projection;

                double threshold = 0;
                if (!double.TryParse(txtElevation.Text, out threshold))
                {
                    MessageBox.Show("请输入有效数字。"); return;
                }

                for (int r = 0; r < demRaster.NumRows; r++)
                {
                    for (int c = 0; c < demRaster.NumColumns; c++)
                    {
                        newRaster.Value[r, c] = (demRaster.Value[r, c] >= threshold) ? 1 : 0;
                    }
                }
                newRaster.Save();
                map1.Layers.Add(newRaster);
            }
            else MessageBox.Show("请在图例中选择一个栅格图层。");
        }
        #endregion

        #region 栅格值查询
        private void chbRasterValue_CheckedChanged(object sender, EventArgs e)
        {
            if (chbRasterValue.Checked)
            {
                ResetToolState(); // 先重置其他工具
                map1.Cursor = Cursors.Cross;
            }
            else
                map1.Cursor = Cursors.Arrow;
        }

        // ★★★ 确保此事件在 Designer 或构造函数中绑定 ★★★
        private void map1_MouseUp(object sender, MouseEventArgs e)
        {
            if (chbRasterValue.Checked && map1.Layers.SelectedLayer is IMapRasterLayer rasterLayer)
            {
                IRaster raster = rasterLayer.DataSet;
                Coordinate coord = map1.PixelToProj(e.Location);
                RcIndex rc = raster.Bounds.ProjToCell(coord);

                if (rc.Row >= 0 && rc.Row < raster.NumRows && rc.Column >= 0 && rc.Column < raster.NumColumns)
                {
                    double val = raster.Value[rc.Row, rc.Column];
                    lblRasterValue.Text = $"行:{rc.Row} 列:{rc.Column} 值:{val:F2}";
                }
            }
        }
        #endregion

        // 空事件处理（保持代码完整性）
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void txtElevation_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e) { }
    }
}