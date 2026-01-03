using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using static 集成.gis软件;

namespace 集成
{
    public partial class Form3: Form
    {
        public Form3(List<PathPoint> pathList)
        {
            InitializeComponent();
            // 创建距离和高程数组
            double[] distanceArray = new double[pathList.Count];
            double[] elevationArray = new double[pathList.Count];
            for (int i = 0; i <= pathList.Count - 1; i++)
            {
                distanceArray[i] = pathList[i].Distance;
                elevationArray[i] = pathList[i].Elevation;
            }
            zedGraphControl1.GraphPane.CurveList.Clear();
            ZedGraph.LineItem myCurve = zedGraphControl1.GraphPane.AddCurve("高程剖面图（Elevation Profile）", distanceArray, elevationArray, Color.Blue);
            myCurve.Line.Width = 2f;
            myCurve.Symbol.Type = ZedGraph.SymbolType.None;
            myCurve.Line.Fill.Color = Color.LightBlue;
            myCurve.Line.Fill.Color = Color.FromArgb(100, 0, 0, 255);
            myCurve.Line.Fill.IsVisible = true;
            zedGraphControl1.GraphPane.XAxis.Title.Text = "距离（米）（Distance (meters)）";
            zedGraphControl1.GraphPane.YAxis.Title.Text = "高程（米）（Elevation (meters)）";
            // 刷新图表
            zedGraphControl1.AxisChange();
            // 设置图表标题
            zedGraphControl1.GraphPane.Title.Text = "徒步路径图表（Hiking Path Graph）";
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
