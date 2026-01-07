namespace 集成
{
    
    partial class FormProjection

    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        private void InitializeComponent()
        {
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lbltitle = new System.Windows.Forms.Label();


            this.gbBasicOperations = new System.Windows.Forms.GroupBox();
            this.btnLoadShapeFile = new System.Windows.Forms.Button();
            this.btnGetTotalArea = new System.Windows.Forms.Button();

            this.gbAdvancedOperations = new System.Windows.Forms.GroupBox();
            this.lblFieldName = new System.Windows.Forms.Label();
            this.cmbFiledName = new System.Windows.Forms.ComboBox();
            this.lblSelectedRegion = new System.Windows.Forms.Label();
            this.cmbSelectedRegion = new System.Windows.Forms.ComboBox();
            this.btnRegionArea = new System.Windows.Forms.Button();
            this.btnCompareProjections = new System.Windows.Forms.Button();


            this.pnlMap1 = new System.Windows.Forms.Panel();
            this.map1 = new DotSpatial.Controls.Map();
            this.pnlMap2 = new System.Windows.Forms.Panel();
            this.map2 = new DotSpatial.Controls.Map();
            this.pnlMap3 = new System.Windows.Forms.Panel();
            this.map3 = new DotSpatial.Controls.Map();
            this.pnlMap4 = new System.Windows.Forms.Panel();
            this.map4 = new DotSpatial.Controls.Map();
            this.pnlMap5 = new System.Windows.Forms.Panel();
            this.map5 = new DotSpatial.Controls.Map();
            this.pnlMap6 = new System.Windows.Forms.Panel();
            this.map6 = new DotSpatial.Controls.Map();


            this.lblmap1Projection = new System.Windows.Forms.Label();
            this.lbltotalAreaMap1 = new System.Windows.Forms.Label();
            this.lblmap1selectedinfo = new System.Windows.Forms.Label();
            this.lblMap1SelectedArea = new System.Windows.Forms.Label();
            this.lblmap1info = new System.Windows.Forms.Label();
            this.lblmap1difference = new System.Windows.Forms.Label();



            this.pnlMain.SuspendLayout();
            this.gbBasicOperations.SuspendLayout();
            this.gbAdvancedOperations.SuspendLayout();
            this.pnlMap1.SuspendLayout();
            this.pnlMap2.SuspendLayout();
            this.pnlMap3.SuspendLayout();
            this.pnlMap4.SuspendLayout();
            this.pnlMap5.SuspendLayout();
            this.pnlMap6.SuspendLayout();
            this.SuspendLayout();


            this.pnlMain.AutoScroll = true;
            this.pnlMain.Controls.Add(this.lbltitle);
            this.pnlMain.Controls.Add(this.gbBasicOperations);
            this.pnlMain.Controls.Add(this.gbAdvancedOperations);

            this.pnlMain.Controls.Add(this.pnlMap1);
            this.pnlMain.Controls.Add(this.pnlMap2);
            this.pnlMain.Controls.Add(this.pnlMap3);
            this.pnlMain.Controls.Add(this.pnlMap4);
            this.pnlMain.Controls.Add(this.pnlMap5);
            this.pnlMain.Controls.Add(this.pnlMap6);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1380, 850);
            this.pnlMain.TabIndex = 0;


            this.lbltitle.AutoSize = true;
            this.lbltitle.Font = new System.Drawing.Font("Microsoft YaHei", 16F, System.Drawing.FontStyle.Bold);
            this.lbltitle.ForeColor = System.Drawing.Color.Blue;
            this.lbltitle.Location = new System.Drawing.Point(500, 10);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Text = "地图投影探索工具 (Projection Explorer)";


            this.gbBasicOperations.Location = new System.Drawing.Point(20, 50);
            this.gbBasicOperations.Size = new System.Drawing.Size(200, 100);
            this.gbBasicOperations.Text = "基础操作";
            this.gbBasicOperations.Controls.Add(this.btnLoadShapeFile);
            this.gbBasicOperations.Controls.Add(this.btnGetTotalArea);

            this.btnLoadShapeFile.Location = new System.Drawing.Point(20, 25);
            this.btnLoadShapeFile.Size = new System.Drawing.Size(160, 30);
            this.btnLoadShapeFile.Text = "加载 Shape 文件";
            this.btnLoadShapeFile.Click += new System.EventHandler(this.btnLoadShapeFile_Click);

            this.btnGetTotalArea.Location = new System.Drawing.Point(20, 60);
            this.btnGetTotalArea.Size = new System.Drawing.Size(160, 30);
            this.btnGetTotalArea.Text = "计算总面积";
            this.btnGetTotalArea.Click += new System.EventHandler(this.btnGetTotalArea_Click);


            this.gbAdvancedOperations.Location = new System.Drawing.Point(240, 50);
            this.gbAdvancedOperations.Size = new System.Drawing.Size(460, 100);
            this.gbAdvancedOperations.Text = "高级操作";
            this.gbAdvancedOperations.Controls.Add(this.lblFieldName);
            this.gbAdvancedOperations.Controls.Add(this.cmbFiledName);
            this.gbAdvancedOperations.Controls.Add(this.lblSelectedRegion);
            this.gbAdvancedOperations.Controls.Add(this.cmbSelectedRegion);
            this.gbAdvancedOperations.Controls.Add(this.btnRegionArea);
            this.gbAdvancedOperations.Controls.Add(this.btnCompareProjections);

            this.lblFieldName.Location = new System.Drawing.Point(15, 30);
            this.lblFieldName.Text = "字段名:";
            this.lblFieldName.AutoSize = true;

            this.cmbFiledName.Location = new System.Drawing.Point(70, 27);
            this.cmbFiledName.Size = new System.Drawing.Size(120, 23);
            this.cmbFiledName.SelectedIndexChanged += new System.EventHandler(this.cmbFiledName_SelectedIndexChanged);

            this.lblSelectedRegion.Location = new System.Drawing.Point(210, 30);
            this.lblSelectedRegion.Text = "区域值:";
            this.lblSelectedRegion.AutoSize = true;

            this.cmbSelectedRegion.Location = new System.Drawing.Point(265, 27);
            this.cmbSelectedRegion.Size = new System.Drawing.Size(120, 23);

            this.btnRegionArea.Location = new System.Drawing.Point(15, 60);
            this.btnRegionArea.Size = new System.Drawing.Size(175, 30);
            this.btnRegionArea.Text = "计算选中区域面积";
            this.btnRegionArea.Click += new System.EventHandler(this.btnRegionArea_Click);

            this.btnCompareProjections.Location = new System.Drawing.Point(210, 60);
            this.btnCompareProjections.Size = new System.Drawing.Size(175, 30);
            this.btnCompareProjections.Text = "对比投影差异";
            this.btnCompareProjections.Click += new System.EventHandler(this.btnCompareProjections_Click);
            this.ClientSize = new System.Drawing.Size(1384, 861);
            this.Controls.Add(this.pnlMain);
            this.Name = "Form1";
            this.Text = "DotSpatial Projection Explorer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            this.gbBasicOperations.ResumeLayout(false);
            this.gbAdvancedOperations.ResumeLayout(false);
            this.gbAdvancedOperations.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();

            this.pnlMap1.ResumeLayout(false); this.pnlMap1.PerformLayout();
            this.pnlMap2.ResumeLayout(false); this.pnlMap2.PerformLayout();
            this.pnlMap3.ResumeLayout(false); this.pnlMap3.PerformLayout();
            this.pnlMap4.ResumeLayout(false); this.pnlMap4.PerformLayout();
            this.pnlMap5.ResumeLayout(false); this.pnlMap5.PerformLayout();
            this.pnlMap6.ResumeLayout(false); this.pnlMap6.PerformLayout();

            this.ResumeLayout(false);
        }

       

       

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.GroupBox gbBasicOperations;
        private System.Windows.Forms.GroupBox gbAdvancedOperations;
        private System.Windows.Forms.Button btnLoadShapeFile;
        private System.Windows.Forms.Button btnGetTotalArea;
        private System.Windows.Forms.Label lblFieldName;
        private System.Windows.Forms.ComboBox cmbFiledName;
        private System.Windows.Forms.Label lblSelectedRegion;
        private System.Windows.Forms.ComboBox cmbSelectedRegion;
        private System.Windows.Forms.Button btnRegionArea;
        private System.Windows.Forms.Button btnCompareProjections;

        private System.Windows.Forms.Panel pnlMap1; private DotSpatial.Controls.Map map1;
        private System.Windows.Forms.Panel pnlMap2; private DotSpatial.Controls.Map map2;
        private System.Windows.Forms.Panel pnlMap3; private DotSpatial.Controls.Map map3;
        private System.Windows.Forms.Panel pnlMap4; private DotSpatial.Controls.Map map4;
        private System.Windows.Forms.Panel pnlMap5; private DotSpatial.Controls.Map map5;
        private System.Windows.Forms.Panel pnlMap6; private DotSpatial.Controls.Map map6;

        private System.Windows.Forms.Label lblmap1Projection; private System.Windows.Forms.Label lbltotalAreaMap1;
        private System.Windows.Forms.Label lblmap1selectedinfo; private System.Windows.Forms.Label lblMap1SelectedArea;
        private System.Windows.Forms.Label lblmap1info; private System.Windows.Forms.Label lblmap1difference;

        private System.Windows.Forms.Label lblmap2Projection; private System.Windows.Forms.Label lbltotalAreaMap2;
        private System.Windows.Forms.Label lblmap2selectedinfo; private System.Windows.Forms.Label lblMap2SelectedArea;
        private System.Windows.Forms.Label lblmap2info; private System.Windows.Forms.Label lblmap2difference;

        private System.Windows.Forms.Label lblmap3Projection; private System.Windows.Forms.Label lbltotalAreaMap3;
        private System.Windows.Forms.Label lblmap3selectedinfo; private System.Windows.Forms.Label lblMap3SelectedArea;
        private System.Windows.Forms.Label lblmap3info; private System.Windows.Forms.Label lblmap3difference;

        private System.Windows.Forms.Label lblmap4Projection; private System.Windows.Forms.Label lbltotalAreaMap4;
        private System.Windows.Forms.Label lblmap4selectedinfo; private System.Windows.Forms.Label lblMap4SelectedArea;
        private System.Windows.Forms.Label lblmap4info; private System.Windows.Forms.Label lblmap4difference;

        private System.Windows.Forms.Label lblmap5Projection; private System.Windows.Forms.Label lbltotalAreaMap5;
        private System.Windows.Forms.Label lblmap5selectedinfo; private System.Windows.Forms.Label lblMap5SelectedArea;
        private System.Windows.Forms.Label lblmap5info; private System.Windows.Forms.Label lblmap5difference;

        private System.Windows.Forms.Label lblmap6Projection; private System.Windows.Forms.Label lbltotalAreaMap6;
        private System.Windows.Forms.Label lblmap6selectedinfo; private System.Windows.Forms.Label lblMap6SelectedArea;
        private System.Windows.Forms.Label lblmap6info; private System.Windows.Forms.Label lblmap6difference;

        #endregion
    }

}