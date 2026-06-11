using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Qi;
using static Qi.QisToolkit3_Datas;

namespace QisToolkit3.Forms
{
    public partial class FFmpegTool : Form
    {
        private Process process;
        private RichTextBox outputBox;
        private bool IsSetCrf = false;
        private bool isUpdating = false;


        private string FFmpegPath = Path.Combine(actualDirectory, @"yt-dlp\ffmpeg.exe");

        public FFmpegTool()
        {
            InitializeComponent();
            outputBox = richTextBox;

            if (!File.Exists(@$"{actualDirectory}\yt-dlp\ffmpeg.exe"))
                MessageBox.Show("环境缺失！这可能会导致一些问题", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //comboBox_FFmpeg.Text = @$"{actualDirectory}\yt-dlp\ffmpeg.exe";
            comboBox_Input.Text = @$"{actualDirectory}\yt-dlp\input.mp3";
            comboBox_Output.Text = @$"{actualDirectory}\yt-dlp\output.mp3";
            comboBox_y_or_n_or_null.SelectedIndex = 0;

            // 初始化
            Qi.FormInitDo(this.Text);
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
            string input = comboBox_Input.Text;
            string output = comboBox_Output.Text;

            string command = string.Empty;

            // 验证输入
            if (string.IsNullOrWhiteSpace(input))
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
            if (AddMainInput)
            {
                command += $"-i \"{input}\" ";
            }



            // 截取片段
            if (EnableClipExtraction)
            {
                // 从指定时间点开始
                if (checkBox_ss.Checked)
                {
                    command += $"-ss {comboBox_ss.Text} ";

                    // 快速模式 / 最佳模式（先指定时间后输入主要文件）
                    if (radioButton_ssto_fast.Checked || radioButton_ssto_best.Checked)
                        command += $"-i \"{input}\" ";
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


            if (checkBox_vn.Checked) command += "-vn ";
            if (checkBox_an.Checked) command += "-an ";
            if (checkBox_sn.Checked) command += "-sn ";
            if (checkBox_crf.Checked) command += $"-crf {trackBar_crf.Value} ";

            if (checkBox_c_v.Checked)
            {
                command += comboBox_c_v.Text switch
                {
                    "H.264" => "-c:v libx264 ",
                    "H.265" => "-c:v libx265 ",
                    "复制" => "-c:v copy ",
                    _ => "-c:v {comboBox_c_v.Text} ",
                };
            }
            if (checkBox_b_v.Checked) command += $"-b:v {comboBox_b_v.Text} ";
            if (checkBox_r.Checked) command += $"-r {comboBox_r.Text} ";
            if (checkBox_s.Checked) command += $"-s {comboBox_s.Text} ";

            if (checkBox_c_a.Checked)
            {
                switch (comboBox_c_v.Text)
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
                        command += "-c:a opus ";
                        break;

                    case "AC3编码器":
                        command += "-c:a ac3 ";
                        break;

                    case "复制":
                        command += "-c:a copy ";
                        break;

                    default:
                        command += $"-c:a {comboBox_c_v.Text} ";
                        break;
                }
            }
            if (checkBox_b_a.Checked) command += $"-b:a {comboBox_b_a.Text} ";
            if (checkBox_ac.Checked) command += $"-ac {comboBox_ac.Text} ";


            // 流处理
            if (checkBox_c_copy.Checked) command += $"-c copy ";

            // 输出
            if (!string.IsNullOrWhiteSpace(output))
            {
                command += $"\"{output}\"";
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

        // 处理用户数据
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

        private void checkBox_crf_CheckedChanged(object sender, EventArgs e)
        {
            trackBar_crf.Enabled = checkBox_crf.Checked;
            comboBox_crf.Enabled = checkBox_crf.Checked;
        }

        private void trackBar_crf_Scroll(object sender, EventArgs e)
        {
            if (IsSetCrf) return;

            IsSetCrf = true;
            comboBox_crf.Text = trackBar_crf.Value.ToString();
            IsSetCrf = false;
        }

        private void comboBox_crf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsSetCrf) return;

            IsSetCrf = true;
            if (comboBox_crf.Text == "极佳") comboBox_crf.Text = "18";
            else if (comboBox_crf.Text == "优秀") comboBox_crf.Text = "21";
            else if (comboBox_crf.Text == "良好") comboBox_crf.Text = "23";
            else if (comboBox_crf.Text == "一般") comboBox_crf.Text = "25";
            else if (comboBox_crf.Text == "较差") comboBox_crf.Text = "28";

            int crf = StrToInt(comboBox_crf.Text);
            if (crf < trackBar_crf.Minimum || crf > trackBar_crf.Maximum)
            {
                crf = trackBar_crf.Minimum;
                comboBox_crf.Text = trackBar_crf.Minimum.ToString();
            }

            trackBar_crf.Value = crf;
            IsSetCrf = false;
        }

        private void checkBox_c_v_CheckedChanged(object sender, EventArgs e) =>
            comboBox_c_v.Enabled = checkBox_c_v.Checked;

        private void checkBox_b_v_CheckedChanged(object sender, EventArgs e) =>
            comboBox_b_v.Enabled = checkBox_b_v.Checked;

        private void checkBox_r_CheckedChanged(object sender, EventArgs e) =>
            comboBox_r.Enabled = checkBox_r.Checked;

        private void checkBox_s_CheckedChanged(object sender, EventArgs e) =>
            comboBox_s.Enabled = checkBox_s.Checked;

        private void checkBox_c_a_CheckedChanged(object sender, EventArgs e) =>
            comboBox_c_a.Enabled = checkBox_c_a.Checked;

        private void checkBox_b_a_CheckedChanged(object sender, EventArgs e) =>
            comboBox_b_a.Enabled = checkBox_b_a.Checked;

        private void checkBox_ac_CheckedChanged(object sender, EventArgs e) =>
            comboBox_ac.Enabled = checkBox_ac.Checked;

        private void FFmpegTool_Load(object sender, EventArgs e)
        {

        }

        private void button_Open_File_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                comboBox_Input.Text = openFileDialog.FileName;
            }
        }

        private void button_Save_File_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                comboBox_Output.Text = saveFileDialog.FileNames[0];
        }

        private void comboBox_FFmpeg_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void label3_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            new TextProcessorForm(comboBox_Input.Text).Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            new TextProcessorForm(comboBox_Output.Text).Show();
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

        private void UpdateMainInputPath()
        {
            if (isUpdating) return;
            isUpdating = true;

            string filePath = comboBox_Input.Text;
            comboBox_Input_FileName.Text = Path.GetFileNameWithoutExtension(filePath);
            comboBox_Input_FileType.Text = Path.GetExtension(filePath);
            comboBox_Input_FilePath.Text = Path.GetDirectoryName(filePath);

            isUpdating = false;
        }

        private void UpdateInputPath()
        {
            if (isUpdating) return;
            isUpdating = true;

            string filePath = comboBox_Input_FilePath.Text;
            string fileName = comboBox_Input_FileName.Text;
            string fileType = comboBox_Input_FileType.Text;

            if (!string.IsNullOrEmpty(fileName))
            {
                comboBox_Input.Text = Path.Combine(filePath, fileName + fileType);
            }

            isUpdating = false;
        }

        private void UpdateMainOutputPath()
        {
            if (isUpdating) return;
            isUpdating = true;

            string filePath = comboBox_Output.Text;
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
                comboBox_Output.Text = Path.Combine(filePath, fileName + fileType);
            }

            isUpdating = false;
        }

        private void comboBox_Input_TextChanged(object sender, EventArgs e)
        {
            UpdateMainInputPath();
        }

        private void comboBox_Input_FileName_TextChanged(object sender, EventArgs e)
        {
            UpdateInputPath();
        }

        private void comboBox_Input_FileType_TextChanged(object sender, EventArgs e)
        {
            UpdateInputPath();
        }

        private void comboBox_Input_FilePath_TextChanged(object sender, EventArgs e)
        {
            UpdateInputPath();
        }

        private void comboBox_Output_TextChanged(object sender, EventArgs e)
        {
            UpdateMainOutputPath();
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
    }
}

