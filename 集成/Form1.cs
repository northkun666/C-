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
    public partial class gis软件: Form
    {
        public Map Map1 => map1;
        string shapeType = ""; // 初始化为空字符串
        #region "类级变量（Class level variables）"
        // 徒步路径绘制完成的布尔变量
        bool hikingpathPathFinished = false;
        #endregion
        #region 点形状文件类级变量
        // 新的点要素集
        FeatureSet pointF = new FeatureSet(FeatureType.Point);
        // 点的 ID
        int pointID = 0;
        // 区分鼠标左右键点击
        bool pointmouseClick = false;
        #endregion

        #region 折线形状文件类级变量
        MapLineLayer lineLayer = default(MapLineLayer);
        // 线要素集
        FeatureSet lineF = new FeatureSet(FeatureType.Line);
        int lineID = 0;
        // 用于首次鼠标点击的布尔变量
        bool firstClick = false;
        // 控制折线保存操作后折线的绘制
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
        }
        /// <summary>
        /// 此函数用于获取高程值
        /// 根据给定线段的起点和终点，将其划分为100个点，并基于这些点计算高程
        /// </summary>
        /// <param name="startX">线段起点的X坐标</param>
        /// <param name="startY">线段起点的Y坐标</param>
        /// <param name="endX">线段终点的X坐标</param>
        /// <param name="endY">线段终点的Y坐标</param>
        /// <param name="raster">栅格DEM</param>
        /// <returns>高程值列表</returns>
        /// <remarks></remarks>
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
                RcIndex rowColumn = raster.DataSet.Bounds.ProjToCell(coordinate);
                curElevation = raster.DataSet.Value[rowColumn.Row, rowColumn.Column];
                // 设置新PathPoint的属性
                newPathPoint.X = curX;
                newPathPoint.Y = curY;
                newPathPoint.Elevation = curElevation;
                pathPointList.Add(newPathPoint);
            }
            return pathPointList;
        }
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 退出ToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void 矢量图形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map1.AddLayer();
        }

        private void 栅格图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map1.AddRasterLayer();
        }

        private void k放大_Click(object sender, EventArgs e)
        {
            map1.ZoomIn();
        }

        private void k缩小_Click(object sender, EventArgs e)
        {
            map1.ZoomOut();
        }

        private void k平移_Click(object sender, EventArgs e)
        {
            map1.FunctionMode = FunctionMode.Pan;
        }

        private void k选择_Click(object sender, EventArgs e)
        {
            map1.FunctionMode = FunctionMode.Select;
        }

        private void k缩放至图层_Click(object sender, EventArgs e)
        {
            map1.ZoomToMaxExtent();
        }

        private void k取消_Click(object sender, EventArgs e)
        {
            map1.FunctionMode = FunctionMode.None;
        }

        private void map1_MouseDown(object sender, MouseEventArgs e)
        {
            if (hikingpathPathFinished) return;
            switch (shapeType)
            {
                case "Point":
                    if (e.Button == MouseButtons.Left)
                    {
                        if ((pointmouseClick))
                        {
                            // 此方法用于将屏幕坐标转换为地图坐标（e.location 是地图控件上的鼠标点击点）
                            Coordinate coord = map1.PixelToProj(e.Location);
                            // 创建新点（输入参数为点击点坐标）
                            DotSpatial.Topology.Point point = new DotSpatial.Topology.Point(coord);
                            // 将点添加到点要素中（将点要素分配给 IFeature，因为只能通过它设置属性）
                            IFeature currentFeature = pointF.AddFeature(point);
                            // 增加点 ID
                            pointID = pointID + 1;
                            // 设置 ID 属性
                            currentFeature.DataRow["PointID"] = pointID;
                            // 刷新地图
                            map1.ResetBuffer();
                        }
                    }
                    else
                    {
                        // 鼠标右键点击
                        map1.Cursor = Cursors.Default;
                        pointmouseClick = false;
                    }
                    break;
                case "line":
                    if (e.Button == MouseButtons.Left)
                    {
                        // 左键点击 - 填充坐标数组（点击点的坐标）
                        Coordinate coord = map1.PixelToProj(e.Location);
                        if (linemouseClick)
                        {
                            // 首次左键点击 - 创建空的线要素
                            if (firstClick)
                            {
                                // 创建名为 lineArray 的新列表（此列表将存储坐标，用于存储鼠标点击坐标）
                                List<Coordinate> lineArray = new List<Coordinate>();
                                // 创建 LineString 类的实例（需要传递列表坐标集合）
                                LineString lineGeometry = new LineString(lineArray);
                                // 将线几何对象添加到线要素中
                                IFeature lineFeature = lineF.AddFeature(lineGeometry);
                                // 向线要素添加第一个坐标
                                lineFeature.Coordinates.Add(coord);
                                // 设置线要素属性
                                lineID = lineID + 1;
                                lineFeature.DataRow["LineID"] = lineID;
                                firstClick = false;
                            }
                            else
                            {
                                if (lineF.Features.Count == 0)
                                    return;
                                // 第二次或多次点击 - 向现有要素添加点
                                IFeature existingFeature = lineF.Features[lineF.Features.Count - 1];
                                existingFeature.Coordinates.Add(coord);
                                // 如果线有 2 个或更多点，则刷新地图
                                if (existingFeature.Coordinates.Count >= 2)
                                {
                                    lineF.InitializeVertices();
                                    map1.ResetBuffer();
                                }
                            }
                        }
                    }
                    else
                    {
                        // 右键点击 - 重置首次鼠标点击
                        firstClick = true;
                        map1.ResetBuffer();
                    }
                    break;
                case "polygon":
                    if (e.Button == MouseButtons.Left)
                    {
                        // 左键点击 - 填充坐标数组
                        Coordinate coord = map1.PixelToProj(e.Location);
                        if (polygonmouseClick)
                        {
                            // 首次左键点击 - 创建空的线要素
                            if (firstClick)
                            {
                                // 创建名为 polygonArray 的新列表（此列表将存储坐标，用于存储鼠标点击坐标）
                                List<Coordinate> polygonArray = new List<Coordinate>();
                                // 创建 LinearRing 类的实例（将多边形列表传递给此类的构造函数）
                                LinearRing polygonGeometry = new LinearRing(polygonArray);
                                // 将 polygonGeometry 实例添加到 PolygonFeature
                                IFeature polygonFeature = polygonF.AddFeature(polygonGeometry);
                                // 向多边形要素添加第一个坐标
                                polygonFeature.Coordinates.Add(coord);
                                // 设置多边形要素属性
                                polygonID = polygonID + 1;
                                polygonFeature.DataRow["PolygonID"] = polygonID;
                                firstClick = false;
                            }
                            else
                            {
                                if (polygonF.Features.Count == 0)
                                    return;
                                // 第二次或多次点击 - 向现有要素添加点
                                IFeature existingFeature = (IFeature)polygonF.Features[polygonF.Features.Count - 1];
                                existingFeature.Coordinates.Add(coord);
                                // 如果线有 2 个或更多点，则刷新地图
                                if (existingFeature.Coordinates.Count >= 3)
                                {
                                    // 刷新地图
                                    polygonF.InitializeVertices();
                                    map1.ResetBuffer();
                                }
                            }
                        }
                    }
                    else
                    {
                        // 右键点击 - 重置首次鼠标点击
                        firstClick = true;
                    }
                    break;

            }
            // 如果徒步路径已完成，则不再绘制线条
            if (hikingpathPathFinished == true)
                return;
            if (e.Button == MouseButtons.Left)
            {
                // 左键点击 - 填充坐标数组
                // 点击点的坐标
                Coordinate coord = map1.PixelToProj(e.Location);
                // 首次左键点击 - 创建空的线要素
                if (firstClick)
                {
                    // 创建一个名为lineArray的新列表
                    // 列表无需定义大小
                    // 此列表将存储坐标
                    // 我们将鼠标点击的坐标存储到该数组中
                    List<Coordinate> lineArray = new List<Coordinate>();
                    // 创建LineString类的实例
                    // 需要传入坐标列表集合
                    LineString lineGeometry = new LineString(lineArray);
                    // 将线几何对象添加到线要素中
                    IFeature lineFeature = lineF.AddFeature(lineGeometry);
                    // 向线要素添加第一个坐标
                    lineFeature.Coordinates.Add(coord);
                    // 设置线要素属性
                    lineID = lineID + 1;
                    lineFeature.DataRow["ID"] = lineID;
                    firstClick = false;
                }
                else
                {
                    if (lineF.Features.Count == 0)
                        return;
                    // 第二次或多次点击 - 向现有要素添加点
                    IFeature existingFeature = lineF.Features[lineF.Features.Count - 1];
                    existingFeature.Coordinates.Add(coord);
                    // 如果线条有2个或更多点，则刷新地图
                    if (existingFeature.Coordinates.Count >= 2)
                    {
                        lineF.InitializeVertices();
                        map1.ResetBuffer();
                    }
                }
            }
            else
            {
                // 右键点击 - 重置首次鼠标点击状态
                firstClick = true;
                map1.ResetBuffer();
                lineF.SaveAs("c:\\2009 Falls\\linepath.shp", true);
                MessageBox.Show("线shapefile已保存。（The line shapefile has been saved.）");
                map1.Cursor = Cursors.Arrow;
                // 徒步路径绘制完成
                hikingpathPathFinished = true;
            }
        }

        private void 添加点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 更改光标样式
            map1.Cursor = Cursors.Cross;
            // 将形状类型设置为类级字符串变量（将在选择语句中使用此变量）
            //shapeType = "Point";
            // 设置投影
            pointF.Projection = map1.Projection;
            // 初始化要素集属性表
            DataColumn column = new DataColumn("ID");
            pointF.DataTable.Columns.Add(column);
            // 将要素集添加为地图图层
            MapPointLayer pointLayer = (MapPointLayer)map1.Layers.Add(pointF);
            // 创建新的符号器
            PointSymbolizer symbol = new PointSymbolizer(Color.Red, DotSpatial.Symbology.PointShape.Ellipse, 3);
            // 为点图层设置符号器
            pointLayer.Symbolizer = symbol;
            // 设置图例文本为“point”
            pointLayer.LegendText = "point";
            // 设置左键点击为 true
            pointmouseClick = true;
        }

        private void 保存点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pointF.SaveAs("c:\\MW\\point.shp", true);
            MessageBox.Show("点形状文件已保存。");
            map1.Cursor = Cursors.Arrow;
        }

        private void 添加线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 更改鼠标光标
            map1.Cursor = Cursors.Cross;
            // 设置形状类型
            shapeType = "line";
            // 设置投影
            lineF.Projection = map1.Projection;
            // 初始化要素集属性表
            DataColumn column = new DataColumn("LineID");
            if (!lineF.DataTable.Columns.Contains("LineID"))
            {
                lineF.DataTable.Columns.Add(column);
            }
            // 将要素集添加为地图图层
            lineLayer = (MapLineLayer)map1.Layers.Add(lineF);
            // 为线要素设置符号器
            LineSymbolizer symbol = new LineSymbolizer(Color.Blue, 2);
            lineLayer.Symbolizer = symbol;
            lineLayer.LegendText = "line";
            firstClick = true;
            linemouseClick = true;
        }

        private void 保存线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            polygonF.SaveAs("c:\\MW\\polygon.shp", true);
            MessageBox.Show("多边形形状文件已保存。");
            map1.Cursor = Cursors.Arrow;
            polygonmouseClick = false;
        }

        private void 添加面ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 初始化多边形要素集
            map1.Cursor = Cursors.Cross;
            // 设置形状类型
            shapeType = "polygon";
            // 设置投影
            polygonF.Projection = map1.Projection;
            // 初始化要素集属性表
            DataColumn column = new DataColumn("PolygonID");
            if (!polygonF.DataTable.Columns.Contains("PolygonID"))
            {
                polygonF.DataTable.Columns.Add(column);
            }
            // 将要素集添加为地图图层
            MapPolygonLayer polygonLayer = (MapPolygonLayer)map1.Layers.Add(polygonF);
            PolygonSymbolizer symbol = new PolygonSymbolizer(Color.Green);
            polygonLayer.Symbolizer = symbol;
            polygonLayer.LegendText = "polygon";
            firstClick = true;
            polygonmouseClick = true;
        }

        private void 保存面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            polygonF.SaveAs("c:\\MW\\polygon.shp", true);
            MessageBox.Show("多边形形状文件已保存。");
            map1.Cursor = Cursors.Arrow;
            polygonmouseClick = false;
        }

        private void 查看属性表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 graphForm = new Form2(this);
            graphForm.Show();
        }

        private void 快捷查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 声明一个数据表（DataTable）
            DataTable dt = null;

            if (map1.Layers.Count > 0)
            {
                MapPolygonLayer stateLayer = default(MapPolygonLayer);

                stateLayer = (MapPolygonLayer)map1.Layers[0];

                if (stateLayer == null)
                {
                    MessageBox.Show("该图层不是面图层。");
                }
                else
                {
                    // 将 shape 文件的属性表赋值给数据表 dt
                    dt = stateLayer.DataSet.DataTable;

                    // 将数据网格视图的数据源设置为数据表 dt
                    属性表.DataSource = dt;
                }
            }
            else
            {
                MessageBox.Show("请向地图添加图层。");
            }
        }

        private void 绘制徒步路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 移除任何现有路径
            foreach (IMapLineLayer existingPath in map1.GetLineLayers())
            {
                map1.Layers.Remove(existingPath);
            }
            lineF = new FeatureSet(FeatureType.Line);
            // 徒步路径未完成
            hikingpathPathFinished = false;
            // 初始化多段线要素集
            map1.Cursor = Cursors.Cross;
            // 设置投影
            lineF.Projection = map1.Projection;
            // 初始化要素集属性表
            DataColumn column = new DataColumn("ID");
            lineF.DataTable.Columns.Add(column);
            // 将要素集添加为地图图层
            lineLayer = (MapLineLayer)map1.Layers.Add(lineF);
            LineSymbolizer symbol = new LineSymbolizer(Color.Blue, 2);
            lineLayer.Symbolizer = symbol;
            lineLayer.LegendText = "徒步路径（Hiking path）";
            firstClick = true;
        }

        private void 计算徒步路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 提取完整高程
                // 获取栅格图层
                IMapRasterLayer rasterLayer = default(IMapRasterLayer);
                if (map1.GetRasterLayers().Count() == 0)
                {
                    MessageBox.Show("请添加栅格图层（Please add a raster layer）");
                    return;
                }
                // 使用地图中的第一个栅格图层
                rasterLayer = map1.GetRasterLayers()[0];
                // 获取徒步路径线图层
                IMapLineLayer pathLayer = default(IMapLineLayer);
                if (map1.GetLineLayers().Count() == 0)
                {
                    MessageBox.Show("请添加徒步路径（Please add the hiking path）");
                    return;
                }
                pathLayer = map1.GetLineLayers()[0];
                IFeatureSet featureSet = pathLayer.DataSet;
                // 获取徒步路径的坐标，这是要素集中的第一个要素
                IList<Coordinate> coordinateList = featureSet.Features[0].Coordinates;
                // 获取路径所有线段的高程
                List<PathPoint> fullPathList = new List<PathPoint>();
                for (int i = 0; i < coordinateList.Count - 1; i++)
                {
                    // 对于每个线段
                    Coordinate startCoord = coordinateList[i];
                    Coordinate endCoord = coordinateList[i + 1];
                    List<PathPoint> segmentPointList = ExtractElevation(startCoord.X, startCoord.Y, endCoord.X, endCoord.Y, rasterLayer);
                    // 将此线段的点列表添加到完整列表中
                    fullPathList.AddRange(segmentPointList);
                }
                // 计算距离
                double distanceFromStart = 0;
                for (int i = 1; i <= fullPathList.Count - 1; i++)
                {
                    // 两个相邻点之间的距离
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
                MessageBox.Show("高程计算错误。整个路径应位于DEM区域内。（Error calculating elevation. The whole path should be inside the DEM area）");
            }
        }

        private void 投影探索工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // 假设你已经完整迁移了 App9 的 Form1 为 FormProjection
            FormProjection projForm = new FormProjection();
            projForm.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtElevation_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

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
                // 示例分类：高程 2500-3000 为红黄渐变
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
                // 创建新栅格
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
                // 确保你在 FormDesigner 里加了这个 TextBox，或者这里改用 InputBox
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
        private void chbRasterValue_CheckedChanged(object sender, EventArgs e)
        {
            if (chbRasterValue.Checked)
                map1.Cursor = Cursors.Cross; // 进入查询模式
            else
                map1.Cursor = Cursors.Arrow; // 恢复默认
        }
        private void map1_MouseUp(object sender, MouseEventArgs e)
        {
            // 只有当勾选了“查询栅格值”才执行
            if (chbRasterValue.Checked && map1.Layers.SelectedLayer is IMapRasterLayer rasterLayer)
            {
                IRaster raster = rasterLayer.DataSet;
                Coordinate coord = map1.PixelToProj(e.Location);
                RcIndex rc = raster.Bounds.ProjToCell(coord);

                if (rc.Row >= 0 && rc.Row < raster.NumRows && rc.Column >= 0 && rc.Column < raster.NumColumns)
                {
                    double val = raster.Value[rc.Row, rc.Column];
                    // 确保你在界面上加了这个 Label
                    lblRasterValue.Text = $"行: {rc.Row} 列: {rc.Column} 值: {val}";
                }
            }
        }
    }
 }
    

