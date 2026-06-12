using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Qi;
using static Qi.QisToolkit3_Datas;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QisToolkit3.Forms
{
    public partial class FFmpegTool : Form
    {
        private Process process;
        private RichTextBox outputBox;
        private DataTable dt_input = new DataTable();
        //private DataTable dt_output = new DataTable();
        private bool isUpdating = false;
        private string outputFile = "";


        private string FFmpegPath = Path.Combine(actualDirectory, @"yt-dlp\ffmpeg.exe");

        public FFmpegTool()
        {
            InitializeComponent();
            outputBox = richTextBox;

            if (!File.Exists(@$"{actualDirectory}\yt-dlp\ffmpeg.exe"))
                MessageBox.Show("环境缺失！这可能会导致一些问题", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);

            comboBox_y_or_n_or_null.SelectedIndex = 0;
            comboBox_preset.SelectedIndex = 1;


            dt_input.Columns.Add("FileName", typeof(string));
            dt_input.Columns.Add("FileType", typeof(string));
            dt_input.Columns.Add("FileNameWithExt", typeof(string));
            dt_input.Columns.Add("FullPath", typeof(string));
            dt_input.Columns.Add("Directory", typeof(string));

            dataGridView_InputFiles.DataSource = dt_input;

            // 初始化
            FormInitDo(this.Text);
        }

        private void FFmpegTool_Load(object sender, EventArgs e)
        {
            outputFile = Path.Combine(actualDirectory, "yt-dlp", "Downloads", "output.mp4");
            UpdateMainOutputPath();
        }

        private void button_Main_Click(object sender, EventArgs e)
        {
            ProcessingUserData();

            if (Inspect())
            {
                // 构建命令
                string command = BuildFFmpegCommand();

                if (!string.IsNullOrEmpty(command))
                    ExecuteCommand(command);
            }
        }

        public string BuildFFmpegCommand()
        {
            string command = string.Empty;

            // 验证输入
            if (!(dt_input.Rows.Count > 0))
            {
                MessageBox.Show("Input 输入目标不存在或不合法！", "FFmpeg 命令构建", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

            bool EnableClipExtraction = checkBox_ss.Checked || checkBox_to.Checked;


            bool AddMainInput =

                // 截取片段相关
                !EnableClipExtraction || EnableClipExtraction
                    && !(radioButton_ssto_fast.Checked || radioButton_ssto_best.Checked);

            // 主要输入
            if (AddMainInput) command += BuildFFmpegInputFileCommand();

            // 截取片段
            if (EnableClipExtraction)
            {
                // 从指定时间点开始
                if (checkBox_ss.Checked)
                {
                    command += $"-ss {comboBox_ss.Text} ";

                    // 快速模式 / 最佳模式（先指定时间后输入主要文件）
                    if (radioButton_ssto_fast.Checked || radioButton_ssto_best.Checked)
                        command += BuildFFmpegInputFileCommand();
                }

                // 到指定时间点结束
                if (checkBox_to.Checked)
                {
                    // to 模式
                    if (Regex.IsMatch(comboBox_to.Text, @"^\d{1,}:[0-5]\d:[0-5]\d$"))
                        command += $"-to {comboBox_to.Text} ";

                    // t 模式
                    else command += $"-t {comboBox_to.Text} ";
                }
            }

            // 流禁用
            if (checkBox_vn.Checked) command += "-vn ";
            if (checkBox_an.Checked) command += "-an ";
            if (checkBox_sn.Checked) command += "-sn ";


            // 流复制 完整复制
            if (checkBox_c_copy.Checked) command += $"-c copy ";

            // 选择复制
            else if (checkBox_cv_copy.Checked || checkBox_ca_copy.Checked || checkBox_cs_copy.Checked || checkBox_cd_copy.Checked)
            {
                if (checkBox_cv_copy.Checked) command += $"-c:v copy ";
                if (checkBox_ca_copy.Checked) command += $"-c:a copy ";
                if (checkBox_cs_copy.Checked) command += $"-c:s copy ";
                if (checkBox_cd_copy.Checked) command += $"-c:d copy ";
            }

            // 重编码（和复制流互斥的部分）
            else
            {

                #region 视频重编码

                // 视频编码器
                if (checkBox_c_v.Checked)
                {
                    command += comboBox_c_v.Text switch
                    {
                        "H.264" => "-c:v libx264 ",
                        "H.265" => "-c:v libx265 ",
                        "VP9" => "-c:v libvpx-vp9 ",
                        "AV1" => "-c:v libaom-av1 ",
                        _ => $"-c:v {comboBox_c_v.Text} ",
                    };
                }

                // 编码速度预设 (-preset) - 仅 libx264/libx265
                if (checkBox_preset.Checked)
                {
                    command += comboBox_preset.Text switch
                    {
                        "极快" => "-preset ultrafast ",
                        "快速" => "-preset fast ",
                        "中等" => "-preset medium ",
                        "慢速" => "-preset slow ",
                        "极慢" => "-preset veryslow ",
                        _ => "-preset medium ",
                    };
                }

                if (checkBox_b_v.Checked) command += $"-b:v {comboBox_b_v.Text} ";
                if (checkBox_r.Checked) command += $"-r {comboBox_r.Text} ";
                if (checkBox_s.Checked) command += $"-s {comboBox_s.Text} ";
                if (checkBox_crf.Checked) command += $"-crf {trackBar_crf.Value} ";

                #endregion

                #region 音频重编码

                if (checkBox_c_a.Checked)
                {
                    switch (comboBox_c_a.Text)
                    {
                        case "MP3编码器":
                            command += "-c:a libmp3lame ";
                            break;

                        case "AAC编码器":
                            command += "-c:a aac ";
                            break;

                        case "OGG编码器":
                            command += "-c:a libvorbis ";
                            break;

                        case "FLAC编码器":
                            command += "-c:a flac ";
                            break;

                        case "OPUS编码器":
                            command += "-c:a libopus ";
                            break;

                        case "AC3编码器":
                            command += "-c:a ac3 ";
                            break;

                        default:
                            command += $"-c:a {comboBox_c_v.Text} ";
                            break;
                    }
                }

                if (checkBox_b_a.Checked) command += $"-b:a {comboBox_b_a.Text} ";
                if (checkBox_ac.Checked) command += $"-ac {comboBox_ac.Text} ";
                if (checkBox_ar.Checked) command += $"-ar {comboBox_ar.Text} ";
                if (checkBox_af_volume.Checked) command += $"-af \"volume={comboBox_af_volume.Text}\" ";
                if (checkBox_q_a.Checked) command += $"-q:a {trackBar_q_a.Value} ";

                #endregion
            }


            // 流作用描述

            // 艺术图
            if (checkBox_disposition_additionalImages.Checked) 
                command += $"-disposition:v:{GetFilAvailableOptionsIndex(comboBox_disposition_additionalImages.Text)} attached_pic ";

            // 输出
            if (!string.IsNullOrWhiteSpace(outputFile))
            {
                command += $"\"{outputFile.Trim()}\"";
            }
            else
            {
                MessageBox.Show("Output 输出目标是必须的！", "FFmpeg 命令构建", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

            switch (comboBox_y_or_n_or_null.SelectedIndex)
            {
                case 0:
                    command += " -y";
                    break;

                case 1:
                    command += " -n";
                    break;
            }

            return command.Trim();
        }

        private string BuildFFmpegInputFileCommand()
        {
            string command = string.Empty;
            foreach (DataRow row in dt_input.Rows)
            {
                // 跳过被标记为删除的行
                if (row.RowState != DataRowState.Deleted)
                {
                    string fullPath = row["FullPath"].ToString();
                    if (!string.IsNullOrEmpty(fullPath))
                    {
                        command += $"-i \"{fullPath.Trim()}\" ";
                        Log.Info($"[FFmpeg工具] 导入文件：{fullPath.Trim()}");
                    }

                }
            }

            for (int i = 0; i < dt_input.Rows.Count; i++)
                command += $"-map {i} ";

            return command;
        }

        private string GetFilAvailableOptionsIndex(string input)
        {
            if (input.Trim() == "(无可用文件)")
                return string.Empty;

            Match match = Regex.Match(input.Trim(), @"^\d+");
            if (match.Success)
            {
                return match.Value;
            }
            return string.Empty;
        }
        private void ProcessingUserData()
        {

        }

        private bool Inspect()
        {
            return true;
        }

        private void ExecuteCommand(string command)
        {
            outputBox.Text = string.Empty;

            // 创建进程启动信息
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = FFmpegPath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = command
            };

            process = new Process
            {
                StartInfo = psi,
                EnableRaisingEvents = true
            };

            // 设置输出和错误流的异步读取
            process.OutputDataReceived += (sender, e) => AppendText(e.Data);
            process.ErrorDataReceived += (sender, e) => AppendText(e.Data);

            process.Start();

            // 开始异步读取输出
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }


        private void AppendText(string text)
        {
            if (!string.IsNullOrEmpty(text))

                // 跨线程更新UI控件
                if (outputBox.InvokeRequired)
                {
                    outputBox.Invoke(new Action<string>(AppendText), text);
                }
                else
                {
                    outputBox.AppendText(text + Environment.NewLine);
                    outputBox.ScrollToCaret(); // 自动滚动到底部
                }
        }

        private void Stop()
        {
            process.Close();
            richTextBox.Text += "\n已终止进程。";
        }

        private void SetAllCopyChecked(bool _checked = false)
        {
            checkBox_c_copy.Checked = _checked;
            checkBox_cv_copy.Checked = _checked;
            checkBox_ca_copy.Checked = _checked;
            checkBox_cs_copy.Checked = _checked;
            checkBox_cd_copy.Checked = _checked;
        }

        private void checkBox_c_v_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_c_v.Enabled = checkBox_c_v.Checked;
            SetAllCopyChecked();
        }

        private void checkBox_b_v_CheckedChanged(object sender, EventArgs e)
        {
            if (isSettingCrf) return;

            comboBox_b_v.Enabled = checkBox_b_v.Checked;
            checkBox_crf.Checked = !checkBox_b_v.Checked ? false : false;
            checkBox_crf.Enabled = !checkBox_b_v.Checked;
            SetAllCopyChecked();
        }

        private void checkBox_r_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_r.Enabled = checkBox_r.Checked;
            SetAllCopyChecked();
        }

        private void checkBox_s_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_s.Enabled = checkBox_s.Checked;
            SetAllCopyChecked();
        }

        private void checkBox_c_a_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_c_a.Enabled = checkBox_c_a.Checked;
            SetAllCopyChecked();
        }

        private void checkBox_b_a_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_b_a.Enabled = checkBox_b_a.Checked;
            SetAllCopyChecked();
        }

        private void checkBox_ac_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_ac.Enabled = checkBox_ac.Checked;
            SetAllCopyChecked();
        }

        private void checkBox_ar_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_ar.Enabled = checkBox_ar.Checked;
            SetAllCopyChecked();
        }

        private void checkBox_af_volume_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_af_volume.Enabled = checkBox_af_volume.Checked;
            SetAllCopyChecked();
        }

        private void checkBox_disposition_additionalImages_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_disposition_additionalImages.Enabled = checkBox_disposition_additionalImages.Checked;
        }

        private void button_Open_File_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                AddFilesToDataTable(openFileDialog.FileNames);
            }
        }

        private void button_Save_File_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                outputFile = saveFileDialog.FileName;
                UpdateMainOutputPath();
            }
        }

        private void FFmpegTool_DragDrop(object sender, DragEventArgs e)
        {
            // 恢复窗口外观
            this.BackColor = SystemColors.Control;

            // 获取拖入的文件路径数组
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // 处理文件
            AddFilesToDataTable(files);
        }

        private void FFmpegTool_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // 显示可拖放的光标
                e.Effect = DragDropEffects.Copy;

                // 可选：改变窗口外观提示用户
                this.BackColor = Color.LightBlue;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void AddFileToDataTable(string fullPath)
        {
            // 1. 检查是否已存在（根据完整路径判断）
            foreach (DataRow row in dt_input.Rows)
            {
                // 跳过已被标记删除的行
                if (row.RowState != DataRowState.Deleted)
                {
                    string existingPath = row["FullPath"].ToString();
                    if (existingPath == fullPath)
                    {
                        // 文件已存在，提示用户
                        string _fileName = Path.GetFileName(fullPath);
                        MessageBox.Show($"文件 \"{_fileName}\" 已经导入过了！",
                                         "重复导入", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // 退出方法，不添加
                    }
                }
            }

            // 2. 解析文件信息
            string fileNameWithExt = Path.GetFileName(fullPath);            // 带后缀文件名
            string fileName = Path.GetFileNameWithoutExtension(fullPath);   // 文件名
            string extension = Path.GetExtension(fullPath);                 // 后缀名
            string directory = Path.GetDirectoryName(fullPath);             // 所在路径

            // 3. 添加到 DataTable（显示列 + 隐藏列）
            dt_input.Rows.Add(fileName, extension, fileNameWithExt, fullPath, directory);
        }

        private void AddFilesToDataTable(string[] fullPaths)
        {
            foreach (string fullPath in fullPaths)
            {
                AddFileToDataTable(fullPath);
            }

            ReloadFilAvailableOptions();
        }

        private void ReloadFilAvailableOptions()
        {
            // 保存当前选中的值（如果有）
            string currentSelection = comboBox_disposition_additionalImages.SelectedItem?.ToString();

            // 清空现有的 Items
            comboBox_disposition_additionalImages.Items.Clear();

            // 检查是否有数据源
            if (dt_input == null || dt_input.Rows.Count == 0)
            {
                comboBox_disposition_additionalImages.Enabled = false;
                comboBox_disposition_additionalImages.Items.Add("(无可用文件)");
                comboBox_disposition_additionalImages.SelectedIndex = 0;
                return;
            }

            //comboBox_disposition_additionalImages.Enabled = true;

            // 遍历 DataTable，将 FileNameWithExt 填入 ComboBox
            int index = 0;
            foreach (DataRow row in dt_input.Rows)
            {
                // 跳过已删除的行
                if (row.RowState == DataRowState.Deleted) continue;

                string fileNameWithExt = row["FileNameWithExt"]?.ToString();

                if (!string.IsNullOrEmpty(fileNameWithExt))
                {
                    string displayText = $"{index}: {fileNameWithExt}";
                    comboBox_disposition_additionalImages.Items.Add(displayText);
                    index++;
                }
            }

            // 恢复之前的选中项（如果还存在）
            if (!string.IsNullOrEmpty(currentSelection) && comboBox_disposition_additionalImages.Items.Contains(currentSelection))
            {
                comboBox_disposition_additionalImages.SelectedItem = currentSelection;
            }
            else if (comboBox_disposition_additionalImages.Items.Count > 0)
            {
                // 否则默认选中第一个
                comboBox_disposition_additionalImages.SelectedIndex = 0;
            }
        }

        private void button_CopyCommand_Click(object sender, EventArgs e)
        {
            ProcessingUserData();

            string command = $"\"{FFmpegPath}\" {BuildFFmpegCommand()}";
            Clipboard.SetText(command);
            Log.Info($"[FFmpeg工具] 复制命令 {command}");
            MessageBox.Show($"已复制命令\n\n{command}");
        }

        private void radioButton_ssto_best_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ssto_best.Checked)
                checkBox_c_copy.Checked = true;
        }

        private void radioButton_ssto_precise_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ssto_precise.Checked)
                checkBox_c_copy.Checked = false;
        }

        private void radioButton_ssto_fast_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ssto_fast.Checked)
                checkBox_c_copy.Checked = false;
        }




        #region 输入输出 路径处理

        private void UpdateMainOutputPath()
        {
            if (isUpdating) return;
            isUpdating = true;

            string filePath = outputFile;
            comboBox_Output_FileName.Text = Path.GetFileNameWithoutExtension(filePath);
            comboBox_Output_FileType.Text = Path.GetExtension(filePath);
            comboBox_Output_FilePath.Text = Path.GetDirectoryName(filePath);

            isUpdating = false;
        }

        private void UpdateOutputPath()
        {
            if (isUpdating) return;
            isUpdating = true;

            string filePath = comboBox_Output_FilePath.Text;
            string fileName = comboBox_Output_FileName.Text;
            string fileType = comboBox_Output_FileType.Text;

            if (!string.IsNullOrEmpty(fileName))
            {
                outputFile = Path.Combine(filePath, fileName + fileType);
            }

            isUpdating = false;
        }

        private void comboBox_Output_FileName_TextChanged(object sender, EventArgs e)
        {
            UpdateOutputPath();
        }

        private void comboBox_Output_FileType_TextChanged(object sender, EventArgs e)
        {
            UpdateOutputPath();
        }

        private void comboBox_Output_FilePath_TextChanged(object sender, EventArgs e)
        {
            UpdateOutputPath();
        }

        #endregion

        private void checkBox_c_copy_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = !checkBox_c_copy.Checked;
            checkBox_cv_copy.Enabled = enabled;
            checkBox_ca_copy.Enabled = enabled;
            checkBox_cs_copy.Enabled = enabled;
            checkBox_cd_copy.Enabled = enabled;

            if (checkBox_c_copy.Checked)
            {
                checkBox_cv_copy.Checked = false;
                checkBox_ca_copy.Checked = false;
                checkBox_cs_copy.Checked = false;
                checkBox_cd_copy.Checked = false;
            }
        }

        #region CRF
        private bool isSettingCrf = false;

        private void checkBox_crf_CheckedChanged(object sender, EventArgs e)
        {
            if (isSettingCrf) return;

            isSettingCrf = true;

            // 启用/禁用控件
            trackBar_crf.Enabled = checkBox_crf.Checked;
            comboBox_crf.Enabled = checkBox_crf.Checked;

            // 当勾选 CRF 时，自动禁用固定比特率（互斥）
            checkBox_b_v.Checked = !checkBox_crf.Checked ? false : false;
            checkBox_b_v.Enabled = !checkBox_crf.Checked;

            isSettingCrf = false;
        }


        private void comboBox_crf_TextChanged(object sender, EventArgs e)
        {
            if (isSettingCrf) return;

            isSettingCrf = true;

            string selected = comboBox_crf.Text;
            int crfValue;

            // 转换预设文字为数值
            switch (selected)
            {
                case "极佳":
                    crfValue = 18;
                    break;

                case "优秀":
                    crfValue = 21;
                    break;

                case "良好":
                    crfValue = 23;
                    break;

                case "一般":
                    crfValue = 25;
                    break;

                case "较差":
                    crfValue = 28;
                    break;

                default:
                    // 尝试解析数值
                    if (!int.TryParse(selected, out crfValue))
                    {
                        crfValue = trackBar_crf.Minimum;
                    }
                    break;
            }

            // 限制在有效范围内
            crfValue = Math.Clamp(crfValue, trackBar_crf.Minimum, trackBar_crf.Maximum);

            // 更新显示和滑块
            comboBox_crf.Text = crfValue.ToString();
            trackBar_crf.Value = crfValue;

            isSettingCrf = false;
        }

        private void trackBar_crf_Scroll(object sender, EventArgs e)
        {
            if (isSettingCrf) return;

            isSettingCrf = true;

            int crfValue = trackBar_crf.Value;
            comboBox_crf.Text = crfValue.ToString();

            isSettingCrf = false;
        }

        #endregion

        #region Preset
        private bool isSettingPreset = false;

        private void checkBox_preset_CheckedChanged(object sender, EventArgs e)
        {
            // 启用/禁用控件
            trackBar_preset.Enabled = checkBox_preset.Checked;
            comboBox_preset.Enabled = checkBox_preset.Checked;
        }

        private void comboBox_preset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isSettingPreset) return;

            isSettingPreset = true;

            // 限制在有效范围内
            int presetValue = Math.Clamp(comboBox_preset.SelectedIndex, trackBar_preset.Minimum, trackBar_preset.Maximum);

            // 更新显示和滑块
            comboBox_preset.Text = presetValue.ToString();
            trackBar_preset.Value = presetValue;

            isSettingPreset = false;
        }

        private void trackBar_preset_Scroll(object sender, EventArgs e)
        {
            if (isSettingPreset) return;

            isSettingPreset = true;

            comboBox_preset.SelectedIndex = trackBar_preset.Value;

            isSettingPreset = false;

        }

        #endregion

        #region QA
        private bool isSettingQA = false;

        private void checkBox_q_a_CheckedChanged(object sender, EventArgs e)
        {
            if (isSettingQA) return;

            isSettingQA = true;

            // 启用/禁用控件
            trackBar_q_a.Enabled = checkBox_q_a.Checked;
            comboBox_q_a.Enabled = checkBox_q_a.Checked;

            isSettingQA = false;
        }

        private void comboBox_q_a_TextChanged(object sender, EventArgs e)
        {
            if (isSettingQA) return;

            isSettingQA = true;

            string selected = comboBox_q_a.Text;
            int qaValue;

            // 转换预设文字为数值
            switch (selected)
            {
                case "最佳":
                    qaValue = 0;
                    break;

                case "极佳":
                    qaValue = 1;
                    break;

                case "高等":
                    qaValue = 2;
                    break;

                case "中等":
                    qaValue = 3;
                    break;

                case "还行":
                    qaValue = 4;
                    break;

                case "一般":
                    qaValue = 5;
                    break;

                case "较差":
                    qaValue = 6;
                    break;

                case "很差":
                    qaValue = 7;
                    break;

                case "极差":
                    qaValue = 8;
                    break;

                case "最差":
                    qaValue = 9;
                    break;

                default:
                    // 尝试解析数值
                    if (!int.TryParse(selected, out qaValue))
                    {
                        qaValue = trackBar_q_a.Minimum;
                    }
                    break;
            }

            // 限制在有效范围内
            qaValue = Math.Clamp(qaValue, trackBar_q_a.Minimum, trackBar_q_a.Maximum);

            // 更新显示和滑块
            comboBox_q_a.Text = qaValue.ToString();
            trackBar_q_a.Value = qaValue;

            isSettingQA = false;
        }

        private void trackBar_q_a_Scroll(object sender, EventArgs e)
        {
            if (isSettingQA) return;

            isSettingQA = true;

            comboBox_q_a.Text = trackBar_q_a.Value.ToString();

            isSettingQA = false;
        }

        #endregion

        private void dataGridView_InputFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 判断点击的是否为删除按钮列
            if (e.ColumnIndex >= 0 && dataGridView_InputFiles.Columns[e.ColumnIndex].Name == "ColumnDeleteFile")
            {
                // 防呆 增强鲁棒性
                if (e.RowIndex < 0 || dt_input.Rows.Count <= 0 || e.RowIndex >= dt_input.Rows.Count)
                    return;

                // 删除数据源中的行
                dt_input.Rows[e.RowIndex].Delete();
                dt_input.AcceptChanges();  // 提交更改，界面会自动刷新
                ReloadFilAvailableOptions();
            }
        }

        private void button_OpenDownloadPath_Click(object sender, EventArgs e)
        {
            string path = comboBox_Output_FilePath.Text;
            Directory.CreateDirectory(path);
            ExplorerStart(path);
        }

        private void button_OpenAutoDownloadPath_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(actualDirectory, "yt-dlp", "Downloads");
            Directory.CreateDirectory(path);
            ExplorerStart(path);
        }



        //private void dataGridView_OutputFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    // 判断点击的是否为删除按钮列
        //    if (e.ColumnIndex >= 0 && dataGridView_InputFiles.Columns[e.ColumnIndex].Name == "ColumnDeleteFile")
        //    {
        //        if (e.RowIndex < 0) return; // 防止点到列头

        //        // 删除数据源中的行
        //        dt_output.Rows[e.RowIndex].Delete();
        //        dt_output.AcceptChanges();  // 提交更改，界面会自动刷新
        //    }
        //}
    }
}

