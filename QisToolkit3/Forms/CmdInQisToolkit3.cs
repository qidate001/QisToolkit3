using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QisToolkit3.Forms
{
    public partial class CmdInQisToolkit3 : Form
    {
        private Process cmdProcess;
        private StreamWriter inputWriter; 
        private bool isProcessInitialized = false;
        private string nsudoPath = $@"{Environment.CurrentDirectory}\NSudoL.exe";

        public CmdInQisToolkit3()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            InitializeComponent();
            InitializeCmdProcess();

            // 设置输入框回车键触发发送
            inputBox.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendCommand();
                    e.SuppressKeyPress = true; // 防止"叮"声
                }
            };

            sendButton.Click += (sender, e) => SendCommand();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void InitializeCmdProcess()
        {
            // 确保关闭现有进程
            SafeKillProcess();

            try
            {
                // 使用完整的命令保持进程运行
                var startInfo = new ProcessStartInfo
                {
                    FileName = nsudoPath,
                    Arguments = $"-U:T -P:E cmd /K \"title NSudoL提权终端 & echo 已获得TrustedInstaller权限 & prompt $$G\"",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = false,  // 改为false以便调试
                    StandardOutputEncoding = Encoding.GetEncoding(936),
                    StandardErrorEncoding = Encoding.GetEncoding(936),
                    WindowStyle = ProcessWindowStyle.Minimized  // 最小化窗口
                };

                cmdProcess = new Process { StartInfo = startInfo };

                // 处理进程退出事件
                cmdProcess.EnableRaisingEvents = true;
                cmdProcess.Exited += (sender, e) =>
                {
                    isProcessInitialized = false;
                    this.Invoke((MethodInvoker)delegate {
                        AppendOutput($"CMD进程已退出，退出代码: {cmdProcess.ExitCode}", isError: true);
                    });
                };

                // 输出数据处理
                cmdProcess.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        this.Invoke((MethodInvoker)delegate { AppendOutput(e.Data); });
                };

                cmdProcess.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        this.Invoke((MethodInvoker)delegate { AppendOutput(e.Data, isError: true); });
                };

                cmdProcess.Start();

                // 开始异步读取
                cmdProcess.BeginOutputReadLine();
                cmdProcess.BeginErrorReadLine();

                inputWriter = cmdProcess.StandardInput;
                isProcessInitialized = true;

                // 发送初始化命令
                inputWriter.WriteLine("echo 终端初始化完成");
                inputWriter.WriteLine("chcp 65001 > nul"); // 设置为UTF-8编码
            }
            catch (Exception ex)
            {
                isProcessInitialized = false;
                this.Invoke((MethodInvoker)delegate {
                    MessageBox.Show($"启动失败: {ex.Message}\n\nStackTrace:\n{ex.StackTrace}",
                        "严重错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
        }

        private void SafeKillProcess()
        {
            try
            {
                if (cmdProcess != null)
                {
                    if (!cmdProcess.HasExited)
                    {
                        cmdProcess.Kill();
                    }
                    cmdProcess.Dispose();
                }
            }
            catch { /* 忽略清理时的异常 */ }
            finally
            {
                cmdProcess = null;
                isProcessInitialized = false;
            }
        }



        private void SendCommand()
        {
            if (!isProcessInitialized || cmdProcess == null || cmdProcess.HasExited)
            {
                AppendOutput("进程未就绪，正在尝试重新初始化...", isError: true);
                InitializeCmdProcess();
                return;
            }

            string command = inputBox.Text.Trim();
            if (!string.IsNullOrEmpty(command))
            {
                try
                {
                    AppendOutput($"> {command}");
                    inputWriter.WriteLine(command);
                    inputBox.Clear();

                    // 添加空命令确保缓冲区刷新
                    inputWriter.WriteLine();
                }
                catch (Exception ex)
                {
                    AppendOutput($"命令发送失败: {ex.Message}", isError: true);
                    InitializeCmdProcess(); // 尝试重新初始化
                }
            }
        }

        private void AppendOutput(string text, bool isError = false)
        {
            if (string.IsNullOrEmpty(text)) return;

            // 跨线程调用UI更新
            if (outputBox.InvokeRequired)
            {
                outputBox.Invoke(new Action(() => AppendOutput(text, isError)));
                return;
            }

            // 保存当前选择位置
            int start = outputBox.SelectionStart;
            int length = outputBox.SelectionLength;

            // 移动到最后
            outputBox.SelectionStart = outputBox.TextLength;

            // 设置颜色
            outputBox.SelectionColor = isError ? Color.Red : Color.Black;

            // 添加文本
            outputBox.AppendText(text + Environment.NewLine);

            // 恢复选择位置
            outputBox.SelectionStart = start;
            outputBox.SelectionLength = length;

            // 自动滚动
            outputBox.ScrollToCaret();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // 关闭进程
            if (cmdProcess != null && !cmdProcess.HasExited)
            {
                try
                {
                    inputWriter?.WriteLine("exit"); // 发送退出命令
                    inputWriter?.Close();
                    cmdProcess.WaitForExit(1000); // 等待1秒

                    if (!cmdProcess.HasExited)
                        cmdProcess.Kill(); // 强制终止
                }
                catch { /* 忽略关闭时的异常 */ }
            }
        }

        private void CmdInQisToolkit3_Load(object sender, EventArgs e)
        {
            // 测试命令
            if (inputWriter != null && !cmdProcess.HasExited)
            {
                inputWriter.WriteLine("echo 测试命令执行成功");
                inputWriter.WriteLine("whoami");
            }
        }
    }
}
