namespace 集成
{
    partial class gis软件
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(gis软件));
            this.pnl菜单 = new System.Windows.Forms.Panel();
            this.menu菜单 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.矢量图形ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.栅格图像ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.视图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.放大ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.缩小ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.平移ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.还原ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.点对象ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.线对象ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.面对象ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加线ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.保存面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.属性表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看属性表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.快捷查看ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.栅格工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制徒步路径ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.计算徒步路径ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnl图层 = new System.Windows.Forms.Panel();
            this.pnl属性 = new System.Windows.Forms.Panel();
            this.属性表 = new System.Windows.Forms.DataGridView();
            this.pnl地图 = new System.Windows.Forms.Panel();
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.投影探索工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.map1 = new DotSpatial.Controls.Map();
            this.legend1 = new DotSpatial.Controls.Legend();
            this.k取消 = new System.Windows.Forms.Button();
            this.k缩放至图层 = new System.Windows.Forms.Button();
            this.k放大 = new System.Windows.Forms.Button();
            this.k缩小 = new System.Windows.Forms.Button();
            this.k选择 = new System.Windows.Forms.Button();
            this.k平移 = new System.Windows.Forms.Button();
            this.山体阴影ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.改变颜色方案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.栅格乘法x2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.栅格重分类ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtElevation = new System.Windows.Forms.TextBox();
            this.chbRasterValue = new System.Windows.Forms.CheckBox();
            this.lblRasterValue = new System.Windows.Forms.Label();
            this.pnl菜单.SuspendLayout();
            this.menu菜单.SuspendLayout();
            this.pnl图层.SuspendLayout();
            this.pnl属性.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.属性表)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl菜单
            // 
            this.pnl菜单.BackColor = System.Drawing.Color.Silver;
            this.pnl菜单.Controls.Add(this.k取消);
            this.pnl菜单.Controls.Add(this.k缩放至图层);
            this.pnl菜单.Controls.Add(this.k放大);
            this.pnl菜单.Controls.Add(this.k缩小);
            this.pnl菜单.Controls.Add(this.k选择);
            this.pnl菜单.Controls.Add(this.k平移);
            this.pnl菜单.Controls.Add(this.menu菜单);
            this.pnl菜单.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl菜单.Location = new System.Drawing.Point(0, 0);
            this.pnl菜单.Name = "pnl菜单";
            this.pnl菜单.Size = new System.Drawing.Size(1141, 48);
            this.pnl菜单.TabIndex = 0;
            // 
            // menu菜单
            // 
            this.menu菜单.BackColor = System.Drawing.Color.Silver;
            this.menu菜单.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menu菜单.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menu菜单.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.视图ToolStripMenuItem,
            this.地图ToolStripMenuItem,
            this.添加ToolStripMenuItem,
            this.属性表ToolStripMenuItem,
            this.栅格工具ToolStripMenuItem,
            this.工具ToolStripMenuItem});
            this.menu菜单.Location = new System.Drawing.Point(0, 0);
            this.menu菜单.Name = "menu菜单";
            this.menu菜单.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menu菜单.Size = new System.Drawing.Size(1141, 26);
            this.menu菜单.TabIndex = 0;
            this.menu菜单.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem,
            this.toolStripMenuItem2});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.矢量图形ToolStripMenuItem,
            this.栅格图像ToolStripMenuItem});
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
            this.打开ToolStripMenuItem.Text = "打开";
            // 
            // 矢量图形ToolStripMenuItem
            // 
            this.矢量图形ToolStripMenuItem.Name = "矢量图形ToolStripMenuItem";
            this.矢量图形ToolStripMenuItem.Size = new System.Drawing.Size(134, 24);
            this.矢量图形ToolStripMenuItem.Text = "矢量图像";
            this.矢量图形ToolStripMenuItem.Click += new System.EventHandler(this.矢量图形ToolStripMenuItem_Click);
            // 
            // 栅格图像ToolStripMenuItem
            // 
            this.栅格图像ToolStripMenuItem.Name = "栅格图像ToolStripMenuItem";
            this.栅格图像ToolStripMenuItem.Size = new System.Drawing.Size(134, 24);
            this.栅格图像ToolStripMenuItem.Text = "栅格图像";
            this.栅格图像ToolStripMenuItem.Click += new System.EventHandler(this.栅格图像ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(106, 24);
            this.toolStripMenuItem2.Text = "退出";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.退出ToolStripMenuItem2_Click);
            // 
            // 视图ToolStripMenuItem
            // 
            this.视图ToolStripMenuItem.Name = "视图ToolStripMenuItem";
            this.视图ToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.视图ToolStripMenuItem.Text = "视图";
            // 
            // 地图ToolStripMenuItem
            // 
            this.地图ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.放大ToolStripMenuItem,
            this.缩小ToolStripMenuItem,
            this.平移ToolStripMenuItem,
            this.还原ToolStripMenuItem,
            this.取消操作ToolStripMenuItem});
            this.地图ToolStripMenuItem.Name = "地图ToolStripMenuItem";
            this.地图ToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.地图ToolStripMenuItem.Text = "地图";
            // 
            // 放大ToolStripMenuItem
            // 
            this.放大ToolStripMenuItem.Name = "放大ToolStripMenuItem";
            this.放大ToolStripMenuItem.Size = new System.Drawing.Size(134, 24);
            this.放大ToolStripMenuItem.Text = "放大";
            // 
            // 缩小ToolStripMenuItem
            // 
            this.缩小ToolStripMenuItem.Name = "缩小ToolStripMenuItem";
            this.缩小ToolStripMenuItem.Size = new System.Drawing.Size(134, 24);
            this.缩小ToolStripMenuItem.Text = "缩小";
            // 
            // 平移ToolStripMenuItem
            // 
            this.平移ToolStripMenuItem.Name = "平移ToolStripMenuItem";
            this.平移ToolStripMenuItem.Size = new System.Drawing.Size(134, 24);
            this.平移ToolStripMenuItem.Text = "平移";
            // 
            // 还原ToolStripMenuItem
            // 
            this.还原ToolStripMenuItem.Name = "还原ToolStripMenuItem";
            this.还原ToolStripMenuItem.Size = new System.Drawing.Size(134, 24);
            this.还原ToolStripMenuItem.Text = "还原";
            // 
            // 取消操作ToolStripMenuItem
            // 
            this.取消操作ToolStripMenuItem.Name = "取消操作ToolStripMenuItem";
            this.取消操作ToolStripMenuItem.Size = new System.Drawing.Size(134, 24);
            this.取消操作ToolStripMenuItem.Text = "取消操作";
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.点对象ToolStripMenuItem,
            this.线对象ToolStripMenuItem,
            this.面对象ToolStripMenuItem});
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(60, 24);
            this.添加ToolStripMenuItem.Text = "添加…";
            // 
            // 点对象ToolStripMenuItem
            // 
            this.点对象ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加点ToolStripMenuItem,
            this.保存点ToolStripMenuItem});
            this.点对象ToolStripMenuItem.Name = "点对象ToolStripMenuItem";
            this.点对象ToolStripMenuItem.Size = new System.Drawing.Size(120, 24);
            this.点对象ToolStripMenuItem.Text = "点对象";
            // 
            // 添加点ToolStripMenuItem
            // 
            this.添加点ToolStripMenuItem.Name = "添加点ToolStripMenuItem";
            this.添加点ToolStripMenuItem.Size = new System.Drawing.Size(120, 24);
            this.添加点ToolStripMenuItem.Text = "添加点";
            this.添加点ToolStripMenuItem.Click += new System.EventHandler(this.添加点ToolStripMenuItem_Click);
            // 
            // 保存点ToolStripMenuItem
            // 
            this.保存点ToolStripMenuItem.Name = "保存点ToolStripMenuItem";
            this.保存点ToolStripMenuItem.Size = new System.Drawing.Size(120, 24);
            this.保存点ToolStripMenuItem.Text = "保存点";
            this.保存点ToolStripMenuItem.Click += new System.EventHandler(this.保存点ToolStripMenuItem_Click);
            // 
            // 线对象ToolStripMenuItem
            // 
            this.线对象ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加线ToolStripMenuItem,
            this.保存线ToolStripMenuItem});
            this.线对象ToolStripMenuItem.Name = "线对象ToolStripMenuItem";
            this.线对象ToolStripMenuItem.Size = new System.Drawing.Size(120, 24);
            this.线对象ToolStripMenuItem.Text = "线对象";
            // 
            // 添加线ToolStripMenuItem
            // 
            this.添加线ToolStripMenuItem.Name = "添加线ToolStripMenuItem";
            this.添加线ToolStripMenuItem.Size = new System.Drawing.Size(120, 24);
            this.添加线ToolStripMenuItem.Text = "添加线";
            this.添加线ToolStripMenuItem.Click += new System.EventHandler(this.添加线ToolStripMenuItem_Click);
            // 
            // 保存线ToolStripMenuItem
            // 
            this.保存线ToolStripMenuItem.Name = "保存线ToolStripMenuItem";
            this.保存线ToolStripMenuItem.Size = new System.Drawing.Size(120, 24);
            this.保存线ToolStripMenuItem.Text = "保存线";
            this.保存线ToolStripMenuItem.Click += new System.EventHandler(this.保存线ToolStripMenuItem_Click);
            // 
            // 面对象ToolStripMenuItem
            // 
            this.面对象ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加线ToolStripMenuItem1,
            this.保存面ToolStripMenuItem});
            this.面对象ToolStripMenuItem.Name = "面对象ToolStripMenuItem";
            this.面对象ToolStripMenuItem.Size = new System.Drawing.Size(120, 24);
            this.面对象ToolStripMenuItem.Text = "面对象";
            // 
            // 添加线ToolStripMenuItem1
            // 
            this.添加线ToolStripMenuItem1.Name = "添加线ToolStripMenuItem1";
            this.添加线ToolStripMenuItem1.Size = new System.Drawing.Size(120, 24);
            this.添加线ToolStripMenuItem1.Text = "添加面";
            this.添加线ToolStripMenuItem1.Click += new System.EventHandler(this.添加面ToolStripMenuItem1_Click);
            // 
            // 保存面ToolStripMenuItem
            // 
            this.保存面ToolStripMenuItem.Name = "保存面ToolStripMenuItem";
            this.保存面ToolStripMenuItem.Size = new System.Drawing.Size(120, 24);
            this.保存面ToolStripMenuItem.Text = "保存面";
            this.保存面ToolStripMenuItem.Click += new System.EventHandler(this.保存面ToolStripMenuItem_Click);
            // 
            // 属性表ToolStripMenuItem
            // 
            this.属性表ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看属性表ToolStripMenuItem,
            this.快捷查看ToolStripMenuItem});
            this.属性表ToolStripMenuItem.Name = "属性表ToolStripMenuItem";
            this.属性表ToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.属性表ToolStripMenuItem.Text = "属性表…";
            // 
            // 查看属性表ToolStripMenuItem
            // 
            this.查看属性表ToolStripMenuItem.Name = "查看属性表ToolStripMenuItem";
            this.查看属性表ToolStripMenuItem.Size = new System.Drawing.Size(148, 24);
            this.查看属性表ToolStripMenuItem.Text = "属性表工具";
            this.查看属性表ToolStripMenuItem.Click += new System.EventHandler(this.查看属性表ToolStripMenuItem_Click);
            // 
            // 快捷查看ToolStripMenuItem
            // 
            this.快捷查看ToolStripMenuItem.Name = "快捷查看ToolStripMenuItem";
            this.快捷查看ToolStripMenuItem.Size = new System.Drawing.Size(148, 24);
            this.快捷查看ToolStripMenuItem.Text = "快捷查看";
            this.快捷查看ToolStripMenuItem.Click += new System.EventHandler(this.快捷查看ToolStripMenuItem_Click);
            // 
            // 栅格工具ToolStripMenuItem
            // 
            this.栅格工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.绘制徒步路径ToolStripMenuItem,
            this.计算徒步路径ToolStripMenuItem,
            this.山体阴影ToolStripMenuItem,
            this.改变颜色方案ToolStripMenuItem,
            this.栅格乘法x2ToolStripMenuItem,
            this.栅格重分类ToolStripMenuItem});
            this.栅格工具ToolStripMenuItem.Name = "栅格工具ToolStripMenuItem";
            this.栅格工具ToolStripMenuItem.Size = new System.Drawing.Size(88, 24);
            this.栅格工具ToolStripMenuItem.Text = "栅格工具…";
            // 
            // 绘制徒步路径ToolStripMenuItem
            // 
            this.绘制徒步路径ToolStripMenuItem.Name = "绘制徒步路径ToolStripMenuItem";
            this.绘制徒步路径ToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.绘制徒步路径ToolStripMenuItem.Text = "绘制徒步路径";
            this.绘制徒步路径ToolStripMenuItem.Click += new System.EventHandler(this.绘制徒步路径ToolStripMenuItem_Click);
            // 
            // 计算徒步路径ToolStripMenuItem
            // 
            this.计算徒步路径ToolStripMenuItem.Name = "计算徒步路径ToolStripMenuItem";
            this.计算徒步路径ToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.计算徒步路径ToolStripMenuItem.Text = "计算徒步路径";
            this.计算徒步路径ToolStripMenuItem.Click += new System.EventHandler(this.计算徒步路径ToolStripMenuItem_Click);
            // 
            // pnl图层
            // 
            this.pnl图层.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnl图层.Controls.Add(this.groupBox1);
            this.pnl图层.Controls.Add(this.legend1);
            this.pnl图层.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnl图层.Location = new System.Drawing.Point(0, 48);
            this.pnl图层.Name = "pnl图层";
            this.pnl图层.Size = new System.Drawing.Size(200, 579);
            this.pnl图层.TabIndex = 1;
            // 
            // pnl属性
            // 
            this.pnl属性.BackColor = System.Drawing.Color.Gray;
            this.pnl属性.Controls.Add(this.属性表);
            this.pnl属性.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl属性.Location = new System.Drawing.Point(200, 495);
            this.pnl属性.Name = "pnl属性";
            this.pnl属性.Size = new System.Drawing.Size(941, 132);
            this.pnl属性.TabIndex = 2;
            // 
            // 属性表
            // 
            this.属性表.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.属性表.Dock = System.Windows.Forms.DockStyle.Fill;
            this.属性表.Location = new System.Drawing.Point(0, 0);
            this.属性表.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.属性表.Name = "属性表";
            this.属性表.RowHeadersWidth = 62;
            this.属性表.RowTemplate.Height = 30;
            this.属性表.Size = new System.Drawing.Size(941, 132);
            this.属性表.TabIndex = 0;
            // 
            // pnl地图
            // 
            this.pnl地图.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl地图.Location = new System.Drawing.Point(0, 0);
            this.pnl地图.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnl地图.Name = "pnl地图";
            this.pnl地图.Size = new System.Drawing.Size(1141, 627);
            this.pnl地图.TabIndex = 4;
            // 
            // 工具ToolStripMenuItem
            // 
            this.工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.投影探索工具ToolStripMenuItem});
            this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.工具ToolStripMenuItem.Text = "工具";
            // 
            // 投影探索工具ToolStripMenuItem
            // 
            this.投影探索工具ToolStripMenuItem.Name = "投影探索工具ToolStripMenuItem";
            this.投影探索工具ToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.投影探索工具ToolStripMenuItem.Text = "投影探索工具";
            this.投影探索工具ToolStripMenuItem.Click += new System.EventHandler(this.投影探索工具ToolStripMenuItem_Click);
            // 
            // map1
            // 
            this.map1.AllowDrop = true;
            this.map1.BackColor = System.Drawing.Color.White;
            this.map1.CollectAfterDraw = false;
            this.map1.CollisionDetection = false;
            this.map1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map1.ExtendBuffer = false;
            this.map1.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.map1.IsBusy = false;
            this.map1.IsZoomedToMaxExtent = false;
            this.map1.Legend = this.legend1;
            this.map1.Location = new System.Drawing.Point(200, 48);
            this.map1.Margin = new System.Windows.Forms.Padding(2);
            this.map1.Name = "map1";
            this.map1.ProgressHandler = null;
            this.map1.ProjectionModeDefine = DotSpatial.Controls.ActionMode.Prompt;
            this.map1.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Prompt;
            this.map1.RedrawLayersWhileResizing = false;
            this.map1.SelectionEnabled = true;
            this.map1.Size = new System.Drawing.Size(941, 447);
            this.map1.TabIndex = 3;
            this.map1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.map1_MouseDown);
            // 
            // legend1
            // 
            this.legend1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.legend1.ControlRectangle = new System.Drawing.Rectangle(0, 0, 200, 579);
            this.legend1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.legend1.DocumentRectangle = new System.Drawing.Rectangle(0, 0, 187, 428);
            this.legend1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.legend1.HorizontalScrollEnabled = true;
            this.legend1.Indentation = 30;
            this.legend1.IsInitialized = false;
            this.legend1.Location = new System.Drawing.Point(0, 0);
            this.legend1.Margin = new System.Windows.Forms.Padding(2);
            this.legend1.MinimumSize = new System.Drawing.Size(3, 3);
            this.legend1.Name = "legend1";
            this.legend1.ProgressHandler = null;
            this.legend1.ResetOnResize = false;
            this.legend1.SelectionFontColor = System.Drawing.Color.Black;
            this.legend1.SelectionHighlight = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.legend1.Size = new System.Drawing.Size(200, 579);
            this.legend1.TabIndex = 0;
            this.legend1.Text = "legend1";
            this.legend1.VerticalScrollEnabled = true;
            // 
            // k取消
            // 
            this.k取消.BackColor = System.Drawing.Color.White;
            this.k取消.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.k取消.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.k取消.Image = ((System.Drawing.Image)(resources.GetObject("k取消.Image")));
            this.k取消.Location = new System.Drawing.Point(200, 24);
            this.k取消.Margin = new System.Windows.Forms.Padding(2);
            this.k取消.Name = "k取消";
            this.k取消.Size = new System.Drawing.Size(24, 24);
            this.k取消.TabIndex = 8;
            this.k取消.UseVisualStyleBackColor = false;
            this.k取消.Click += new System.EventHandler(this.k取消_Click);
            // 
            // k缩放至图层
            // 
            this.k缩放至图层.BackColor = System.Drawing.Color.White;
            this.k缩放至图层.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.k缩放至图层.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.k缩放至图层.Image = ((System.Drawing.Image)(resources.GetObject("k缩放至图层.Image")));
            this.k缩放至图层.Location = new System.Drawing.Point(163, 24);
            this.k缩放至图层.Margin = new System.Windows.Forms.Padding(2);
            this.k缩放至图层.Name = "k缩放至图层";
            this.k缩放至图层.Size = new System.Drawing.Size(24, 24);
            this.k缩放至图层.TabIndex = 7;
            this.k缩放至图层.UseVisualStyleBackColor = false;
            this.k缩放至图层.Click += new System.EventHandler(this.k缩放至图层_Click);
            // 
            // k放大
            // 
            this.k放大.BackColor = System.Drawing.Color.White;
            this.k放大.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.k放大.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.k放大.Image = ((System.Drawing.Image)(resources.GetObject("k放大.Image")));
            this.k放大.Location = new System.Drawing.Point(13, 24);
            this.k放大.Margin = new System.Windows.Forms.Padding(2);
            this.k放大.Name = "k放大";
            this.k放大.Size = new System.Drawing.Size(24, 24);
            this.k放大.TabIndex = 6;
            this.k放大.UseVisualStyleBackColor = false;
            this.k放大.Click += new System.EventHandler(this.k放大_Click);
            // 
            // k缩小
            // 
            this.k缩小.BackColor = System.Drawing.Color.White;
            this.k缩小.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.k缩小.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.k缩小.Image = ((System.Drawing.Image)(resources.GetObject("k缩小.Image")));
            this.k缩小.Location = new System.Drawing.Point(51, 24);
            this.k缩小.Margin = new System.Windows.Forms.Padding(2);
            this.k缩小.Name = "k缩小";
            this.k缩小.Size = new System.Drawing.Size(24, 24);
            this.k缩小.TabIndex = 5;
            this.k缩小.UseVisualStyleBackColor = false;
            this.k缩小.Click += new System.EventHandler(this.k缩小_Click);
            // 
            // k选择
            // 
            this.k选择.BackColor = System.Drawing.Color.White;
            this.k选择.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.k选择.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.k选择.Image = ((System.Drawing.Image)(resources.GetObject("k选择.Image")));
            this.k选择.Location = new System.Drawing.Point(125, 24);
            this.k选择.Margin = new System.Windows.Forms.Padding(2);
            this.k选择.Name = "k选择";
            this.k选择.Size = new System.Drawing.Size(24, 24);
            this.k选择.TabIndex = 4;
            this.k选择.UseVisualStyleBackColor = false;
            this.k选择.Click += new System.EventHandler(this.k选择_Click);
            // 
            // k平移
            // 
            this.k平移.BackColor = System.Drawing.Color.White;
            this.k平移.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.k平移.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.k平移.Image = ((System.Drawing.Image)(resources.GetObject("k平移.Image")));
            this.k平移.Location = new System.Drawing.Point(88, 24);
            this.k平移.Margin = new System.Windows.Forms.Padding(2);
            this.k平移.Name = "k平移";
            this.k平移.Size = new System.Drawing.Size(24, 24);
            this.k平移.TabIndex = 3;
            this.k平移.UseVisualStyleBackColor = false;
            this.k平移.Click += new System.EventHandler(this.k平移_Click);
            // 
            // 山体阴影ToolStripMenuItem
            // 
            this.山体阴影ToolStripMenuItem.Name = "山体阴影ToolStripMenuItem";
            this.山体阴影ToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.山体阴影ToolStripMenuItem.Text = "山体阴影";
            this.山体阴影ToolStripMenuItem.Click += new System.EventHandler(this.山体阴影ToolStripMenuItem_Click);
            // 
            // 改变颜色方案ToolStripMenuItem
            // 
            this.改变颜色方案ToolStripMenuItem.Name = "改变颜色方案ToolStripMenuItem";
            this.改变颜色方案ToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.改变颜色方案ToolStripMenuItem.Text = "改变颜色方案";
            this.改变颜色方案ToolStripMenuItem.Click += new System.EventHandler(this.改变颜色方案ToolStripMenuItem_Click);
            // 
            // 栅格乘法x2ToolStripMenuItem
            // 
            this.栅格乘法x2ToolStripMenuItem.Name = "栅格乘法x2ToolStripMenuItem";
            this.栅格乘法x2ToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.栅格乘法x2ToolStripMenuItem.Text = "栅格乘法 (x2)";
            this.栅格乘法x2ToolStripMenuItem.Click += new System.EventHandler(this.栅格乘法x2ToolStripMenuItem_Click);
            // 
            // 栅格重分类ToolStripMenuItem
            // 
            this.栅格重分类ToolStripMenuItem.Name = "栅格重分类ToolStripMenuItem";
            this.栅格重分类ToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.栅格重分类ToolStripMenuItem.Text = "栅格重分类";
            this.栅格重分类ToolStripMenuItem.Click += new System.EventHandler(this.栅格重分类ToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblRasterValue);
            this.groupBox1.Controls.Add(this.chbRasterValue);
            this.groupBox1.Controls.Add(this.txtElevation);
            this.groupBox1.Location = new System.Drawing.Point(0, 476);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 103);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "栅格工具参数调整";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // txtElevation
            // 
            this.txtElevation.Location = new System.Drawing.Point(51, 20);
            this.txtElevation.Name = "txtElevation";
            this.txtElevation.Size = new System.Drawing.Size(100, 21);
            this.txtElevation.TabIndex = 0;
            this.txtElevation.Text = "3000";
            this.txtElevation.TextChanged += new System.EventHandler(this.txtElevation_TextChanged);
            // 
            // chbRasterValue
            // 
            this.chbRasterValue.AutoSize = true;
            this.chbRasterValue.Location = new System.Drawing.Point(13, 62);
            this.chbRasterValue.Name = "chbRasterValue";
            this.chbRasterValue.Size = new System.Drawing.Size(84, 16);
            this.chbRasterValue.TabIndex = 1;
            this.chbRasterValue.Text = "查询栅格值";
            this.chbRasterValue.UseVisualStyleBackColor = true;
            // 
            // lblRasterValue
            // 
            this.lblRasterValue.AutoSize = true;
            this.lblRasterValue.Location = new System.Drawing.Point(103, 62);
            this.lblRasterValue.Name = "lblRasterValue";
            this.lblRasterValue.Size = new System.Drawing.Size(0, 12);
            this.lblRasterValue.TabIndex = 2;
            this.lblRasterValue.Click += new System.EventHandler(this.label1_Click);
            // 
            // gis软件
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1141, 627);
            this.Controls.Add(this.map1);
            this.Controls.Add(this.pnl属性);
            this.Controls.Add(this.pnl图层);
            this.Controls.Add(this.pnl菜单);
            this.Controls.Add(this.pnl地图);
            this.MainMenuStrip = this.menu菜单;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "gis软件";
            this.Text = "gis软件";
            this.pnl菜单.ResumeLayout(false);
            this.pnl菜单.PerformLayout();
            this.menu菜单.ResumeLayout(false);
            this.menu菜单.PerformLayout();
            this.pnl图层.ResumeLayout(false);
            this.pnl属性.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.属性表)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl菜单;
        private System.Windows.Forms.MenuStrip menu菜单;
        private System.Windows.Forms.Panel pnl图层;
        private System.Windows.Forms.Panel pnl属性;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 视图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 矢量图形ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 栅格图像ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 地图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 放大ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 缩小ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 平移ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 还原ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 点对象ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 线对象ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 面对象ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加线ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 保存面ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 属性表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看属性表ToolStripMenuItem;
        private DotSpatial.Controls.Map map1;
        private System.Windows.Forms.Panel pnl地图;
        private DotSpatial.Controls.Legend legend1;
        private System.Windows.Forms.Button k平移;
        private System.Windows.Forms.Button k放大;
        private System.Windows.Forms.Button k缩小;
        private System.Windows.Forms.Button k选择;
        private System.Windows.Forms.Button k缩放至图层;
        private System.Windows.Forms.Button k取消;
        private System.Windows.Forms.ToolStripMenuItem 取消操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 栅格工具ToolStripMenuItem;
        private System.Windows.Forms.DataGridView 属性表;
        private System.Windows.Forms.ToolStripMenuItem 快捷查看ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制徒步路径ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 计算徒步路径ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 投影探索工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 山体阴影ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 改变颜色方案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 栅格乘法x2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 栅格重分类ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtElevation;
        private System.Windows.Forms.Label lblRasterValue;
        private System.Windows.Forms.CheckBox chbRasterValue;
    }
}

