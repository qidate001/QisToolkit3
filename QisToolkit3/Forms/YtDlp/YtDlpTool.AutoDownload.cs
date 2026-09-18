using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Qi.QisToolkit3_Datas;

namespace QisToolkit3.Forms
{
    public partial class YtDlpTool
    {
        private async Task DoAutoDownload(string customMatchFiltersData = null)
        {
            // 检查配置文件是否存在
            if (!File.Exists(AutoDownloadConfigFilePath))
            {
                CreateDefaultAutoDownloadConfig(AutoDownloadConfigFilePath);
                MessageBox.Show("已创建默认配置文件，请编辑后再执行自动下载。\n" +
                                $"文件位置：{AutoDownloadConfigFilePath}",
                                "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 解析配置文件
            var downloadItems = ParseAutoDownloadConfig(AutoDownloadConfigFilePath);

            if (downloadItems.Count == 0)
            {
                MessageBox.Show("配置文件中没有有效的下载项，请检查格式。", "提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 禁用按钮防止重复点击
            button_AutoDownload.Enabled = false;
            button_DoDownload.Enabled = false;
            button_DoAnalysis.Enabled = false;

            // 保存原始设置，以便下载完成后恢复
            var originalSettings = new
            {
                UseArchive = checkBox_UseArchive.Checked,
                ArchivePlus = checkBox_ArchivePlus.Checked,
                ClearURLData = checkBox_ClearURLData.Checked,
                OpenF = checkBox_OpenF.Checked,
                MatchFilters = checkBox_MatchFilters.Checked,
                PathHome = checkBox_Path_Home.Checked,
                HomePath = comboBox_Path_Home.Text,
                SetPaths = checkBox_SetPaths.Checked,
                Paths = textBox_Paths.Text
            };

            // 设置自动下载需要的配置
            checkBox_UseArchive.Checked = true;
            checkBox_ArchivePlus.Checked = true;
            checkBox_ClearURLData.Checked = true;
            checkBox_OpenF.Checked = false;

            int successCount = 0;
            int failCount = 0;
            bool UsePlayListItems = false;

            try
            {
                foreach (var item in downloadItems)
                {
                    Log.Info($"[YtDlp工具] 开始处理: {item.Url}");
                    Log.Info($"  名称: {(item.HasCustomName ? item.Name : "(自动解析)")}");
                    Log.Info($"  MatchFilters: {item.MatchFilters}");
                    Log.Info($"  Playlist: {item.Playlist}");

                    UsePlayListItems =
                        !string.IsNullOrWhiteSpace(item.Playlist) &&
                        item.Playlist.ToLower() != "false" &&
                        item.Playlist.ToLower() != "no";

                    // 设置启用状态
                    checkBox_MatchFilters.Checked = item.MatchFilters;

                    // 确定是否传递自定义规则数据
                    //string customMatchFiltersData = null;
                    if (item.MatchFilters && item.HasCustomMatchFilters)
                    {
                        customMatchFiltersData = item.MatchFiltersData;
                    }

                    comboBox_URL.Text = item.Url;

                    // 执行下载，传递自定义规则（如果有）
                    try
                    {
                        await DoDownload(customMatchFiltersData);
                        successCount++;
                        Log.Info($"[YtDlpTool] 完成: {item.Url}");
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        Log.Err($"[YtDlpTool] 下载失败: {item.Url}, 错误: {ex.Message}");
                    }
                }

                // 显示完成信息
                string resultMsg = $"自动下载完成！\n成功: {successCount} 个\n失败: {failCount} 个";
                Log.Info($"[YtDlp工具] {resultMsg}");
                MessageBox.Show(resultMsg, "下载完成", MessageBoxButtons.OK,
                                failCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Err($"[YtDlp工具] 自动下载过程中出现错误：{ex.Message}");
                MessageBox.Show($"下载过程中出现错误：{ex.Message}", "错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复原始设置
                checkBox_UseArchive.Checked = originalSettings.UseArchive;
                checkBox_ArchivePlus.Checked = originalSettings.ArchivePlus;
                checkBox_ClearURLData.Checked = originalSettings.ClearURLData;
                checkBox_OpenF.Checked = originalSettings.OpenF;
                checkBox_MatchFilters.Checked = originalSettings.MatchFilters;

                if (originalSettings.PathHome)
                {
                    checkBox_Path_Home.Checked = true;
                    comboBox_Path_Home.Text = originalSettings.HomePath;
                }
                else if (originalSettings.SetPaths)
                {
                    checkBox_SetPaths.Checked = true;
                    textBox_Paths.Text = originalSettings.Paths;
                }

                // 恢复按钮状态
                button_AutoDownload.Enabled = true;
                button_DoDownload.Enabled = true;
                button_DoAnalysis.Enabled = true;
            }
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

# 花间允诺
https://space.bilibili.com/697987209/lists/8731871
Name: 《花间允诺》

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

# 同桌小希
https://space.bilibili.com/3546914949695626/lists/8683776
Name: 《同桌小希》

# 我竟是伟大存在
# https://space.bilibili.com/32160535/lists/5578590
# Name: 《我竟是伟大存在》

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

# 戏神道
https://www.bilibili.com/video/BV1J1316bEZo
Name: 《戏神道》
MatchFilters: true
MatchFiltersData: duration>=60
";
            File.WriteAllText(configPath, defaultContent, Encoding.UTF8);
        }
    }
}
