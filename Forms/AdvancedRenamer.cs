using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QisToolkit3.Forms
{
    public partial class AdvancedRenamer : Form
    {
        private string[] selectedFiles;
        private string[] selectedFileNames;

        public AdvancedRenamer()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void AdvancedRenamer_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // 设置拖放效果为复制
                e.Effect = DragDropEffects.Copy;

                // 可以添加视觉反馈
                BackColor = Color.LightYellow;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void AdvancedRenamer_DragDrop(object sender, DragEventArgs e)
        {
            // 恢复背景色
            BackColor = SystemColors.Control;

            try
            {
                // 文件
                selectedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                // 按修改时间排序（从旧到新，即最早修改的在上方）
                selectedFiles = selectedFiles
                    .OrderBy(f => new FileInfo(f).LastWriteTime)
                    .ToArray();

                // 文件名
                selectedFileNames = selectedFiles.Select(f => Path.GetFileName(f)).ToArray();

                // 显示到 ListBoxInput
                if (selectedFiles != null && selectedFiles.Length > 0)
                    SetListBoxOutput(true);
            }
            catch { }
        }

        private void SetListBoxOutput(bool ShowName)
        {
            // 清空
            listBoxInput.Items.Clear();

            // 添加到列表
            if (ShowName)
                foreach (string file in selectedFileNames)
                    listBoxInput.Items.Add(file);
            else
                foreach (string file in selectedFiles)
                    listBoxInput.Items.Add(file);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            // 检查是否有文件
            if (selectedFiles == null || selectedFiles.Length == 0)
            {
                MessageBox.Show("请先拖入文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 检查格式是否包含 $
            string pattern = textBox1.Text;
            if (string.IsNullOrWhiteSpace(pattern))
            {
                MessageBox.Show("请输入重命名格式！\n使用 $ 表示自增变量（从1开始）", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!pattern.Contains("$"))
            {
                DialogResult result = MessageBox.Show("格式中未包含 $（自增变量），是否继续？\n这将把所有文件改成相同名称。",
                    "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;
            }

            // 清空输出列表
            listBoxOutput.Items.Clear();

            // 生成新文件名（按修改时间排序后的顺序）
            int counter = 1;
            List<string> newFileNames = new List<string>();

            foreach (string filePath in selectedFiles)
            {
                // 获取原文件名（不含路径）
                string oldFileName = Path.GetFileName(filePath);

                // 获取原文件扩展名
                string extension = Path.GetExtension(oldFileName);

                // 替换 $ 为当前计数器
                string newNameWithoutExt = pattern.Replace("$", counter.ToString());

                // 如果格式中没有包含扩展名，需要加上原扩展名
                string newFileName;
                if (newNameWithoutExt.EndsWith(extension))
                {
                    newFileName = newNameWithoutExt;
                }
                else
                {
                    newFileName = newNameWithoutExt + extension;
                }

                newFileNames.Add(newFileName);
                counter++;
            }

            // 显示到 listBoxOutput
            for (int i = 0; i < newFileNames.Count; i++)
            {
                listBoxOutput.Items.Add($"{newFileNames[i]}   ←   ( {selectedFileNames[i]} )");
            }

            // 可选：询问是否实际执行重命名
            DialogResult renameResult = MessageBox.Show($"即将重命名 {newFileNames.Count} 个文件，是否执行？\n\n注意：此操作不可撤销！",
                "确认重命名", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (renameResult == DialogResult.Yes)
            {
                PerformRename(newFileNames);
            }
        }

        private void PerformRename(List<string> newFileNames)
        {
            int successCount = 0;
            int failCount = 0;
            List<string> errors = new List<string>();

            for (int i = 0; i < selectedFiles.Length; i++)
            {
                try
                {
                    string oldPath = selectedFiles[i];
                    string directory = Path.GetDirectoryName(oldPath);
                    string newPath = Path.Combine(directory, newFileNames[i]);

                    // 如果新文件名已存在，添加重名处理
                    if (File.Exists(newPath))
                    {
                        string nameWithoutExt = Path.GetFileNameWithoutExtension(newFileNames[i]);
                        string extension = Path.GetExtension(newFileNames[i]);
                        int dupCounter = 1;
                        while (File.Exists(newPath))
                        {
                            newPath = Path.Combine(directory, $"{nameWithoutExt} - 副本{dupCounter}{extension}");
                            dupCounter++;
                        }
                    }

                    File.Move(oldPath, newPath);
                    successCount++;

                    // 更新文件路径列表（用于后续操作）
                    selectedFiles[i] = newPath;
                    selectedFileNames[i] = newFileNames[i];
                }
                catch (Exception ex)
                {
                    failCount++;
                    errors.Add($"{selectedFileNames[i]}: {ex.Message}");
                }
            }

            // 更新显示
            SetListBoxOutput(true);

            // 显示结果
            string message = $"重命名完成！\n成功：{successCount} 个\n失败：{failCount} 个";
            if (errors.Count > 0)
            {
                message += $"\n\n错误详情：\n{string.Join("\n", errors.Take(5))}";
                if (errors.Count > 5)
                    message += $"\n...等 {errors.Count - 5} 个错误";
            }
            MessageBox.Show(message, "重命名结果", MessageBoxButtons.OK,
                failCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        private void AdvancedRenamer_Load(object sender, EventArgs e)
        {
            // 设置提示文字
            textBox1.Text = "a$.txt";
            textBox1.SelectAll();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}