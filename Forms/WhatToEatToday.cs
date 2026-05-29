using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QisToolkit3.Forms
{
    public partial class WhatToEatToday : Form
    {
        private FoodItem[] foodItem;
        private string IdURL = string.Empty;


        public WhatToEatToday()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void WhatToEatToday_Load(object sender, EventArgs e)
        {
            comboBox_RamType.Items.Clear();

            foreach (string item in Directory.GetFiles(@"Datas\Foods"))
                comboBox_RamType.Items.Add(Path.ChangeExtension(Path.GetFileName(item), null));
        }

        private struct FoodItem
        {
            public string Name;             // 名称
            public string Type;             // 菜品类型
            public string[] RawMaterials;   // 原材料数组
            public string Url;              // 菜品链接URL

            public override string ToString()
            {
                return $"菜品: {Name}\n类型: {Type}\n原材料: {string.Join(", ", RawMaterials)}\n链接: {Url}\n";
            }
        }

        // 解析食品数据文件
        private static FoodItem[] ParseFoodData(string filePath)
        {
            // 检查文件是否存在
            if (!File.Exists(filePath))
                throw new FileNotFoundException("数据文件不存在", filePath);

            // 读取所有行
            string[] allLines = File.ReadAllLines(filePath);
            List<FoodItem> foodList = new List<FoodItem>();

            for (int i = 0; i < allLines.Length; i++)
            {
                string line = allLines[i].Trim();

                // 跳过空行和注释行
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("#") || line.StartsWith("//")) continue;

                try
                {
                    // 拆分数据行
                    string[] parts = line.Split('|');

                    // 验证数据格式
                    if (parts.Length < 4)
                        throw new FormatException($"数据格式错误，应有4个部分但找到{parts.Length}");

                    // 创建食品条目
                    FoodItem item = new FoodItem
                    {
                        Name = parts[0].Trim(),
                        Type = parts[1].Trim(),
                        RawMaterials = ParseRawMaterials(parts[2]),
                        Url = parts[3].Trim()
                    };

                    // 验证URL格式
                    if (!item.Url.StartsWith("http://") && !item.Url.StartsWith("https://"))
                        throw new UriFormatException("URL格式无效");

                    foodList.Add(item);
                }
                catch (Exception ex)
                {
                    // 添加行号信息到异常
                    throw new InvalidDataException($"第 {i + 1} 行处理失败: {ex.Message}", ex);
                }
            }

            return foodList.ToArray();
        }

        // 解析原材料字符串
        private static string[] ParseRawMaterials(string materialsString)
        {
            // 分割原材料并清理空白
            string[] materials = materialsString.Split(
                new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries
            );

            // 移除每个原材料的前后空格
            for (int i = 0; i < materials.Length; i++)
                materials[i] = materials[i].Trim();

            return materials;
        }

        private void button_DoRam_Click(object sender, EventArgs e)
        {
            if (foodItem != null)
            {
                Random random = new Random();
                int id = random.Next(0, foodItem.Length - 1);
                label_FoodName.Text = foodItem[id].Name;
                IdURL = foodItem[id].Url;
            }

            label_FoodName.Visible = true;
        }

        private void button_Url_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo(IdURL) { UseShellExecute = true });
        }

        private void comboBox_RamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DataPath = @$"Datas\Foods\{comboBox_RamType.Text}.txt";
            if (File.Exists(DataPath))
            {
                foodItem = ParseFoodData(DataPath);
            }

            else
            {
                MessageBox.Show("菜单列表未找到", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
