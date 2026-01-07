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

// 引入三层架构的命名空间
using 集成.Models;
using 集成.BLL;
// 注意：UI层不再直接引用 DAL 命名空间
// using 集成.DAL; 

namespace 集成
{
    public partial class gis软件 : Form
    {
        // ---------------------------------------------------------
        // 1. 核心服务实例 (Service Instances)
        // ---------------------------------------------------------
        // UI 只与 BLL (业务逻辑层) 交互
        private GisAnalysisService _gisService = new GisAnalysisService();

        // 已移除 DAL 层的直接引用 (_fileRepo)

        public Map Map1 => map1;

        // 当前操作的形状类型： "Point", "line", "polygon", "hikingPath"
        string shapeType = "";

        // ---------------------------------------------------------
        // 2. 类级变量 (State Variables)
        // ---------------------------------------------------------
        #region "类级变量"
        // 徒步路径绘制完成的布尔变量
        bool hikingpathPathFinished = true;
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
        bool firstClick = true;
        bool linemouseClick = false;
        #endregion

        #region 多边形形状文件类级变量
        FeatureSet polygonF = new FeatureSet(FeatureType.Polygon);
        int polygonID = 0;
        bool polygonmouseClick = false;
        #endregion

        public gis软件()
        {
            InitializeComponent();
            // 手动绑定设计器中可能丢失的事件
            BindMenuEvents();
        }

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

        // 重置工具状态，防止逻辑冲突
        private void ResetToolState()
        {
            map1.Cursor = Cursors.Arrow;
            map1.FunctionMode = FunctionMode.None;
            shapeType = "";

            pointmouseClick = false;
            linemouseClick = false;
            polygonmouseClick = false;

            hikingpathPathFinished = true;
            firstClick = true;
        }

        // ---------------------------------------------------------
        // 新增辅助方法：统一处理保存逻辑 (符合三层架构 UI -> BLL)
        // ---------------------------------------------------------
        private void SaveFeatureSet(IFeatureSet fs, string defaultName)
        {
            if (fs == null || fs.Features.Count == 0)
            {
                MessageBox.Show("没有可保存的数据。");
                return;
            }

            // 1. UI 层负责交互：询问用户存到哪里
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = defaultName;
            sfd.Filter = "Shapefiles (*.shp)|*.shp";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 2. 将路径传递给 BLL 层进行处理
                    _gisService.SaveShapefile(fs, sfd.FileName);
                    MessageBox.Show("保存成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存失败: " + ex.Message);
                }
            }
        }

        #region 基础地图操作 (放大/缩小/平移)
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
            map1.ZoomToMaxExtent();
        }

        private void k取消_Click(object sender, EventArgs e)
        {
            ResetToolState();
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

        #region 鼠标绘图逻辑
        private void map1_MouseDown(object sender, MouseEventArgs e)
        {
            if (map1.FunctionMode != FunctionMode.None) return;

            Coordinate coord = map1.PixelToProj(e.Location);

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
                        shapeType = "";
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
                        // 结束绘制并保存
                        firstClick = true;
                        map1.ResetBuffer();

                        // 使用新的保存方法，弹出对话框
                        SaveFeatureSet(lineF, "hiking_path.shp");

                        map1.Cursor = Cursors.Arrow;
                        hikingpathPathFinished = true;
                        shapeType = "";
                    }
                    break;
            }
        }
        #endregion

        #region 工具栏菜单点击事件
        private void 添加点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetToolState();
            map1.Cursor = Cursors.Cross;
            shapeType = "Point";

            pointF = new FeatureSet(FeatureType.Point);
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
            ResetToolState();
            map1.Cursor = Cursors.Cross;
            shapeType = "hikingPath";
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

        #region 保存操作 (修正为 UI -> BLL)
        private void 保存点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFeatureSet(pointF, "point.shp");
            ResetToolState();
        }

        private void 保存线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFeatureSet(lineF, "line.shp");
            ResetToolState();
        }

        private void 保存面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFeatureSet(polygonF, "polygon.shp");
            ResetToolState();
        }
        #endregion

        #region 核心业务逻辑 (使用 BLL)

        private void 计算徒步路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 获取输入数据 (UI 职责)
                if (map1.GetRasterLayers().Count() == 0)
                {
                    MessageBox.Show("请添加栅格图层（DEM）。");
                    return;
                }
                var demLayer = map1.GetRasterLayers()[0];

                if (map1.GetLineLayers().Count() == 0)
                {
                    MessageBox.Show("请绘制徒步路径。");
                    return;
                }
                var pathLayer = map1.GetLineLayers().Last();
                if (pathLayer.DataSet.Features.Count == 0) return;

                // 2. 调用 BLL 计算
                var pathFeature = pathLayer.DataSet.Features[0];
                List<PathPoint> resultList = _gisService.CalculateHikingProfile(pathFeature, demLayer.DataSet);

                // 3. 显示结果 (UI 职责)
                Form3 graphForm = new Form3(resultList);
                graphForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("计算错误: " + ex.Message);
            }
        }

        private void 栅格乘法x2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMapRasterLayer layer = map1.Layers.SelectedLayer as IMapRasterLayer;
            if (layer == null && map1.Layers.Count > 0) layer = map1.Layers[0] as IMapRasterLayer;
            if (layer == null) { MessageBox.Show("请选择栅格图层。"); return; }

            // UI 获取路径
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Binary Grid (*.bgd)|*.bgd";
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                // 调用 BLL (BLL负责计算并创建文件)
                IRaster resultRaster = _gisService.MultiplyRaster(layer.DataSet, 2.0, sfd.FileName);

                // 确保数据写入磁盘
                _gisService.SaveRaster(resultRaster);

                // 添加到地图显示
                map1.Layers.Add(resultRaster);
                MessageBox.Show("计算完成。");
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }
        }

        private void 栅格重分类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMapRasterLayer layer = map1.Layers.SelectedLayer as IMapRasterLayer;
            if (layer == null) { MessageBox.Show("请在图例中选择一个栅格图层。"); return; }

            double threshold = 0;
            if (!double.TryParse(txtElevation.Text, out threshold))
            {
                MessageBox.Show("请输入有效数字。"); return;
            }

            // UI 获取路径
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Binary Grid (*.bgd)|*.bgd";
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                // 调用 BLL
                IRaster resultRaster = _gisService.ReclassifyRaster(layer.DataSet, threshold, sfd.FileName);

                // 确保数据写入磁盘
                _gisService.SaveRaster(resultRaster);

                // 添加到地图显示
                map1.Layers.Add(resultRaster);
                MessageBox.Show("重分类完成。");
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }
        }
        #endregion

        #region 其他 UI 逻辑
        private void 查看属性表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 graphForm = new Form2(this);
            graphForm.Show();
        }

        private void 快捷查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map1.Layers.SelectedLayer is MapPolygonLayer selectedLayer)
            {
                属性表.DataSource = selectedLayer.DataSet.DataTable;
            }
            else if (map1.Layers.Count > 0 && map1.Layers[0] is MapPolygonLayer layer0)
            {
                属性表.DataSource = layer0.DataSet.DataTable;
            }
            else
            {
                MessageBox.Show("请选择一个面图层进行查看。");
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
            IMapRasterLayer layer = map1.Layers.SelectedLayer as IMapRasterLayer;
            if (layer == null && map1.Layers.Count > 0) layer = map1.Layers[0] as IMapRasterLayer;

            if (layer != null)
            {
                try
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
                    map1.Refresh();
                }
                catch (Exception ex) { MessageBox.Show("错误: " + ex.Message); }
            }
            else MessageBox.Show("请先选择一个栅格图层。");
        }

        private void chbRasterValue_CheckedChanged(object sender, EventArgs e)
        {
            if (chbRasterValue.Checked)
            {
                ResetToolState();
                map1.Cursor = Cursors.Cross;
            }
            else
            {
                map1.Cursor = Cursors.Arrow;
            }
        }

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

        // 空事件处理（保持完整性）
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void txtElevation_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e) { }
    }
}