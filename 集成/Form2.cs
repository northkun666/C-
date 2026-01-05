using DotSpatial.Controls;
using DotSpatial.Symbology; // 必须引用
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing; // 必须引用 (Font, Color)
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 集成
{
    public partial class Form2 : Form
    {
        private readonly gis软件 _gisForm;

        // 默认字体和颜色
        private Font _currentFont = new Font("Tahoma", 8f);
        private Color _currentColor = Color.Black;

        public Form2(gis软件 gisForm)
        {
            InitializeComponent();
            _gisForm = gisForm ?? throw new ArgumentNullException(nameof(gisForm));
        }

        // 辅助方法：获取目标图层
        private MapPolygonLayer GetTargetLayer()
        {
            if (_gisForm.Map1.Layers.SelectedLayer is MapPolygonLayer selectedLayer)
            {
                return selectedLayer;
            }
            foreach (var layer in _gisForm.Map1.Layers)
            {
                if (layer is MapPolygonLayer polygonLayer)
                {
                    return polygonLayer;
                }
            }
            return null;
        }

        private void b显示_Click(object sender, EventArgs e)
        {
            var layer = GetTargetLayer();
            if (layer != null)
                属性表2.DataSource = layer.DataSet.DataTable;
            else
                MessageBox.Show("请先在地图中添加或选择一个面图层（Polygon Layer）。");
        }

        // ★★★ 重点修复：手动创建 MapLabelLayer ★★★
        private void b图层_Click(object sender, EventArgs e)
        {
            var layer = GetTargetLayer();
            if (layer == null)
            {
                MessageBox.Show("未找到面图层。");
                return;
            }

            string attributeName = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(attributeName))
            {
                MessageBox.Show("请输入属性名！");
                return;
            }

            if (layer.DataSet.DataTable.Columns.Contains(attributeName))
            {
                string labelExpression = string.Format("[{0}]", attributeName);

                // 1. 先清除旧的标签，防止重叠
                _gisForm.Map1.ClearLabels(layer);

                // 2. 手动创建标签图层 (避开 AddLabels 的参数冲突 bug)
                MapLabelLayer labelLayer = new MapLabelLayer(layer);

                // 3. 设置表达式
                ILabelCategory category = labelLayer.Symbology.Categories[0];
                category.Expression = labelExpression;

                // 4. 设置字体和颜色
                category.Symbolizer.FontFamily = _currentFont.FontFamily.Name;
                category.Symbolizer.FontSize = _currentFont.Size;
                category.Symbolizer.FontStyle = _currentFont.Style;
                category.Symbolizer.FontColor = _currentColor;
                category.Symbolizer.Orientation = ContentAlignment.MiddleCenter;

                // 5. 添加到地图
                _gisForm.Map1.Layers.Add(labelLayer);

                MessageBox.Show($"已显示 [{attributeName}] 标签");
            }
            else
            {
                MessageBox.Show($"属性表中不存在 [{attributeName}] 字段！");
            }
        }

        private void b选中_Click(object sender, EventArgs e)
        {
            string attributeName = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(attributeName)) return;

            if (属性表2.DataSource != null)
            {
                if (属性表2.Columns.Contains(attributeName))
                {
                    属性表2.ClearSelection();
                    属性表2.Columns[attributeName].Selected = true;
                }
                else
                {
                    MessageBox.Show("表中无此列。");
                }
            }
        }

        private void b添加_Click(object sender, EventArgs e)
        {
            var layer = GetTargetLayer();
            if (layer == null) return;

            string newColumnName = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(newColumnName))
            {
                MessageBox.Show("请输入新属性名。");
                return;
            }

            if (!layer.DataSet.DataTable.Columns.Contains(newColumnName))
            {
                layer.DataSet.DataTable.Columns.Add(new DataColumn(newColumnName, typeof(string)));
                属性表2.DataSource = null;
                属性表2.DataSource = layer.DataSet.DataTable;
                MessageBox.Show($"已添加列: {newColumnName}");
            }
            else
            {
                MessageBox.Show("该列已存在。");
            }
        }

        private void b删除_Click(object sender, EventArgs e)
        {
            var layer = GetTargetLayer();
            if (layer == null) return;

            string attributeName = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(attributeName)) return;

            if (attributeName == "PolygonID" || attributeName == "FID")
            {
                MessageBox.Show("系统关键字段不可删除。");
                return;
            }

            if (layer.DataSet.DataTable.Columns.Contains(attributeName))
            {
                layer.DataSet.DataTable.Columns.Remove(attributeName);
                属性表2.DataSource = null;
                属性表2.DataSource = layer.DataSet.DataTable;
                MessageBox.Show($"已删除列: {attributeName}");
            }
            else
            {
                MessageBox.Show("找不到该列。");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var layer = GetTargetLayer();
            if (layer != null)
            {
                _gisForm.Map1.ClearLabels(layer);
                MessageBox.Show("已清除标签。");
            }
        }

        private void btnSetFont_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
                _currentFont = fontDialog1.Font;
        }

        private void btnSetColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                _currentColor = colorDialog1.Color;
        }
    }
}