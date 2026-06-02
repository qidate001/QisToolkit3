using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static Qi.QisToolkit3_Datas;

namespace QisToolkit3.Forms
{
    public partial class SoftwareDownload : Form
    {
        Software NowSoftware = new();

        public SoftwareDownload()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void SoftwareDownload_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(actualDirectory + @"\Datas\SoftwareDownload.zip"))
                {
                    using (var zip = ZipFile.OpenRead(actualDirectory + @"\Datas\SoftwareDownload.zip"))
                    {
                        int count = 0;
                        foreach (var entry in zip.Entries)
                        {
                            using Stream stream = entry.Open();
                            using StreamReader reader = new StreamReader(stream);

                            string name, owl, owdl_A, owdl_B, owdl_C, edl;

                            if ((name = reader.ReadLine()) == null) name = "未知软件"; 
                            if ((owl = reader.ReadLine()) == null) owl = string.Empty; 
                            if ((owdl_A = reader.ReadLine()) == null) owdl_A = string.Empty; 
                            if ((owdl_B = reader.ReadLine()) == null) owdl_B = string.Empty; 
                            if ((owdl_C = reader.ReadLine()) == null) owdl_C = string.Empty; 
                            if ((edl = reader.ReadLine()) == null) edl = string.Empty;


                            softwares[count] = new(name, owl, owdl_A, owdl_B, owdl_C, edl);
                            ++count;
                        }
                    }

                    /*
                    using (StreamReader sr = new StreamReader(@"Datas\SoftwareDownload.qidata"))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line[0] != '#' || line != string.Empty)
                            {

                            }
                        }
                    }
                    */
                }
                else
                    MessageBox.Show("程序列表文件丢失，请联系开发者解决问题\n丢失文件：Datas\\SoftwareDownload.zip");
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取程序列表文件失败，请联系开发者解决问题\n错误信息：" + ex.Message + "\n\n完整报错：\n" + ex);
            }
        }


        private Software[] softwares = new Software[3000];

        /*
        private Software[] softwares =
        {
            // A

            // B
            new("百度网盘", "https://pan.baidu.com/", "https://pan.baidu.com/download#win"),

            // C
            new("Cheat Engine (CE)", "https://www.cheatengine.org/"),
            new("Cheat Engine (CE) 语言包 zh-CN", "https://www.cheatengine.org/", "https://www.cheatengine.org/download/zh_cn.rar"),
            new("Cheat Engine (CE) 语言包 ch-CN", "https://www.cheatengine.org/", "https://www.cheatengine.org/download/ch_cn.rar"),
            new("Cheat Engine (CE) 语言包 ch-TW", "https://www.cheatengine.org/", "https://www.cheatengine.org/download/ch_tw.rar"),
            new("Cheat Engine (CE) 语言包 pl-PL", "https://www.cheatengine.org/", "https://www.cheatengine.org/download/pl_pl.rar"),
            new("Cheat Engine (CE) 语言包 ru-RU", "https://www.cheatengine.org/", "https://www.cheatengine.org/download/ru_RU.rar"),
            new("Cheat Engine (CE) 语言包 pt-BR", "https://www.cheatengine.org/", "https://www.cheatengine.org/download/pt_BR.rar"),
            new("Cheat Engine (CE) 语言包 es-ES", "https://www.cheatengine.org/", "https://www.cheatengine.org/download/es_ES.rar"),
            new("Cheat Engine (CE) 语言包 ko-KR", "https://www.cheatengine.org/", "https://www.cheatengine.org/download/ko_KR.rar"),
            new("Cheat Engine (CE) 语言包 cl-CL", "https://www.cheatengine.org/", "https://www.cheatengine.org/download/cl_CL.rar"),

            // D

            // E
            new("Edge 浏览器", "https://www.microsoft.com/zh-cn/edge/?form=QI", "https://www.microsoft.com/zh-cn/edge/download?form=QI"),

            // F

            // G
            new("Geek Uninstaller 免费版", "https://geekuninstaller.com/", "https://geekuninstaller.com/geek.zip", "https://geekuninstaller.com/geek.7z"),
            new("Geek Uninstaller 专业版", "https://crystalidea.com/uninstall-tool/", "https://crystalidea.com/downloads/uninstalltool_setup.exe"),

            // H

            // I

            // J

            // K
            new("酷狗音乐", "https://www.kugou.com/"),

            // L
            
            // M

            // N

            // O

            // P

            // Q

            // R

            // S
            new("Steam (蒸汽平台)", "https://store.steampowered.com/", "https://media.cdn.queniuqe.com/client/installer/SteamSetup.exe"),

            // T
            new("腾讯QQ", "https://im.qq.com/index/"),

            // U

            // V
            new("Visual Studio Code (VS Code)", "https://code.visualstudio.com/"),
            new("Visual Studio 2022", "https://visualstudio.microsoft.com/zh-hans/#vs-section"),

            // W
            new("Watt Toolkit (Steam++)", "https://steampp.net/", "https://gitee.com/rmbgame/SteamTools/releases/tag/3.0.0-rc.13", "https://github.com/BeyondDimension/SteamTools/releases/tag/3.0.0-rc.13", "https://pan.baidu.com/share/init?surl=S-ZzjqJ_gnu9V1A593iZAQ&pwd=1234"),
            new("微信", "https://weixin.qq.com/", "https://dldir1v6.qq.com/weixin/Windows/WeChatSetup.exe"),

            // X
            new("迅雷", "https://www.xunlei.com/"),

            // Y

            // Z
        };
        */




        private void buttonSearch_Click(object sender, EventArgs e)
        {
            comboBoxSearch.Items.Clear();
            foreach (Software software in softwares)
            {
                if (software.Name == null) break;
                else if (Regex.IsMatch(software.Name.ToLower(), textBoxSearch.Text.ToLower()))
                    comboBoxSearch.Items.Add(software.Name);
            }
            comboBoxSearch.Enabled = comboBoxSearch.Items.Count > 0;
        }

        private void comboBoxSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Software software in softwares)
            {
                if (software.Name == comboBoxSearch.Text)
                {
                    NowSoftware = software;
                    break;
                }
            }


            buttonOffWebDownloadA.Enabled = NowSoftware.OffWebDownloadLinkA != string.Empty;
            buttonOffWebDownloadB.Enabled = NowSoftware.OffWebDownloadLinkB != string.Empty;
            buttonOffWebDownloadC.Enabled = NowSoftware.OffWebDownloadLinkC != string.Empty;
            buttonElseDownload.Enabled = NowSoftware.ElseDownloadLink != string.Empty;
            buttonOffWeb.Enabled = NowSoftware.OffWebLink != string.Empty;
        }

        private void buttonElseDownload_Click(object sender, EventArgs e)
        {
            if (NowSoftware.ElseDownloadLink != string.Empty)
                Process.Start(new ProcessStartInfo(NowSoftware.ElseDownloadLink) { UseShellExecute = true });
        }

        private void buttonOffWeb_Click(object sender, EventArgs e)
        {
            if (NowSoftware.OffWebLink != string.Empty)
                Process.Start(new ProcessStartInfo(NowSoftware.OffWebLink) { UseShellExecute = true });
        }

        private void buttonOffWebDownloadA_Click(object sender, EventArgs e)
        {
            if (NowSoftware.OffWebDownloadLinkA != string.Empty)
                Process.Start(new ProcessStartInfo(NowSoftware.OffWebDownloadLinkA) { UseShellExecute = true });
        }

        private void buttonOffWebDownloadB_Click(object sender, EventArgs e)
        {
            if (NowSoftware.OffWebDownloadLinkB != string.Empty)
                Process.Start(new ProcessStartInfo(NowSoftware.OffWebDownloadLinkB) { UseShellExecute = true });
        }

        private void buttonOffWebDownloadC_Click(object sender, EventArgs e)
        {
            if (NowSoftware.OffWebDownloadLinkC != string.Empty)
                Process.Start(new ProcessStartInfo(NowSoftware.OffWebDownloadLinkC) { UseShellExecute = true });
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            buttonSearch.Enabled = Qi.NoRN(Qi.NoST(textBoxSearch.Text)) != string.Empty;
            comboBoxSearch.Enabled = comboBoxSearch.Items.Count > 0;
        }

        public struct Software
        {
            public string Name; // 软件名
            public string OffWebLink; // 官方网址
            public string OffWebDownloadLinkA; // 官方下载网址 A
            public string OffWebDownloadLinkB; // 官方下载网址 B
            public string OffWebDownloadLinkC; // 官方下载网址 C
            public string ElseDownloadLink; // 第三方下载网址

            public Software(string name = "未知软件", string offWebLink = "", string offWebDownloadLinkA = "", string offWebDownloadLinkB = "", string offWebDownloadLinkC = "", string elseDownloadLink = "")
            {
                Name = name;
                OffWebLink = offWebLink;
                OffWebDownloadLinkA = offWebDownloadLinkA;
                OffWebDownloadLinkB = offWebDownloadLinkB;
                OffWebDownloadLinkC = offWebDownloadLinkC;
                ElseDownloadLink = elseDownloadLink;
            }
        }
    }
}
