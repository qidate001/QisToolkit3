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