// using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Qi;

namespace QisToolkit3.Forms
{
    public partial class CleaningUpTrash : Form
    {
        string userName = Environment.UserName;
        string LocalDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string RoamingDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string LocalLowDir = @"C:\Users\" + Environment.UserName + @"\AppData\LocalLow";

        long AllFileCount = 0, AllFileSize = 0;
        long AllDeleteFileCount = 0, AllDeleteFileSize = 0;

        public CleaningUpTrash()
        {
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo(GetLanguage());
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void buttonScanGarbage_Click(object sender, EventArgs e)
        {
            checkedListBox.Items.Clear();
            AllFileCount = 0;
            AllFileSize = 0;
            buttonScanGarbage.Enabled = false;
            richTextBox.Clear(); // 清空文本框用于显示进度

            // 创建进度报告器
            var progress = new Progress<string>(message =>
            {
                richTextBox.AppendText(message);
                richTextBox.ScrollToCaret(); // 自动滚动到底部

                // 实时更新计数
                switch (GetLanguage())
                {
                    case "en":
                        label_AllFileCount.Text = "Total: " + AllFileCount;
                        label_AllFileSize.Text = "Size: " + BytesIntelligentConversion(AllFileSize);
                        break;

                    case "zh-CN":
                    default:
                        label_AllFileCount.Text = "总数：" + AllFileCount;
                        label_AllFileSize.Text = "大小：" + BytesIntelligentConversion(AllFileSize);
                        break;
                }
            });

            Log.Info($"[CUT] ==================== 开始执行扫描 ====================");

            // 异步执行扫描
            await Task.Run(() => ScanGarbage(progress));

            buttonCleaningUp.Enabled = true;
            buttonScanGarbage.Enabled = true;
            label_AllFileCount.Visible = true;
            label_AllFileSize.Visible = true;
        }

        private void ScanGarbage(IProgress<string> progress)
        {
            #region 系统垃圾
            ScanGarbageForDirs([@"C:\Windows\Temp", @"C:\Windows\System32\SleepStudy\ScreenOn", @"C:\Windows\System32\WDI"], "系统缓存");
            ScanGarbageForDirs([@"C:\Windows\Logs", @"C:\Windows\security\logs", @"C:\ProgramData\Microsoft\Network\Downloader",
                                @"C:\Windows\System32\LogFiles", @"C:\Windows\System32\WDI\LogFiles",
                                @"C:\Windows\Panther\Rollback\MachineIndependent\Transformers\CBS\boot_volume\WinLH\WinSxS\Catalogs"], "系统日志");
            ScanGarbageForDirs([@"C:\ProgramData\USOShared\Logs\System", @"C:\ProgramData\USOShared\Logs\User"], "系统USO日志");
            ScanGarbageForDirs([@"C:\ProgramData\Microsoft\Windows\WER\ReportArchive", @"C:\ProgramData\Microsoft\Windows\WER\ReportQueue", @"C:\ProgramData\Microsoft\Windows\WER\Temp"], "系统错误报告");
            ScanGarbageForDir(LocalDir + @"\Temp", "本地程序缓存");
            ScanGarbageForDir(LocalLowDir + @"\Temp", "低优先级程序缓存");
            ScanGarbageForDir(LocalDir + @"\ElevatedDiagnostics", "诊断报告");
            ScanGarbageForDir(@"C:\Users\" + userName + @"\Recent", "最近启动");
            ScanGarbageForDir(@"C:\Users\" + userName + @"\‌Cookies", "通用互联网临时文件");
            ScanGarbageForDir(LocalDir + @"\Microsoft\Windows\Explorer", "缩率图缓存", false);
            ScanGarbageForFile(@"C:\Windows\DIFx.log", "驱动程序安装日志");
            ScanGarbageForDirs([
                LocalLowDir + @"\Microsoft\CryptnetUrlCache\Content",
                @"C:\Windows\System32\config\systemprofile\AppData\LocalLow\Microsoft\CryptnetUrlCache\Content",
                @"C:\Windows\System32\config\systemprofile\AppData\LocalLow\Microsoft\CryptnetUrlCache\MetaData",
                @"C:\Windows\SysWOW64\config\systemprofile\AppData\LocalLow\Microsoft\CryptnetUrlCache\Content",
                @"C:\Windows\SysWOW64\config\systemprofile\AppData\LocalLow\Microsoft\CryptnetUrlCache\MetaData"
            ], "缓存的证书文件");
            ScanGarbageForFile(@"C:\Windows\DirectX.log", "DX 接口日志");
            ScanGarbageForFile(@"C:\Windows\PFRO.log", "ISA 日志文件");
            ScanGarbageForFile(@"C:\Windows\DtcInstall.log", "DTC 安装日志");
            ScanGarbageForFile(@"C:\Windows\setuperr.log", "系统安装错误日志");
            ScanGarbageForFile(@"C:\Windows\WindowsUpdate.log", "系统更新日志");
            ScanGarbageForDir(@"C:\Windows\SoftwareDistribution\DataStore\Logs", "系统更新信息");
            ScanGarbageForDir(@"C:\Windows\SoftwareDistribution\Download", "系统更新下载");
            ScanGarbageForDir(RoamingDir + @"\PLogs", "彩色日志");
            ScanGarbageForDirs([@"C:\Users\Administrator\AppData\LocalLow\Microsoft\CryptnetUrlCache\Content", @"C:\Users\Administrator\AppData\LocalLow\Microsoft\CryptnetUrlCache\MetaData"], "CRL缓存");
            ScanGarbageForDir(@"C:\Windows\ServiceProfiles\NetworkService\AppData\Local\Microsoft\Windows\DeliveryOptimization\Logs", "Delivery Optimization日志");
            ScanGarbageForDir(@"C:\ProgramData\Microsoft\Search\Data\Applications\Windows\GatherLogs\SystemIndex", "MS搜索缓存");
            ScanGarbageForDir(@"C:\Windows\System32\sru", "SubSystems");
            ScanGarbageForDir(LocalDir + @"\D3DSCache", "3D着色器");
            ScanGarbageForDir(@"C:\Windows\assembly", ".NET 全局程序集缓存");
            ScanGarbageForDir(@"C:\Windows\Prefetch", "预读文件缓存", false);
            ScanGarbageForDir(RoamingDir + @"\Microsoft\Windows\Recent", "最近打开的文件");
            #endregion

            #region 游戏垃圾
            ScanGarbageForDir(LocalDir + @"\.i18nupdatemod", "i18nupdatemod");
            #endregion

            #region 主要面向用户的软件垃圾
            ScanGarbageForDir(@"C:\QiAppDatas\Temps", "齐系列程序缓存");
            ScanGarbageForDirs([LocalDir + @"\Microsoft\Windows\WebCache", LocalDir + @"\Microsoft\Internet Explorer\CacheStorage"], "IE 网页缓存");
            ScanGarbageForDir(LocalDir + @"\Microsoft\Edge\User Data\Default\VideoDecodeStats", "Edge 视频解码统计信息");
            ScanGarbageForFiles([
                LocalDir + @"\Microsoft\Edge\User Data\Default\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Asset Store\assets.db\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Asset Store\assets.db\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\AutofillStrikeDatabase\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\AutofillStrikeDatabase\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\BudgetDatabase\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\BudgetDatabase\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\ClientCertificates\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\ClientCertificates\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\commerce_subscription_db\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\commerce_subscription_db\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\discounts_db\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\discounts_db\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Download Service\EntryDB\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Download Service\EntryDB\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\EdgeCoupons\coupons_data.db\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\EdgeCoupons\coupons_data.db\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\EdgePushStorageWithConnectTokenAndKey\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\EdgePushStorageWithConnectTokenAndKey\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\EdgePushStorageWithWinRt\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\EdgePushStorageWithWinRt\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\EntityExtraction\EntityExtractionAssetStore.db\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\EntityExtraction\EntityExtractionAssetStore.db\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Extension State\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Extension State\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Feature Engagement Tracker\AvailabilityDB\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Feature Engagement Tracker\AvailabilityDB\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Feature Engagement Tracker\EventDB\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Feature Engagement Tracker\EventDB\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\File System\Origins\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\File System\Origins\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Local Storage\leveldb\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Local Storage\leveldb\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\optimization_guide_hint_cache_store\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\optimization_guide_hint_cache_store\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\parcel_tracking_db\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\parcel_tracking_db\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\PersistentOriginTrials\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\PersistentOriginTrials\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Platform Notifications\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Platform Notifications\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\PriceComparison\PriceComparisonAssetStore.db\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\PriceComparison\PriceComparisonAssetStore.db\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Segmentation Platform\SegmentInfoDB\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Segmentation Platform\SegmentInfoDB\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Segmentation Platform\SignalDB\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Segmentation Platform\SignalDB\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Segmentation Platform\SignalStorageConfigDB\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Segmentation Platform\SignalStorageConfigDB\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Service Worker\Database\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Service Worker\Database\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Session Storage\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Session Storage\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\shared_proto_db\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\shared_proto_db\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\shared_proto_db\metadata\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\shared_proto_db\metadata\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Site Characteristics Database\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Site Characteristics Database\LOG.old",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Sync Data\LevelDB\LOG",
                LocalDir + @"\Microsoft\Edge\User Data\Default\Sync Data\LevelDB\LOG.old"
            ], "Edge 日志");
            ScanGarbageForDirs([LocalDir + @"\Microsoft\Edge\User Data\Default\Code Cache", LocalDir + @"\Microsoft\Edge\User Data\Default\Cache\Cache_Data"], "Edge 缓存");
            ScanGarbageForDir(@"C:\Program Files(x86)\Microsoft\EdgeUpdate\Download", "Edge 更新缓存");
            ScanGarbageForDirs([@"C:\KuGou\Temp", @"D:\KuGou\Temp", @"E:\KuGou\Temp", @"F:\KuGou\Temp"], "酷狗歌曲缓存");
            ScanGarbageForDir(RoamingDir + @"\KuGou8\log", "酷狗日志");
            ScanGarbageForDir(LocalDir + @"\Quark\User Data\Default\Cache\Cache_Data", "夸克浏览器 缓存");
            ScanGarbageForFile(RoamingDir + @"\KuGou8\Patch\install.log", "酷狗安装日志");
            ScanGarbageForFile(RoamingDir + @"\KuGou8\kugou.ini.bak", "酷狗其他垃圾");
            ScanGarbageForDir(LocalDir + @"\obsidian-updater", "Obsidian 更新");
            ScanGarbageForFile(RoamingDir + @"\StardewValley\ErrorLogs\SMAPI-latest.txt", "SMAPI 错误报告");
            ScanGarbageForDir(LocalLowDir + @"\Adobe\CRLogs", "Adobe CR日志");
            ScanGarbageForDir(RoamingDir + @"\Tencent\Logs", "腾讯程序日志");
            ScanGarbageForDir(RoamingDir + @"\Tencent\pallas\teniodl\Logs", "腾讯程序日志2");
            ScanGarbageForDir(RoamingDir + @"\IDM\DwnlData", "IDM下载数据");
            ScanGarbageForFile(@"C:\Windows\unlocker.log", "Unlocker日志");
            ScanGarbageForDirs([
                @"C:\ProgramData\Battle.net\Agent\Logs", @"C:\ProgramData\Battle.net\Setup\fenris_2\Logs",
                LocalDir + @"\Battle.net\Logs", @"C:\ProgramData\Blizzard Entertainment\Battle.net\Cache",
                LocalDir + @"\Battle.net\Cache"
            ], "暴雪战网");
            ScanGarbageForDirs([
                LocalDir + @"\Microsoft\Office\16.0\WebServiceCache\AllUsers",
                RoamingDir + @"\Microsoft\Office\Recent"
            ], "Office16");
            ScanGarbageForDirs([
                LocalDir + @"\Microsoft\Office\OTele",
                @"C:\Windows\System32\config\systemprofile\AppData\Local\Microsoft\Office\OTele",
                @"C:\Windows\SysWOW64\config\systemprofile\AppData\Local\Microsoft\Office\OTele"
            ], "MS Office");
            #endregion

            #region 主要面向高级用户的软件垃圾
            ScanGarbageForDir(LocalDir + @"\Comms\Unistore\data\temp", "Unistore缓存");
            ScanGarbageForDir(LocalDir + @"\Xamarin\Logs", "Xamarin日志");
            ScanGarbageForDir(@"C:\Users\" + userName + @"\.mcreator\logs", "MCreator日志");
            ScanGarbageForDir(@"C:\Users\" + userName + @"\.mcreator\gradle\.tmp", "MCreator构建缓存");
            ScanGarbageForDir(RoamingDir + @"\NVIDIA\ComputeCache", "NVIDIA计算缓存");
            ScanGarbageForDir(@"C:\ProgramData\NVIDIA Corporation\FrameViewSDK", "FrameViewSDK");
            ScanGarbageForFiles([
                @"C:\ProgramData\NVIDIA\DisplaySessionContainer1.log",
                @"C:\ProgramData\NVIDIA\DisplaySessionContainer1.log.log_backup1",
                @"C:\ProgramData\NVIDIA\DisplaySessionContainer2.log",
                @"C:\ProgramData\NVIDIA\DisplaySessionContainer2.log.log_backup1"
            ], "NVDisplay日志");
            ScanGarbageForDir(@"C:\ProgramData\Microsoft\VisualStudio\Packages", "MS Visual Studio 安装包");
            
            #endregion

            void ScanGarbageForDir(string path, string name, bool Checked = true)
            {
                try
                {
                    progress?.Report($"正在扫描: {name}...\n");
                    Log.Info($"[CUT] 正在扫描: {name}...");

                    if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
                    {
                        checkedListBox.Invoke((MethodInvoker)delegate
                        {
                            checkedListBox.Items.Add(name);
                            checkedListBox.SetItemChecked(checkedListBox.Items.Count - 1, Checked);
                        });

                        FileGetMain(path);
                    }
                    else
                    {
                        progress?.Report($"  {name} 未发现垃圾\n");
                        Log.Info($"[CUT] {name} 未发现垃圾");
                    }
                }
                catch (Exception ex)
                {
                    progress?.Report($"扫描 {name} 时出错: {ex.Message}\n");
                    Log.Err($"[CUT] 扫描 {name} 时出错: {ex.Message}");
                }
            }

            void ScanGarbageForDirs(string[] paths, string name, bool Checked = true)
            {
                try
                {
                    progress?.Report($"正在扫描: {name}...\n");
                    Log.Info($"[CUT] 正在扫描: {name}...");

                    bool hasFiles = false;
                    foreach (string path in paths)
                    {
                        if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
                        {
                            hasFiles = true;
                            FileGetMain(path);
                        }
                    }

                    if (hasFiles)
                    {
                        checkedListBox.Invoke((MethodInvoker)delegate
                        {
                            checkedListBox.Items.Add(name);
                            checkedListBox.SetItemChecked(checkedListBox.Items.Count - 1, Checked);
                        });
                    }
                    else
                    {
                        progress?.Report($"  {name} 未发现垃圾\n");
                        Log.Info($"[CUT] {name} 未发现垃圾");
                    }
                }
                catch (Exception ex)
                {
                    progress?.Report($"扫描 {name} 时出错: {ex.Message}\n");
                    Log.Err($"[CUT] 扫描 {name} 时出错: {ex.Message}");
                }
            }

            void ScanGarbageForFile(string path, string name, bool Checked = true)
            {
                try
                {
                    progress?.Report($"正在扫描: {name}...\n");
                    Log.Info($"[CUT] 正在扫描: {name}...");

                    if (File.Exists(path))
                    {
                        checkedListBox.Invoke((MethodInvoker)delegate
                        {
                            checkedListBox.Items.Add(name);
                            checkedListBox.SetItemChecked(checkedListBox.Items.Count - 1, Checked);
                        });

                        var info = new FileInfo(path);
                        AllFileSize += info.Length;
                        AllFileCount++;
                        progress?.Report($"  找到文件: {path} - 大小: {BytesIntelligentConversion(info.Length)}\n");
                    }
                    else
                    {
                        progress?.Report($"  {name} 未发现垃圾\n");
                        Log.Info($"[CUT] {name} 未发现垃圾");
                    }
                }
                catch (Exception ex)
                {
                    progress?.Report($"扫描 {name} 时出错: {ex.Message}\n");
                    Log.Err($"[CUT] 扫描 {name} 时出错: {ex.Message}");
                }
            }

            void ScanGarbageForFiles(string[] paths, string name, bool Checked = true)
            {
                try
                {
                    progress?.Report($"正在扫描: {name}...\n");
                    Log.Info($"[CUT] 正在扫描: {name}...");

                    bool hasFiles = false;
                    foreach (string path in paths)
                    {
                        if (File.Exists(path))
                        {
                            hasFiles = true;
                            var info = new FileInfo(path);
                            AllFileSize += info.Length;
                            AllFileCount++;
                            progress?.Report($"  找到文件: {path} - 大小: {BytesIntelligentConversion(info.Length)}\n");
                            Log.Info($"[CUT] 找到文件: {path} - 大小: {BytesIntelligentConversion(info.Length)}");
                        }
                    }

                    if (hasFiles)
                    {
                        checkedListBox.Invoke((MethodInvoker)delegate
                        {
                            checkedListBox.Items.Add(name);
                            checkedListBox.SetItemChecked(checkedListBox.Items.Count - 1, Checked);
                        });
                    }
                    else
                    {
                        progress?.Report($"  {name} 未发现垃圾\n");
                        Log.Info($"[CUT] {name} 未发现垃圾");
                    }
                }
                catch (Exception ex)
                {
                    progress?.Report($"扫描 {name} 时出错: {ex.Message}\n");
                    Log.Err($"[CUT] 扫描 {name} 时出错: {ex.Message}");
                }
            }

            void FileGetMain(string path)
            {
                try
                {
                    progress?.Report($"  扫描目录: {path}\n");
                    Log.Info($"[CUT] 扫描目录: {path}");

                    // 扫描文件
                    foreach (string file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                    {
                        try
                        {
                            var info = new FileInfo(file);
                            AllFileSize += info.Length;
                            AllFileCount++;

                            // 每扫描100个文件报告一次进度
                            if (AllFileCount % 100 == 0)
                            {
                                progress?.Report($"  已扫描 {AllFileCount} 个文件, 大小: {BytesIntelligentConversion(AllFileSize)}\n");
                                Log.Info($"[CUT] 已扫描 {AllFileCount} 个文件, 大小: {BytesIntelligentConversion(AllFileSize)}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Err($"[CUT] 扫描目录 {path} 时出错: {ex.Message}");
                        }
                    }

                    progress?.Report($"  完成扫描: {path} - 找到 {AllFileCount} 个文件\n");
                    Log.Info($"[CUT] 完成扫描: {path} - 找到 {AllFileCount} 个文件");
                }
                catch (Exception ex)
                {
                    progress?.Report($"  扫描目录 {path} 时出错: {ex.Message}\n");
                    Log.Err($"[CUT] 扫描目录 {path} 时出错: {ex.Message}");
                }
            }
        }

        private async void buttonCleaningUp_Click(object sender, EventArgs e)
        {
            AllDeleteFileCount = 0;
            AllDeleteFileSize = 0;

            richTextBox.Clear();  // 清空文本框
            buttonCleaningUp.Enabled = false;
            label_AllDeleteFileSize.Visible = true;
            label_AllDeleteFileCount.Visible = true;

            // 创建进度报告器
            var progress = new Progress<string>(log =>
            {
                richTextBox.AppendText(log);  // 实时追加日志
                richTextBox.ScrollToCaret();  // 自动滚动到底部

                // 实时更新计数
                switch (GetLanguage())
                {
                    case "en":
                        label_AllDeleteFileSize.Text = "Del total: " + BytesIntelligentConversion(AllDeleteFileSize);
                        label_AllDeleteFileCount.Text = "Del size: " + AllDeleteFileCount;
                        break;

                    case "zh-CN":
                    default:
                        label_AllDeleteFileSize.Text = "删除大小：" + BytesIntelligentConversion(AllDeleteFileSize);
                        label_AllDeleteFileCount.Text = "删除总数：" + AllDeleteFileCount;
                        break;
                }
            });

            Log.Info($"[CUT] ==================== 开始执行清理 ====================");

            // 异步执行清理
            await Task.Run(() => RunClean(progress));

            checkedListBox.Items.Clear();
        }


        private void RunClean(IProgress<string> progress)
        {
            for (int i = 0; i < checkedListBox.Items.Count; ++i)
            {
                if (checkedListBox.GetItemChecked(i))
                {
                    string item = checkedListBox.GetItemText(checkedListBox.Items[i]);
                    string logs = CleanMain(item);

                    // 报告当前项的清理结果
                    progress.Report($"\n===== 正在清理: {item} =====\n");
                    progress.Report(logs);

                    // 写入日志
                    Log.Info($"[CUT] ===== 正在清理: {item} =====\n");
                    Log.Info(StringMakeLineHead(logs, "[CUT] "));
                }
            }
        }

        private string CleanMain(string Str)
        {
            string logs = string.Empty;
            switch (Str)
            {
                #region 系统垃圾
                case "系统缓存":
                    logs += KillProc("OfficeClickToRun");
                    logs += KillProc("OfficeClickToRun");
                    logs += DeleteDirectoryNd(@"C:\Windows\Temp");
                    logs += DeleteDirectoryNd(@"C:\Windows\System32\WDI", "{");
                    logs += DeleteDirectoryNd(@"C:\Windows\System32\SleepStudy");
                    logs += DeleteFile(@"C:\Windows\SoftwareDistribution\ReportingEvents.log");
                    break;
                case "系统日志":
                    logs += DeleteDirectoryNd(@"C:\Windows\Logs");
                    logs += DeleteDirectoryNd(@"C:\Windows\security\logs");
                    logs += DeleteDirectoryNd(@"C:\Windows\System32\LogFiles");
                    logs += DeleteDirectoryNd(@"C:\Windows\System32\WDI\LogFiles");
                    logs += DeleteDirectoryNd(@"C:\ProgramData\Microsoft\Network\Downloader");
                    logs += DeleteDirectoryNd(@"C:\Windows\Panther\Rollback\MachineIndependent\Transformers\CBS\boot_volume\WinLH\WinSxS\Catalogs");
                    logs += DeleteFile(@"C:\Windows\System32\catroot2\dberr.txt");
                    logs += DeleteFile(@"C:\Windows\Panther\UnattendGC\diagerr.xml");
                    logs += DeleteFile(@"C:\Windows\Panther\UnattendGC\diagwrn.xml");
                    logs += DeleteFile(@"C:\Windows\Panther\DDACLSys.log");
                    logs += DeleteFile(@"C:\Windows\INF\setupapi.setup.log");
                    logs += DeleteFile(@"C:\Windows\debug\WIA\wiatrace.log");
                    logs += DeleteFile(@"C:\Windows\debug\mrt.log");
                    logs += DeleteFile(@"C:\Windows\debug\NetSetup.LOG");
                    logs += DeleteFile(@"C:\Windows\INF\setupapi.dev.log");
                    logs += DeleteFile(@"C:\Windows\INF\setupapi.offline.log");
                    logs += DeleteFile(@"C:\Windows\INF\setupapi.upgrade.log");
                    logs += DeleteFile(@"C:\Windows\Panther\Rollback\EFI\Microsoft\Boot\BCD.LOG");
                    logs += DeleteFile(@"C:\Windows\Panther\Rollback\WinPE\setupapi\setupapi.offline.log");
                    logs += DeleteFile(@"C:\Windows\Performance\WinSAT\winsat.log");
                    break;
                case "系统USO日志":
                    logs += DeleteDirectoryNd(@"C:\ProgramData\USOShared\Logs\System");
                    logs += DeleteDirectoryNd(@"C:\ProgramData\USOShared\Logs\User");
                    break;
                case "系统错误报告":
                    logs += DeleteDirectoryNd(@"C:\ProgramData\Microsoft\Windows\WER\ReportArchive");
                    logs += DeleteDirectoryNd(@"C:\ProgramData\Microsoft\Windows\WER\ReportQueue");
                    logs += DeleteDirectoryNd(@"C:\ProgramData\Microsoft\Windows\WER\Temp");
                    break;
                case "本地程序缓存":
                    logs += KillProc("steamwebhelper");
                    logs += KillProc("msedge");
                    logs += KillProc("IDMan");
                    logs += KillProc("KuGou");
                    logs += DeleteDirectoryNd(LocalDir + @"\Temp");
                    break;
                case "低优先级程序缓存":
                    logs = DeleteDirectoryNd(LocalLowDir + @"\Temp");
                    break;
                case "诊断报告":
                    logs = DeleteDirectoryNd(LocalDir + @"\ElevatedDiagnostics");
                    break;
                case "最近启动":
                    logs = DeleteDirectoryNd(@"C:\Users\" + userName + @"\Recent");
                    break;
                case "通用互联网临时文件":
                    logs = DeleteDirectoryNd(@"C:\Users\" + userName + @"\‌Cookies");
                    break;
                case "驱动程序安装日志":
                    logs = DeleteFile(@"C:\Windows\DIFx.log");
                    break;
                case "缩率图缓存":
                    logs = DeleteDirectoryNd(LocalDir + @"\Microsoft\Windows\Explorer");
                    break;
                case "缓存的证书文件":
                    logs = DeleteDirectoryNd(LocalLowDir + @"\Microsoft\CryptnetUrlCache\Content");
                    logs += DeleteDirectoryNd(@"C:\Windows\System32\config\systemprofile\AppData\LocalLow\Microsoft\CryptnetUrlCache\Content");
                    logs += DeleteDirectoryNd(@"C:\Windows\System32\config\systemprofile\AppData\LocalLow\Microsoft\CryptnetUrlCache\MetaData");
                    logs += DeleteDirectoryNd(@"C:\Windows\SysWOW64\config\systemprofile\AppData\LocalLow\Microsoft\CryptnetUrlCache\Content");
                    logs += DeleteDirectoryNd(@"C:\Windows\SysWOW64\config\systemprofile\AppData\LocalLow\Microsoft\CryptnetUrlCache\MetaData");
                    break;
                case "DX 接口日志":
                    logs = DeleteFile(@"C:\Windows\DirectX.log");
                    break;
                case "ISA 日志文件":
                    logs = DeleteFile(@"C:\Windows\PFRO.log");
                    break;
                case "DTC 安装日志":
                    logs = DeleteFile(@"C:\Windows\DtcInstall.log");
                    break;
                case "系统安装错误日志":
                    logs = DeleteFile(@"C:\Windows\setuperr.log");
                    break;
                case "系统更新日志":
                    logs = DeleteFile(@"C:\Windows\WindowsUpdate.log");
                    break;
                case "系统更新信息":
                    logs = DeleteDirectoryNd(@"C:\Windows\SoftwareDistribution\DataStore\Logs");
                    break;
                case "系统更新下载":
                    logs = DeleteDirectoryNd(@"C:\Windows\SoftwareDistribution\Download");
                    break;
                case "彩色日志":
                    logs = DeleteDirectoryNd(RoamingDir + @"\PLogs");
                    break;
                case "CRL缓存":
                    logs = DeleteDirectoryNd(@"C:\Users\Administrator\AppData\LocalLow\Microsoft\CryptnetUrlCache\Content");
                    logs += DeleteDirectoryNd(@"C:\Users\Administrator\AppData\LocalLow\Microsoft\CryptnetUrlCache\MetaData");
                    break;
                case "Delivery Optimization日志":
                    logs = DeleteDirectoryNd(@"C:\Windows\ServiceProfiles\NetworkService\AppData\Local\Microsoft\Windows\DeliveryOptimization\Logs");
                    break;
                case "MS搜索缓存":
                    logs = DeleteDirectoryNd(@"C:\ProgramData\Microsoft\Search\Data\Applications\Windows\GatherLogs\SystemIndex");
                    break;
                case "SubSystems":
                    logs = DeleteDirectoryNd(@"C:\Windows\System32\sru");
                    break;
                case "3D着色器":
                    logs = DeleteDirectoryNd(LocalDir + @"\D3DSCache");
                    break;
                case ".NET 全局程序集缓存":
                    logs = DeleteDirectoryNd(@"C:\Windows\assembly");
                    break;
                case "预读文件缓存":
                    logs = DeleteDirectoryNd(@"C:\Windows\Prefetch");
                    break;
                case "最近打开的文件":
                    logs = DeleteDirectoryNd(RoamingDir + @"\Microsoft\Windows\Recent");
                    break;
                #endregion

                #region 游戏垃圾
                case "i18nupdatemod":
                    logs = DeleteDirectoryNd(LocalDir + @"\.i18nupdatemod");
                    break;
                #endregion

                #region 主要面向用户的软件垃圾
                case "齐系列程序缓存":
                    logs = KillProc("qit");
                    logs += KillProc("QisToolkit");
                    logs += KillProc("QisToolkitIS");
                    logs += DeleteDirectoryNd(@"C:\QiAppDatas\Temps");
                    break;
                case "IE 网页缓存":
                    logs = DeleteDirectoryNd(LocalDir + @"\Microsoft\Windows\WebCache");
                    logs += DeleteDirectoryNd(LocalDir + @"\Microsoft\Internet Explorer\CacheStorage");
                    logs += DeleteDirectoryNd(LocalDir + @"\Microsoft\Windows\INetCache");
                    break;
                case "Edge 日志":
                    logs = KillProc("msedge");
                    logs += DeleteDirectoryNd(LocalDir + @"\Microsoft\Edge\User Data\Default", "LOG");
                    logs += DeleteDirectoryNd(LocalDir + @"\Microsoft\Edge\User Data\Default", "LOG.old");
                    break;
                case "Edge 缓存":
                    logs = KillProc("msedge");
                    logs += DeleteDirectoryNd(LocalDir + @"\Microsoft\Edge\User Data\Default\Code Cache");
                    logs += DeleteDirectoryNd(LocalDir + @"\Microsoft\Edge\User Data\Default\Cache\Cache_Data");
                    break;
                case "Edge 视频解码统计信息":
                    logs = KillProc("msedge");
                    logs += DeleteDirectoryNd(LocalDir + @"\Microsoft\Edge\User Data\Default\VideoDecodeStats");
                    break;
                case "Edge 更新缓存":
                    logs = KillProc("msedge");
                    logs += DeleteDirectoryNd(@"C:\Program Files (x86)\Microsoft\EdgeUpdate\Download");
                    break;
                case "夸克浏览器 缓存":
                    logs += DeleteDirectoryNd(LocalDir + @"\Quark\User Data\Default\Cache\Cache_Data");
                    break;
                case "Obsidian 更新":
                    logs = DeleteDirectoryNd(LocalDir + @"\obsidian-updater");
                    break;
                case "SMAPI 错误报告":
                    logs = DeleteFile(RoamingDir + @"\StardewValley\ErrorLogs\SMAPI-latest.txt");
                    break;
                case "Adobe CR日志":
                    logs = DeleteDirectoryNd(LocalLowDir + @"\Adobe\CRLogs");
                    break;
                case "腾讯程序日志":
                    logs = DeleteDirectoryNd(RoamingDir + @"\Tencent\Logs");
                    break;
                case "腾讯程序日志2":
                    logs = DeleteDirectoryNd(RoamingDir + @"\Tencent\pallas\teniodl\Logs");
                    break;
                case "酷狗歌曲缓存":
                    logs = KillProc("KuGou");
                    logs += DeleteDirectoryNd(@"C:\KuGou\Temp");
                    logs += DeleteDirectoryNd(@"D:\KuGou\Temp");
                    logs += DeleteDirectoryNd(@"E:\KuGou\Temp");
                    break;
                case "酷狗安装日志":
                    logs = KillProc("KuGou");
                    logs += DeleteFile(RoamingDir + @"\KuGou8\Patch\install.log");
                    break;
                case "酷狗日志":
                    logs = KillProc("KuGou");
                    logs += DeleteDirectoryNd(RoamingDir + @"\KuGou8\log");
                    break;
                case "酷狗其他垃圾":
                    logs = KillProc("KuGou");
                    logs += DeleteFile(RoamingDir + @"\KuGou8\kugou.ini.bak");
                    break;
                case "IDM下载数据":
                    logs = KillProc("IDMan");
                    logs += DeleteDirectoryNd(RoamingDir + @"\IDM\DwnlData");
                    break;
                case "Unlocker日志":
                    logs = KillProc("IObitUnlocker");
                    logs += DeleteFile(@"C:\Windows\unlocker.log");
                    break;
                case "暴雪战网":
                    logs = KillProc("IObitUnlocker");
                    logs += DeleteDirectoryNd(@"C:\ProgramData\Battle.net\Agent\Logs");
                    logs += DeleteDirectoryNd(@"C:\ProgramData\Battle.net\Setup\fenris_2\Logs");
                    logs += DeleteDirectoryNd(LocalDir + @"\Battle.net\Logs");
                    logs += DeleteDirectoryNd(@"C:\ProgramData\Blizzard Entertainment\Battle.net\Cache");
                    logs += DeleteDirectoryNd(LocalDir + @"\Battle.net\Cache");
                    break;
                case "Office16":
                    logs += DeleteDirectoryNd(LocalDir + @"\Microsoft\Office\16.0\WebServiceCache\AllUsers");
                    logs += DeleteDirectoryNd(RoamingDir + @"\Microsoft\Office\Recent");
                    break;
                case "MS Office":
                    logs += DeleteDirectoryNd(LocalDir + @"\Microsoft\Office\OTele");
                    logs += DeleteDirectoryNd(@"C:\Windows\System32\config\systemprofile\AppData\Local\Microsoft\Office\OTele");
                    logs += DeleteDirectoryNd(@"C:\Windows\SysWOW64\config\systemprofile\AppData\Local\Microsoft\Office\OTele");
                    break;
                #endregion

                #region 主要面向开发者的软件垃圾
                case "Unistore缓存":
                    logs = DeleteDirectoryNd(LocalDir + @"\Comms\Unistore\data\temp");
                    break;
                case "Xamarin日志":
                    logs = DeleteDirectoryNd(LocalDir + @"\Xamarin\Logs");
                    break;
                case "MCreator日志":
                    logs = KillProc("javaw");
                    logs += KillProc("mcreator");
                    logs += DeleteDirectoryNd(@"C:\Users\" + userName + @"\.mcreator\logs");
                    break;
                case "MCreator构建缓存":
                    logs = KillProc("javaw");
                    logs += KillProc("mcreator");
                    logs += DeleteDirectoryNd(@"C:\Users\" + userName + @"\.mcreator\gradle\.tmp");
                    break;
                case "NVIDIA计算缓存":
                    logs = DeleteDirectoryNd(RoamingDir + @"\NVIDIA\ComputeCache");
                    break;
                case "FrameViewSDK":
                    logs = KillProc("NVIDIA Overlay");
                    logs += DeleteDirectoryNd(@"C:\ProgramData\NVIDIA Corporation\FrameViewSDK");
                    break;
                case "NVDisplay日志":
                    logs = DeleteFile(@"C:\ProgramData\NVIDIA\DisplaySessionContainer1.log");
                    logs += DeleteFile(@"C:\ProgramData\NVIDIA\DisplaySessionContainer2.log");
                    logs += DeleteFile(@"C:\ProgramData\NVIDIA\DisplaySessionContainer1.log.log_backup1");
                    logs += DeleteFile(@"C:\ProgramData\NVIDIA\DisplaySessionContainer2.log.log_backup1");
                    break;
                case "MS Visual Studio 安装包":
                    logs = DeleteDirectoryNd(@"C:\ProgramData\Microsoft\VisualStudio\Packages");
                    break;
                    #endregion
            }

            return logs;
        }

        private string KillProc(string Proc)
        {
            if (checkBoxKillProcess.Checked)
                return TryProcKill(Proc);
            return string.Empty;
        }

        //public void FileGetMain(string targetDir, string searchPattern = "*")
        //{
        //    if (Directory.Exists(targetDir))
        //    {
        //        string[] files = TryGetFiles(targetDir, searchPattern);
        //        string[] dirs = TryGetDirectories(targetDir);
        //        if (files != null && dirs != null)
        //        {
        //            foreach (string file in files)
        //            {
        //                AllFileSize += new FileInfo(file).Length;
        //                ++AllFileCount;
        //            }

        //            foreach (string dir in dirs)
        //                if (dir != targetDir)
        //                    FileGetMain(dir);
        //        }
        //    }
        //}

        private void checkBoxUserCompatibilityMode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxUserCompatibilityMode.Checked)
            {
                LocalDir = $@"C:\Users\{userName}\AppData\Local";
                LocalLowDir = $@"C:\Users\{userName}\AppData\LocalLow";
                RoamingDir = $@"C:\Users\{userName}\AppData\Roaming";
            }
            else
            {
                LocalDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                RoamingDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                LocalLowDir = LocalDir + "Low";
            }
        }

        private void label_AllFileSize_Click(object sender, EventArgs e) =>
            MessageBox.Show(AllFileSize + "B", "详细大小");

        private void label_AllDeleteFileSize_Click(object sender, EventArgs e) =>
            MessageBox.Show(AllDeleteFileSize + "B", "详细大小");

        private void checkBoxKillAllUserProcess_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxKillProcess.Checked)
                checkBoxKillProcess.Checked =
                    MessageBox.Show("此功能会在将垃圾清理之前先关闭相关进程，\n这可以使更多垃圾都可以被清理，但同样\n这将有可能导致部分程序在未保存的情况下被关闭！！\n\n此选项可能延长清理时间", "警告", MessageBoxButtons.OKCancel) == DialogResult.OK;
        }



        // 尝试删除文件
        public string DeleteFile(string path)
        {
            try
            {
                long FileLengthTmp = new FileInfo(path).Length;
                File.Delete(path); ++AllDeleteFileCount;
                AllDeleteFileSize += FileLengthTmp;
                return MakeLogText_1(path, FileLengthTmp);
            }
            catch (Exception ex)
            {
                return MakeLogText_0(path, ex);
            }
        }

        // 尝试删除文件夹下的所有子文件与子目录或目录树
        public string DeleteDirectoryNd(string targetDir, string searchPattern = "*")
        {
            string CleanUpLog = "";

            if (Directory.Exists(targetDir))
            {
                long FileLengthTmp;
                string[] files = TryGetFiles(targetDir, searchPattern);
                string[] dirs = TryGetDirectories(targetDir);

                if (files != null && dirs != null)
                {
                    foreach (string file in files)
                    {
                        try
                        {
                            File.SetAttributes(file, FileAttributes.Normal);
                            try
                            {
                                FileLengthTmp = new FileInfo(file).Length;
                                File.Delete(file); ++AllDeleteFileCount;
                                AllDeleteFileSize += FileLengthTmp;
                                CleanUpLog += MakeLogText_1(file, FileLengthTmp);
                            }
                            catch (Exception ex)
                            {
                                CleanUpLog += MakeLogText_0(file, ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            CleanUpLog += MakeLogText__0(file, ex);
                        }
                    }



                    foreach (string dir in dirs)
                    {
                        DeleteDirectoryNd(dir, searchPattern);

                        try
                        {
                            if (IsDirectoryEmpty(dir))
                                Directory.Delete(dir);
                        }
                        catch (Exception ex)
                        {
                            CleanUpLog += MakeLogText__0(dir, ex);
                        }
                    }
                }
            }

            return CleanUpLog;
        }

        // 尝试删除文件夹下的所有子文件
        public string DeleteDirectoryFiles(string targetDir, string searchPattern = "*")
        {
            string CleanUpLog = "";

            if (Directory.Exists(targetDir))
            {
                long FileLengthTmp;
                string[] files = Directory.GetFiles(targetDir, searchPattern);

                foreach (string file in files)
                {
                    try
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        try
                        {
                            FileLengthTmp = new FileInfo(file).Length;
                            File.Delete(file); ++AllDeleteFileCount;
                            AllDeleteFileSize += FileLengthTmp;
                            CleanUpLog += MakeLogText_1(file, FileLengthTmp);
                        }
                        catch (Exception ex)
                        {
                            CleanUpLog += MakeLogText_0(file, ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        CleanUpLog += MakeLogText__0(file, ex);
                    }
                }
            }

            return CleanUpLog;
        }

        private void CleaningUpTrash_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            MessageBox.Show("本功能在以下情况下严禁使用！\n\n·在系统更新时", "提示", MessageBoxButtons.OK);
        }

        static string[] TryGetFiles(string targetDir, string searchPattern)
        {
            try
            {
                return Directory.GetFiles(targetDir, searchPattern);
            }
            catch
            {
                return null;
            }
        }

        static string[] TryGetDirectories(string targetDir)
        {
            try
            {
                return Directory.GetDirectories(targetDir);
            }
            catch
            {
                return null;
            }
        }

        private void CleaningUpTrash_Load(object sender, EventArgs e)
        {

        }
    }
}
