using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Symbology;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using 集成.BLL; 

namespace 集成
{
    public partial class FormProjection : Form
    {
        private GisAnalysisService _gisService = new GisAnalysisService();

        public FormProjection()
        {
            InitializeComponent();
            SetupMaps();
        }

        #region 初始化与布局逻辑 (保持不变)
        private void SetupMaps()
        {
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

            map.Dock = DockStyle.Top;
            map.Height = 200;
            map.Legend = null;

            lblProj = new Label { Text = title, ForeColor = Color.DarkRed, Font = new Font("Arial", 9F, FontStyle.Bold), Location = new Point(5, 205), AutoSize = true };
            lblTotal = new Label { Text = "总面积: -", Location = new Point(5, 230), AutoSize = true };
            lblSelInfo = new Label { Text = "选中区域面积:", Location = new Point(5, 255), AutoSize = true, Visible = true };
            lblSelArea = new Label { Text = "0.00", Location = new Point(150, 255), AutoSize = true };
            lblDiffInfo = new Label { Text = "与基准差异:", Location = new Point(5, 280), AutoSize = true, Visible = false };
            lblDiffVal = new Label { Text = "0.00", Location = new Point(150, 280), AutoSize = true, Visible = false };

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

        #region 文件加载 (UI 逻辑)
        private void btnLoadShapeFile_Click(object sender, EventArgs e)
        {
            map1.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
            map2.Projection = KnownCoordinateSystems.Projected.World.WebMercator;
            map3.Projection = ProjectionInfo.FromProj4String("+proj=moll +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs");
            map4.Projection = ProjectionInfo.FromProj4String("+proj=robin +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs");
            map5.Projection = ProjectionInfo.FromProj4String("+proj=aeqd +lat_0=90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs");
            map6.Projection = ProjectionInfo.FromProj4String("+proj=sinu +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs");

            lblmap1Projection.Text = "投影1: WGS1984 (经纬度直投)";
            lblmap2Projection.Text = "投影2: Web Mercator (墨卡托)";
            lblmap3Projection.Text = "投影3: Mollweide (摩尔威德等积)";
            lblmap4Projection.Text = "投影4: Robinson (罗宾逊折衷)";
            lblmap5Projection.Text = "投影5: North Pole AEQD (北极方位等距)";
            lblmap6Projection.Text = "投影6: Sinusoidal (正弦曲线等积)";

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Shapefiles (*.shp)|*.shp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ClearAllMaps();
                    string fileName = fileDialog.FileName;
                    LoadAndReproject(map1, fileName);
                    LoadAndReproject(map2, fileName);
                    LoadAndReproject(map3, fileName);
                    LoadAndReproject(map4, fileName);
                    LoadAndReproject(map5, fileName);
                    LoadAndReproject(map6, fileName);

                    if (map1.Layers.Count > 0) FillColumnNames(map1.Layers[0].DataSet as IFeatureSet);
                    MessageBox.Show("文件加载成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误: " + ex.Message);
                }
            }
        }

        private void LoadAndReproject(DotSpatial.Controls.Map map, string fileName)
        {
            try
            {
                IFeatureSet featureSet = FeatureSet.Open(fileName);
                if (featureSet.Projection == null) featureSet.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
                featureSet.Reproject(map.Projection);
                map.Layers.Add(featureSet);
                map.ZoomToMaxExtent();
            }
            catch { }
        }

        private void ClearAllMaps()
        {
            map1.Layers.Clear(); map2.Layers.Clear(); map3.Layers.Clear();
            map4.Layers.Clear(); map5.Layers.Clear(); map6.Layers.Clear();
            cmbFiledName.Items.Clear(); cmbSelectedRegion.Items.Clear();
            lbltotalAreaMap1.Text = "Total Area: -";
            lblMap1SelectedArea.Text = "0.00";
        }
        #endregion

        #region 面积计算 (修改：调用 Service 层)

        private void btnGetTotalArea_Click(object sender, EventArgs e)
        {
            lbltotalAreaMap1.Text = "总面积: " + GetTotalAreaFromService(map1).ToString("N2");
            lbltotalAreaMap2.Text = "总面积: " + GetTotalAreaFromService(map2).ToString("N0");
            lbltotalAreaMap3.Text = "总面积: " + GetTotalAreaFromService(map3).ToString("N0");
            lbltotalAreaMap4.Text = "总面积: " + GetTotalAreaFromService(map4).ToString("N0");
            lbltotalAreaMap5.Text = "总面积: " + GetTotalAreaFromService(map5).ToString("N0");
            lbltotalAreaMap6.Text = "总面积: " + GetTotalAreaFromService(map6).ToString("N0");
        }

        private double GetTotalAreaFromService(DotSpatial.Controls.Map map)
        {
            if (map.Layers.Count > 0 && map.Layers[0] is MapPolygonLayer layer)
            {
                return _gisService.CalculateTotalArea(layer.DataSet);
            }
            return 0;
        }

        private void btnRegionArea_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbFiledName.Text) || string.IsNullOrEmpty(cmbSelectedRegion.Text)) return;

            string field = cmbFiledName.Text;
            string val = cmbSelectedRegion.Text;

            UpdateRegionLabel(lblMap1SelectedArea, map1, field, val);
            UpdateRegionLabel(lblMap2SelectedArea, map2, field, val);
            UpdateRegionLabel(lblMap3SelectedArea, map3, field, val);
            UpdateRegionLabel(lblMap4SelectedArea, map4, field, val);
            UpdateRegionLabel(lblMap5SelectedArea, map5, field, val);
            UpdateRegionLabel(lblMap6SelectedArea, map6, field, val);

            ToggleLabels(true);
        }

        private void UpdateRegionLabel(Label lbl, DotSpatial.Controls.Map map, string field, string val)
        {
            if (map.Layers.Count > 0 && map.Layers[0] is MapPolygonLayer layer)
            {
                layer.UnSelectAll();
                double area = _gisService.CalculateRegionArea(layer.DataSet, field, val);
                lbl.Text = area.ToString(map == map1 ? "N2" : "N0"); 
                layer.SelectByAttribute($"[{field}] = '{val}'");
            }
        }
        #endregion

        #region 辅助逻辑 (下拉框与对比)
        private void FillColumnNames(IFeatureSet featureSet)
        {
            cmbFiledName.Items.Clear();
            if (featureSet == null) return;
            foreach (DataColumn column in featureSet.DataTable.Columns)
                cmbFiledName.Items.Add(column.ColumnName);
            if (cmbFiledName.Items.Count > 0) cmbFiledName.SelectedIndex = 0;
        }

        private void cmbFiledName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (map1.Layers.Count > 0) FillUniqueValues(cmbFiledName.Text, map1);
        }

        private void FillUniqueValues(string uniqueField, DotSpatial.Controls.Map mapInput)
        {
            if (mapInput.Layers.Count > 0 && mapInput.Layers[0] is MapPolygonLayer layer)
            {
                DataTable dt = layer.DataSet.DataTable;
                cmbSelectedRegion.Items.Clear();
                HashSet<string> uniqueSet = new HashSet<string>();
                foreach (DataRow row in dt.Rows)
                {
                    string val = row[uniqueField].ToString();
                    if (!uniqueSet.Contains(val)) { uniqueSet.Add(val); cmbSelectedRegion.Items.Add(val); }
                }
                if (cmbSelectedRegion.Items.Count > 0) cmbSelectedRegion.SelectedIndex = 0;
            }
        }

        private void btnCompareProjections_Click(object sender, EventArgs e)
        {
            double baseArea;
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
            if (double.TryParse(targetText.Split(' ')[0], out target))
                return (target - baseArea).ToString("N2");
            return "Error";
        }

        private void ToggleLabels(bool visible)
        {
            lblmap1selectedinfo.Visible = visible; lblmap2selectedinfo.Visible = visible;
            lblmap3selectedinfo.Visible = visible; lblmap4selectedinfo.Visible = visible;
            lblmap5selectedinfo.Visible = visible; lblmap6selectedinfo.Visible = visible;
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
        #endregion

        private void map6_Load(object sender, EventArgs e) { }
        private void lbltitle_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label2_Click_1(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void pnlMap5_Paint(object sender, PaintEventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}