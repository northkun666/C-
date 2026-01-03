using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Projections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 集成
{
    public partial class FormProjection : Form
    {
        public FormProjection()
        {
            InitializeComponent();
        }

        private void btnLoadShapeFile_Click(object sender, EventArgs e)
        {
            // 1. 定义 6 种不同的全球通用投影，以便展示明显差异
            // 投影1: WGS84 (经纬度直投，看起来扁平)
            map1.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
            map2.Projection = KnownCoordinateSystems.Projected.World.WebMercator;
            map3.Projection = ProjectionInfo.FromProj4String("+proj=moll +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs");
            map4.Projection = ProjectionInfo.FromProj4String("+proj=robin +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs");
            map5.Projection = ProjectionInfo.FromProj4String("+proj=aeqd +lat_0=90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs");
            map6.Projection = ProjectionInfo.FromProj4String("+proj=sinu +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs");

            // 更新界面标签
            lblmap1Projection.Text = "投影1: WGS1984 (经纬度直投)";
            lblmap2Projection.Text = "投影2: Web Mercator (墨卡托)";
            lblmap3Projection.Text = "投影3: Mollweide (摩尔威德等积)";
            lblmap4Projection.Text = "投影4: Robinson (罗宾逊折衷)";
            lblmap5Projection.Text = "投影5: North Pole AEQD (北极方位等距)";
            lblmap6Projection.Text = "投影6: Sinusoidal (正弦曲线等积)";

            // 2. 选择文件
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Shapefiles (*.shp)|*.shp";
            fileDialog.Title = "请选择一个 Shapefile 文件 (建议选择世界或国家级地图)";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ClearAllMaps(); // 清空旧数据
                    string fileName = fileDialog.FileName;

                    // 依次加载到6个地图
                    // 注意：必须分别为每个地图打开独立的文件副本，才能进行独立的重投影操作
                    LoadAndReproject(map1, fileName);
                    LoadAndReproject(map2, fileName);
                    LoadAndReproject(map3, fileName);
                    LoadAndReproject(map4, fileName);
                    LoadAndReproject(map5, fileName);
                    LoadAndReproject(map6, fileName);

                    // 填充下拉框 (用第一个地图的数据)
                    if (map1.Layers.Count > 0)
                    {
                        FillColumnNames(map1.Layers[0].DataSet as IFeatureSet);
                    }

                    MessageBox.Show("文件加载并重投影成功！\n请观察不同投影下的形状差异。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("加载或重投影时出错: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 核心方法：加载并安全重投影
        private void LoadAndReproject(DotSpatial.Controls.Map map, string fileName)
        {
            try
            {
                // 打开文件
                IFeatureSet featureSet = FeatureSet.Open(fileName);

                // 关键修正：如果源文件没有投影信息 (.prj缺失)，默认为 WGS84
                // 否则 Reproject() 会因为不知道源坐标系而报错或乱飞
                if (featureSet.Projection == null)
                {
                    featureSet.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
                }

                // 执行重投影 (修改内存中的坐标)
                featureSet.Reproject(map.Projection);

                // 添加图层
                map.Layers.Add(featureSet);

                // 关键修正：必须缩放到全图，否则坐标变了之后可能看不见地图
                map.ZoomToMaxExtent();
            }
            catch (Exception ex)
            {
                // 单个地图失败不影响其他地图
                System.Diagnostics.Debug.WriteLine("Map load error: " + ex.Message);
            }
        }

        private void ClearAllMaps()
        {
            map1.Layers.Clear(); map2.Layers.Clear(); map3.Layers.Clear();
            map4.Layers.Clear(); map5.Layers.Clear(); map6.Layers.Clear();
            cmbFiledName.Items.Clear();
            cmbSelectedRegion.Items.Clear();
        }

        private void btnGetTotalArea_Click(object sender, EventArgs e)
        {
            // Map 1: WGS84 是经纬度，计算结果是"平方度"，不代表物理面积
            lbltotalAreaMap1.Text = "总面积: " + _getTotalArea(map1).ToString("N2") + " (平方度 - 无物理意义)";

            // Map 2: 墨卡托单位是米，但面积变形极其严重（偏大）
            lbltotalAreaMap2.Text = "总面积: " + _getTotalArea(map2).ToString("N0") + " (平方米 - 变形严重偏大)";

            // Map 3: 摩尔威德是等积投影，面积准确
            lbltotalAreaMap3.Text = "总面积: " + _getTotalArea(map3).ToString("N0") + " (平方米 - 准确)";

            // Map 4: 罗宾逊是折衷投影，面积有一定变形
            lbltotalAreaMap4.Text = "总面积: " + _getTotalArea(map4).ToString("N0") + " (平方米 - 轻微变形)";

            // Map 5: 北极方位等距，距离准确但面积有变形
            lbltotalAreaMap5.Text = "总面积: " + _getTotalArea(map5).ToString("N0") + " (平方米 - 有变形)";

            // Map 6: 正弦曲线是等积投影，面积准确
            lbltotalAreaMap6.Text = "总面积: " + _getTotalArea(map6).ToString("N0") + " (平方米 - 准确)";
        }

        private double _getTotalArea(DotSpatial.Controls.Map mapInput)
        {
            double totalArea = 0;
            if (mapInput.Layers.Count > 0)
            {
                var layer = mapInput.Layers[0] as MapPolygonLayer;
                if (layer != null)
                {
                    foreach (IFeature feature in layer.DataSet.Features)
                    {
                        totalArea += feature.Area();
                    }
                }
            }
            return totalArea;
        }

        private void FillColumnNames(IFeatureSet featureSet)
        {
            cmbFiledName.Items.Clear();
            if (featureSet == null) return;
            foreach (DataColumn column in featureSet.DataTable.Columns)
            {
                cmbFiledName.Items.Add(column.ColumnName);
            }
            if (cmbFiledName.Items.Count > 0) cmbFiledName.SelectedIndex = 0;
        }

        private void cmbFiledName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (map1.Layers.Count > 0)
            {
                FillUniqueValues(cmbFiledName.Text, map1);
            }
        }

        private void FillUniqueValues(string uniqueField, DotSpatial.Controls.Map mapInput)
        {
            if (mapInput.Layers.Count > 0)
            {
                var layer = mapInput.Layers[0] as MapPolygonLayer;
                if (layer == null) return;

                DataTable dt = layer.DataSet.DataTable;
                cmbSelectedRegion.Items.Clear();
                HashSet<string> uniqueSet = new HashSet<string>();

                foreach (DataRow row in dt.Rows)
                {
                    string val = row[uniqueField].ToString();
                    if (!uniqueSet.Contains(val))
                    {
                        uniqueSet.Add(val);
                        cmbSelectedRegion.Items.Add(val);
                    }
                }
                if (cmbSelectedRegion.Items.Count > 0) cmbSelectedRegion.SelectedIndex = 0;
            }
        }

        private void btnRegionArea_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbFiledName.Text) || string.IsNullOrEmpty(cmbSelectedRegion.Text)) return;

            // 更新各个地图的选中面积标签
            // Map 1
            double area1 = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map1);
            lblMap1SelectedArea.Text = area1.ToString("N2") + " (平方度)";

            // Map 2
            double area2 = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map2);
            lblMap2SelectedArea.Text = area2.ToString("N0") + " (平方米 - 虚高)";

            // Map 3 (基准)
            double area3 = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map3);
            lblMap3SelectedArea.Text = area3.ToString("N0") + " (平方米 - 准确)";

            // Map 4
            double area4 = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map4);
            lblMap4SelectedArea.Text = area4.ToString("N0") + " (平方米)";

            // Map 5
            double area5 = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map5);
            lblMap5SelectedArea.Text = area5.ToString("N0") + " (平方米)";

            // Map 6
            double area6 = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map6);
            lblMap6SelectedArea.Text = area6.ToString("N0") + " (平方米 - 准确)";

            // 显示所有相关标签
            ToggleLabels(true);
        }

        private void UpdateRegionAreaLabel(Label lbl, DotSpatial.Controls.Map map)
        {
            double area = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map);
            lbl.Text = area.ToString("N2");
        }

        private double _getArea(string field, string value, DotSpatial.Controls.Map map)
        {
            double area = 0;
            if (map.Layers.Count > 0)
            {
                var layer = map.Layers[0] as MapPolygonLayer;
                if (layer != null)
                {
                    // 查找属性匹配的要素
                    layer.SelectByAttribute($"[{field}] = '{value}'");
                    foreach (IFeature feature in layer.Selection.ToFeatureList())
                    {
                        area += feature.Area();
                    }
                    layer.UnSelectAll(); // 算完后取消选中
                }
            }
            return area;
        }

        private void btnCompareProjections_Click(object sender, EventArgs e)
        {
            // 以 Map3 (Mollweide 等积投影) 作为面积对比的基准
            // 因为 Map1 是经纬度，面积单位不同；Map2 是墨卡托，面积变形极大
            double baseArea;
            if (!double.TryParse(lblMap3SelectedArea.Text, out baseArea)) return;

            lblmap1difference.Text = "单位不同，不可比";
            lblmap2difference.Text = GetDiff(baseArea, lblMap2SelectedArea.Text);
            lblmap3difference.Text = "基准 (0)";
            lblmap4difference.Text = GetDiff(baseArea, lblMap4SelectedArea.Text);
            lblmap5difference.Text = GetDiff(baseArea, lblMap5SelectedArea.Text);
            lblmap6difference.Text = GetDiff(baseArea, lblMap6SelectedArea.Text);

            ToggleDiffLabels(true);
        }

        private string GetDiff(double baseArea, string targetText)
        {
            double target;
            if (double.TryParse(targetText, out target))
            {
                double diff = target - baseArea;
                return diff.ToString("N2");
            }
            return "Error";
        }

        private void ToggleLabels(bool visible)
        {
            lblmap1selectedinfo.Visible = visible;
            lblmap2selectedinfo.Visible = visible;
            lblmap3selectedinfo.Visible = visible;
            lblmap4selectedinfo.Visible = visible;
            lblmap5selectedinfo.Visible = visible;
            lblmap6selectedinfo.Visible = visible;
        }

        private void ToggleDiffLabels(bool visible)
        {
            lblmap1difference.Visible = visible; lblmap1info.Visible = visible;
            lblmap2difference.Visible = visible; lblmap2info.Visible = visible;
            lblmap3difference.Visible = visible; lblmap3info.Visible = visible;
            lblmap4difference.Visible = visible; lblmap4info.Visible = visible;
            lblmap5difference.Visible = visible; lblmap5info.Visible = visible;
            lblmap6difference.Visible = visible; lblmap6info.Visible = visible;
        }

        // 占位符，防止设计器报错
        private void map6_Load(object sender, EventArgs e) { }
        private void lbltitle_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label2_Click_1(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void pnlMap5_Paint(object sender, PaintEventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}
