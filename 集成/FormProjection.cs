using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Symbology;
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

            // 手动初始化地图面板布局，避免在 Designer.cs 中编写逻辑导致设计器崩溃
            SetupMaps();
        }

        #region 初始化与布局逻辑 (修复设计器问题)

        private void SetupMaps()
        {
            // 初始化6个地图面板的布局和控件引用
            ConfigureMapPanel(pnlMap1, map1, "投影1", 20, 160, ref lblmap1Projection, ref lbltotalAreaMap1, ref lblmap1selectedinfo, ref lblMap1SelectedArea, ref lblmap1info, ref lblmap1difference);
            ConfigureMapPanel(pnlMap2, map2, "投影2", 470, 160, ref lblmap2Projection, ref lbltotalAreaMap2, ref lblmap2selectedinfo, ref lblMap2SelectedArea, ref lblmap2info, ref lblmap2difference);
            ConfigureMapPanel(pnlMap3, map3, "投影3", 920, 160, ref lblmap3Projection, ref lbltotalAreaMap3, ref lblmap3selectedinfo, ref lblMap3SelectedArea, ref lblmap3info, ref lblmap3difference);

            ConfigureMapPanel(pnlMap4, map4, "投影4", 20, 520, ref lblmap4Projection, ref lbltotalAreaMap4, ref lblmap4selectedinfo, ref lblMap4SelectedArea, ref lblmap4info, ref lblmap4difference);
            ConfigureMapPanel(pnlMap5, map5, "投影5", 470, 520, ref lblmap5Projection, ref lbltotalAreaMap5, ref lblmap5selectedinfo, ref lblMap5SelectedArea, ref lblmap5info, ref lblmap5difference);
            ConfigureMapPanel(pnlMap6, map6, "投影6", 920, 520, ref lblmap6Projection, ref lbltotalAreaMap6, ref lblmap6selectedinfo, ref lblMap6SelectedArea, ref lblmap6info, ref lblmap6difference);
        }

        private void ConfigureMapPanel(Panel pnl, DotSpatial.Controls.Map map, string title, int x, int y,
            ref Label lblProj, ref Label lblTotal,
            ref Label lblSelInfo, ref Label lblSelArea,
            ref Label lblDiffInfo, ref Label lblDiffVal)
        {
            pnl.SuspendLayout();
            pnl.BorderStyle = BorderStyle.Fixed3D;
            pnl.Location = new Point(x, y);
            pnl.Size = new Size(440, 350);
            pnl.BackColor = SystemColors.ControlLightLight;

            // Map 设置
            map.Dock = DockStyle.Top;
            map.Height = 200;
            map.Legend = null;

            // 标签初始化
            // 注意：这里实例化 Label 并赋值给 ref 参数，这会更新类级别的字段引用
            lblProj = new Label
            {
                Text = title,
                ForeColor = Color.DarkRed,
                Font = new Font("Arial", 9F, FontStyle.Bold),
                Location = new Point(5, 205),
                AutoSize = true
            };

            lblTotal = new Label
            {
                Text = "Total Area: -",
                Location = new Point(5, 230),
                AutoSize = true
            };

            lblSelInfo = new Label
            {
                Text = "Area of Selected Region:",
                Location = new Point(5, 255),
                AutoSize = true,
                Visible = true
            };

            lblSelArea = new Label
            {
                Text = "0.00",
                Location = new Point(150, 255),
                AutoSize = true
            };

            lblDiffInfo = new Label
            {
                Text = "Diff from base:",
                Location = new Point(5, 280),
                AutoSize = true,
                Visible = false
            };

            lblDiffVal = new Label
            {
                Text = "0.00",
                Location = new Point(150, 280),
                AutoSize = true,
                Visible = false
            };

            // 将控件添加到面板
            // 检查防止重复添加
            if (!pnl.Controls.Contains(map)) pnl.Controls.Add(map);
            pnl.Controls.Add(lblProj);
            pnl.Controls.Add(lblTotal);
            pnl.Controls.Add(lblSelInfo);
            pnl.Controls.Add(lblSelArea);
            pnl.Controls.Add(lblDiffInfo);
            pnl.Controls.Add(lblDiffVal);

            pnl.ResumeLayout(false);
        }

        #endregion

        private void btnLoadShapeFile_Click(object sender, EventArgs e)
        {
            // 定义 6 种不同的投影以展示差异
            map1.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
            map2.Projection = KnownCoordinateSystems.Projected.World.WebMercator;
            // 使用 Proj4 字符串定义其他投影
            map3.Projection = ProjectionInfo.FromProj4String("+proj=moll +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs");
            map4.Projection = ProjectionInfo.FromProj4String("+proj=robin +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs");
            map5.Projection = ProjectionInfo.FromProj4String("+proj=aeqd +lat_0=90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs");
            map6.Projection = ProjectionInfo.FromProj4String("+proj=sinu +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs");

            // 更新标签文本
            lblmap1Projection.Text = "投影1: WGS1984 (经纬度直投)";
            lblmap2Projection.Text = "投影2: Web Mercator (墨卡托)";
            lblmap3Projection.Text = "投影3: Mollweide (摩尔威德等积)";
            lblmap4Projection.Text = "投影4: Robinson (罗宾逊折衷)";
            lblmap5Projection.Text = "投影5: North Pole AEQD (北极方位等距)";
            lblmap6Projection.Text = "投影6: Sinusoidal (正弦曲线等积)";

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Shapefiles (*.shp)|*.shp";
            fileDialog.Title = "请选择一个 Shapefile 文件";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ClearAllMaps();
                    string fileName = fileDialog.FileName;

                    // 依次加载到6个地图
                    LoadAndReproject(map1, fileName);
                    LoadAndReproject(map2, fileName);
                    LoadAndReproject(map3, fileName);
                    LoadAndReproject(map4, fileName);
                    LoadAndReproject(map5, fileName);
                    LoadAndReproject(map6, fileName);

                    // 填充字段下拉框 (使用第一个地图的数据)
                    if (map1.Layers.Count > 0)
                    {
                        FillColumnNames(map1.Layers[0].DataSet as IFeatureSet);
                    }

                    MessageBox.Show("文件加载并重投影成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("加载错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 加载并执行重投影
        private void LoadAndReproject(DotSpatial.Controls.Map map, string fileName)
        {
            try
            {
                IFeatureSet featureSet = FeatureSet.Open(fileName);

                // 如果源文件缺失投影信息，默认为 WGS84，防止报错
                if (featureSet.Projection == null)
                {
                    featureSet.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
                }

                // 执行重投影
                featureSet.Reproject(map.Projection);

                map.Layers.Add(featureSet);
                map.ZoomToMaxExtent();
            }
            catch (Exception ex)
            {
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
            lbltotalAreaMap1.Text = "总面积: " + _getTotalArea(map1).ToString("N2") + " (平方度)";
            lbltotalAreaMap2.Text = "总面积: " + _getTotalArea(map2).ToString("N0") + " (平方米 - 变形大)";
            lbltotalAreaMap3.Text = "总面积: " + _getTotalArea(map3).ToString("N0") + " (平方米 - 准确)";
            lbltotalAreaMap4.Text = "总面积: " + _getTotalArea(map4).ToString("N0") + " (平方米)";
            lbltotalAreaMap5.Text = "总面积: " + _getTotalArea(map5).ToString("N0") + " (平方米)";
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

            // 更新各个地图的选中面积
            lblMap1SelectedArea.Text = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map1).ToString("N2") + " (平方度)";
            lblMap2SelectedArea.Text = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map2).ToString("N0") + " (平方米)";
            lblMap3SelectedArea.Text = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map3).ToString("N0") + " (平方米)";
            lblMap4SelectedArea.Text = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map4).ToString("N0") + " (平方米)";
            lblMap5SelectedArea.Text = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map5).ToString("N0") + " (平方米)";
            lblMap6SelectedArea.Text = _getArea(cmbFiledName.Text, cmbSelectedRegion.Text, map6).ToString("N0") + " (平方米)";

            ToggleLabels(true);
        }

        // 辅助方法：保留此存根以防被引用
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
                    layer.SelectByAttribute($"[{field}] = '{value}'");
                    foreach (IFeature feature in layer.Selection.ToFeatureList())
                    {
                        area += feature.Area();
                    }
                    layer.UnSelectAll();
                }
            }
            return area;
        }

        private void btnCompareProjections_Click(object sender, EventArgs e)
        {
            // 以 Map3 (Mollweide 等积投影) 作为基准
            double baseArea;
            // 简单解析去除单位文本
            string map3Text = lblMap3SelectedArea.Text.Split(' ')[0];

            if (!double.TryParse(map3Text, out baseArea)) return;

            lblmap1difference.Text = "N/A";
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
            string cleanText = targetText.Split(' ')[0];
            if (double.TryParse(cleanText, out target))
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

        // 事件占位符，防止设计器报错
        private void map6_Load(object sender, EventArgs e) { }
        private void lbltitle_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label2_Click_1(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void pnlMap5_Paint(object sender, PaintEventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}