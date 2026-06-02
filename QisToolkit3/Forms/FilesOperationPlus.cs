using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Qi;
//using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QisToolkit3.Forms
{
    public partial class FilesOperationPlus : Form
    {
        private HashSet<string> imageExtensions = new HashSet<string> { ".jpg", ".png", ".gif", ".bmp" };

        private string[] selectedFiles;
        private bool RunCommand = false;
        GpsData gpsInfo = null;

        public FilesOperationPlus()
        {
            InitializeComponent();
            comboBox_Encoding.SelectedIndex = 0;

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void OpenFileMain()
        {
            ReSetData();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 获取用户选择的文件路径
                selectedFiles = openFileDialog.FileNames;

                //labelFileName.Text = string.Empty;
                if (selectedFiles != null)
                {
                    SetListBox(checkBox_ShowName.Checked);
                }
            }
        }

        private void SetListBox(bool ShowName)
        {
            // 清空
            listBox.Items.Clear();

            // 添加到列表
            if (ShowName)
                foreach (string file in selectedFiles)
                    listBox.Items.Add(Path.GetFileName(file));
            else
                foreach (string file in selectedFiles)
                    listBox.Items.Add(file);
        }

        private void SetDatas()
        {
            if (listBox.SelectedItems != null && listBox.SelectedIndex >= 0 && listBox.SelectedIndex < selectedFiles.Count())
            {
                string selectedItem = GetselectedFile();
                string extension = Path.GetExtension(selectedItem).ToLower();
                bool IsTextFile = Qi.IsTextFile(selectedItem);
                bool IsImgFile = imageExtensions.Contains(extension);
                bool enabled = selectedFiles != null && File.Exists(selectedItem);

                checkBoxSystem.Enabled = enabled;
                checkBoxHidden.Enabled = enabled;
                checkBoxArchive.Enabled = enabled;
                checkBoxOffline.Enabled = enabled;
                checkBoxReadOnly.Enabled = enabled;
                button_DeleteFile.Enabled = enabled;
                checkBoxNotContentIndexed.Enabled = enabled;

                checkBoxSystem.Checked = (File.GetAttributes(selectedItem) & FileAttributes.System) == FileAttributes.System;
                checkBoxHidden.Checked = (File.GetAttributes(selectedItem) & FileAttributes.Hidden) == FileAttributes.Hidden;
                checkBoxOffline.Checked = (File.GetAttributes(selectedItem) & FileAttributes.Offline) == FileAttributes.Offline;
                checkBoxArchive.Checked = (File.GetAttributes(selectedItem) & FileAttributes.Archive) == FileAttributes.Archive;
                checkBoxReadOnly.Checked = (File.GetAttributes(selectedItem) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                checkBoxNotContentIndexed.Checked = (File.GetAttributes(selectedItem) & FileAttributes.NotContentIndexed) == FileAttributes.NotContentIndexed;


                // 文本文件功能
                button_SetTextFile.Enabled = IsTextFile;
                checkBox_E_Encoding.Enabled = IsTextFile;
                textBox_OverwriteTheOriginalFile.Enabled = IsTextFile;
                checkBox_OverwriteTheOriginalFile.Enabled = IsTextFile;

                // 文本文件功能 预览
                if (IsTextFile)
                {
                    richTextBox_Preview.ForeColor = Color.Black;
                    richTextBox_Preview.Text = ReadFirstChars(selectedItem, 1024);
                    richTextBox_Preview.AppendText("\n\n*** 仅预览前1024个字符 ***" + Environment.NewLine);
                }
                else
                {
                    richTextBox_Preview.ForeColor = Color.Red;
                    richTextBox_Preview.Text = "*** 非文本类型文件不支持预览 ***";
                }

                // 文件头
                ComponentResourceManager resources = new ComponentResourceManager(typeof(FilesOperation));
                label_FileHead.Text = resources.GetString("label_FileHead.Text") + GetFileHexHeader(selectedItem, 16);

                button_ReadImgEXIF.Enabled = IsImgFile;
                button_SaveImgEXIF.Enabled = false;
            }


        }



        private string GetselectedFile()
        {
            if (listBox.SelectedItems != null && listBox.SelectedIndex >= 0 && listBox.SelectedIndex < selectedFiles.Count())
                return selectedFiles[listBox.SelectedIndex];
            return string.Empty;
        }

        private string[] GetselectedFiles()
        {
            string[] datas = new string[listBox.SelectedIndices.Count];
            if (listBox.SelectedItems != null)
            {
                int i = 0;
                foreach (int index in listBox.SelectedIndices)
                {
                    if (listBox.SelectedIndex >= 0 && listBox.SelectedIndex < selectedFiles.Count())
                    {
                        datas[i] = selectedFiles[index];
                        ++i;
                    }
                }
            }

            return datas;
        }

        private void ReSetData()
        {
            checkBoxSystem.Enabled = false;
            checkBoxHidden.Enabled = false;
            checkBoxArchive.Enabled = false;
            checkBoxOffline.Enabled = false;
            checkBoxReadOnly.Enabled = false;
            button_DeleteFile.Enabled = false;
            comboBox_Encoding.Enabled = false;
            checkBoxNotContentIndexed.Enabled = false;
        }

        private void button_UseOpenDialog_Click(object sender, EventArgs e) => OpenFileMain();

        private void checkBox_ShowName_CheckedChanged(object sender, EventArgs e) => SetListBox(checkBox_ShowName.Checked);

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RunCommand = true;
            SetDatas();
            RunCommand = false;
        }



        private void checkBoxHidden_CheckedChanged(object sender, EventArgs e)
        {
            if (RunCommand) return;
            foreach (string path in GetselectedFiles())
                SetHiddenFileMain(path, checkBoxHidden.Checked);
        }

        private void checkBoxArchive_CheckedChanged(object sender, EventArgs e)
        {
            if (RunCommand) return;
            foreach (string path in GetselectedFiles())
                SetArchiveFileMain(path, checkBoxArchive.Checked);
        }

        private void checkBoxReadOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (RunCommand) return;
            foreach (string path in GetselectedFiles())
                SetReadOnlyFileMain(path, checkBoxReadOnly.Checked);
        }

        private void checkBoxSystem_CheckedChanged(object sender, EventArgs e)
        {
            if (RunCommand) return;
            foreach (string path in GetselectedFiles())
                SetSystemFileMain(path, checkBoxSystem.Checked);
        }

        private void checkBoxOffline_CheckedChanged(object sender, EventArgs e)
        {
            if (RunCommand) return;
            foreach (string path in GetselectedFiles())
                SetOfflineFileMain(path, checkBoxOffline.Checked);
        }

        private void checkBoxNotContentIndexed_CheckedChanged(object sender, EventArgs e)
        {
            if (RunCommand) return;
            foreach (string path in GetselectedFiles())
                SetNotContentIndexedFileMain(path, checkBoxNotContentIndexed.Checked);
        }

        private void buttonSetFileName_Click(object sender, EventArgs e)
        {
            if (NoST(NoRN(richTextBoxNameRule.Text)) != string.Empty)
            {
                // 拆解字符串并去除空行
                string[] lines = Regex.Split(richTextBoxNameRule.Text, "\r\n|\n|\r");
                string[] AllRule_Head = new string[3000];
                string[] AllRule_Main = new string[3000];
                string[] AllRule_Feet = new string[3000];
                int AllRule_MaxId = 0;

                // 主要规则字符串
                string[] MainRule = new string[3000];
                int MainRuleCount = 0;


                // 获取规则
                foreach (string line in lines)
                {
                    // 特殊文本规则声明
                    if (line[..3] == ":::")
                    {
                        // 剔除开头的字符串
                        string _line = line[3..];
                        string __head = string.Empty, __main = string.Empty, __feet = string.Empty;
                        bool aIsOn = false;

                        for (int i = 0; i < _line.Length; i++)
                        {
                            if (!aIsOn)
                                if (_line.Length - i >= 2)
                                    if (_line[i..(i + 2)] == ">>" || _line[i..(i + 2)] == "<<" || _line[i..(i + 2)] == "^^")
                                    {
                                        __main = _line[i..(i + 2)];
                                        aIsOn = true;
                                        i++; // 跳过 表达式，因为循环结束也会自增一，所以这里只需自增一即可
                                    }
                                    else
                                        __head += _line[i];
                                else
                                {
                                    MessageBox.Show("表达式错误，不存在主表达式", "错误");
                                    break;
                                }
                            else
                                __feet += _line[i];
                        }

                        AllRule_Head[AllRule_MaxId] = __head;
                        AllRule_Main[AllRule_MaxId] = __main;
                        AllRule_Feet[AllRule_MaxId] = __feet;
                        ++AllRule_MaxId;
                    }

                    // 主要表达式
                    else if (MainRuleCount == 0)
                    {
                        for (int i = 0; i < line.Length; i++)
                        {
                            // 数字
                            if (line[i] == ':')
                            {
                                string _rule = string.Empty;
                                for (int j = i; j < line.Length; j++)
                                {
                                    if (line[j] == ':')
                                        break;

                                    else
                                        _rule += line[j];

                                    i = j; // 外循环变量需和内循环变量保持一致
                                }
                                MainRule[++MainRuleCount] = _rule;
                                ++MainRuleCount;
                            }

                            else
                                MainRule[MainRuleCount] += line[i];
                        }
                    }
                }



                // 执行重命名
                foreach (string path in GetselectedFiles())
                {
                    string oldName = Path.GetFileName(path);
                    string newName = oldName;

                    for (int i = 0; i < AllRule_MaxId; i++)
                    {
                        if (AllRule_Main[i] == ">>")
                            newName = newName.Replace(AllRule_Head[i], AllRule_Feet[i]);

                        else if (AllRule_Main[i] == "<<")
                            newName = newName.Replace(AllRule_Feet[i], AllRule_Head[i]);
                    }


                    // 执行重命名
                    File.Move(path, @$"{Path.GetDirectoryName(path)}\{newName}");
                }

                listBox.Items.Clear();
            }
            else
                MessageBox.Show("规则不可为空！", "错误");
        }

        private void listBox_SetFileName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_SetFileName.SelectedItem.ToString() == "英文括号转中文括号")
                richTextBoxNameRule.Text = ":::(>>（\r\n:::)>>）";

            else if (listBox_SetFileName.SelectedItem.ToString() == "中文括号转英文括号")
                richTextBoxNameRule.Text = ":::（>>(\r\n:::）>>)";

            else if (listBox_SetFileName.SelectedItem.ToString() == "去除数字")
                richTextBoxNameRule.Text = ":::<<0\r\n:::<<1\r\n:::<<2\r\n:::<<3\r\n:::<<4\r\n:::<<5\r\n:::<<6\r\n:::<<7\r\n:::<<8\r\n:::<<9\r\n";
        }

        private void button_C_MaxPath_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("此功能有一定风险！容易出现文件卡死等情况！！", "警告", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            try
            {
                const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                // 规范化路径（确保以 \ 结尾）
                textBox_MaxPath_Path.Text = textBox_MaxPath_Path.Text.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;

                // 计算剩余可用的文件名长度
                int remainingLength = 259 - textBox_MaxPath_Path.Text.Length;

                // 磁盘根目录长度为 259 而非 260
                if (IsRootDirectory(textBox_MaxPath_Path.Text))
                    --remainingLength;

                if (remainingLength <= 0)
                {
                    MessageBox.Show("路径已超过最大限制！", "错误");
                    return;
                }

                // 生成随机文件名
                StringBuilder result = new StringBuilder();
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    byte[] randomBytes = new byte[remainingLength];
                    rng.GetBytes(randomBytes);
                    for (int i = 0; i < remainingLength; i++)
                    {
                        result.Append(validChars[randomBytes[i] % validChars.Length]);
                    }
                }

                // 创建文件
                string fullPath = Path.Combine(textBox_MaxPath_Path.Text, result.ToString());
                File.Create(fullPath).Close();
                MessageBox.Show("创建成功！", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"创建失败！错误信息: {ex.Message}", "错误");
            }
        }

        private void FakeModeCreateDirectory(string directory)
        {
            try
            {
                if (checkBox_Create_FakeMode.Checked)
                    Directory.CreateDirectory(@$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{directory}");
                else
                {
                    if (textBox_Create_FakeMode.Text.Last() == '\\')
                        Directory.CreateDirectory(@$"{textBox_Create_FakeMode.Text}{directory}");
                    else
                        Directory.CreateDirectory(@$"{textBox_Create_FakeMode.Text}\{directory}");
                }
                MessageBox.Show("创建成功！", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"创建失败！\n原因：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void checkBox_Create_FakeMode_CheckedChanged(object sender, EventArgs e) =>
            textBox_Create_FakeMode.Enabled = !checkBox_Create_FakeMode.Checked;

        private void button_Create_20D04FE0_Click(object sender, EventArgs e) =>
            FakeModeCreateDirectory("伪·此电脑.{20D04FE0-3AEA-1069-A2D8-08002B30309D}");

        private void button_Create_645FF040_Click(object sender, EventArgs e) =>
            FakeModeCreateDirectory("伪·回收站.{645FF040-5081-101B-9F08-00AA002F954E}");

        private void button_Create_208D2C60_Click(object sender, EventArgs e) =>
            FakeModeCreateDirectory("伪·网络.{208D2C60-3AEA-1069-A2D7-08002B30309D}");

        private void button_Create_2227A280_Click(object sender, EventArgs e) =>
            FakeModeCreateDirectory("伪·打印机.{2227A280-3AEA-1069-A2DE-08002B30309D}");

        private void button_Create_26EE066X_Click(object sender, EventArgs e) =>
            FakeModeCreateDirectory("伪·控制面板.{26EE0668-A00A-44D7-9371-BEB064C98683}");

        private void button1_Click(object sender, EventArgs e)
        {
            string folderPath = @"D:\UserDir\Desktop\新建文件夹";
            string desktopIniPath = Path.Combine(folderPath, "desktop.ini");
            string iniContent =
                @"[.ShellClassInfo]
IconResource=x.ico,0
InfoTip=这里是你的提示文字
[ViewState]
Mode=
Vid=
FolderType=Videos";

            try
            {
                // 确保目标文件夹存在
                Directory.CreateDirectory(folderPath);

                // 以ANSI编码写入desktop.ini
                File.WriteAllText(desktopIniPath, iniContent, Encoding.GetEncoding("GB2312"));

                // 设置文件属性为隐藏+系统（需管理员权限）
                File.SetAttributes(desktopIniPath, FileAttributes.Hidden | FileAttributes.System);

                System.Console.WriteLine("desktop.ini 创建成功！");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"错误: {ex.Message}");
            }
        }

        private void button_SetTextFile_Click(object sender, EventArgs e)
        {
            int MS = 0, MF = 0;

            SetTextFileMain();

            MessageBox.Show($"转化完成！\n\n成功：{MS}个\n失败：{MF}个", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);


            // 核心运算
            void SetTextFileMain()
            {
                bool fx = true;

                foreach (string path in GetselectedFiles())
                {
                    fx = OutTextFile(path, StrToEncoding(comboBox_Encoding.Text)) && fx ? true : false;

                    // 成功失败计数器
                    if (fx) ++MS; else ++MF;
                }
            }
        }



        private bool OutTextFile(string filePath/*, Encoding originalEncoding*/, Encoding newEncoding)
        {
            // 注册编码提供程序
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string text = File.ReadAllText(filePath);
            string rule = richTextBox_SetText_Rule.Text.Replace("\r\n", "");

            // 应用规则
            if (!string.IsNullOrWhiteSpace(rule))
            {
                var ruleEngine = new TextProcessor.RuleEngine();
                text = ruleEngine.ProcessText(text, rule);
            }

            try
            {
                // 启用编码格式
                if (checkBox_E_Encoding.Checked)
                {
                    // 覆盖原文件
                    if (checkBox_OverwriteTheOriginalFile.Checked)
                        File.WriteAllText(filePath, text, newEncoding);

                    // 创建新目录
                    else if (checkBox_NewDir.Checked)
                    {
                        string NewPath = @$"{Path.GetDirectoryName(filePath)}\{textBox_OverwriteTheOriginalFile.Text}";

                        Directory.CreateDirectory(NewPath);
                        File.WriteAllText(Path.Combine(NewPath, Path.GetFileName(filePath)), text, newEncoding);
                    }

                    // 前缀附加
                    else
                        File.WriteAllText(AddPrefixToFileName(filePath, textBox_OverwriteTheOriginalFile.Text), text, newEncoding);
                }

                // 禁用编码格式
                else
                {
                    // 覆盖原文件
                    if (checkBox_OverwriteTheOriginalFile.Checked)
                        File.WriteAllText(filePath, text);

                    // 创建新目录
                    else if (checkBox_NewDir.Checked)
                    {
                        string NewPath = @$"{Path.GetDirectoryName(filePath)}\{textBox_OverwriteTheOriginalFile.Text}";

                        Directory.CreateDirectory(NewPath);
                        File.WriteAllText(Path.Combine(NewPath, Path.GetFileName(filePath)), text);
                    }

                    // 前缀附加
                    else
                        File.WriteAllText(AddPrefixToFileName(filePath, textBox_OverwriteTheOriginalFile.Text), text);
                }

                if (checkBox_AllPrint.Checked)
                    MessageBox.Show($"文件 {filePath} 转换完成！", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{filePath} 转换失败，请联系开发者。\n原因：{ex.Message}\n\n完整报错：{ex}", "报错", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;

        }

        private void checkBox_E_Encoding_CheckedChanged(object sender, EventArgs e) =>
            comboBox_Encoding.Enabled = checkBox_E_Encoding.Checked;

        private void checkBox_OverwriteTheOriginalFile_CheckedChanged(object sender, EventArgs e)
        {
            textBox_OverwriteTheOriginalFile.Enabled = !checkBox_OverwriteTheOriginalFile.Checked;
            checkBox_NewDir.Enabled = !checkBox_OverwriteTheOriginalFile.Checked;
        }

        private void button_ManualPath_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox_ManualPath.Text))
            {
                selectedFiles = [textBox_ManualPath.Text];
                SetListBox(checkBox_ShowName.Checked);
            }
        }

        private void button_DeleteFile_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("此操作无法撤销！", "警告", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (string path in GetselectedFiles())
                {
                    try
                    {
                        File.Delete(path);

                        if (checkBox_AllPrint.Checked)
                            MessageBox.Show($"文件 {path} 删除成功！", "提示");
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"文件 {path} 删除失败！\n\n原因：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                MessageBox.Show("删除操作已全部完成！", "提示");
                listBox.Items.Clear();
                ReSetData();
            }
        }

        private void button_RunDelete_Click(object sender, EventArgs e)
        {
            // 智能模式
            if (radioButton_DeleteMode_Auto.Checked)
            {
                foreach (string path in GetselectedFiles())
                {
                    Log.Info($"尝试删除文件 {path}");
                    for (int i = 0; i < 10; i++)
                    {
                        HandleFile(path);

                        bool FileExists = !File.Exists(path);
                        Log.Info($"文件是否已真正删除：{FileExists}");

                        // 验证文件是否存在
                        if (FileExists)
                        {
                            break;
                        }
                    }
                        
                }
                listBox.Items.Clear();
                MessageBox.Show("删除执行完成", "提示");
            }
        }

        public static bool HandleFile(string filePath)
        {
            try
            {
                // 1. 检查文件是否存在
                if (!File.Exists(filePath))
                {
                    System.Console.WriteLine($"文件不存在: {filePath}");
                    return false;
                }

                // 2. 获取文件扩展名并判断是否为exe
                string extension = Path.GetExtension(filePath).ToLower();
                bool isExe = extension == ".exe";

                if (isExe)
                {
                    // 3. 获取文件名（不带路径）
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                    // 4. 查找同名进程
                    Process[] processes = Process.GetProcessesByName(fileNameWithoutExtension);

                    if (processes.Length > 0)
                    {
                        Log.Info($"找到 {processes.Length} 个进程名为 '{fileNameWithoutExtension}' 的进程");

                        for (int i = 0; i < 10; i++)
                        {
                            // 5. 结束所有找到的进程
                            foreach (Process process in processes)
                            {
                                try
                                {
                                    process.Kill();
                                    process.WaitForExit(5000); // 等待进程结束，最多5秒
                                    Log.Info($"已结束进程: {process.ProcessName} (PID: {process.Id})");
                                }
                                catch (Exception ex)
                                {
                                    Log.Info($"无法结束进程 {process.ProcessName}: {ex.Message}");
                                    return false;
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        Log.Info($"未找到进程名为 '{fileNameWithoutExtension}' 的进程");
                    }
                }

                // 6. 删除文件
                File.Delete(filePath);
                Log.Info($"文件已删除执行完成: {filePath}");
                return true;
            }
            catch (Exception ex)
            {
                Log.Info($"处理文件时出错: {ex.Message}");
                return false;
            }
        }

        private void checkBox_MaxPath_Create_FakeMode_CheckedChanged(object sender, EventArgs e)
        {
            textBox_MaxPath_Path.Enabled = !checkBox_MaxPath_Create_FakeMode.Checked;
            if (checkBox_MaxPath_Create_FakeMode.Checked)
                textBox_MaxPath_Path.Text = @"C:\";
        }

        private void button_CopyFileList_Click(object sender, EventArgs e)
        {
            if (selectedFiles != null)
            {
                Clipboard.SetText(string.Join(Environment.NewLine, selectedFiles));
                MessageBox.Show("已复制至剪切板", "提示");
            }
            else
                MessageBox.Show("列表为空", "提示");
        }

        private void button_Comparison_Click(object sender, EventArgs e)
        {
            try
            {
                string[] selectedFiles = GetselectedFiles();
                ProcessFiles(selectedFiles);
                MessageBox.Show("处理完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ProcessFiles(string[] aFiles)
        {
            if (aFiles == null || aFiles.Length == 0)
            {
                MessageBox.Show("A类文件为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 解析规则
            var rules = ParseRules(richTextBox_Comparison.Text);

            // 获取B类文件
            var bFiles = new List<string>();
            foreach (var item in listBox_Comparison.Items)
                bFiles.Add(item.ToString());

            // 获取输出目录
            string outputFolder = rules.ContainsKey("Out") ? rules["Out"] : "Out";
            string firstFileDir = Path.GetDirectoryName(aFiles[0]);
            string outputPath = Path.Combine(firstFileDir, outputFolder);

            // 创建输出目录
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            // 根据规则类型处理文件
            switch (rules["Type"].ToUpper())
            {
                case "IC":
                    ProcessICRule(aFiles, bFiles, outputPath, rules["Value"]);
                    break;
                default:
                    MessageBox.Show($"未知的规则类型: {rules["Type"]}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private Dictionary<string, string> ParseRules(string rulesText)
        {
            var rules = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var lines = rulesText.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split([':'], 2);
                if (parts.Length != 2) continue;

                string key = parts[0].Trim();
                string value = parts[1].Trim();

                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    rules[key] = value;
                }
            }

            // 验证必要规则
            if (!rules.ContainsKey("Type"))
            {
                throw new Exception("规则中缺少Type定义");
            }

            if (!rules.ContainsKey("Value"))
            {
                throw new Exception("规则中缺少Value定义");
            }

            return rules;
        }

        private void ProcessICRule(string[] aFiles, List<string> bFiles, string outputPath, string pattern)
        {
            foreach (string aFile in aFiles)
            {
                string aFileName = Path.GetFileName(aFile);

                // 从A文件名中提取比对内容
                string aContent = ExtractContent(aFileName, pattern);
                if (aContent == null) continue;

                foreach (string bFile in bFiles)
                {
                    string bFileName = Path.GetFileName(bFile);

                    // 从B文件名中提取比对内容
                    string bContent = ExtractContent(bFileName, pattern);
                    if (bContent == null) continue;

                    // 比对内容
                    if (aContent.Equals(bContent, StringComparison.OrdinalIgnoreCase))
                    {
                        // 匹配成功，复制文件
                        string destPath = Path.Combine(outputPath, bFileName);
                        File.Copy(aFile, destPath, true);
                        break; // 找到一个匹配即可
                    }
                }
            }
        }

        private string ExtractContent(string fileName, string pattern)
        {
            //// 处理《#》模式
            //if (pattern == "《#》")
            //{
            //    var match = Regex.Match(fileName, @"《(.+?)》");
            //    return match.Success ? match.Groups[1].Value : null;
            //}

            // 处理模式
            if (pattern.Contains("#"))
            {
                string[] parts = pattern.Split('#');
                if (parts.Length != 2) return null;

                string start = Regex.Escape(parts[0]);
                string end = Regex.Escape(parts[1]);

                var regex = new Regex($"{start}(.+?){end}");
                var match = regex.Match(fileName);

                return match.Success ? match.Groups[1].Value : null;
            }

            // 可以添加其他模式处理
            return null;
        }

        private void button_Comparison_UseOpenDialog_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                foreach (string name in openFileDialog.FileNames)
                    listBox_Comparison.Items.Add(name);
        }

        // 读取 EXIF
        private void button_ReadImgEXIF_Click(object sender, EventArgs e)
        {
            // 经纬度 海拔
            textBox_EXIF_GpsGpsLatitude.Text = string.Empty;
            textBox_EXIF_GpsGpsLatitudeRef.Text = string.Empty;
            textBox_EXIF_GpsLongitude.Text = string.Empty;
            textBox_EXIF_GpsLongitudeRef.Text = string.Empty;
            textBox_EXIF_GpsAltitude.Text = string.Empty;

            textBox_EXIF_0x9003.Text = string.Empty;

            try
            {
                // VivoLocationReader.ExampleUsage();

                using (System.Drawing.Image image = System.Drawing.Image.FromFile(GetselectedFile()))
                {
                    // 获取属性项列表
                    PropertyItem[] propItems = image.PropertyItems;

                    // 获取 GPS 信息
                    GpsData gpsInfo = GetImageGpsInfo(image);

                    if (gpsInfo != null)
                    {
                        // 纬度
                        textBox_EXIF_GpsGpsLatitude.Text = gpsInfo.Latitude.ToString() ?? "无数据";
                        textBox_EXIF_GpsGpsLatitudeRef.Text = gpsInfo.LatitudeRef ?? "无数据";

                        // 经度
                        textBox_EXIF_GpsLongitude.Text = gpsInfo.Longitude.ToString() ?? "无数据";
                        textBox_EXIF_GpsLongitudeRef.Text = gpsInfo.LongitudeRef ?? "无数据";

                        // 海拔
                        textBox_EXIF_GpsAltitude.Text = gpsInfo.Altitude.HasValue ? gpsInfo.Altitude.Value + "米" : "无数据";
                    }

                    button_OpenAmapWithGps.Enabled = gpsInfo != null;

                    // 其他信息获取
                    foreach (PropertyItem item in propItems)
                    {
                        switch (item.Id)
                        {
                            case 0x9003:
                                textBox_EXIF_0x9003.Text = ASCII_SG(item.Value);
                                break;
                        }

                    }
                }
            }

            catch
            {

            }

            finally
            {
                button_SaveImgEXIF.Enabled = true;
            }
        }

        // 写入 EXIF
        private void button_SaveImgEXIF_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromFile(GetselectedFile()))
                {
                    // 获取属性项列表
                    PropertyItem[] propItems = image.PropertyItems;

                    // 创建新的PropertyItem来存储修改后的数据
                    PropertyItem propItem = image.PropertyItems[0]; // 复制一个现有的作为模板

                    // 设置属性ID (EXIF标签ID)
                    propItem.Id = 0x9003;

                    // 设置数据类型 (1 = byte, 2 = ASCII字符串, 3 = short, 4 = long等)
                    propItem.Type = 2; // ASCII字符串

                    // 将字符串转换为字节数组并添加null终止符
                    byte[] valueBytes = Encoding.ASCII.GetBytes(textBox_EXIF_0x9003.Text + "\0");
                    propItem.Value = valueBytes;

                    // 设置值的长度
                    propItem.Len = valueBytes.Length;

                    // 将修改后的属性项添加回图像
                    image.SetPropertyItem(propItem);

                    // 保存图像 (可能需要使用新的文件名)
                    image.Save(GetselectedFile() + "_modified.jpg", ImageFormat.Jpeg);
                }
            }

            catch
            {

            }
        }



        // ASCII 字符串 转 String
        static string ASCII_SG(byte[] item)
        {
            if (item == null)
                return "null";

            return
                Encoding.ASCII
                    .GetString(item)
                        .TrimEnd('\0');
        }

        public GpsData GetImageGpsInfo(System.Drawing.Image image)
        {
            // EXIF GPS标签ID
            const int PropertyTagGpsLatitudeRef = 0x0001;
            const int PropertyTagGpsLatitude = 0x0002;
            const int PropertyTagGpsLongitudeRef = 0x0003;
            const int PropertyTagGpsLongitude = 0x0004;
            const int PropertyTagGpsAltitudeRef = 0x0005;
            const int PropertyTagGpsAltitude = 0x0006;

            GpsData gpsData = new GpsData();
            bool hasData = false;

            // 读取纬度信息
            if (Array.Exists(image.PropertyItems, p => p.Id == PropertyTagGpsLatitudeRef) &&
                Array.Exists(image.PropertyItems, p => p.Id == PropertyTagGpsLatitude))
            {
                PropertyItem latRef = image.GetPropertyItem(PropertyTagGpsLatitudeRef);
                PropertyItem lat = image.GetPropertyItem(PropertyTagGpsLatitude);

                gpsData.LatitudeRef = Encoding.ASCII.GetString(latRef.Value).TrimEnd('\0');
                gpsData.Latitude = ConvertGpsCoordinate(lat.Value);
                hasData = true;
            }

            // 读取经度信息
            if (Array.Exists(image.PropertyItems, p => p.Id == PropertyTagGpsLongitudeRef) &&
                Array.Exists(image.PropertyItems, p => p.Id == PropertyTagGpsLongitude))
            {
                PropertyItem lonRef = image.GetPropertyItem(PropertyTagGpsLongitudeRef);
                PropertyItem lon = image.GetPropertyItem(PropertyTagGpsLongitude);

                gpsData.LongitudeRef = Encoding.ASCII.GetString(lonRef.Value).TrimEnd('\0');
                gpsData.Longitude = ConvertGpsCoordinate(lon.Value);
                hasData = true;
            }

            // 读取海拔信息
            if (Array.Exists(image.PropertyItems, p => p.Id == PropertyTagGpsAltitudeRef) &&
                Array.Exists(image.PropertyItems, p => p.Id == PropertyTagGpsAltitude))
            {
                PropertyItem altRef = image.GetPropertyItem(PropertyTagGpsAltitudeRef);
                PropertyItem alt = image.GetPropertyItem(PropertyTagGpsAltitude);

                byte refValue = altRef.Value[0]; // 0表示海平面以上，1表示以下
                gpsData.Altitude = ConvertGpsAltitude(alt.Value, refValue);
                hasData = true;
            }

            return hasData ? gpsData : null;
        }

        // 将GPS坐标从EXIF格式转换为十进制
        private static double ConvertGpsCoordinate(byte[] value)
        {
            // EXIF存储的是三个有理数(度,分,秒)
            uint degreesNumerator = BitConverter.ToUInt32(value, 0);
            uint degreesDenominator = BitConverter.ToUInt32(value, 4);
            uint minutesNumerator = BitConverter.ToUInt32(value, 8);
            uint minutesDenominator = BitConverter.ToUInt32(value, 12);
            uint secondsNumerator = BitConverter.ToUInt32(value, 16);
            uint secondsDenominator = BitConverter.ToUInt32(value, 20);

            double degrees = degreesNumerator / (double)degreesDenominator;
            double minutes = minutesNumerator / (double)minutesDenominator;
            double seconds = secondsNumerator / (double)secondsDenominator;

            return degrees + (minutes / 60.0) + (seconds / 3600.0);
        }

        // 转换海拔高度
        private static double? ConvertGpsAltitude(byte[] value, byte refValue)
        {
            if (value == null || value.Length < 8) return null;

            uint numerator = BitConverter.ToUInt32(value, 0);
            uint denominator = BitConverter.ToUInt32(value, 4);

            double altitude = numerator / (double)denominator;
            return refValue == 0 ? altitude : -altitude;
        }

        // 高德地图
        public static void OpenAmapWithGps(GpsData gpsData, bool showDialog = true)
        {
            if (gpsData == null)
            {
                MessageBox.Show("没有可用的GPS数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 确保坐标方向正确
            double lat = gpsData.LatitudeRef == "S" ? -gpsData.Latitude : gpsData.Latitude;
            double lon = gpsData.LongitudeRef == "W" ? -gpsData.Longitude : gpsData.Longitude;

            // 高德地图URL格式: https://uri.amap.com/marker?position=经度,纬度
            string amapUrl = $"https://uri.amap.com/marker?position={lon},{lat}";

            // 可选: 添加位置名称标记
            // amapUrl += "&name=拍摄地点";

            if (showDialog)
            {
                var result = MessageBox.Show($"要在高德地图中查看此位置吗?\n纬度: {lat}\n经度: {lon}",
                                            "打开高德地图",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            try
            {
                // 使用默认浏览器打开链接
                Process.Start(new ProcessStartInfo
                {
                    FileName = amapUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开高德地图: {ex.Message}", "错误",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class GpsData
        {
            public double Latitude { get; set; }
            public string LatitudeRef { get; set; } // "N" 或 "S"
            public double Longitude { get; set; }
            public string LongitudeRef { get; set; } // "E" 或 "W"
            public double? Altitude { get; set; } // 海拔高度(米)
        }

        private void button_OpenAmapWithGps_Click(object sender, EventArgs e) =>
            OpenAmapWithGps(gpsInfo);

        private void FilesOperationPlus_Load(object sender, EventArgs e)
        {

        }

        private async void button_JsonLanguage_Run_Click(object sender, EventArgs e)
        {
            // 禁用按钮防止重复点击
            button_JsonLanguage_Run.Enabled = false;

            try
            {
                if (listBox_JsonLanguage.SelectedItem == null)
                {
                    AppendLog("❌ 请先在列表框中选择第二个JSON文件");
                    return;
                }

                string path2 = listBox_JsonLanguage.SelectedItem.ToString();

                if (!File.Exists(path2))
                {
                    AppendLog($"❌ 文件不存在: {path2}");
                    return;
                }

                var selectedFiles = GetselectedFiles();
                if (selectedFiles.Count() == 0)
                {
                    AppendLog("❌ 请选择要合并的第一个JSON文件");
                    return;
                }

                AppendLog($"🔧 开始批量合并JSON文件...");
                AppendLog($"目标文件: {path2}");
                AppendLog($"输出目录: {Path.GetDirectoryName(path2)}");
                AppendLog("=".PadRight(50, '='));

                // 异步执行合并操作
                await Task.Run(() =>
                {
                    foreach (string path in selectedFiles)
                    {
                        // 异步执行单个文件合并
                        Task.Run(() => MergeJsonFilesAsync(path, path2, Path.Combine(Path.GetDirectoryName(path2), $"Output_{Path.GetFileNameWithoutExtension(path)}.json")))
                            .ContinueWith(task =>
                            {
                                if (task.IsFaulted)
                                {
                                    Invoke((MethodInvoker)delegate
                                    {
                                        AppendLog($"❌ 处理失败 {Path.GetFileName(path)}: {task.Exception?.InnerException?.Message}");
                                    });
                                }
                            });
                    }
                });

                AppendLog("=".PadRight(50, '='));
                AppendLog("✅ 所有合并任务已提交，正在后台处理...");
            }
            catch (Exception ex)
            {
                AppendLog($"❌ 发生错误: {ex.Message}");
            }
            finally
            {
                // 重新启用按钮
                button_JsonLanguage_Run.Enabled = true;
            }
        }

        private void button_JsonLanguage_Open_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string name in openFileDialog.FileNames)
                {
                    // 检查是否已存在
                    if (!listBox_JsonLanguage.Items.Contains(name))
                        listBox_JsonLanguage.Items.Add(name);
                }
            }
        }

        // 异步合并JSON文件
        private async Task MergeJsonFilesAsync(string file1, string file2, string outputFile)
        {
            try
            {
                // 更新UI显示开始处理
                Invoke((MethodInvoker)delegate
                {
                    AppendLog($"🔄 开始处理: {Path.GetFileName(file1)}");
                });

                // 异步读取文件
                string json1Content = await Task.Run(() => File.ReadAllText(file1));
                string json2Content = await Task.Run(() => File.ReadAllText(file2));

                // 异步解析JSON
                JObject obj1 = await Task.Run(() => JObject.Parse(json1Content));
                JObject obj2 = await Task.Run(() => JObject.Parse(json2Content));

                // 创建输出对象
                JObject output = new JObject();
                int replacedCount = 0;
                int totalCount = 0;

                // 处理每个键
                foreach (var property in obj1.Properties())
                {
                    totalCount++;
                    string key = property.Name;

                    if (obj2.TryGetValue(key, out JToken value2))
                    {
                        output[key] = value2;
                        replacedCount++;
                    }
                    else
                    {
                        output[key] = property.Value;
                    }
                }

                // 异步写入文件
                string outputJson = output.ToString(Formatting.Indented);
                await Task.Run(() => File.WriteAllText(outputFile, outputJson));

                // 更新UI显示结果
                Invoke((MethodInvoker)delegate
                {
                    AppendLog($"✅ 完成: {Path.GetFileName(file1)}");
                    AppendLog($"   替换: {replacedCount}/{totalCount} 输出: {Path.GetFileName(outputFile)}");
                });
            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker)delegate
                {
                    AppendLog($"❌ 处理失败 {Path.GetFileName(file1)}: {ex.Message}");
                });
            }
        }

        // 批量处理的优化版本（并行处理）
        private async void button_JsonLanguage_RunParallel_Click(object sender, EventArgs e)
        {
            button_JsonLanguage_Run.Enabled = false;

            try
            {
                if (listBox_JsonLanguage.SelectedItem == null)
                {
                    AppendLog("❌ 请先在列表框中选择第二个JSON文件");
                    return;
                }

                string path2 = listBox_JsonLanguage.SelectedItem.ToString();

                var selectedFiles = GetselectedFiles();
                if (selectedFiles.Count() == 0)
                {
                    AppendLog("❌ 请选择要合并的第一个JSON文件");
                    return;
                }

                AppendLog($"🔧 开始并行处理 {selectedFiles.Count()} 个文件...");
                AppendLog($"参考文件: {Path.GetFileName(path2)}");
                AppendLog("=".PadRight(50, '='));

                // 创建任务列表
                var tasks = new List<Task>();
                int processed = 0;
                int total = selectedFiles.Count();

                // 并行处理每个文件（限制并发数）
                var parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount // 根据CPU核心数限制并发
                };

                await Task.Run(() =>
                {
                    Parallel.ForEach(selectedFiles, parallelOptions, (file) =>
                    {
                        var outputFile = Path.Combine(Path.GetDirectoryName(path2),
                            $"Output_{Path.GetFileNameWithoutExtension(file)}.json");

                        var task = MergeJsonFilesParallelAsync(file, path2, outputFile);
                        tasks.Add(task);

                        // 等待任务完成（为了控制并发）
                        task.Wait();

                        // 更新进度
                        Interlocked.Increment(ref processed);
                        Invoke((MethodInvoker)delegate
                        {
                            AppendLog($"📊 进度: {processed}/{total}");
                        });
                    });
                });

                // 等待所有任务完成
                await Task.WhenAll(tasks);

                AppendLog("=".PadRight(50, '='));
                AppendLog($"✅ 全部完成！共处理 {total} 个文件");
            }
            catch (Exception ex)
            {
                AppendLog($"❌ 发生错误: {ex.Message}");
            }
            finally
            {
                button_JsonLanguage_Run.Enabled = true;
            }
        }

        // 并行处理的合并方法
        private async Task MergeJsonFilesParallelAsync(string file1, string file2, string outputFile)
        {
            try
            {
                // 异步读取两个文件
                var readTasks = new Task<string>[]
                {
            Task.Run(() => File.ReadAllText(file1)),
            Task.Run(() => File.ReadAllText(file2))
                };

                var contents = await Task.WhenAll(readTasks);

                // 异步解析JSON
                var parseTasks = new Task<JObject>[]
                {
            Task.Run(() => JObject.Parse(contents[0])),
            Task.Run(() => JObject.Parse(contents[1]))
                };

                var jsons = await Task.WhenAll(parseTasks);

                var obj1 = jsons[0];
                var obj2 = jsons[1];

                // 处理合并
                var output = new JObject();
                int replaced = 0;

                foreach (var property in obj1.Properties())
                {
                    if (obj2.TryGetValue(property.Name, out JToken value2))
                    {
                        output[property.Name] = value2;
                        replaced++;
                    }
                    else
                    {
                        output[property.Name] = property.Value;
                    }
                }

                // 异步写入文件
                await Task.Run(() =>
                {
                    File.WriteAllText(outputFile, output.ToString(Formatting.Indented));
                });

                Invoke((MethodInvoker)delegate
                {
                    AppendLog($"✓ {Path.GetFileName(file1)}: {replaced}/{obj1.Count}");
                });
            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker)delegate
                {
                    AppendLog($"✗ {Path.GetFileName(file1)}: {ex.Message}");
                });
            }
        }

        // 添加日志到 richTextBox（线程安全）
        private void AppendLog(string message)
        {
            if (richTextBox_JsonLanguage.InvokeRequired)
            {
                richTextBox_JsonLanguage.Invoke(new Action<string>(AppendLog), message);
                return;
            }

            // 添加时间戳
            string logMessage = $"[{DateTime.Now:HH:mm:ss}] {message}\n";

            // 添加到文本框
            richTextBox_JsonLanguage.AppendText(logMessage);

            // 自动滚动到底部
            richTextBox_JsonLanguage.SelectionStart = richTextBox_JsonLanguage.Text.Length;
            richTextBox_JsonLanguage.ScrollToCaret();

            // 确保UI更新
            Application.DoEvents();
        }

        // 清空日志
        private void button_ClearLog_Click(object sender, EventArgs e)
        {
            richTextBox_JsonLanguage.Clear();
        }
    }
}

public class VivoLocationReader
{
    // 定义可能的属性标签
    private const int PropertyTagImageDescription = 0x010E;
    private const int PropertyTagExifUserComment = 0x9286;
    private const int PropertyTagXMP = 0x02BC;
    private const int PropertyTagGPSArea = 0x8824; // 可能存储地点信息

    public static string GetVivoLocation(string imagePath)
    {
        try
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath))
            {
                // 1. 检查XMP数据
                string location = ReadXmpData(image);
                if (!string.IsNullOrEmpty(location))
                    return location;

                // 2. 检查图像描述
                location = ReadProperty(image, PropertyTagImageDescription);
                if (IsValidLocation(location))
                    return location;

                // 3. 检查用户评论
                location = ReadProperty(image, PropertyTagExifUserComment);
                if (IsValidLocation(location))
                    return location;

                // 4. 检查GPS区域信息
                location = ReadProperty(image, PropertyTagGPSArea);
                if (IsValidLocation(location))
                    return location;

                return "未找到地点信息";
            }
        }
        catch (Exception ex)
        {
            return $"读取失败: {ex.Message}";
        }
    }

    private static string ReadXmpData(System.Drawing.Image image)
    {
        if (!Array.Exists(image.PropertyItems, p => p.Id == PropertyTagXMP))
            return null;

        PropertyItem xmpProp = image.GetPropertyItem(PropertyTagXMP);
        string xmpData = Encoding.UTF8.GetString(xmpProp.Value);

        // 查找VIVO特有的XMP字段
        if (xmpData.Contains("<vivo:Location>"))
        {
            int start = xmpData.IndexOf("<vivo:Location>") + 15;
            int end = xmpData.IndexOf("</vivo:Location>", start);
            if (end > start)
                return xmpData.Substring(start, end - start);
        }

        // 查找标准XMP位置字段
        if (xmpData.Contains("<dc:subject>"))
        {
            int start = xmpData.IndexOf("<dc:subject>") + 12;
            int end = xmpData.IndexOf("</dc:subject>", start);
            if (end > start)
            {
                string subject = xmpData.Substring(start, end - start);
                if (subject.Contains("中国") || subject.Contains("省") || subject.Contains("市"))
                    return subject;
            }
        }

        return null;
    }

    private static string ReadProperty(System.Drawing.Image image, int propertyId)
    {
        if (!Array.Exists(image.PropertyItems, p => p.Id == propertyId))
            return null;

        PropertyItem prop = image.GetPropertyItem(propertyId);

        // 尝试UTF-8编码
        try
        {
            string utf8Value = Encoding.UTF8.GetString(prop.Value).TrimEnd('\0');
            if (IsValidLocation(utf8Value))
                return utf8Value;
        }
        catch { }

        // 尝试GB2312编码（VIVO常用）
        try
        {
            string gbValue = Encoding.GetEncoding("GB2312").GetString(prop.Value).TrimEnd('\0');
            if (IsValidLocation(gbValue))
                return gbValue;
        }
        catch { }

        return null;
    }

    private static bool IsValidLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            return false;

        // 检查是否包含典型的地点关键词
        return location.Contains("中国") ||
               location.Contains("省") ||
               location.Contains("市") ||
               location.Contains("区") ||
               location.Contains("县");
    }

    // 使用示例
    public static void ExampleUsage()
    {
        string imagePath = "D:\\UserDir\\Pictures\\123.jpg";
        string location = GetVivoLocation(imagePath);

        MessageBox.Show($"拍摄地点: {location}", "地点信息");

        // 在高德地图中打开
        if (location != "未找到地点信息" && !location.StartsWith("读取失败"))
        {
            string amapUrl = $"https://www.amap.com/search?query={Uri.EscapeDataString(location)}";
            try
            {
                Process.Start(amapUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开地图: {ex.Message}", "错误");
            }
        }
    }
}