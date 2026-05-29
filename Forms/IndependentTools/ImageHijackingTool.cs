using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QisToolkit3.Forms
{
    public partial class ImageHijackingTool : Form
    {
        private string[] names;
        private string[] SystemCriticalPrograms = { "cmd.exe", "powershell.exe", "conhost.exe", "explorer.exe" };

        public ImageHijackingTool()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void ImageHijackingTool_Load(object sender, EventArgs e)
        {
            // 获取 names
            try
            {
                names =
                    Registry.LocalMachine
                        .CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options")
                            .GetSubKeyNames();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"IFEO路径获取失败！\n请您检查相关权限，联系开发者\n错误原因：{ex.Message}\n完整报错：\n{ex}",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            LoadDatas();
            comboBoxData.Items.Add(AppDomain.CurrentDomain.BaseDirectory + "QisToolkit3.exe");
        }

        private void LoadDatas()
        {
            // 展示所有项
            if (checkBox_ShowAll.Checked)
                foreach (string name in names)
                    listBox.Items.Add(name);

            // 展示劫持项
            else
            {
                foreach (string name in names)
                {
                    object item = GetIFEOData(name);

                    if (item != null)
                        listBox.Items.Add(name);
                }
            }


            static object GetIFEOData(string name)
            {
                try
                {
                    return Registry.LocalMachine
                        .CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" + name)
                            .GetValue("debugger");
                }
                catch (Exception ex)
                {
                    // MessageBox.Show($"报错：{ex.Message} 值获取失败", "错误");
                    return null;
                }
            }
        }

        private void ReLoadDatas()
        {
            names = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options").GetSubKeyNames();
            listBox.Items.Clear();
            LoadDatas();
        }

        private void buttonDeleteItem_Click(object sender, EventArgs e)
        {
            foreach (object item in listBox.SelectedItems)
            {
                try
                {
                    if (Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" + item.ToString()) != null)
                    {
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options").DeleteSubKey(item.ToString());
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

            foreach (string str in SystemCriticalPrograms)
                if (comboBoxName.Text == str)
                    if (MessageBox.Show("当前目标程序为系统关键程序，若操作不当，\n可能对系统的正常运行造成影响！！！\n\n是否继续？", "警告", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        return;

            if (AddListItem(comboBoxName.Text, comboBoxData.Text, comboBoxDataMO.Text, checkBox_DisableUserModeCallbackFilter.Checked))
                MessageBox.Show("项目已添加/修改成功！", "提示");
        }

        private bool AddListItem(string name, string data, string dataMO, bool dataDUMCF)
        {
            try
            {
                RegistryKey reg =
                    Registry.LocalMachine
                        .CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" + name);

                if (!string.IsNullOrWhiteSpace(data))
                    reg.SetValue("debugger", data);

                if (!string.IsNullOrWhiteSpace(dataMO))
                {
                    // 计算十六进制表达式并转换为QWORD值
                    ulong mitigationValue = CalculateHexExpression(dataMO);
                    reg.SetValue("MitigationOptions", (long)mitigationValue, RegistryValueKind.QWord);
                }

                if (reg.GetValue("DisableUserModeCallbackFilter") != null || dataDUMCF)
                    reg.SetValue("DisableUserModeCallbackFilter", dataDUMCF ? 1 : 0, RegistryValueKind.DWord);
                
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

        private ulong CalculateHexExpression(string expression)
        {
            try
            {
                // 移除所有空格
                string cleanExpr = expression.Replace(" ", "");

                // 分割表达式中的各个部分（支持 | & ^ 运算符）
                string[] parts = cleanExpr.Split(new char[] { '|', '&', '^' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0)
                    throw new ArgumentException("表达式为空");

                // 计算第一个值
                ulong result = ParseHexValue(parts[0]);

                // 如果有多个部分，进行位运算
                if (parts.Length > 1)
                {
                    int operatorIndex = parts[0].Length;

                    for (int i = 1; i < parts.Length; i++)
                    {
                        char op = cleanExpr[operatorIndex];
                        ulong value = ParseHexValue(parts[i]);

                        switch (op)
                        {
                            case '|':
                                result |= value;
                                break;
                            case '&':
                                result &= value;
                                break;
                            case '^':
                                result ^= value;
                                break;
                            default:
                                throw new ArgumentException($"不支持的运算符: {op}");
                        }

                        operatorIndex += parts[i].Length + 1;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"无效的十六进制表达式: {expression}", ex);
            }
        }

        private ulong ParseHexValue(string hexValue)
        {
            try
            {
                string cleanHex = hexValue.Trim().ToLower();
                if (cleanHex.StartsWith("0x"))
                    cleanHex = cleanHex.Substring(2);

                return Convert.ToUInt64(cleanHex, 16);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"无效的十六进制值: {hexValue}", ex);
            }
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
                            .CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" + listBox.SelectedItem.ToString());

                    object _TEMP = reg.GetValue("debugger");
                    object _TEMPMO = reg.GetValue("MitigationOptions");
                    object _TEMPDUMCF = reg.GetValue("DisableUserModeCallbackFilter");

                    comboBoxName.Text = listBox.SelectedItem.ToString(); 
                    comboBoxData.Text = _TEMP ?.ToString() ?? string.Empty;
                    if (_TEMPMO != null)
                    {
                        try
                        {
                            // 统一转换为64位整数再格式化为十六进制
                            long value = Convert.ToInt64(_TEMPMO);
                            comboBoxDataMO.Text = "0x" + value.ToString("X16");
                        }
                        catch
                        {
                            // 如果转换失败，使用原始字符串
                            comboBoxDataMO.Text = _TEMPMO.ToString();
                        }
                    }
                    else
                    {
                        comboBoxDataMO.Text = string.Empty;
                    }

                    checkBox_DisableUserModeCallbackFilter.Checked = Convert.ToInt32(_TEMPDUMCF) >= 1;
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


        private string IFEO_CF = "C:\\WINDOWS\\System32\\cmd.exe /C \"" + Directory.GetCurrentDirectory() + "\\Customize\\IFEO Command.bat\"";

        private string GetIFEO_CF()
        {
            //if (checkBox_UseIFEOCommandFile.Checked)
            //    return IFEO_CF;
            return "*";
        }

        private void button_ReLoad_Click(object sender, EventArgs e) =>
            ReLoadDatas();

        private void checkBox_ShowAll_CheckedChanged(object sender, EventArgs e) =>
            ReLoadDatas();
    }
}
