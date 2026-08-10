using QisToolkit3.Forms.YtDlp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static Qi;
using static Qi.QisToolkit3_Datas;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QisToolkit3.Forms
{
    public partial class YtDlpTool : Form
    {
        private Process process;
        private RichTextBox outputBox;
        private IdNameMapper _idNameMapper;
        private List<string> _matchFiltersRules = new List<string>();
        public static event Action MatchFiltersSaved;
        //private FileSystemWatcher _matchFiltersWatcher;
        //private System.Windows.Forms.Timer _reloadDebounceTimer;  // 用于防抖
        //private bool _isReloading = false;  // 防止并发重载

        // 文件路径
        private static string YtDlpPath = Path.Combine(actualDirectory, "yt-dlp");
        private static string DefaultDownloadPath = Path.Combine(YtDlpPath, "Downloads");
        private static string YtDlpExePath = Path.Combine(YtDlpPath, "yt-dlp.exe");
        private static string DanmakuFactoryPath = Path.Combine(YtDlpPath, "DanmakuFactory.exe");
        private static string CookiesFilePath = Path.Combine(YtDlpPath, "cookies.txt");
        private static string AutoDownloadFilePath = Path.Combine(YtDlpPath, "AutoDownload.txt");
        private static string AutoDownloadNameFilePath = Path.Combine(YtDlpPath, "AutoDownloadName.txt");
        private static string HeadersFilePath = Path.Combine(YtDlpPath, "Headers.txt");
        private static string DefaultConfigFilePath = Path.Combine(YtDlpPath, "DefaultConfig.xml");
        private static string AutoDownloadConfigFilePath = Path.Combine(YtDlpPath, "AutoDownloadConfig.txt");
        public static string MatchFiltersPath = Path.Combine(YtDlpPath, "MatchFilters.txt");
        public static string YtDlpWorkDirPath = YtDlpPath;


        public YtDlpTool()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
            comboBox_StringRuleEngine.SelectedIndex = 0;

            //if (YtDlpPath.Contains(' '))
            //{
            //    Log.Err($"[YtDlp工具] 齐的工具包3路径包含空格或特殊符号，无法运行YtDlp。");
            //    MessageBox.Show("齐的工具包3路径包含空格或特殊符号，无法运行YtDlp。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    button_DoDownload.Enabled = false;
            //    button_AutoDownload.Enabled = false;
            //    button_DoAnalysis.Enabled = false;
            //    button_Update.Enabled = false;
            //    button_Stop.Enabled = false;
            //}

            if (!File.Exists(actualDirectory + @"\yt-dlp\ffmpeg.exe"))
            {
                Log.Warn($"[YtDlp工具] 工具环境缺失！这可能会导致一些问题。");
                MessageBox.Show("环境缺失！这可能会导致一些问题", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            outputBox = richTextBox;
        }

        private async void YtDlpTool_Load(object sender, EventArgs e)
        {
            // 下载存放路径
            //textBox_Paths.Text = DefaultDownloadPath;
            //comboBox_Path_Home.Text = DefaultDownloadPath;
            //comboBox_Path_Temp.Text = DefaultDownloadPath;
            //comboBox_Path_Video.Text = DefaultDownloadPath;
            //comboBox_Path_Audio.Text = DefaultDownloadPath;
            //comboBox_Path_SubTitle.Text = DefaultDownloadPath;
            //comboBox_Path_Description.Text = DefaultDownloadPath;
            //comboBox_Path_Thumbnail.Text = DefaultDownloadPath;
            //comboBox_Path_InfoJson.Text = DefaultDownloadPath;

            // .netrc 文件
            textBox_netrc_Location.Text = Path.Combine(actualDirectory, @$"yt-dlp") + '\\';

            button_AutoDownload.Enabled = File.Exists(comboBox_URL.Text) || File.Exists(AutoDownloadFilePath);
            checkBox_IdNameMapper.Checked = File.Exists(AutoDownloadNameFilePath);

            int IsCookies = IsCookieFile(CookiesFilePath);
            checkBox_UseCookies.Checked = IsCookies == 1 || IsCookies == 2;

            comboBox_PlayListMode.SelectedIndex = 0;
            comboBox_OverWritesMode.SelectedIndex = 1;

            if (File.Exists(DefaultConfigFilePath))
                YtDlpConfigManager.ImportConfig(this, DefaultConfigFilePath);

            // 加载自动下载名称映射
            await LoadIdNameMapper(AutoDownloadNameFilePath);

            if (string.IsNullOrWhiteSpace(textBox_Paths.Text))
                textBox_Paths.Text = Path.Combine(actualDirectory, "yt-dlp", "Downloads");

            LoadMatchFilters();

            MatchFiltersSaved += () =>
            {
                if (this.InvokeRequired)
                    this.Invoke(new Action(LoadMatchFilters));
                else
                    LoadMatchFilters();
                Log.Info("[YtDlp工具] 收到规则已保存通知，已重新加载");
            };
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

        // 
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

        private bool Inspect()
        {
            if (NoST(comboBox_URL.Text) == string.Empty)
            {
                MessageBox.Show("URL不可为空", "错误");
                return false;
            }
            return true;
        }

        
        

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

        public void Stop()
        {
            process.Close();
            richTextBox.Text += "\n已终止进程。";
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

        
        

        /// <summary>
        /// 解析自动下载配置文件
        /// </summary>
        private List<AutoDownloadItem> ParseAutoDownloadConfig(string configPath)
        {
            var items = new List<AutoDownloadItem>();

            if (!File.Exists(configPath))
            {
                CreateDefaultAutoDownloadConfig(configPath);
                return items;
            }

            var lines = File.ReadAllLines(configPath);
            AutoDownloadItem currentItem = null;

            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();

                // 跳过空行
                if (string.IsNullOrEmpty(line))
                    continue;

                // 注释行（以 # 开头）
                if (line.StartsWith("#"))
                    continue;

                // 检查是否是 URL
                bool isUrl = line.StartsWith("http://") || line.StartsWith("https://") ||
                             (line.Contains("bilibili.com") && !line.Contains(':'));

                if (isUrl)
                {
                    // 如果有当前项，先添加到列表
                    if (currentItem != null && !string.IsNullOrEmpty(currentItem.Url))
                    {
                        items.Add(currentItem);
                    }

                    // 创建新项
                    currentItem = new AutoDownloadItem { Url = line };
                    continue;
                }

                // 解析属性（仅当有当前项时）
                if (currentItem != null && line.Contains(':'))
                {
                    var colonIndex = line.IndexOf(':');
                    var key = line.Substring(0, colonIndex).Trim().ToLowerInvariant();
                    var value = line.Substring(colonIndex + 1).Trim();

                    switch (key)
                    {
                        case "name":
                            currentItem.Name = value;
                            break;
                        case "matchfilters":
                            currentItem.MatchFilters = value.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                                                       value.Equals("1") ||
                                                       value.Equals("yes");
                            break;
                        case "playlist":
                            currentItem.Playlist = value;
                            break;
                        case "matchfiltersdata":
                            currentItem.MatchFiltersData = value;
                            break;
                    }
                }
            }

            // 添加最后一个项
            if (currentItem != null && !string.IsNullOrEmpty(currentItem.Url))
            {
                items.Add(currentItem);
            }

            // 输出解析结果日志
            Log.Info($"[YtDlp工具] 解析到 {items.Count} 个下载项");
            foreach (var item in items)
            {
                Log.Info($"  - {item.Url}");
                Log.Info($"    名称: {(string.IsNullOrEmpty(item.Name) ? "(自动解析)" : item.Name)}");
                Log.Info($"    MatchFilters: {item.MatchFilters}");
                Log.Info($"    Playlist: {(string.IsNullOrEmpty(item.Playlist) ? "(未设置)" : item.Playlist)}");
            }

            return items;
        }

        /// <summary>
        /// 创建默认的自动下载配置文件
        /// </summary>
        private void CreateDefaultAutoDownloadConfig(string configPath)
        {
            var defaultContent = @"# ============================================
# 自动下载配置文件
# ============================================
# 格式说明：
# 1. URL 单独一行
# 2. 可选参数：
#    Name: 自定义名称（不指定则自动从 URL 解析）
#    MatchFilters: true/false（是否启用匹配过滤器，默认 false）
#    MatchFiltersData: 指定规则数据（需要先启用匹配过滤器）
#       - 逻辑与: 使用 '&' 符连接
#       - 逻辑或: 使用 ';' 符分隔
#    Playlist: 下载指定播放列表（默认不设置）
#       - false: 不指定下载播放列表索引，下载完整列表。
#       - 数字: 指定索引，如 ""5"" 只下载第5个
#       - 范围: 如 ""1-10"" 下载第1到第10个
#       - 倒序: 如 ""-1:-1"" 只下载最后一个
#       - 复杂: 如 ""1,3,5-7,10"" 或 ""1:-1:2""（起始:结束:步进）
#       - 末尾: 如 ""-5:"" 最后5个，""::-2"" 所有偶数项倒序
#
# ============================================

# 师傅你变了
https://space.bilibili.com/3494379457087582/lists/4459703
Name: 《师傅你变了》
MatchFilters: false

# 甜橙盛夏
https://space.bilibili.com/697987209/lists/8033161
Name: 《甜橙盛夏》
MatchFilters: false
Playlist: false

# 姜糖恋语
https://space.bilibili.com/1411920158/lists/6668649
Name: 《姜糖恋语》

# 逆徒与逆师
https://space.bilibili.com/5538/lists/6715609
Name: 《逆徒与逆师》

# 师兄绝非反派
https://space.bilibili.com/3546830767917809/lists/4728895
Name: 《师兄绝非反派》

# 熙玥救赎
https://space.bilibili.com/3690975687870895/lists/7035241
Name: 《熙玥救赎》

# 我竟是伟大存在
https://space.bilibili.com/32160535/lists/5578590
Name: 《我竟是伟大存在》

# 不当人的选手
https://space.bilibili.com/589953538/lists/7712111
Name: 《不当人的选手》

# 御兽老大
https://space.bilibili.com/429403980/lists/2639451
Name: 《御兽老大》

# 我的灵根是系统
https://space.bilibili.com/3494369006979716/lists/6714915
Name: 《我的灵根是系统》

# 摆烂也能无敌第二季
https://space.bilibili.com/3546743408953776/lists/7706348
Name: 《摆烂也能无敌第二季》

# 无免之门（已注释，如需下载请取消注释）
# https://space.bilibili.com/3546697831549463/lists/5684545
# Name: 《无免之门》

# 遗忘世间
https://space.bilibili.com/71130413/lists/5525218
Name: 《遗忘世间》
";
            File.WriteAllText(configPath, defaultContent, Encoding.UTF8);
        }
    }
}



