using DotSpatial.Controls;
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
    public partial class Form2: Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private readonly gis软件 _gisForm;

        // 重构构造函数，接收 gis软件 实例
        public Form2(gis软件 gisForm)
        {
            InitializeComponent();
            // 校验实例不为空
            _gisForm = gisForm ?? throw new ArgumentNullException(nameof(gisForm));
        }

        private void b显示_Click(object sender, EventArgs e)
        {
            DataTable dt = null;

            // 通过公共属性 Map1 访问 map1
            if (_gisForm.Map1.Layers.Count > 0)
            {
                // 注意：原代码直接强转 Layers[0] 为 MapPolygonLayer 有风险，增加类型校验
                if (_gisForm.Map1.Layers[0] is MapPolygonLayer stateLayer)
                {
                    dt = stateLayer.DataSet.DataTable;
                    属性表2.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("第一个图层不是面图层！");
                }
            }
            else
            {
                MessageBox.Show("请向地图添加图层。");
            }
        }

        private void b图层_Click(object sender, EventArgs e)
        {
            // 1. 基础检查：地图是否有图层
            if (_gisForm.Map1.Layers.Count > 0)
            {
                // 2. 获取并验证输入
                string attributeName = textBox1.Text.Trim();
                if (string.IsNullOrEmpty(attributeName))
                {
                    MessageBox.Show("请输入要显示的属性名！");
                    return;
                }

                // 3. 安全获取图层（修复 "stateLayer" 声明前无法使用的问题）
                // 使用 'as' 关键字将图层转换为面图层，如果转换失败则 stateLayer 为 null
                MapPolygonLayer stateLayer = _gisForm.Map1.Layers[0] as MapPolygonLayer;

                // 4. 验证图层类型
                if (stateLayer != null)
                {
                    // 5. 检查属性表中是否存在该字段
                    if (stateLayer.DataSet.DataTable.Columns.Contains(attributeName))
                    {
                        // 格式化属性名为标签表达式，例如 "[Name]"
                        string labelExpression = string.Format("[{0}]", attributeName);

                        // 6. 添加标签（关键修改：使用您定义的 _currentFont 和 _currentColor 变量）
                        _gisForm.Map1.AddLabels(stateLayer, labelExpression, _currentFont, _currentColor);

                        MessageBox.Show("已显示 [" + attributeName + "] 标签");
                    }
                    else
                    {
                        MessageBox.Show("属性表中不存在 [" + attributeName + "] 字段！");
                    }
                }
                else
                {
                    MessageBox.Show("第一个图层不是面图层（Polygon Layer），无法显示标签。");
                }
            }
            else
            {
                MessageBox.Show("请先向地图添加图层。");
            }
        }

        private void b选中_Click(object sender, EventArgs e)
        {
            // 检查地图控件中的图层数量
            if (_gisForm.Map1.Layers.Count == 0)
            {
                MessageBox.Show("请向地图添加图层。");
                return;
            }

            // 获取文本框输入的属性名
            string attributeName = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(attributeName))
            {
                MessageBox.Show("请输入要选中的属性名！");
                return;
            }

            // 验证图层类型
            if (!(_gisForm.Map1.Layers[0] is MapPolygonLayer stateLayer))
            {
                MessageBox.Show("第一个图层不是面图层！");
                return;
            }

            // 检查属性是否存在
            if (!stateLayer.DataSet.DataTable.Columns.Contains(attributeName))
            {
                MessageBox.Show($"属性表中不存在 [{attributeName}] 字段！");
                return;
            }

            // 选中属性列
            foreach (DataGridViewColumn column in 属性表2.Columns)
            {
                column.Selected = column.Name == attributeName;
            }

            MessageBox.Show($"已选中 [{attributeName}] 属性列");
        }

        private void b添加_Click(object sender, EventArgs e)
        {
            // 检查地图控件中的图层数量
            if (_gisForm.Map1.Layers.Count == 0)
            {
                MessageBox.Show("请向地图添加图层。");
                return;
            }

            // 获取文本框输入的属性名
            string newColumnName = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(newColumnName))
            {
                MessageBox.Show("请输入要添加的属性名！");
                return;
            }

            // 验证图层类型
            if (!(_gisForm.Map1.Layers[0] is MapPolygonLayer stateLayer))
            {
                MessageBox.Show("第一个图层不是面图层！");
                return;
            }

            // 检查属性是否已存在
            if (stateLayer.DataSet.DataTable.Columns.Contains(newColumnName))
            {
                MessageBox.Show($"属性表中已存在 [{newColumnName}] 字段！");
                return;
            }

            // 添加新属性列
            DataColumn newColumn = new DataColumn(newColumnName, typeof(string));
            stateLayer.DataSet.DataTable.Columns.Add(newColumn);

            // 更新属性表显示
            属性表2.DataSource = null;
            属性表2.DataSource = stateLayer.DataSet.DataTable;

            MessageBox.Show($"已添加 [{newColumnName}] 属性列");
        }

        private void b删除_Click(object sender, EventArgs e)
        {
            // 检查地图控件中的图层数量
            if (_gisForm.Map1.Layers.Count == 0)
            {
                MessageBox.Show("请向地图添加图层。");
                return;
            }

            // 获取文本框输入的属性名
            string attributeName = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(attributeName))
            {
                MessageBox.Show("请输入要删除的属性名！");
                return;
            }

            // 验证图层类型
            if (!(_gisForm.Map1.Layers[0] is MapPolygonLayer stateLayer))
            {
                MessageBox.Show("第一个图层不是面图层！");
                return;
            }

            // 检查属性是否存在
            if (!stateLayer.DataSet.DataTable.Columns.Contains(attributeName))
            {
                MessageBox.Show($"属性表中不存在 [{attributeName}] 字段！");
                return;
            }

            // 防止删除关键ID字段
            if (attributeName == "PolygonID")
            {
                MessageBox.Show("不能删除系统自带的ID字段！");
                return;
            }

            // 删除属性列
            stateLayer.DataSet.DataTable.Columns.Remove(attributeName);

            // 更新属性表显示
            属性表2.DataSource = null;
            属性表2.DataSource = stateLayer.DataSet.DataTable;

            MessageBox.Show($"已删除 [{attributeName}] 属性列");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            属性表2.DataSource = null;
            属性表2.Rows.Clear();
            // 检查地图控件中的图层数量
            if (_gisForm.Map1.Layers.Count > 0)
            {
                // 尝试获取第一个图层并验证是否为面图层
                if (_gisForm.Map1.Layers[0] is MapPolygonLayer stateLayer)
                {
                    // 清除该图层的所有标签
                    _gisForm.Map1.ClearLabels(stateLayer);
                    MessageBox.Show("已清除属性标签");
                }
                else
                {
                    MessageBox.Show("第一个图层不是面图层！");
                }
            }
            else
            {
                MessageBox.Show("请向地图添加图层。");
            }
        }
        private Font _currentFont = new Font("Tahoma", 8f);
        private Color _currentColor = Color.Black;
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

        // 3. 修改现有的显示标签方法

    }
}
