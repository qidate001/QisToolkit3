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

namespace QisToolkit3.Forms.IndependentTools
{
    public partial class ShellFolderSetPathTool : Form
    {
        private string[] names;
        private RegistryKey registry = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders");

        public ShellFolderSetPathTool()
        {
            InitializeComponent();
        }

        private void ShellFolderSetPathTool_Load(object sender, EventArgs e)
        {
            // 获取 names
            try
            {
                names = registry.GetValueNames();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"路径获取失败！\n请您检查相关权限，联系开发者\n错误原因：{ex.Message}\n完整报错：\n{ex}",
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
                if (name != "")
                    listBox.Items.Add(name);
        }

        private void ReLoadDatas()
        {
            names = registry.GetSubKeyNames();
            listBox.Items.Clear();
            LoadDatas();
        }

        private bool AddListItem(string name, string data)
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
                    registry.SetValue(comboBoxName.Text, data);

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

        private void buttonAddItem_Click(object sender, EventArgs e)
        {
            comboBoxName.Text = Qi.NoRN(comboBoxName.Text);
            comboBoxData.Text = Qi.NoRN(comboBoxData.Text);


            if (AddListItem(comboBoxName.Text, comboBoxData.Text))
                MessageBox.Show("项目已添加/修改成功！", "提示");
        }

        private void buttonDeleteItem_Click(object sender, EventArgs e)
        {

            foreach (object item in listBox.SelectedItems)
            {
                try
                {

                    if (registry.GetValue(item.ToString()) != null)
                    {
                        registry.DeleteValue(item.ToString());
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

            listBox.Items.Remove(listBox.SelectedItem);
            buttonDeleteItem.Enabled = listBox.SelectedItems.Count > 0;
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                buttonDeleteItem.Enabled = true;
                try
                {
                    object _TEMP =
                        registry
                            .GetValue(listBox.SelectedItem.ToString());

                    comboBoxName.Text = listBox.SelectedItem.ToString();
                    comboBoxData.Text = _TEMP != null ? _TEMP.ToString() : string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取数据时发生错误，请联系开发者\n错误原因：" + ex.Message + "\n\n完整报错：\n" + ex, "报错");
                }
            }
        }

        private void button_ReLoad_Click(object sender, EventArgs e)
        {
            LoadDatas();
        }

        private void button_FixShellFolderNameAndIcon_lnk_Click(object sender, EventArgs e)
        {
            var tools = new CommonFunctionalTools();
            tools.button_FixShellFolderNameAndIcon_Click(sender, e);
        }

        private void comboBoxName_TextChanged(object sender, EventArgs e)
        {
            buttonAddItem.Enabled = Qi.NoST(Qi.NoRN(comboBoxName.Text)) != string.Empty;
            if (listBox.Items.Contains(comboBoxName.Text))
                buttonAddItem.Text = "修改对应项";
            else
                buttonAddItem.Text = "添加至列表";
        }

        private void button_ReStartExplorer_Click(object sender, EventArgs e)
        {
            Qi.RestartExplorer();
        }
    }
}
