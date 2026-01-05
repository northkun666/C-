using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

// 1. 引入 Models 命名空间，不再使用 Form1 内部的定义
using 集成.Models;

namespace 集成
{
    public partial class Form3 : Form
    {
        // 2. 这里的 List<PathPoint> 现在会自动指向 集成.Models.PathPoint
        public Form3(List<PathPoint> pathList)
        {
            InitializeComponent();

            // 3. 数据校验，防止空列表报错
            if (pathList == null || pathList.Count == 0)
            {
                MessageBox.Show("没有路径数据可显示。");
                return;
            }

            // 创建距离和高程数组
            double[] distanceArray = new double[pathList.Count];
            double[] elevationArray = new double[pathList.Count];

            for (int i = 0; i <= pathList.Count - 1; i++)
            {
                distanceArray[i] = pathList[i].Distance;
                elevationArray[i] = pathList[i].Elevation;
            }

            // 配置 ZedGraph
            zedGraphControl1.GraphPane.CurveList.Clear();
            ZedGraph.LineItem myCurve = zedGraphControl1.GraphPane.AddCurve("高程剖面图（Elevation Profile）", distanceArray, elevationArray, Color.Blue);

            myCurve.Line.Width = 2f;
            myCurve.Symbol.Type = ZedGraph.SymbolType.None;
            myCurve.Line.Fill.Color = Color.FromArgb(100, 0, 0, 255);
            myCurve.Line.Fill.IsVisible = true;

            zedGraphControl1.GraphPane.XAxis.Title.Text = "距离（米）";
            zedGraphControl1.GraphPane.YAxis.Title.Text = "高程（米）";
            zedGraphControl1.GraphPane.Title.Text = "徒步路径剖面分析";

            // 刷新图表
            zedGraphControl1.AxisChange();
            zedGraphControl1.Refresh();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}