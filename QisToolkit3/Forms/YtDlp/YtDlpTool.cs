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

        private static readonly Dictionary<string, string> ChineseTranslationMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "has already been recorded in the archive", "已经记录在档案（黑名单）中" },
            { "add --no-playlist to download just the video", "勾选参数【不下载播放列表】，将不再下载播放列表，只下载视频" },
            { "are missing; you have to become a premium member to download them. Use --cookies-from-browser or --cookies for the authentication. See", "未寻获；你必须成为高级会员下载它们。使用浏览器中的Cookie或用于身份验证的Cookie。见" },
            { "This video may be deleted or geo-restricted. You might want to try a VPN or a proxy server", "此视频可能已被删除或受地域限制。您不妨尝试使用 VPN 或代理服务器。" },
            { "for how to manually pass cookies", "了解如何手动传递Cookie" },
            { "Finished downloading playlist: ", "完成列表下载" },
            { "Downloading video formats for cid", "正在下载cid视频格式" },
            { "Extracting videos in anthology", "正在选集中提取视频" },
            { "has no automatic captions", "没有自动字幕" },
            { "has no subtitles", "没有手动字幕" },
            { "Available thumbnails for ", "可用缩略图 " },
            { "Available subtitles for", "可用语言" },
            { "Available formats for", "可用格式" },
            { "Downloading playlist:", "正在下载播放列表：" },
            { "Downloading playlist", "正在下载播放列表" },
            { "Downloading webpage", "正在下载网页" },
            { "Downloading item", "正在下载项目" },
            { "Merging formats into", "将格式合并到" },
            { "Adding thumbnail to", "正在添加缩略图" },
            { "Deleting original file", "删除原始文件" },
            { "Downloading wbi sign", "正在下载WIB标志" },
            { "File Loading Complete", "文件加载完成" },
            { "Extracting chapters", "提取章节" },
            { "Loading files", "加载文件列表" },
            { "Loading file", "加载文件" },
            { "video thumbnail", "视频缩略图" },
            { "EmbedThumbnail", "嵌入缩略图" },
            { "Output file", "输出文件" },
            { "Input file", "输入文件" },
            { "Playlist ", "播放列表" },
            { "TBR PROTO", "TBR 协议" },
            { "TimeShift", "时间偏移" },
            { "Got error", "出现错误" },
            { "VCODEC", "视频解码器" },
            { "video only", "仅视频" },
            { "audio only", "仅音频" },
            { "FILESIZE", "文件大小" },
            { "FileName ", "文件名" },
            { "items of", "个项目" },
            { "ABR", "可用比特率" },
            { "Template", "模板" },
            { "Retrying", "重试" },
            { "Sorting", "排序" },
            { "Writing", "写入" },
            { "Merger", "合并" },
            { "Height", "高度" },
            { "Number", "编号" },
            { "Width", "宽度" },
            { "ERROR", "错误" },
            { "Done", "完成" },
            { "host", "主机" },
            { "port", "端口" },
            { "of", "总计" },  // ⚠️ 小心：这个词会误替换 "offer" 等，建议保留原样或增加边界处理
            { "Downloading", "正在下载" },
            { "unknown", "未知" },
            { "format", "格式" },
            { "Exec", "外部程序调用" },
            { "download", "下载" },
            { "info", "信息" },

            // 其他长句
            { "Destination", "目标" },
            { "Danmaku Pool: ", "弹幕池：" },
            { "Read timed out", "读取超时" },
            { "Language Formats", "语言格式" },
            { "Extracting URL", "正在提取URL" },
            { "Writting file", "正在写入文件" },
            { "HTTPSConnectionPool", "HTTPS连接池" },
            { "EXT RESOLUTION FPS", "外部分辨率 帧率" },
            { "VBR ACODEC", "可变比特率音频编解码器" },
            { "Unable to obtain version", "无法获取版本" },
            { "certificate verify failed", "证书验证失败" },
            { "unable to get local issuer certificate", "无法获取本地颁发者证书" },
            { "Please try again later or visit", "请稍后再试或直接访问" },
        };


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

    }
}