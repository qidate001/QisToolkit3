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
using System.Xml.Linq;

namespace QisToolkit3.Forms
{
    public partial class SystemServiceTools : Form
    {
        private string[] names;
        private string[] SystemCriticalPrograms = { "wuauserv" };

        public SystemServiceTools()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }


        private void SystemServiceTools_Load(object sender, EventArgs e)
        {
            // 获取 names
            try
            {
                names =
                    Registry.LocalMachine
                        .CreateSubKey(@"SYSTEM\CurrentControlSet\Services")
                            .GetSubKeyNames();
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
            if (checkBox_ShowAll.Checked)
                foreach (string name in names)
                    listBox.Items.Add(name);

            // 验证有效
            else
            {
                foreach (string name in names)
                {
                    try
                    {
                        // 验证服务是否完整存在
                        using (var key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{name}"))
                        {
                            if (key != null && key.GetValue("ImagePath") != null)
                            {
                                // 只有包含ImagePath的有效服务才显示
                                listBox.Items.Add(name);
                            }
                        }
                    }
                    catch
                    {
                        // 忽略无法访问的服务项
                    }
                }
            }
        }

        private void buttonAddItem_Click(object sender, EventArgs e)
        {
            comboBoxName.Text = Qi.NoRN(comboBoxName.Text);
            comboBoxStart.Text = Qi.NoRN(comboBoxStart.Text);

            foreach (string str in SystemCriticalPrograms)
                if (comboBoxName.Text == str)
                    if (MessageBox.Show("当前目标程序为系统关键服务，若操作不当，\n可能对系统的正常运行造成影响！！！\n\n是否继续？", "警告", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        return;

            if (AddListItem(comboBoxName.Text, comboBoxStart.SelectedIndex))
                MessageBox.Show("项目已添加/修改成功！", "提示");
        }

        private bool AddListItem(string name, int start)
        {
            try
            {
                RegistryKey reg =
                    Registry.LocalMachine
                        .CreateSubKey(@"SYSTEM\CurrentControlSet\Services\" + name);

                // 设置Start值
                if (start >= 0 && start <= 4)
                    reg.SetValue("Start", start);

                // 否则尝试删除Start值
                else if (reg.GetValue("Start") != null)
                    reg.DeleteValue("Start");

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

        private void buttonDeleteItem_Click(object sender, EventArgs e)
        {
            foreach (object item in listBox.SelectedItems)
            {
                try
                {
                    RegistryKey reg =
                        Registry.LocalMachine
                            .CreateSubKey(@"SYSTEM\CurrentControlSet\Services");

                    if (reg.CreateSubKey(item.ToString()) != null)
                    {
                        reg.DeleteSubKey(item.ToString());
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

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                buttonDeleteItem.Enabled = true;
                try
                {
                    RegistryKey reg =
                        Registry.LocalMachine
                            .CreateSubKey(@"SYSTEM\CurrentControlSet\Services\" + listBox.SelectedItem.ToString());

                    object tmpStart = reg.GetValue("Start");

                    // ID
                    comboBoxName.Text = listBox.SelectedItem.ToString();

                    // Start
                    System.Console.WriteLine($"tmpStart 值: {tmpStart}");
                    System.Console.WriteLine($"tmpStart 类型: {tmpStart?.GetType().Name ?? "null"}");
                    System.Console.WriteLine($"tmpStart 是否为null: {tmpStart == null}");

                    if (tmpStart != null)
                    {
                        System.Console.WriteLine($"_TEMP.ToString(): {tmpStart.ToString()}");

                        // 尝试多种转换方式
                        if (tmpStart is int intValue)
                        {
                            System.Console.WriteLine($"直接类型转换成功: {intValue}");
                            if (intValue >= 0 && intValue <= 4)
                            {
                                comboBoxStart.SelectedIndex = intValue;
                                System.Console.WriteLine($"设置 SelectedIndex 为: {intValue}");
                                return;
                            }
                        }

                        // 尝试字符串转换
                        if (int.TryParse(tmpStart.ToString(), out int parsedValue))
                        {
                            System.Console.WriteLine($"字符串转换成功: {parsedValue}");
                            if (parsedValue >= 0 && parsedValue <= 4)
                            {
                                comboBoxStart.SelectedIndex = parsedValue;
                                System.Console.WriteLine($"设置 SelectedIndex 为: {parsedValue}");
                                return;
                            }
                        }
                    }

                    // 如果所有条件都不满足，设置为 -1
                    System.Console.WriteLine("最终设置 SelectedIndex 为: -1");
                    comboBoxStart.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取数据时发生错误，请联系开发者\n错误原因：" + ex.Message + "\n\n完整报错：\n" + ex, "报错");
                }
            }
        }

        private void comboBoxName_TextChanged(object sender, EventArgs e)
        {
            buttonAddItem.Enabled = Qi.NoST(Qi.NoRN(comboBoxName.Text)) != string.Empty;
            if (listBox.Items.Contains(comboBoxName.Text))
                buttonAddItem.Text = "修改对应项";
            else
                buttonAddItem.Text = "添加至列表";
        }

        private void ReLoadDatas()
        {
            names = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services").GetSubKeyNames();
            listBox.Items.Clear();
            LoadDatas();
        }

        private void button_ReLoad_Click(object sender, EventArgs e) =>
            ReLoadDatas();

        private void checkBox_ShowAll_CheckedChanged(object sender, EventArgs e) =>
            ReLoadDatas();
    }
}
