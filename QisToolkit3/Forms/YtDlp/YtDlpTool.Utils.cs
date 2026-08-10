using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Qi;
using static Qi.QisToolkit3_Datas;

namespace QisToolkit3.Forms
{
    public partial class YtDlpTool
    {
        private void AppendText(string text, string form = "YtDlp")
        {
            if (string.IsNullOrEmpty(text)) return;


            // 提示汉化
            if (checkBox_ChineseTip.Checked)
            {
                // 按键的长度降序排序，确保长句优先（避免子串误替换）
                foreach (var kv in ChineseTranslationMap.OrderByDescending(k => k.Key.Length))
                {
                    text = text.Replace(kv.Key, kv.Value);
                }
            }



            // 跨线程更新UI控件
            if (outputBox.InvokeRequired)
            {
                outputBox.Invoke(new Action<string, string>(AppendText), text, form);
            }
            else
            {
                outputBox.AppendText(text + Environment.NewLine);
                outputBox.ScrollToCaret(); // 自动滚动到底部

                // 添加到日志
                Log.Info($"[{form}] {text}");
            }
        }



        private string ProcessExecArguments(string lines)
        {
            var result = new StringBuilder();
            // 按行分割，移除空行
            var lineArray = lines.Split(new[] { Environment.NewLine, "\n", "\r\n" },
                                         StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lineArray)
            {
                var trimmedLine = line.Trim();

                // 跳过空行
                if (string.IsNullOrEmpty(trimmedLine))
                    continue;

                // 跳过注释行
                if (trimmedLine.StartsWith('#') || trimmedLine.StartsWith("//"))
                    continue;

                // 检查是否已经被引号包裹
                bool isQuoted = trimmedLine.Length >= 2 &&
                                trimmedLine.StartsWith("\"") &&
                                trimmedLine.EndsWith("\"");

                trimmedLine = trimmedLine.Replace("%(Qi:ActualDirectory)", actualDirectory);
                trimmedLine = trimmedLine.Replace("%(Qi.YtDlpTool:Paths)", textBox_Paths.Text);
                trimmedLine = trimmedLine.Replace("\"", "\\\"");

                if (isQuoted)
                {
                    result.Append($" --exec {trimmedLine}");
                }
                else if (trimmedLine.Contains(' '))
                {
                    // 包含空格，需要加引号
                    result.Append($" --exec \"{trimmedLine}\"");
                }
                else
                {
                    result.Append($" --exec {trimmedLine}");
                }
            }

            return result.ToString();
        }

        public async Task LoadIdNameMapper(string mapFilePath)
        {
            _idNameMapper = new IdNameMapper(mapFilePath);
            try
            {
                await _idNameMapper.LoadMapAsync();
                Log.Info($"成功加载 {_idNameMapper.GetMapCount()} 个ID映射");
            }
            catch (Exception ex)
            {
                Log.Err($"加载映射文件失败: {ex.Message}");
                // 如果映射文件加载失败，仍然继续运行，只是不使用名称替换
                _idNameMapper = null;
            }
        }


        // 处理用户数据
        private void ProcessingUserData()
        {
            string containsText = string.Empty;

            if (checkBox_ReadOnClipboard.Checked)
            {
                containsText = GetContainsText();


                if (!string.IsNullOrWhiteSpace(containsText))
                    comboBox_URL.Text = containsText;
            }

            if (checkBox_ClearURLData.Checked)
                comboBox_URL.Text = ClearURLData(comboBox_URL.Text);
        }

        private string ClearURLData(string url)
        {
            string _url = string.Empty;

            if (url != string.Empty && url.Contains('&'))
                for (int i = 0; i < url.Length; i++)
                    if (url[i] == '&')
                        break;
                    else
                        _url += url[i];
            else
                return url;

            return _url;
        }

        private string GetContainsText()
        {
            try
            {
                if (!Clipboard.ContainsText())
                {
                    Log.Warn("[YtDlp工具] 剪贴板中没有文本内容。");
                    MessageBox.Show("剪贴板中没有文本内容。");
                    return null;
                }

                string text = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(text))
                {
                    Log.Warn("[YtDlp工具] 剪贴板文本为空。");
                    MessageBox.Show("剪贴板文本为空。");
                    return null;
                }

                // 匹配 URL（支持 http/https，可扩展其他协议）
                const string urlPattern = @"https?://[^\s]+";
                Match match = Regex.Match(text, urlPattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string url = match.Value;
                    Log.Info($"[YtDlp工具] 剪贴板文本成功提取 URL: {url}");
                    return url;
                }
                else
                {
                    Log.Warn("[YtDlp工具] 剪贴板中未找到有效的 URL。");
                    MessageBox.Show("剪贴板中未找到有效的 URL。");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[YtDlp工具] 访问剪贴板失败: {ex.Message}");
                MessageBox.Show($"访问剪贴板失败: {ex.Message}");
                return null;
            }
        }

        private void ProcessFiles(string[] files)
        {
            foreach (string file in files)
            {
                // 检查是否为文件（不是文件夹）
                if (File.Exists(file))
                {
                    Log.Info($"[YtDlp工具] 开始处理拖入的文件：{file}");

                    int type = IsCookieFile(file);
                    switch (type)
                    {
                        // 发生错误
                        case -1:
                            Log.Err($"[YtDlp工具] 处理拖入的文件 {file} 时出现意料之外的异常");
                            break;

                        // 文件不存在（按理来说不可能，已经验证过一次了）
                        case 0:
                            Log.Warn($"[YtDlp工具] 处理拖入的文件，因未知原因该文件不存在：{file}");
                            break;

                        // 文件头为 cookie 文件
                        case 1:
                            SetCookieFile(file);
                            break;

                        // 文件名为 cookie.txt
                        case 2:
                            Log.Info($"[YtDlp工具] 该文件的头行文本并非标准 # Netscape HTTP Cookie File");
                            var MsgBoxTmp = MessageBox.Show("该文件的头行并非标准的 # Netscape HTTP Cookie File，" +
                                                            "是否将其作为 Cookie 导入？", "YtDlp工具",
                                                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                            if (MsgBoxTmp == DialogResult.OK)
                                SetCookieFile(file);

                            else
                                Log.Info($"[YtDlp工具] 用户取消导入。");

                            break;

                        // 其他文件
                        case 3:
                            comboBox_URL.Text = file;
                            break;

                        default:
                            break;
                    }
                    Log.Info($"[YtDlp工具] 处理文件 {file}");
                }
                else
                {
                    MessageBox.Show("目前仅支持拖入文件，不支持拖入文件夹或其他内容");
                    Log.Warn("[YtDlp工具] 用户拖入了非文件或不存在内容");
                }
            }
        }

        private void SetCookieFile(string file)
        {
            try
            {
                File.WriteAllText(CookiesFilePath, File.ReadAllText(file));
                checkBox_UseCookies.Checked = true;

                Log.Info($"[YtDlp工具] 成功将Cookies文件 {file} 的内容导入 cookie.txt");
                MessageBox.Show($"成功将Cookies文件 {file} 的内容导入 cookie.txt", "YtDlp工具",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Err($"[YtDlp工具] 处理Cookies文件 {file} 时出现意料之外的异常，错误信息：{ex.Message}");
                MessageBox.Show($"文件 {file} 导入 cookie.txt 时出现异常\n\n错误信息：{ex.Message}",
                                 "YtDlp工具", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
         * 检查是否为 Cookie 文件
         * -1：异常
         *  0：文件不存在
         *  1：文件头行正确
         *  2：文件名正确
         *  3：其他文件
         */
        public int IsCookieFile(string filePath)
        {
            // 检查文件是否存在
            if (!File.Exists(filePath))
                return 0;

            // 检查文件名是否为 cookie.txt
            string fileName = Path.GetFileName(filePath);
            bool isCorrectName = fileName.Equals("cookie.txt", StringComparison.OrdinalIgnoreCase);

            // 检查第一行内容
            bool hasCorrectHeader = false;

            try
            {
                // 只读取第一行，不读取整个文件
                string firstLine = File.ReadLines(filePath).FirstOrDefault();
                hasCorrectHeader = firstLine?.Trim() == "# Netscape HTTP Cookie File";
            }
            catch (Exception ex)
            {
                // 处理读取异常
                Log.Err($"[YtDlp工具] 处理文件 {filePath} 时出现意料之外的异常，错误信息：{ex.Message}");
                return -1;
            }

            // 文件头行正确
            if (hasCorrectHeader)
                return 1;

            // 文件名正确
            else if (isCorrectName)
                return 2;

            // 其他文件
            return 3;
        }




        private bool Inspect()
        {
            if (NoST(comboBox_URL.Text) == string.Empty)
            {
                MessageBox.Show("URL不可为空", "错误");
                return false;
            }
            return true;
        }

        public void Stop()
        {
            process.Close();
            richTextBox.Text += "\n已终止进程。";
        }
    }
}
