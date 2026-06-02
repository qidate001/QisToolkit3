using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QisToolkit3.Forms.YtDlp
{
    // 控件配置项
    public class YtDlpControlConfigItem
    {
        public string Name { get; set; }           // 控件名称
        public string ControlType { get; set; }     // 控件类型
        public string Value { get; set; }           // 主要值

        // ComboBox专用字段
        public int SelectedIndex { get; set; } = -1; // 选中项的索引
        public string SelectedText { get; set; }     // 选中的文本

        // CheckedListBox专用
        public List<string> CheckedItems { get; set; }

        // 元数据
        public string Description { get; set; }      // 说明
        public bool IsImportant { get; set; }        // 是否重要配置
    }

    // 配置类
    public class YtDlpFormConfig
    {
        public List<YtDlpControlConfigItem> Controls { get; set; } = new List<YtDlpControlConfigItem>();
        public DateTime SaveTime { get; set; }
        public string FormName { get; set; }
        public string Version { get; set; } = "1.0";
    }

    // 配置管理类
    public class YtDlpConfigManager
    {
        // 导出配置（完整版）
        public static void ExportConfig(Form form, string filePath)
        {
            var config = new YtDlpFormConfig
            {
                SaveTime = DateTime.Now,
                FormName = form.Name,
                Version = "1.0"
            };

            // 获取所有控件
            var allControls = GetAllControls(form);

            foreach (Control control in allControls)
            {
                // 跳过没有名称的控件（如ToolTip等辅助控件）
                if (string.IsNullOrEmpty(control.Name)) continue;

                var configItem = new YtDlpControlConfigItem
                {
                    Name = control.Name,
                    ControlType = control.GetType().Name,
                    Description = control.Tag?.ToString() ?? "" // 利用Tag属性存储描述
                };

                // 根据控件类型处理
                switch (control)
                {
                    case CheckBox checkBox:
                        configItem.Value = checkBox.Checked.ToString();
                        configItem.IsImportant = checkBox.Checked;
                        break;

                    case TextBox textBox:
                        configItem.Value = textBox.Text;
                        configItem.IsImportant = !string.IsNullOrEmpty(textBox.Text);
                        break;

                    case ComboBox comboBox:
                        HandleComboBox(comboBox, configItem);
                        break;

                    case RadioButton radioButton:
                        if (radioButton.Checked)
                        {
                            configItem.Value = radioButton.Text;
                        }
                        break;

                    case NumericUpDown numericUpDown:
                        configItem.Value = numericUpDown.Value.ToString();
                        break;

                    case DateTimePicker dateTimePicker:
                        configItem.Value = dateTimePicker.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        break;

                    case RichTextBox richTextBox:
                        configItem.Value = richTextBox.Text;
                        break;

                    case MaskedTextBox maskedTextBox:
                        configItem.Value = maskedTextBox.Text;
                        break;

                    case CheckedListBox checkedListBox:
                        configItem.CheckedItems = new List<string>();
                        for (int i = 0; i < checkedListBox.Items.Count; i++)
                        {
                            if (checkedListBox.GetItemChecked(i))
                                configItem.CheckedItems.Add(checkedListBox.Items[i].ToString());
                        }
                        break;
                }

                config.Controls.Add(configItem);
            }

            // 保存到文件
            SaveConfigToFile(config, filePath);
        }

        // 专门处理ComboBox的保存逻辑
        private static void HandleComboBox(ComboBox comboBox, YtDlpControlConfigItem configItem)
        {
            if (comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
            {
                // 下拉选择模式：保存选中索引和选中文本
                configItem.SelectedIndex = comboBox.SelectedIndex;
                configItem.SelectedText = comboBox.Text;
                configItem.Value = comboBox.SelectedIndex >= 0 ?
                    comboBox.SelectedItem?.ToString() : "";

                // 添加标记便于识别
                configItem.Description = "DropdownList模式";
            }
            else
            {
                // 可输入模式：主要保存用户输入的文本
                configItem.Value = comboBox.Text;
                configItem.Description = "Editable模式";

                // 如果有选中项，也保存索引（可选）
                if (comboBox.SelectedIndex >= 0)
                {
                    configItem.SelectedIndex = comboBox.SelectedIndex;
                }
            }
        }

        // 导入配置
        public static void ImportConfig(Form form, string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"配置文件不存在: {filePath}");

            // 读取配置
            var config = LoadConfigFromFile(filePath);
            if (config == null) return;

            // 应用配置
            foreach (var configItem in config.Controls)
            {
                var control = FindControl(form, configItem.Name);
                if (control == null) continue;

                switch (control)
                {
                    case CheckBox checkBox:
                        if (bool.TryParse(configItem.Value, out bool checkedValue))
                            checkBox.Checked = checkedValue;
                        break;

                    case TextBox textBox:
                        textBox.Text = configItem.Value;
                        break;

                    case ComboBox comboBox:
                        LoadComboBoxConfig(comboBox, configItem);
                        break;

                    case RadioButton radioButton:
                        // 根据保存的值选中对应的RadioButton
                        if (radioButton.Text == configItem.Value)
                            radioButton.Checked = true;
                        break;

                    case NumericUpDown numericUpDown:
                        if (decimal.TryParse(configItem.Value, out decimal numValue))
                            numericUpDown.Value = numValue;
                        break;

                    case DateTimePicker dateTimePicker:
                        if (DateTime.TryParse(configItem.Value, out DateTime dateValue))
                            dateTimePicker.Value = dateValue;
                        break;

                    case RichTextBox richTextBox:
                        richTextBox.Text = configItem.Value;
                        break;

                    case MaskedTextBox maskedTextBox:
                        maskedTextBox.Text = configItem.Value;
                        break;

                    case CheckedListBox checkedListBox:
                        if (configItem.CheckedItems != null)
                        {
                            for (int i = 0; i < checkedListBox.Items.Count; i++)
                            {
                                string itemText = checkedListBox.Items[i].ToString();
                                checkedListBox.SetItemChecked(i, configItem.CheckedItems.Contains(itemText));
                            }
                        }
                        break;
                }
            }
        }

        // 专门处理ComboBox的加载逻辑
        private static void LoadComboBoxConfig(ComboBox comboBox, YtDlpControlConfigItem configItem)
        {
            if (comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
            {
                // 下拉选择模式：优先使用索引恢复
                if (configItem.SelectedIndex >= 0 &&
                    configItem.SelectedIndex < comboBox.Items.Count)
                {
                    comboBox.SelectedIndex = configItem.SelectedIndex;
                }
                else if (!string.IsNullOrEmpty(configItem.SelectedText))
                {
                    // 如果索引无效，尝试通过文本查找
                    int index = comboBox.FindStringExact(configItem.SelectedText);
                    if (index >= 0) comboBox.SelectedIndex = index;
                }
            }
            else
            {
                // 可输入模式：直接恢复文本
                comboBox.Text = configItem.Value;

                // 可选：如果保存了选中索引且有效，也恢复选中状态
                if (configItem.SelectedIndex >= 0 &&
                    configItem.SelectedIndex < comboBox.Items.Count)
                {
                    comboBox.SelectedIndex = configItem.SelectedIndex;
                }
            }
        }

        // 获取所有控件（递归遍历）
        private static List<Control> GetAllControls(Control parent)
        {
            var controls = new List<Control>();

            foreach (Control control in parent.Controls)
            {
                controls.Add(control);

                // 递归获取子控件（如GroupBox、Panel内的控件）
                if (control.HasChildren)
                {
                    controls.AddRange(GetAllControls(control));
                }
            }

            return controls;
        }

        // 查找指定名称的控件（递归）
        private static Control FindControl(Control parent, string name)
        {
            if (parent.Name == name)
                return parent;

            foreach (Control control in parent.Controls)
            {
                var result = FindControl(control, name);
                if (result != null)
                    return result;
            }

            return null;
        }

        // 保存配置到文件
        private static void SaveConfigToFile(YtDlpFormConfig config, string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(YtDlpFormConfig));
                using (var writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, config);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"保存配置文件失败: {ex.Message}", ex);
            }
        }

        // 从文件加载配置
        private static YtDlpFormConfig LoadConfigFromFile(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(YtDlpFormConfig));
                using (var reader = new StreamReader(filePath))
                {
                    return (YtDlpFormConfig)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"加载配置文件失败: {ex.Message}", ex);
            }
        }

        // ========== 扩展功能 ==========

        // 只保存重要的控件（通过Tag标记）
        public static void ExportImportantControlsOnly(Form form, string filePath)
        {
            var config = new YtDlpFormConfig
            {
                SaveTime = DateTime.Now,
                FormName = form.Name,
                Version = "1.0"
            };

            var allControls = GetAllControls(form);

            foreach (var control in allControls)
            {
                // 只保存有名称且Tag属性标记为"Important"的控件
                if (!string.IsNullOrEmpty(control.Name) &&
                    control.Tag?.ToString() == "Important")
                {
                    var configItem = CreateConfigItem(control);
                    if (configItem != null)
                        config.Controls.Add(configItem);
                }
            }

            SaveConfigToFile(config, filePath);
        }

        // 保存指定的控件列表
        public static void ExportSpecificControls(Form form, string filePath,
            params string[] controlNames)
        {
            var config = new YtDlpFormConfig
            {
                SaveTime = DateTime.Now,
                FormName = form.Name,
                Version = "1.0"
            };

            var targetNames = new HashSet<string>(controlNames);

            foreach (var control in GetAllControls(form))
            {
                if (targetNames.Contains(control.Name))
                {
                    var configItem = CreateConfigItem(control);
                    if (configItem != null)
                        config.Controls.Add(configItem);
                }
            }

            SaveConfigToFile(config, filePath);
        }

        // 创建配置项的辅助方法
        private static YtDlpControlConfigItem CreateConfigItem(Control control)
        {
            if (string.IsNullOrEmpty(control.Name)) return null;

            var configItem = new YtDlpControlConfigItem
            {
                Name = control.Name,
                ControlType = control.GetType().Name,
                Description = control.Tag?.ToString() ?? ""
            };

            switch (control)
            {
                case CheckBox checkBox:
                    configItem.Value = checkBox.Checked.ToString();
                    break;

                case TextBox textBox:
                    configItem.Value = textBox.Text;
                    break;

                case ComboBox comboBox:
                    HandleComboBox(comboBox, configItem);
                    break;

                case RadioButton radioButton:
                    if (radioButton.Checked)
                        configItem.Value = radioButton.Text;
                    break;

                case NumericUpDown numericUpDown:
                    configItem.Value = numericUpDown.Value.ToString();
                    break;

                case DateTimePicker dateTimePicker:
                    configItem.Value = dateTimePicker.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    break;

                default:
                    return null;
            }

            return configItem;
        }
    }
}