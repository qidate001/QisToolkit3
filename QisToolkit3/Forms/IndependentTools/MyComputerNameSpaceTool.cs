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
    public partial class MyComputerNameSpaceTool : Form
    {
        private string[] names;
        private RegistryKey registry = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace");

        public MyComputerNameSpaceTool()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);

            comboBoxType.SelectedIndex = 0;
        }

        private void MyComputerNameSpaceTool_Load(object sender, EventArgs e)
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
                object item = GetMCNSData(name);

                if (name != "DelegateFolders")
                    listBox.Items.Add(name);
            }

            object GetMCNSData(string name)
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
            comboBoxData.Text = Qi.NoRN(comboBoxData.Text);


            if (AddListItem(comboBoxName.Text, comboBoxData.Text))
                MessageBox.Show("项目已添加/修改成功！", "提示");
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
                    registry.CreateSubKey(comboBoxName.Text).SetValue("", data);

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

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                buttonDeleteItem.Enabled = true;
                try
                {
                    object _TEMP =
                        registry
                            .CreateSubKey(listBox.SelectedItem.ToString())
                                .GetValue("");

                    comboBoxName.Text = listBox.SelectedItem.ToString();
                    comboBoxData.Text = _TEMP != null ? _TEMP.ToString() : string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取数据时发生错误，请联系开发者\n错误原因：" + ex.Message + "\n\n完整报错：\n" + ex, "报错");
                }
            }
        }

        private void comboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonAddItem.Enabled = Qi.NoST(Qi.NoRN(comboBoxName.Text)) != string.Empty;
            if (listBox.Items.Contains(comboBoxName.Text))
                buttonAddItem.Text = "修改对应项";
            else
                buttonAddItem.Text = "添加至列表";
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            registry = comboBoxType.SelectedIndex == 0 ?
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace") :
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace");
            MyComputerNameSpaceTool_Load(null, null);
        }

        private void button_TryRepairFolder_Click(object sender, EventArgs e)
        {
            try
            {
                // 释放 GUID
                registry = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace");
                AddListItem("{088E3905-0323-4B02-9826-5D99428E115F}", "");
                AddListItem("{0DB7E03F-FC29-4DC6-9020-FF41B59E513A}", "");
                AddListItem("{24AD3AD4-A569-4530-98E1-AB02F9417AA8}", "");
                AddListItem("{3DFDF296-DBEC-4FB4-81D1-6A3438BCF4DE}", "");
                AddListItem("{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", "");
                AddListItem("{D3162B92-9365-467A-956B-92703ACA08AF}", "");
                AddListItem("{F86FA3AB-70D2-4FC7-9C99-FCBF05467F3A}", "");

                // 显示 GUID
                registry = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FolderDescriptions");
                registry.CreateSubKey(@"{088E3905-0323-4B02-9826-5D99428E115F}\PropertyBag").SetValue("ThisPCPolicy", "Show");
                registry.CreateSubKey(@"{0DB7E03F-FC29-4DC6-9020-FF41B59E513A}\PropertyBag").SetValue("ThisPCPolicy", "Show");
                registry.CreateSubKey(@"{24AD3AD4-A569-4530-98E1-AB02F9417AA8}\PropertyBag").SetValue("ThisPCPolicy", "Show");
                registry.CreateSubKey(@"{3DFDF296-DBEC-4FB4-81D1-6A3438BCF4DE}\PropertyBag").SetValue("ThisPCPolicy", "Show");
                registry.CreateSubKey(@"{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}\PropertyBag").SetValue("ThisPCPolicy", "Show");
                registry.CreateSubKey(@"{D3162B92-9365-467A-956B-92703ACA08AF}\PropertyBag").SetValue("ThisPCPolicy", "Show");
                registry.CreateSubKey(@"{F86FA3AB-70D2-4FC7-9C99-FCBF05467F3A\PropertyBag").SetValue("ThisPCPolicy", "Show");

                // 还原 registry
                comboBoxType_SelectedIndexChanged(sender, e);
                MessageBox.Show("修复完成！请检查此电脑是否已经恢复！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行时出现错误，原因：{ex.Message}\n\n完整报错：\n{ex}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
