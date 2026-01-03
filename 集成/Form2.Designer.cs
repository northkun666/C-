namespace 集成
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnl菜单2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.b删除 = new System.Windows.Forms.Button();
            this.b添加 = new System.Windows.Forms.Button();
            this.b图层 = new System.Windows.Forms.Button();
            this.b选中 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.b显示 = new System.Windows.Forms.Button();
            this.pnl属性表 = new System.Windows.Forms.Panel();
            this.属性表2 = new System.Windows.Forms.DataGridView();
            this.btnSetFont = new System.Windows.Forms.Button();
            this.btnSetColor = new System.Windows.Forms.Button();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.pnl菜单2.SuspendLayout();
            this.pnl属性表.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.属性表2)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl菜单2
            // 
            this.pnl菜单2.Controls.Add(this.btnSetColor);
            this.pnl菜单2.Controls.Add(this.btnSetFont);
            this.pnl菜单2.Controls.Add(this.button1);
            this.pnl菜单2.Controls.Add(this.b删除);
            this.pnl菜单2.Controls.Add(this.b添加);
            this.pnl菜单2.Controls.Add(this.b图层);
            this.pnl菜单2.Controls.Add(this.b选中);
            this.pnl菜单2.Controls.Add(this.textBox1);
            this.pnl菜单2.Controls.Add(this.b显示);
            this.pnl菜单2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl菜单2.Location = new System.Drawing.Point(0, 0);
            this.pnl菜单2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnl菜单2.Name = "pnl菜单2";
            this.pnl菜单2.Size = new System.Drawing.Size(678, 82);
            this.pnl菜单2.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(609, 9);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 24);
            this.button1.TabIndex = 6;
            this.button1.Text = "取消显示";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // b删除
            // 
            this.b删除.Location = new System.Drawing.Point(409, 9);
            this.b删除.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.b删除.Name = "b删除";
            this.b删除.Size = new System.Drawing.Size(91, 24);
            this.b删除.TabIndex = 5;
            this.b删除.Text = "删除属性";
            this.b删除.UseVisualStyleBackColor = true;
            this.b删除.Click += new System.EventHandler(this.b删除_Click);
            // 
            // b添加
            // 
            this.b添加.Location = new System.Drawing.Point(309, 9);
            this.b添加.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.b添加.Name = "b添加";
            this.b添加.Size = new System.Drawing.Size(91, 24);
            this.b添加.TabIndex = 4;
            this.b添加.Text = "添加属性";
            this.b添加.UseVisualStyleBackColor = true;
            this.b添加.Click += new System.EventHandler(this.b添加_Click);
            // 
            // b图层
            // 
            this.b图层.Location = new System.Drawing.Point(509, 9);
            this.b图层.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.b图层.Name = "b图层";
            this.b图层.Size = new System.Drawing.Size(91, 24);
            this.b图层.TabIndex = 3;
            this.b图层.Text = "显示在图层";
            this.b图层.UseVisualStyleBackColor = true;
            this.b图层.Click += new System.EventHandler(this.b图层_Click);
            // 
            // b选中
            // 
            this.b选中.Location = new System.Drawing.Point(209, 9);
            this.b选中.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.b选中.Name = "b选中";
            this.b选中.Size = new System.Drawing.Size(91, 24);
            this.b选中.TabIndex = 2;
            this.b选中.Text = "选中属性";
            this.b选中.UseVisualStyleBackColor = true;
            this.b选中.Click += new System.EventHandler(this.b选中_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(109, 9);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(92, 23);
            this.textBox1.TabIndex = 1;
            // 
            // b显示
            // 
            this.b显示.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.b显示.Location = new System.Drawing.Point(9, 9);
            this.b显示.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.b显示.Name = "b显示";
            this.b显示.Size = new System.Drawing.Size(91, 24);
            this.b显示.TabIndex = 0;
            this.b显示.Text = "显示属性表";
            this.b显示.UseVisualStyleBackColor = true;
            this.b显示.Click += new System.EventHandler(this.b显示_Click);
            // 
            // pnl属性表
            // 
            this.pnl属性表.Controls.Add(this.属性表2);
            this.pnl属性表.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl属性表.Location = new System.Drawing.Point(0, 82);
            this.pnl属性表.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnl属性表.Name = "pnl属性表";
            this.pnl属性表.Size = new System.Drawing.Size(678, 287);
            this.pnl属性表.TabIndex = 2;
            // 
            // 属性表2
            // 
            this.属性表2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.属性表2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.属性表2.Location = new System.Drawing.Point(0, 0);
            this.属性表2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.属性表2.Name = "属性表2";
            this.属性表2.RowHeadersWidth = 62;
            this.属性表2.RowTemplate.Height = 30;
            this.属性表2.Size = new System.Drawing.Size(678, 287);
            this.属性表2.TabIndex = 0;
            // 
            // btnSetFont
            // 
            this.btnSetFont.Location = new System.Drawing.Point(9, 38);
            this.btnSetFont.Name = "btnSetFont";
            this.btnSetFont.Size = new System.Drawing.Size(91, 23);
            this.btnSetFont.TabIndex = 7;
            this.btnSetFont.Text = "设置字体";
            this.btnSetFont.UseVisualStyleBackColor = true;
            this.btnSetFont.Click += new System.EventHandler(this.btnSetFont_Click);
            // 
            // btnSetColor
            // 
            this.btnSetColor.Location = new System.Drawing.Point(106, 38);
            this.btnSetColor.Name = "btnSetColor";
            this.btnSetColor.Size = new System.Drawing.Size(92, 23);
            this.btnSetColor.TabIndex = 8;
            this.btnSetColor.Text = "设置颜色";
            this.btnSetColor.UseVisualStyleBackColor = true;
            this.btnSetColor.Click += new System.EventHandler(this.btnSetColor_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 369);
            this.Controls.Add(this.pnl属性表);
            this.Controls.Add(this.pnl菜单2);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form2";
            this.Text = "属性表工具";
            this.pnl菜单2.ResumeLayout(false);
            this.pnl菜单2.PerformLayout();
            this.pnl属性表.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.属性表2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl菜单2;
        private System.Windows.Forms.Panel pnl属性表;
        private System.Windows.Forms.Button b显示;
        private System.Windows.Forms.Button b删除;
        private System.Windows.Forms.Button b添加;
        private System.Windows.Forms.Button b图层;
        private System.Windows.Forms.Button b选中;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView 属性表2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSetColor;
        private System.Windows.Forms.Button btnSetFont;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}