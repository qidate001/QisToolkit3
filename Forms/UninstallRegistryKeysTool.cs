using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QisToolkit3.Forms
{
    public partial class UninstallRegistryKeysTool : Form
    {
        private string[] names;
        private RegistryKey registry = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall");

        public UninstallRegistryKeysTool()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);

            comboBoxType.SelectedIndex = 0;
        }

        private void UninstallRegistryKeysTool_Load(object sender, EventArgs e)
        {
            // 获取 names
            try
            {
                names = registry.GetSubKeyNames();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"数据获取失败！\n请您检查相关权限，联系开发者\n错误原因：{ex.Message}\n完整报错：\n{ex}",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            LoadDatas();
        }

        private void LoadDatas()
        {
            listBox.Items.Clear();
            foreach (string name in names)
            {
                object item = GetURKData(name);

                //if (name != "")
                listBox.Items.Add(name);
            }

            object GetURKData(string name)
            {
                try
                {
                    return registry.GetValue("");
                }
                catch
                {
                    return null;
                }
            }
        }

        private void buttonDeleteItem_Click(object sender, EventArgs e)
        {
            foreach (object item in listBox.SelectedItems)
            {
                try
                {

                    if (registry.CreateSubKey(item.ToString()) != null)
                    {
                        registry.DeleteSubKey(item.ToString());
                        MessageBox.Show("项目 " + item.ToString() + " 删除成功", "提示");
                    }
                    else
                        MessageBox.Show("项目不存在！", "报错");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("执行出现错误，请联系开发者\n错误原因：" + ex.Message + "\n\n完整报错：\n" + ex, "报错");
                }
            }

            for (int i = listBox.Items.Count; i > 0; --i)
                listBox.Items.Remove(listBox.SelectedItem);

            buttonDeleteItem.Enabled = listBox.SelectedItems.Count > 0;
        }

        private void buttonAddItem_Click(object sender, EventArgs e)
        {
            comboBoxName.Text = Qi.NoRN(comboBoxName.Text);
            comboBoxDisplayName.Text = Qi.NoRN(comboBoxDisplayName.Text);


            if (AddListItem(comboBoxName.Text))
                MessageBox.Show("项目已添加/修改成功！", "提示");
        }

        private bool AddListItem(string name)
        {
            bool NotHave = true;
            for (int i = 0; i < listBox.Items.Count; ++i)
            {
                if (listBox.Items[i].ToString() == name)
                {
                    NotHave = false;
                    break;
                }
            }

            if (NotHave || true)
            {
                try
                {
                    registry.CreateSubKey(comboBoxName.Text).SetValue("DisplayName", comboBoxDisplayName.Text);
                    registry.CreateSubKey(comboBoxName.Text).SetValue("DisplayIcon", comboBoxDisplayIcon.Text);
                    registry.CreateSubKey(comboBoxName.Text).SetValue("InstallLocation", comboBoxInstallLocation.Text);
                    registry.CreateSubKey(comboBoxName.Text).SetValue("UninstallString", comboBoxUninstallString.Text);
                    registry.CreateSubKey(comboBoxName.Text).SetValue("DisplayVersion", comboBoxDisplayVersion.Text);
                    registry.CreateSubKey(comboBoxName.Text).SetValue("Publisher", comboBoxPublisher.Text);
                    registry.CreateSubKey(comboBoxName.Text).SetValue("HelpLink", comboBoxHelpLink.Text);

                    if (!listBox.Items.Contains(name))
                        listBox.Items.Add(name);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出时发生错误，请联系开发者\n错误原因：" + ex.Message + "\n\n完整报错：\n" + ex, "报错");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("项目已存在，不可重复添加", "提示");
            }
            return false;
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            registry = comboBoxType.SelectedIndex == 0 ?
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall") :
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            UninstallRegistryKeysTool_Load(null, null);
        }

        private void comboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonAddItem.Enabled = Qi.NoST(Qi.NoRN(comboBoxName.Text)) != string.Empty;
            if (listBox.Items.Contains(comboBoxName.Text))
                buttonAddItem.Text = "修改对应项";
            else
                buttonAddItem.Text = "添加至列表";
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listBox.SelectedItem != null)
            {
                buttonDeleteItem.Enabled = true;
                try
                {
                    RegistryKey registryKey = registry.CreateSubKey(listBox.SelectedItem.ToString());
                    object _TEMP = registryKey.GetValue("DisplayName");

                    comboBoxName.Text = listBox.SelectedItem.ToString();
                    comboBoxDisplayName.Text = _TEMP != null ? _TEMP.ToString() : string.Empty;

                    _TEMP = registryKey.GetValue("DisplayIcon");
                    comboBoxDisplayIcon.Text = _TEMP != null ? _TEMP.ToString() : string.Empty;

                    _TEMP = registryKey.GetValue("InstallLocation");
                    comboBoxInstallLocation.Text = _TEMP != null ? _TEMP.ToString() : string.Empty;

                    _TEMP = registryKey.GetValue("UninstallString");
                    comboBoxUninstallString.Text = _TEMP != null ? _TEMP.ToString() : string.Empty;

                    _TEMP = registryKey.GetValue("DisplayVersion");
                    comboBoxDisplayVersion.Text = _TEMP != null ? _TEMP.ToString() : string.Empty;

                    _TEMP = registryKey.GetValue("Publisher");
                    comboBoxPublisher.Text = _TEMP != null ? _TEMP.ToString() : string.Empty;

                    _TEMP = registryKey.GetValue("HelpLink");
                    comboBoxHelpLink.Text = _TEMP != null ? _TEMP.ToString() : string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取数据时发生错误，请联系开发者\n错误原因：" + ex.Message + "\n\n完整报错：\n" + ex, "报错");
                }
            }
        }

        private void buttonMakeCode_Click(object sender, EventArgs e)
        {
            string code =
                "private void UninstallRegistryKeys()\n{\n" +
                "\ttry\n\t{\n" + 
               $"\t\tRegistry registry = Registry.LocalMachine.CreateSubKey(@\"SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{comboBoxName}\");\n" +
               $"\t\tregistry.SetValue(\"DisplayName\", \"{comboBoxDisplayName.Text.Replace(@"\", @"\\").Replace("\"", "\\\"")}\");\n" +
               $"\t\tregistry.SetValue(\"DisplayIcon\", \"{comboBoxDisplayIcon.Text.Replace(@"\", @"\\").Replace("\"", "\\\"")}\");\n" +
               $"\t\tregistry.SetValue(\"InstallLocation\", \"{comboBoxInstallLocation.Text.Replace(@"\", @"\\").Replace("\"", "\\\"")}\");\n" +
               $"\t\tregistry.SetValue(\"UninstallString\", \"{comboBoxUninstallString.Text.Replace(@"\", @"\\").Replace("\"", "\\\"")}\");\n" +
               $"\t\tregistry.SetValue(\"DisplayVersion\", \"{comboBoxDisplayVersion.Text.Replace(@"\", @"\\").Replace("\"", "\\\"")}\");\n" +
               $"\t\tregistry.SetValue(\"Publisher\", \"{comboBoxPublisher.Text.Replace(@"\", @"\\").Replace("\"", "\\\"")}\");\n" +
               $"\t\tregistry.SetValue(\"HelpLink\", \"{comboBoxHelpLink.Text.Replace(@"\", @"\\").Replace("\"", "\\\"")}\");\n" +
                "\t}\n\tcatch (Exception ex)\n\t{\n" +
                "\t\tMessageBox.Show(\"输出程序卸载注册表项时发生错误\\n错误原因：\" + ex.Message + \"\\n\\n完整报错：\\n\" + ex, \"报错\");\n" +
                "\t}\n}";

            MessageBox.Show(code + "\n\n已复制到粘贴板", "命令");
            Clipboard.SetText(code);
        }
    }
}