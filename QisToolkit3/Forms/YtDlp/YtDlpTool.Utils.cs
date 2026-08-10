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


            //汉化提示
            if (checkBox_ChineseTip.Checked)
            {
                text = text.Replace("Destination", "目标");
                text = text.Replace("Danmaku Pool: ", "弹幕池：");
                text = text.Replace("Read timed out", "读取超时");
                text = text.Replace("Language Formats", "语言格式");
                text = text.Replace("Extracting URL", "正在提取URL");
                text = text.Replace("Writting file", "正在写入文件");
                text = text.Replace("Downloading item", "正在下载项目");
                text = text.Replace("Available formats for", "可用格式");
                text = text.Replace("HTTPSConnectionPool", "HTTPS连接池");
                text = text.Replace("Available subtitles for", "可用语言");
                text = text.Replace("Downloading webpage", "正在下载网页");
                text = text.Replace("Merging formats into", "将格式合并到");
                text = text.Replace("Adding thumbnail to", "正在添加缩略图");
                text = text.Replace("has already been downloaded", "已下载");
                text = text.Replace("EXT RESOLUTION FPS", "外部分辨率 帧率");
                text = text.Replace("File Loading Complete", "文件加载完成");
                text = text.Replace("Deleting original file", "删除原始文件");
                text = text.Replace("Downloading wbi sign", "正在下载WIB标志");
                text = text.Replace("Available thumbnails for ", "可用缩略图 ");
                text = text.Replace("Downloading playlist", "正在下载播放列表");
                text = text.Replace("Downloading playlist:", "正在下载播放列表：");
                text = text.Replace("Finished downloading playlist: ", "完成列表下载");
                text = text.Replace("Extracting videos in anthology", "正在选集中提取视频");
                text = text.Replace("Downloading video formats for cid", "正在下载cid视频格式");
                text = text.Replace("for how to manually pass cookies", "了解如何手动传递Cookie");
                text = text.Replace("has already been recorded in the archive", "已经记录在档案（黑名单）中");
                text = text.Replace("add --no-playlist to download just the video", "添加参数--no-playlist，将不再下载播放列表，只下载视频");
                text = text.Replace("are missing; you have to become a premium member to download them. Use --cookies-from-browser or --cookies for the authentication. See", "未寻获；你必须成为高级会员下载它们。使用浏览器中的Cookie或用于身份验证的Cookie。见");

                text = text.Replace("VBR ACODEC", "可变比特率音频编解码器");
                text = text.Replace("Extracting chapters", "提取章节");
                text = text.Replace("Loading files", "加载文件列表");
                text = text.Replace("video thumbnail", "视频缩略图");
                text = text.Replace("EmbedThumbnail", "嵌入缩略图");
                text = text.Replace("Loading file", "加载文件");
                text = text.Replace("Downloading", "正在下载");
                text = text.Replace("Output file", "输出文件");
                text = text.Replace("Input file", "输入文件");
                text = text.Replace("Playlist ", "播放列表");
                text = text.Replace("TBR PROTO", "TBR 协议");
                text = text.Replace("TimeShift", "时间偏移");
                text = text.Replace("Got error", "出现错误");
                text = text.Replace("VCODEC", "视频解码器");
                text = text.Replace("video only", "仅视频");
                text = text.Replace("audio only", "仅音频");
                text = text.Replace("FILESIZE", "文件大小");
                text = text.Replace("Exec", "外部程序调用");
                text = text.Replace("FileName ", "文件名");
                text = text.Replace("items of", "个项目");
                text = text.Replace("ABR", "可用比特率");
                text = text.Replace("download", "下载");
                text = text.Replace("Template", "模板");
                text = text.Replace("Retrying", "重试");
                text = text.Replace("Sorting", "排序");
                text = text.Replace("Unknown", "未知");
                text = text.Replace("unknown", "未知");
                text = text.Replace("Writing", "写入");
                text = text.Replace("Format", "格式");
                text = text.Replace("format", "格式");
                text = text.Replace("Merger", "合并");
                text = text.Replace("Height", "高度");
                text = text.Replace("Number", "编号");
                text = text.Replace("Width", "宽度");
                text = text.Replace("ERROR", "错误");
                text = text.Replace("info", "信息");
                text = text.Replace("Done", "完成");
                text = text.Replace("host", "主机");
                text = text.Replace("port", "端口");
                text = text.Replace("of", "总计");

                text = text.Replace("Unable to obtain version", "无法获取版本");
                text = text.Replace("certificate verify failed", "证书验证失败");
                text = text.Replace("unable to get local issuer certificate", "无法获取本地颁发者证书");
                text = text.Replace("Please try again later or visit", "请稍后再试或直接访问");
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
