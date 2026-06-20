using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Uninstall_QisToolkit3
{
    public partial class Main : Form
    {
        string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public Main()
        {
            InitializeComponent();
        }

        private void button_Yes_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定卸载吗？\n这会删除所有齐的工具包3相关文件！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (MessageBox.Show("最后警告！您确定卸载吗？\n这会删除所有齐的工具包3相关文件！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {
                        // 结束主进程
                        Process[] processes = Process.GetProcessesByName("QisToolkit3.exe");
                        foreach (Process process in processes)
                            process.Kill();

                        // CQAA
                        processes = Process.GetProcessesByName("QisToolkit3_CQAA.exe");
                        foreach (Process process in processes)
                            process.Kill();

                        // GEEK
                        processes = Process.GetProcessesByName("geek.exe");
                        foreach (Process process in processes)
                            process.Kill();

                        // QiFavoriteSiteOpener
                        processes = Process.GetProcessesByName("QiFavoriteSiteOpener.exe");
                        foreach (Process process in processes)
                            process.Kill();

                        // 齐之防御
                        processes = Process.GetProcessesByName("QisDefense.exe");
                        foreach (Process process in processes)
                            process.Kill();

                        // QiCmd
                        processes = Process.GetProcessesByName("QiCmd.exe");
                        foreach (Process process in processes)
                            process.Kill();

                        // yt-dlp
                        processes = Process.GetProcessesByName("yt-dlp.exe");
                        foreach (Process process in processes)
                            process.Kill();

                        // ffmpeg
                        processes = Process.GetProcessesByName("ffmpeg.exe");
                        foreach (Process process in processes)
                            process.Kill();


                        string[] directoriesToDelete = new[]
                        {
                            // 语言 dll 文件夹
                            "de", "en", "es", "fr", "it", "ja", "pl", "ru", "sv", "tr",
                            "zh", "zh-CN", "zh-Hans", "zh-Hant",
    
                            // 特殊文件夹
                            "Customize", "CQAA", "Datas", "ElseTool", "Logs",
                            "QisDefenseRUN", "yt-dlp",
    
                            // 核心运行库
                            "ref", "runtimes", "Win32", "x64"
                        };

                        string[] filesToDelete = new[]
                        {
                            // 潜在不分发文件
                            "(c)", "Microsoft", "QisToolkit3.pdb",

                            // 配置文件
                            "Config.ini", "QisDefenseConfig.ini",
                            "QisToolkit3.deps.json", "QisToolkit3.runtimeconfig.json",

                            // 主程序
                            "Microsoft.Win32.TaskScheduler.dll", "Newtonsoft.Json.dll",
                            "QisDefense.exe", "QisToolkit3.dll", "QisToolkit3.exe",

                            // 辅助
                            "qt3.bat", "NSudoAPI.dll",

                            // 运行库
                            "System.CodeDom.dll",
                            "System.Diagnostics.EventLog.dll",
                            "System.Management.dll",
                            "Microsoft.Bcl.Cryptography.dll",
                            "System.Configuration.ConfigurationManager.dll",
                            "System.DirectoryServices.AccountManagement.dll",
                            "System.DirectoryServices.dll",
                            "System.DirectoryServices.Protocols.dll",
                            "System.Formats.Asn1.dll",
                            "System.Security.Cryptography.ProtectedData.dll",

                            // 文档
                            "《问身 问己 问心》游戏介绍.docx", "YtDlp 介绍.docx",
                            "使用手册.txt", "启动参数说明.txt", "打不开请看此文档.txt",
                            "文本规则介绍.txt", "更新日志.txt", "更新日志（内部）.txt",
                            "文本规则介绍.md"
                        };

                        // 删除文件夹
                        foreach (var dir in directoriesToDelete) DeleteDirectory(GetPath(dir));

                        // 删除文件
                        foreach (var file in filesToDelete) DeleteFile(GetPath(file));


                        // 删除自己
                        ScheduleSelfDeletion();
                        Environment.Exit(0);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void button_No_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private string GetPath(string str) => Path.Combine(currentDir, str);

        private static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch { }
            }
        }

        private static void DeleteDirectory(string targetDir)
        {
            // 修改方法名以更准确反映行为
            if (!Directory.Exists(targetDir))
                return;

            try
            {
                Directory.Delete(targetDir, true); // 第二个参数 true 表示递归删除
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"删除目录失败: {targetDir} - {ex.Message}");
                // 可以尝试强制删除
                try
                {
                    ForceDeleteDirectory(targetDir);
                }
                catch { }
            }
        }

        private static void ForceDeleteDirectory(string targetDir)
        {
            if (!Directory.Exists(targetDir))
                return;

            // 先删除所有文件的只读属性
            foreach (string file in Directory.GetFiles(targetDir, "*", SearchOption.AllDirectories))
            {
                try
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                }
                catch { }
            }

            // 递归删除
            foreach (string dir in Directory.GetDirectories(targetDir))
            {
                ForceDeleteDirectory(dir);
            }

            // 删除目录
            try
            {
                Directory.Delete(targetDir, true);
            }
            catch
            {
                // 如果还是失败，可能文件被占用，记录日志
                Debug.WriteLine($"无法删除目录: {targetDir}");
            }
        }

        private static void ScheduleSelfDeletion()
        {
            try
            {
                string selfPath = Assembly.GetExecutingAssembly().Location;

                CreateSelfDeletingBatchFile(selfPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"安排自删除失败: {ex.Message}");
            }
        }

        private static void CreateSelfDeletingBatchFile(string exePath)
        {
            string batchPath = Path.Combine(Path.GetDirectoryName(exePath), "delete_self.bat");
            string batchContent = $@"
@echo off
chcp 65001 >nul
:loop
tasklist /fi ""IMAGENAME eq {Path.GetFileName(exePath)}"" | find ""{Path.GetFileName(exePath)}"" >nul
if %errorlevel%==0 (
    timeout /t 1 /nobreak >nul
    goto loop
)
del /f /q ""{exePath}""
del /f /q ""%~f0""
rmdir /s /q ""%~f0""
";

            File.WriteAllText(batchPath, batchContent);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = batchPath,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(psi);
        }
    }
}