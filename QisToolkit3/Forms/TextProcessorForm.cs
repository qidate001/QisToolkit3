using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace QisToolkit3.Forms
{
    public partial class TextProcessorForm : Form
    {
        private string path_fullPath = "", path_directory = "", path_fileName = "";
        private bool IsRunningText = false;

        public TextProcessorForm(string text = "")
        {
            InitializeComponent();
            richTextBox.Text = text;
            richTextBox_TextChanged(null, null);

            // 初始化
            Qi.FormInitDo(this.Text);

            // 启用RichTextBox的拖放功能
            //richTextBox.AllowDrop = true;

            // 注册拖放事件
            //richTextBox.DragEnter += RichTextBox_DragEnter;
            //richTextBox.DragDrop += RichTextBox_DragDrop;
        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            if (IsRunningText)
                return;

            bool isValidFilePath = PathData(richTextBox.Text);

            IsRunningText = true;

            // 路径
            if (isValidFilePath)
            {
                textBox_Path_FullPath.Text = path_fullPath;
                textBox_Path_FileName.Text = path_fileName;
                textBox_Path_Directory.Text = path_directory;
            }

            // 路径启用
            if (textBox_Path_FullPath.Enabled != isValidFilePath)
            {
                textBox_Path_FullPath.Enabled = isValidFilePath;
                textBox_Path_FileName.Enabled = isValidFilePath;
                textBox_Path_Directory.Enabled = isValidFilePath;
            }

            IsRunningText = false;
        }

        public bool PathData(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    return false;

                // 检查路径中的非法字符
                if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                    return false;

                // 获取路径的根目录部分（如果有）
                string root = Path.GetPathRoot(path);

                // 如果是绝对路径，检查根目录格式
                if (!string.IsNullOrEmpty(root))
                {
                    // Windows 系统下检查驱动器格式
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        // 根目录应该是类似 "C:\" 的格式
                        if (root.Length < 2 || root[1] != ':')
                            return false;
                    }
                }

                // 尝试获取完整路径来验证格式
                path_fullPath = Path.GetFullPath(path);

                // 分离目录和文件名部分
                path_directory = Path.GetDirectoryName(path_fullPath);
                path_fileName = Path.GetFileName(path_fullPath);

                // 如果是目录路径（以分隔符结尾或者没有文件名）
                bool isDirectoryPath = path.EndsWith(Path.DirectorySeparatorChar.ToString()) ||
                                      path.EndsWith(Path.AltDirectorySeparatorChar.ToString()) ||
                                      string.IsNullOrEmpty(path_fileName);

                if (isDirectoryPath)
                {
                    // 目录路径：验证目录名部分
                    if (!string.IsNullOrEmpty(path_fileName))
                    {
                        // 如果有文件名部分，检查是否包含非法字符
                        if (path_fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                            return false;
                    }
                }
                else
                {
                    // 文件路径：必须验证文件名
                    if (string.IsNullOrEmpty(path_fileName) ||
                        path_fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // 可以记录日志
                // Console.WriteLine($"路径验证异常: {ex.Message}");
                return false;
            }
        }

        public static string GetParentDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            // 处理带引号的情况
            path = path.Trim().Trim('"');

            // 如果是根目录，直接返回自身
            if (IsRootDirectory(path))
                return path;

            // 移除末尾的分隔符（如果有）
            string normalizedPath = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // 获取父目录
            string parent = Directory.GetParent(normalizedPath)?.FullName;

            // 如果无法获取父目录（已经是根目录），返回原路径
            if (string.IsNullOrEmpty(parent))
                return path;

            // 返回父目录并确保以分隔符结尾，但要避免重复分隔符
            return EnsureTrailingSeparator(parent);
        }

        private static bool IsRootDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            string normalizedPath = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // 检查是否是驱动器根目录（如 C:\）
            if (normalizedPath.Length == 3 && char.IsLetter(normalizedPath[0]) && normalizedPath[1] == ':' && normalizedPath[2] == '\\')
                return true;

            // 检查是否是网络路径根目录（如 \\server\share）
            if (normalizedPath.StartsWith(@"\\") && normalizedPath.Split('\\').Length <= 3)
                return true;

            return false;
        }

        private static string EnsureTrailingSeparator(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            // 如果路径已经是根目录格式，直接返回
            if (IsRootDirectory(path))
                return path;

            // 如果路径不以分隔符结尾，添加分隔符
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()) &&
                !path.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
            {
                return path + Path.DirectorySeparatorChar;
            }

            return path;
        }

        private void textBox_Path_FullPath_TextChanged(object sender, EventArgs e)
        {
            if (IsRunningText)
                return;

            IsRunningText = true;

            try
            {
                // 直接使用完整路径更新 richTextBox
                richTextBox.Text = textBox_Path_FullPath.Text;
            }
            finally
            {
                IsRunningText = false;
            }
        }

        private void textBox_Path_FileName_TextChanged(object sender, EventArgs e)
        {
            if (IsRunningText)
                return;

            IsRunningText = true;

            try
            {
                // 组合目录和文件名形成完整路径
                string directory = textBox_Path_Directory.Text;
                string fileName = textBox_Path_FileName.Text;

                if (!string.IsNullOrEmpty(directory) && !string.IsNullOrEmpty(fileName))
                {
                    string fullPath = Path.Combine(directory, fileName);
                    richTextBox.Text = fullPath;
                }
                else if (!string.IsNullOrEmpty(fileName))
                {
                    // 只有文件名时，直接使用文件名
                    richTextBox.Text = fileName;
                }
            }
            finally
            {
                IsRunningText = false;
            }
        }

        private void textBox_Path_Directory_TextChanged(object sender, EventArgs e)
        {
            if (IsRunningText)
                return;

            IsRunningText = true;

            try
            {
                // 组合目录和文件名形成完整路径
                string directory = textBox_Path_Directory.Text;
                string fileName = textBox_Path_FileName.Text;

                if (!string.IsNullOrEmpty(directory) && !string.IsNullOrEmpty(fileName))
                {
                    string fullPath = Path.Combine(directory, fileName);
                    richTextBox.Text = fullPath;
                }
                else if (!string.IsNullOrEmpty(directory))
                {
                    // 只有目录时，使用目录路径（确保以分隔符结尾）
                    string dirPath = directory;
                    if (!dirPath.EndsWith(Path.DirectorySeparatorChar.ToString()) &&
                        !dirPath.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
                    {
                        dirPath += Path.DirectorySeparatorChar;
                    }
                    richTextBox.Text = dirPath;
                }
            }
            finally
            {
                IsRunningText = false;
            }
        }

        private void button_Path_Open_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                richTextBox.Text = openFileDialog.FileNames[0];
        }

        private void button_Path_OpenDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                richTextBox.Text = folderBrowserDialog.SelectedPath + '\\';
        }

        private void button_Path_SetParentDirectory_Click(object sender, EventArgs e)
        {
            richTextBox.Text = GetParentDirectory(path_fullPath);
        }

        private void button_Path_SetSystemSystemDrive_Click(object sender, EventArgs e)
        {
            richTextBox.Text = SystemPaths.SystemDrive + '\\';
        }

        private void button_Path_SetWindows_Click(object sender, EventArgs e)
        {
            richTextBox.Text = SystemPaths.WindowsPath + '\\';
        }
    }
}