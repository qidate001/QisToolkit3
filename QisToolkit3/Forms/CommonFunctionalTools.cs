using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QisToolkit3.Forms.ExtendedFeatures;

namespace QisToolkit3.Forms
{
    public partial class CommonFunctionalTools : Form
    {
        string userName = Environment.UserName;
        string LocalDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string RoamingDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string LocalLowDir = @"C:\Users\" + Environment.UserName + @"\AppData\LocalLow";

        public CommonFunctionalTools()
        {
            InitializeComponent();
            ShowWifiPassword();
            button_FolderView_Read_Click(null, null);
        }


        #region 辅助方法

        private void RunCommand(string cmd, string args, string successMsg, bool showMsg = true)
        {
            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = cmd,
                        Arguments = args,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        Verb = "runas"  // 请求管理员权限
                    }
                };
                process.Start();
                process.WaitForExit();

                if (showMsg)
                    MessageBox.Show($"{successMsg}", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string RunCommandAndGetOutput(string cmd, string args)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = cmd,
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Verb = "runas"
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        private string ExtractProfileName(string line)
        {
            // 格式示例: "    所有用户配置文件 : TP-LINK_1234"
            int colonIndex = line.IndexOf(':');
            if (colonIndex > 0 && colonIndex < line.Length - 1)
            {
                string name = line.Substring(colonIndex + 1).Trim();
                if (!string.IsNullOrEmpty(name) && name != "*")
                    return name;
            }
            return null;
        }

        private string ExtractPassword(string detail)
        {
            // 查找 "关键内容" 或 "Key Content"
            string[] markers = { "关键内容", "Key Content" };
            foreach (string marker in markers)
            {
                int idx = detail.IndexOf(marker);
                if (idx > 0)
                {
                    int start = idx + marker.Length;
                    int end = detail.IndexOf('\n', start);
                    if (end == -1) end = detail.Length;
                    string pass = detail.Substring(start, end - start).Trim();
                    if (!string.IsNullOrEmpty(pass) && pass != "(无)")
                        return pass;
                }
            }
            return null;
        }

        #endregion

        private void button_FixIconCache_Click(object sender, EventArgs e)
        {
            try
            {
                Log.Info("[CFT] [FixIconCache] 开始清理图标缓存");

                // 1. 关闭资源管理器（只执行一次，不循环）
                Log.Info("[CFT] [FixIconCache] 正在关闭资源管理器进程...");
                var explorerProcs = Process.GetProcessesByName("explorer");
                int killedCount = 0;

                foreach (var proc in explorerProcs)
                {
                    try
                    {
                        proc.Kill();
                        proc.WaitForExit(3000);
                        killedCount++;
                        Log.Info($"[CFT] [FixIconCache] 已终止资源管理器进程 (PID: {proc.Id})");
                    }
                    catch (Exception ex)
                    {
                        Log.Err($"[CFT] [FixIconCache] 终止资源管理器进程失败 (PID: {proc.Id})，错误：{ex.Message}");
                    }
                    finally
                    {
                        proc.Dispose();
                    }
                }
                Log.Info($"[CFT] [FixIconCache] 共终止 {killedCount} 个资源管理器进程");

                // 等待文件解锁
                if (killedCount > 0)
                {
                    Log.Info("[CFT] [FixIconCache] 等待文件系统解锁...");
                    Thread.Sleep(1500);
                }

                // 2. 删除 Win10/Win11 通用的旧位置缓存
                string oldPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    @"Microsoft\Windows\Explorer");

                Log.Info($"[CFT] [FixIconCache] 开始清理旧缓存路径：{oldPath}");

                int totalDeletedFiles = 0;
                long totalDeletedSize = 0;

                if (Directory.Exists(oldPath))
                {
                    // 删除所有图标缓存文件
                    totalDeletedFiles += DeleteCacheFilesByPattern(oldPath, "iconcache_*.db", ref totalDeletedSize);
                    totalDeletedFiles += DeleteCacheFilesByPattern(oldPath, "thumbcache_*.db", ref totalDeletedSize);

                    // 额外清理：删除其他可能的缓存文件
                    totalDeletedFiles += DeleteCacheFilesByPattern(oldPath, "*.db", ref totalDeletedSize);

                    Log.Info($"[CFT] [FixIconCache] 旧缓存路径清理完成，共删除 {totalDeletedFiles} 个文件，释放约 {FormatFileSize(totalDeletedSize)}");
                }
                else
                {
                    Log.Info($"[CFT] [FixIconCache] 旧缓存路径不存在，跳过：{oldPath}");
                }

                // 3. 删除 Win11 新增的图标缓存
                string newPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    @"Microsoft\Windows\Explorer");

                Log.Info($"[CFT] [FixIconCache] 开始清理新缓存路径：{newPath}");

                if (Directory.Exists(newPath))
                {
                    string iconCacheDb = Path.Combine(newPath, "IconCache.db");
                    if (System.IO.File.Exists(iconCacheDb))
                    {
                        long fileSize = DeleteFileWithRetry(iconCacheDb);
                        if (fileSize >= 0)
                        {
                            totalDeletedFiles++;
                            totalDeletedSize += fileSize;
                            Log.Info($"[CFT] [FixIconCache] 成功删除 Win11 图标缓存文件，大小：{FormatFileSize(fileSize)} ({iconCacheDb})");
                        }
                    }
                    else
                    {
                        Log.Info($"[CFT] [FixIconCache] Win11 图标缓存文件不存在：{iconCacheDb}");
                    }

                    // 清理 Win11 缩略图缓存（如果有）
                    string thumbCacheDb = Path.Combine(newPath, "ThumbCache.db");
                    if (System.IO.File.Exists(thumbCacheDb))
                    {
                        long fileSize = DeleteFileWithRetry(thumbCacheDb);
                        if (fileSize >= 0)
                        {
                            totalDeletedFiles++;
                            totalDeletedSize += fileSize;
                            Log.Info($"[CFT] [FixIconCache] 成功删除 Win11 缩略图缓存文件，大小：{FormatFileSize(fileSize)} ({thumbCacheDb})");
                        }
                    }
                }
                else
                {
                    Log.Info($"[CFT] [FixIconCache] 新缓存路径不存在，跳过：{newPath}");
                }

                // 4. 清除注册表图标缓存
                Log.Info("[CFT] [FixIconCache] 开始清除注册表图标缓存");
                int registryDeletedCount = 0;
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                        @"Software\Microsoft\Windows\CurrentVersion\Explorer\IconCache", true))
                    {
                        if (key != null)
                        {
                            var valueNames = key.GetValueNames();
                            foreach (string valueName in valueNames)
                            {
                                try
                                {
                                    key.DeleteValue(valueName);
                                    registryDeletedCount++;
                                    Log.Info($"[CFT] [FixIconCache] 已删除注册表项：{valueName}");
                                }
                                catch (Exception ex)
                                {
                                    Log.Err($"[CFT] [FixIconCache] 删除注册表项失败：{valueName}，错误：{ex.Message}");
                                }
                            }

                            // 确保图标缓存键存在（即使没有值也要创建）
                            if (registryDeletedCount == 0)
                            {
                                Log.Info("[CFT] [FixIconCache] 注册表键存在但没有可删除的值");
                            }
                            else
                            {
                                Log.Info($"[CFT] [FixIconCache] 注册表清理完成，共删除 {registryDeletedCount} 个注册表值");
                            }
                        }
                        else
                        {
                            Log.Info("[CFT] [FixIconCache] 注册表键不存在，跳过");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Err($"[CFT] [FixIconCache] 清理注册表时出错：{ex.Message}");
                }

                // 5. 清理任务栏图标缓存（Win11）
                try
                {
                    string taskbarCachePath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        @"Microsoft\Windows\Recent\AutomaticDestinations");

                    if (Directory.Exists(taskbarCachePath))
                    {
                        Log.Info($"[CFT] [FixIconCache] 开始清理任务栏缓存：{taskbarCachePath}");
                        int taskbarDeleted = DeleteCacheFilesByPattern(taskbarCachePath, "*.automaticDestinations-ms", ref totalDeletedSize);
                        if (taskbarDeleted > 0)
                        {
                            totalDeletedFiles += taskbarDeleted;
                            Log.Info($"[CFT] [FixIconCache] 任务栏缓存清理完成，删除 {taskbarDeleted} 个文件");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Err($"[CFT] [FixIconCache] 清理任务栏缓存时出错：{ex.Message}");
                }

                // 6. 重启资源管理器
                Log.Info("[CFT] [FixIconCache] 正在重新启动资源管理器...");
                try
                {
                    Process.Start("explorer.exe");
                    Log.Info("[CFT] [FixIconCache] 资源管理器已启动");
                }
                catch (Exception ex)
                {
                    Log.Err($"[CFT] [FixIconCache] 启动资源管理器失败：{ex.Message}");
                }

                // 7. 发送系统刷新通知（取消注释并修正）
                Log.Info("[CFT] [FixIconCache] 发送系统刷新通知");
                try
                {
                    // 使用正确的刷新消息
                    NativeMethods.SendNotifyMessage(0xFFFF, 0x001A, IntPtr.Zero, "Environment");
                    // 或者使用 SHChangeNotify
                    // NativeMethods.SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
                    Log.Info("[CFT] [FixIconCache] 系统刷新通知已发送");
                }
                catch (Exception ex)
                {
                    Log.Err($"[CFT] [FixIconCache] 发送刷新通知失败：{ex.Message}");
                }

                // 8. 显示清理结果
                string summary = $"图标缓存清理完成！共删除 {totalDeletedFiles} 个文件，释放约 {FormatFileSize(totalDeletedSize)}";
                if (registryDeletedCount > 0)
                {
                    summary += $"，清理 {registryDeletedCount} 个注册表项";
                }

                Log.Info($"[CFT] [FixIconCache] {summary}");

                // 可选：显示消息框通知用户
                // MessageBox.Show(summary, "清理完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Err($"[CFT] [FixIconCache] 清理图标缓存时出现严重错误：{ex.Message}");
                Log.Err($"[CFT] [FixIconCache] 堆栈跟踪：{ex.StackTrace}");
            }
        }

        /// <summary>
        /// 按模式删除缓存文件并统计大小
        /// </summary>
        private int DeleteCacheFilesByPattern(string directory, string pattern, ref long totalSize)
        {
            int deletedCount = 0;
            try
            {
                var files = Directory.GetFiles(directory, pattern);
                foreach (var file in files)
                {
                    long fileSize = DeleteFileWithRetry(file);
                    if (fileSize >= 0)
                    {
                        deletedCount++;
                        totalSize += fileSize;
                        Log.Info($"[CFT] [FixIconCache] 已删除缓存文件：{Path.GetFileName(file)}，大小：{FormatFileSize(fileSize)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[CFT] [FixIconCache] 扫描目录 {directory} 时出错：{ex.Message}");
            }
            return deletedCount;
        }

        /// <summary>
        /// 带重试机制的文件删除，返回文件大小（失败返回 -1）
        /// </summary>
        private long DeleteFileWithRetry(string filePath, int maxRetries = 3)
        {
            long fileSize = -1;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    if (!System.IO.File.Exists(filePath))
                    {
                        return -1;
                    }

                    // 获取文件大小
                    var fileInfo = new FileInfo(filePath);
                    fileSize = fileInfo.Length;

                    // 删除文件
                    System.IO.File.Delete(filePath);

                    // 验证是否删除成功
                    if (!System.IO.File.Exists(filePath))
                    {
                        return fileSize;
                    }
                    else
                    {
                        Log.Warn($"[CFT] [FixIconCache] 文件删除后仍然存在，重试中... ({i + 1}/{maxRetries})");
                    }
                }
                catch (IOException) when (i < maxRetries - 1)
                {
                    Log.Warn($"[CFT] [FixIconCache] 文件被占用，等待后重试... ({i + 1}/{maxRetries})：{Path.GetFileName(filePath)}");
                    Thread.Sleep(1000 * (i + 1)); // 递增等待时间
                }
                catch (UnauthorizedAccessException) when (i < maxRetries - 1)
                {
                    Log.Warn($"[CFT] [FixIconCache] 权限不足，等待后重试... ({i + 1}/{maxRetries})：{Path.GetFileName(filePath)}");
                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    Log.Err($"[CFT] [FixIconCache] 删除文件失败：{Path.GetFileName(filePath)}，错误：{ex.Message}");
                    break;
                }
            }

            return -1; // 删除失败
        }

        /// <summary>
        /// 格式化文件大小显示
        /// </summary>
        private string FormatFileSize(long bytes)
        {
            if (bytes < 0) return "0 B";

            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        // NativeMethods 类
        internal static class NativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SendNotifyMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

            // 可选：使用 SHChangeNotify 更可靠
            [DllImport("shell32.dll", CharSet = CharSet.Auto)]
            public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
        }

        private void button_ipconfig_flushdns_Click(object sender, EventArgs e)
        {
            RunCommand("ipconfig", "/flushdns", "DNS 缓存已刷新");
        }

        private void button_netsh_winsock_reset_Click(object sender, EventArgs e)
        {
            RunCommand("netsh", "winsock reset", "Winsock 已重置，需要重启电脑生效");
        }

        private void button_release_and_renew_IP_Click(object sender, EventArgs e)
        {
            RunCommand("ipconfig", "/release", "IP 已释放", false);
            RunCommand("ipconfig", "/renew", "IP 已更新", true);
        }

        private void button_release_IP_Click(object sender, EventArgs e)
        {
            RunCommand("ipconfig", "/release", "IP 已释放");
        }

        private void button_renew_IP_Click(object sender, EventArgs e)
        {
            RunCommand("ipconfig", "/renew", "IP 已更新");
        }

        private void ShowWifiPassword()
        {
            try
            {
                // 先获取所有配置文件
                string profilesOutput = RunCommandAndGetOutput("netsh", "wlan show profiles");
                Log.Info($"[CFT] [WlanInfo] 配置信息读取：{profilesOutput}");

                var profileLines = profilesOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder result = new StringBuilder();

                foreach (string line in profileLines)
                {
                    if (line.Contains("所有用户配置文件") || line.Contains("User Profiles"))
                    {
                        string profileName = ExtractProfileName(line);
                        if (!string.IsNullOrEmpty(profileName))
                        {
                            string detail = RunCommandAndGetOutput("netsh", $"wlan show profile name=\"{profileName}\" key=clear");
                            string password = ExtractPassword(detail);
                            result.AppendLine($"名称: {profileName}");
                            result.AppendLine($"密码: {(string.IsNullOrEmpty(password) ? "无密码或开放网络" : password)}");
                            result.AppendLine("-------------------");
                        }
                    }
                }

                if (result.Length == 0)
                {
                    if (profilesOutput.Contains("无线自动配置服务(wlansvc)没有运行"))
                    {
                        label_Wlan.Text = "无限配置服务未开启，可能是网络服务崩溃或使用的是有线链接。";
                        Log.Info($"[CFT] [WlanInfo] 无线自动配置服务(wlansvc)没有运行。");
                    }
                    else
                    {
                        label_Wlan.Text = "未找到已保存的 Wlan 配置文件";
                        Log.Info($"[CFT] [WlanInfo] 未找到已保存的 Wlan 配置文件");
                    }
                }
                else
                {
                    label_Wlan.Text = result.ToString();
                    Log.Info($"[CFT] [WlanInfo] {result.ToString()}");
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[CFT] [WlanInfo] 获取失败：{ex.Message}");
            }
        }

        private void button_FolderView_Read_Click(object sender, EventArgs e)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"))
                {
                    if (key != null)
                    {
                        comboBox_FolderView_ShowEncryptCompressedColor.SelectedIndex = GetFolderViewRegData(key, "ShowEncryptCompressedColor", 0, 1);
                        comboBox_FolderView_HideDrivesWithNoMedia.SelectedIndex = GetFolderViewRegData(key, "HideDrivesWithNoMedia", 0, 1);
                        comboBox_FolderView_ShowTypeOverlay.SelectedIndex = GetFolderViewRegData(key, "ShowTypeOverlay", 0, 1);
                        comboBox_FolderView_ShowSuperHidden.SelectedIndex = GetFolderViewRegData(key, "ShowSuperHidden", 0, 1);
                        comboBox_FolderView_ShowStatusBar.SelectedIndex = GetFolderViewRegData(key, "ShowStatusBar", 0, 1);
                        comboBox_FolderView_HideFileExt.SelectedIndex = GetFolderViewRegData(key, "HideFileExt", 0, 1);
                        comboBox_FolderView_ShowInfoTip.SelectedIndex = GetFolderViewRegData(key, "ShowInfoTip", 0, 1);
                        comboBox_FolderView_IconsOnly.SelectedIndex = GetFolderViewRegData(key, "IconsOnly", 0, 1);
                        comboBox_FolderView_Hidden.SelectedIndex = GetFolderViewRegData(key, "Hidden", 1, 2) - 1;

                        comboBox_TaskbarBehavioral_DisablePreviewDesktop.SelectedIndex = GetFolderViewRegData(key, "DisablePreviewDesktop", 0, 1);
                        comboBox_TaskbarBehavioral_StoreAppsOnTaskbar.SelectedIndex = GetFolderViewRegData(key, "StoreAppsOnTaskbar", 0, 1);
                        comboBox_TaskbarBehavioral_ShowCortanaButton.SelectedIndex = GetFolderViewRegData(key, "ShowCortanaButton", 0, 1);
                        comboBox_TaskbarBehavioral_TaskbarSmallIcons.SelectedIndex = GetFolderViewRegData(key, "TaskbarSmallIcons", 0, 1);
                        comboBox_TaskbarBehavioral_TaskbarAnimations.SelectedIndex = GetFolderViewRegData(key, "TaskbarAnimations", 0, 1);
                        comboBox_TaskbarBehavioral_TaskbarGlomLevel.SelectedIndex = GetFolderViewRegData(key, "TaskbarGlomLevel", 0, 2);
                        comboBox_TaskbarBehavioral_TaskbarBadges.SelectedIndex = GetFolderViewRegData(key, "TaskbarBadges", 0, 1);
                    }
                    else
                    {
                        MessageBox.Show("未找到系统注册表项，请检查运行环境！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取注册表时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetFolderViewRegData(RegistryKey key, string Value, int min, int max)
        {
            return Math.Clamp(Qi.StrToInt(key.GetValue(Value).ToString()), min, max);
        }

        private void button_FolderView_Save_Click(object sender, EventArgs e)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true))
                {
                    if (key != null)
                    {
                        key.SetValue("ShowEncryptCompressedColor", comboBox_FolderView_ShowEncryptCompressedColor.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("HideDrivesWithNoMedia", comboBox_FolderView_HideDrivesWithNoMedia.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("ShowSuperHidden", comboBox_FolderView_ShowSuperHidden.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("ShowTypeOverlay", comboBox_FolderView_ShowTypeOverlay.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("ShowStatusBar", comboBox_FolderView_ShowStatusBar.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("ShowInfoTip", comboBox_FolderView_ShowInfoTip.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("HideFileExt", comboBox_FolderView_HideFileExt.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("IconsOnly", comboBox_FolderView_IconsOnly.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("Hidden", comboBox_FolderView_Hidden.SelectedIndex + 1, RegistryValueKind.DWord);

                        key.SetValue("DisablePreviewDesktop", comboBox_TaskbarBehavioral_DisablePreviewDesktop.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("StoreAppsOnTaskbar", comboBox_TaskbarBehavioral_StoreAppsOnTaskbar.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("ShowCortanaButton", comboBox_TaskbarBehavioral_ShowCortanaButton.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("TaskbarSmallIcons", comboBox_TaskbarBehavioral_TaskbarSmallIcons.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("TaskbarAnimations", comboBox_TaskbarBehavioral_TaskbarAnimations.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("TaskbarGlomLevel", comboBox_TaskbarBehavioral_TaskbarGlomLevel.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("TaskbarBadges", comboBox_TaskbarBehavioral_TaskbarBadges.SelectedIndex, RegistryValueKind.DWord);

                        Qi.RestartExplorer();
                        MessageBox.Show("保存成功。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("未找到系统注册表项，请检查运行环境！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"写入注册表时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_FolderView_Preset_Click(object sender, EventArgs e)
        {
            comboBox_FolderView_ShowEncryptCompressedColor.SelectedIndex = 1;
            comboBox_FolderView_HideDrivesWithNoMedia.SelectedIndex = 1;
            comboBox_FolderView_ShowSuperHidden.SelectedIndex = 1;
            comboBox_FolderView_ShowTypeOverlay.SelectedIndex = 0;
            comboBox_FolderView_ShowStatusBar.SelectedIndex = 1;
            comboBox_FolderView_HideFileExt.SelectedIndex = 0;
            comboBox_FolderView_ShowInfoTip.SelectedIndex = 1;
            comboBox_FolderView_IconsOnly.SelectedIndex = 0;
            comboBox_FolderView_Hidden.SelectedIndex = 1;

            comboBox_TaskbarBehavioral_DisablePreviewDesktop.SelectedIndex = 0;
            comboBox_TaskbarBehavioral_StoreAppsOnTaskbar.SelectedIndex = 1;
            comboBox_TaskbarBehavioral_ShowCortanaButton.SelectedIndex = 1;
            comboBox_TaskbarBehavioral_TaskbarSmallIcons.SelectedIndex = 0;
            comboBox_TaskbarBehavioral_TaskbarAnimations.SelectedIndex = 1;
            comboBox_TaskbarBehavioral_TaskbarGlomLevel.SelectedIndex = 0;
            comboBox_TaskbarBehavioral_TaskbarBadges.SelectedIndex = 1;
        }



        #region Shell 文件夹名称与图标

        public void button_FixShellFolderNameAndIcon_Click(object sender, EventArgs e)
        {
            var paths = ShellFolderHelper.GetAllShellFolders();

            // 修复 桌面
            if (paths.TryGetValue("桌面", out string path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI修复] 桌面路径: {path}");
                FixShellFolderIni(
                    path,
                    "\r\n[.ShellClassInfo]\r\n" +
                    "LocalizedResourceName=@%SystemRoot%\\system32\\shell32.dll,-21769\r\n" +
                    "IconResource=%SystemRoot%\\system32\\imageres.dll,-183\r\n"
                );
            }

            // 修复 下载
            if (paths.TryGetValue("下载", out path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI修复] 下载路径: {path}");
                FixShellFolderIni(
                    path,
                    "\r\n[.ShellClassInfo]\r\n" +
                    "LocalizedResourceName=@%SystemRoot%\\system32\\shell32.dll,-21798\r\n" +
                    "IconResource=%SystemRoot%\\system32\\imageres.dll,-184\r\n"
                );
            }

            // 修复 文档
            if (paths.TryGetValue("文档", out path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI修复] 文档路径: {path}");
                FixShellFolderIni(
                    path,
                    "\r\n[.ShellClassInfo]\r\n" +
                    "LocalizedResourceName=@%SystemRoot%\\system32\\shell32.dll,-21770\r\n" +
                    "IconResource=%SystemRoot%\\system32\\imageres.dll,-112\r\n" +
                    "IconFile=%SystemRoot%\\system32\\shell32.dll\r\n" +
                    "IconIndex=-235\r\n"
                );
            }

            // 修复 视频
            if (paths.TryGetValue("视频", out path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI修复] 视频路径: {path}");
                FixShellFolderIni(
                    path,
                    "\r\n[.ShellClassInfo]\r\n" +
                    "LocalizedResourceName=@%SystemRoot%\\system32\\shell32.dll,-21791\r\n" +
                    "InfoTip=@%SystemRoot%\\system32\\shell32.dll,-12690\r\n" +
                    "IconResource=%SystemRoot%\\system32\\imageres.dll,-189\r\n" +
                    "IconFile=%SystemRoot%\\system32\\shell32.dll\r\n" +
                    "IconIndex=-238\r\n"
                );
            }

            // 修复 图片
            if (paths.TryGetValue("图片", out path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI修复] 图片路径: {path}");
                FixShellFolderIni(
                    path,
                    "\r\n[.ShellClassInfo]\r\n" +
                    "LocalizedResourceName=@%SystemRoot%\\system32\\shell32.dll,-21779\r\n" +
                    "InfoTip=@%SystemRoot%\\system32\\shell32.dll,-12688\r\n" +
                    "IconResource=%SystemRoot%\\system32\\imageres.dll,-113\r\n" +
                    "IconFile=%SystemRoot%\\system32\\shell32.dll\r\n" +
                    "IconIndex=-236\r\n"
                );
            }

            // 修复 音乐
            if (paths.TryGetValue("音乐", out path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI修复] 音乐路径: {path}");
                FixShellFolderIni(
                    path,
                    "\r\n[.ShellClassInfo]\r\n" +
                    "LocalizedResourceName=@%SystemRoot%\\system32\\shell32.dll,-21790\r\n" +
                    "InfoTip=@%SystemRoot%\\system32\\shell32.dll,-12689\r\n" +
                    "IconResource=%SystemRoot%\\system32\\imageres.dll,-108\r\n" +
                    "IconFile=%SystemRoot%\\system32\\shell32.dll\r\n" +
                    "IconIndex=-237\r\n"
                );
            }

            // 修复 3D对象
            if (paths.TryGetValue("3D对象", out path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI修复] 3D对象路径: {path}");
                FixShellFolderIni(
                    path,
                    "\r\n[.ShellClassInfo]\r\n" +
                    "LocalizedResourceName=@%SystemRoot%\\system32\\windows.storage.dll,-21825\r\n" +
                    "IconResource=%SystemRoot%\\system32\\imageres.dll,-198\r\n"
                );
            }


            // 刷新
            Qi.RestartExplorer();

            MessageBox.Show("修复完成！", "齐的工具包3", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void button_DoShellFolderNameAndIconBug_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    "警告！\n此功能会复现Shell 文件夹名称与图标异常的问题！\n" +
                    "仅供测试使用！！！一切后果自负！！！\n确定执行吗？",
                    "警告！", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                ) != DialogResult.Yes) return;

            if (MessageBox.Show(
                    "再次警告！\n此功能会复现Shell 文件夹名称与图标异常的问题！\n" +
                    "仅供测试使用！！！一切后果自负！！！\n确定执行吗？",
                    "再次警告！", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                ) != DialogResult.Yes) return;

            var paths = ShellFolderHelper.GetAllShellFolders();

            // 删除 桌面
            if (paths.TryGetValue("桌面", out string path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI删除] 桌面路径: {path}");
                DelShellFolderIni(path);
            }

            // 删除 下载
            if (paths.TryGetValue("下载", out path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI删除] 下载路径: {path}");
                DelShellFolderIni(path);
            }

            // 删除 文档
            if (paths.TryGetValue("文档", out path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI删除] 文档路径: {path}");
                DelShellFolderIni(path);
            }

            // 删除 视频
            if (paths.TryGetValue("视频", out path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI删除] 视频路径: {path}");
                DelShellFolderIni(path);
            }

            // 删除 图片
            if (paths.TryGetValue("图片", out path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI删除] 图片路径: {path}");
                DelShellFolderIni(path);
            }

            // 删除 音乐
            if (paths.TryGetValue("音乐", out path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI删除] 音乐路径: {path}");
                DelShellFolderIni(path);
            }

            // 删除 3D对象
            if (paths.TryGetValue("3D对象", out path))
            {
                Log.Info($"[CFT] [Shell 文件夹INI删除] 3D对象路径: {path}");
                DelShellFolderIni(path);
            }

            // 刷新资源管理器
            Qi.RestartExplorer();

            MessageBox.Show("操作完成！", "齐的工具包3", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 修复 desktop.ini
        static void FixShellFolderIni(string folderPath, string content)
        {
            try
            {
                string iniPath = Path.Combine(folderPath, "desktop.ini");

                Qi.TryDeleteFile(iniPath);

                System.IO.File.WriteAllText(iniPath, content, Encoding.UTF8);

                // 设置为隐藏 + 系统文件
                System.IO.File.SetAttributes(iniPath, FileAttributes.Hidden | FileAttributes.System);

                // 设置文件夹为只读（让系统读取 desktop.ini）
                System.IO.File.SetAttributes(folderPath, System.IO.File.GetAttributes(folderPath) | FileAttributes.ReadOnly);
            }
            catch (Exception ex)
            {
                Log.Err($"修复 {folderPath} 时出现错误；错误信息：{ex.Message}。");
            }
        }

        // 删除 desktop.ini
        static void DelShellFolderIni(string folderPath)
        {
            string iniPath = Path.Combine(folderPath, "desktop.ini");

            Qi.TryDeleteFile(iniPath);
        }

        public class ShellFolderHelper
        {
            /// <summary>
            /// 显示名称 -> 注册表值名 映射（包含3D对象）
            /// </summary>
            private static readonly Dictionary<string, string> DisplayToReg = new Dictionary<string, string>
            {
                { "桌面", "Desktop" },
                { "文档", "Personal" },
                { "下载", "{374DE290-123F-4565-9164-39C4925E467B}" },
                { "音乐", "My Music" },
                { "图片", "My Pictures" },
                { "视频", "My Video" },
                { "3D对象", "{0DB7E03F-FC29-4DC6-9020-FF41B59E513A}" },
                { "收藏夹", "Favorites" },
                { "链接", "Links" },
                { "保存的游戏", "{4C5C0FF7-8CC7-4B2F-9FA3-020E3EAFAD25}" },
                { "联系人", "{56784854-C6CB-462B-8169-88E350ACB882}" },
                { "OneDrive", "{018D5C66-4533-4307-9B53-224DE2ED1FE6}" }
            };

            /// <summary>
            /// 获取所有 Shell 文件夹路径字典（中文名 -> 路径）
            /// </summary>
            public static Dictionary<string, string> GetAllShellFolders()
            {
                var result = new Dictionary<string, string>();
                string regPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders";

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(regPath))
                {
                    if (key == null) return result;

                    foreach (var kvp in DisplayToReg)
                    {
                        string value = key.GetValue(kvp.Value)?.ToString();

                        if (!string.IsNullOrEmpty(value))
                        {
                            // 展开环境变量
                            string expandedPath = Environment.ExpandEnvironmentVariables(value);

                            if (Directory.Exists(expandedPath))
                            {
                                result[kvp.Key] = expandedPath;
                            }
                            else
                            {
                                // 如果路径不存在，尝试默认路径
                                string defaultPath = GetDefaultPath(kvp.Key);
                                if (!string.IsNullOrEmpty(defaultPath))
                                {
                                    result[kvp.Key] = defaultPath;
                                }
                            }
                        }
                        else
                        {
                            // 注册表没有这个值，使用默认路径
                            string defaultPath = GetDefaultPath(kvp.Key);
                            if (!string.IsNullOrEmpty(defaultPath))
                            {
                                result[kvp.Key] = defaultPath;
                            }
                        }
                    }
                }

                return result;
            }

            /// <summary>
            /// 获取默认路径（当注册表读取失败时）
            /// </summary>
            private static string GetDefaultPath(string displayName)
            {
                string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                return displayName switch
                {
                    "桌面" => Path.Combine(userProfile, "Desktop"),
                    "文档" => Path.Combine(userProfile, "Documents"),
                    "下载" => Path.Combine(userProfile, "Downloads"),
                    "音乐" => Path.Combine(userProfile, "Music"),
                    "图片" => Path.Combine(userProfile, "Pictures"),
                    "视频" => Path.Combine(userProfile, "Videos"),
                    "3D对象" => Path.Combine(userProfile, "3D Objects"),
                    _ => null
                };
            }

            /// <summary>
            /// 获取单个文件夹路径
            /// </summary>
            public static string GetFolderPath(string folderName)
            {
                var folders = GetAllShellFolders();
                return folders.TryGetValue(folderName, out string path) ? path : null;
            }
        }

        #endregion
    }
}
