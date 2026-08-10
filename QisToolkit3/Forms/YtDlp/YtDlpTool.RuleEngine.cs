using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Qi;

namespace QisToolkit3.Forms
{
    public partial class YtDlpTool
    {
        private async Task RenameFilesWithRuleEngine(string directoryPath)
        {
            if (!checkBox_StringRuleEngine.Checked ||
                string.IsNullOrWhiteSpace(richTextBox_StringRuleEngine.Text))
                return;

            AppendText("开始应用规则引擎重命名文件...", "QisToolkit");

            var ruleEngine = new RuleEngine();
            var rules = richTextBox_StringRuleEngine.Text;

            // 获取所有文件
            var videoFiles = GetAllDownloadFiles();
            int renamedCount = 0;

            foreach (var videoPath in videoFiles)
            {
                try
                {
                    string fileName = Path.GetFileNameWithoutExtension(videoPath);
                    string extension = Path.GetExtension(videoPath);
                    string directory = Path.GetDirectoryName(videoPath);

                    // 应用规则引擎处理文件名
                    string newFileName = ruleEngine.ProcessText(fileName, rules);

                    // 清理非法字符
                    newFileName = IdNameMapper.SanitizeFileName(newFileName);

                    // 如果新文件名与旧文件名不同，则重命名
                    if (!string.Equals(newFileName, fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        string newPath = Path.Combine(directory, newFileName + extension);

                        // 处理重名情况
                        if (File.Exists(newPath))
                        {
                            if (comboBox_StringRuleEngine.SelectedIndex == 0)
                            {
                                AppendText($"✅ 保护模式跳过文件: {fileName}{extension}", "QisToolkit");
                            }

                            else if (comboBox_StringRuleEngine.SelectedIndex == 1)
                            {
                                AppendText($"✅ 强制模式删除文件: {fileName}{extension}", "QisToolkit");
                                TryDeleteFile(newPath);
                                File.Move(videoPath, newPath);
                                renamedCount++;
                            }
                        }

                        else
                        {
                            File.Move(videoPath, newPath);
                            renamedCount++;
                        }


                        AppendText($"✅ 重命名: {fileName}{extension} → {newFileName}{extension}", "QisToolkit");

                        // 同时重命名关联的 .info.json 和 .description 文件
                        RenameAssociatedFiles(directory, fileName, newFileName);
                    }
                }
                catch (Exception ex)
                {
                    AppendText($"❌ 重命名失败 {Path.GetFileName(videoPath)}: {ex.Message}", "QisToolkit");
                    Log.Err($"重命名文件失败: {ex.Message}");
                }
            }

            AppendText($"规则引擎重命名完成，共重命名 {renamedCount} 个文件", "QisToolkit");
        }

        /// <summary>
        /// 重命名关联文件（.info.json, .description 等）
        /// </summary>
        private void RenameAssociatedFiles(string directory, string oldName, string newName)
        {
            string[] extensions = { ".info.json", ".信息.json", ".description", ".danmaku.xml", ".ass", ".html" };

            foreach (var ext in extensions)
            {
                string oldPath = Path.Combine(directory, oldName + ext);
                string newPath = Path.Combine(directory, newName + ext);
                Log.Info(oldPath + " → " + newPath);

                if (File.Exists(oldPath))
                {
                    try
                    {
                        // 处理重名情况
                        if (File.Exists(newPath))
                        {
                            if (comboBox_StringRuleEngine.SelectedIndex == 0)
                            {
                                File.Move(oldPath, newPath);
                                AppendText($"   📎 跳过重命名关联: {oldName}{ext}", "QisToolkit");
                                TryDeleteFile(newPath);
                            }

                            else if (comboBox_StringRuleEngine.SelectedIndex == 1)
                            {
                                AppendText($"   📎 覆盖重命名关联: {oldName}{ext}", "QisToolkit");
                                TryDeleteFile(newPath);
                                File.Move(oldPath, newPath);
                            }
                        }

                        else
                        {
                            File.Move(oldPath, newPath);
                            AppendText($"   📎 重命名关联: {oldName}{ext} → {newName}{ext}", "QisToolkit");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Warn($"[YtDlp工具] 重命名关联文件失败 {oldName}{ext}: {ex.Message}");
                    }
                }
            }
        }
    }
}
