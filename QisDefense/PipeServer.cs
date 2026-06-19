using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace QisDefense
{
    public class PipeServer : IDisposable
    {
        private NamedPipeServerStream _pipeServer;
        private Thread _listenerThread;
        private bool _isRunning;
        private readonly string _pipeName;

        public event EventHandler<PipeCommandEventArgs> CommandReceived;

        public PipeServer(string pipeName = "QisDefensePipe")
        {
            _pipeName = pipeName;
            _isRunning = true;
        }

        public void Start()
        {
            _listenerThread = new Thread(ListenForClients)
            {
                IsBackground = true,
                Name = "PipeListener"
            };
            _listenerThread.Start();
        }

        private void ListenForClients()
        {
            while (_isRunning)
            {
                try
                {
                    // 创建管道服务器（允许一个客户端连接）
                    using (var server = new NamedPipeServerStream(
                        _pipeName,
                        PipeDirection.InOut,
                        1, // 最多1个客户端同时连接
                        PipeTransmissionMode.Message,
                        PipeOptions.Asynchronous))
                    {
                        // 等待客户端连接（阻塞）
                        server.WaitForConnection();

                        // 处理客户端请求
                        HandleClient(server);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"管道服务异常: {ex.Message}");
                    Thread.Sleep(1000);
                }
            }
        }

        private void HandleClient(NamedPipeServerStream server)
        {
            try
            {
                // 读取客户端数据
                byte[] buffer = new byte[4096];
                int bytesRead = server.Read(buffer, 0, buffer.Length);
                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Debug.WriteLine($"收到命令: {request}");

                // 解析并执行命令
                string response = ProcessCommand(request);

                // 发送响应
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                server.Write(responseBytes, 0, responseBytes.Length);
                server.Flush();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"处理客户端请求失败: {ex.Message}");
            }
        }

        private string ProcessCommand(string request)
        {
            try
            {
                string[] parts = request.Split('|');
                string command = parts[0].ToUpper();

                switch (command)
                {
                    case "LOCK":
                        if (parts.Length >= 3)
                        {
                            string filePath = parts[1];
                            int mode = int.Parse(parts[2]);
                            bool result = Program.LockFile(filePath, mode);
                            return result ? $"OK|文件已锁定 (模式:{mode})" : "ERROR|锁定失败";
                        }
                        return "ERROR|参数不足: LOCK|文件路径|模式(0/1/2)";

                    case "UNLOCK":
                        if (parts.Length >= 2)
                        {
                            string filePath = parts[1];
                            bool result = Program.UnlockFile(filePath);
                            return result ? "OK|文件已解锁" : "ERROR|解锁失败";
                        }
                        return "ERROR|参数不足: UNLOCK|文件路径";

                    case "STATUS":
                        if (parts.Length >= 2)
                        {
                            string filePath = parts[1];
                            bool isLocked = Program.IsFileLocked(filePath);
                            return $"LOCKED|{isLocked}";
                        }
                        return "ERROR|参数不足: STATUS|文件路径";

                    case "EXIT":
                        _isRunning = false;
                        return "OK|服务端即将退出";

                    default:
                        return $"ERROR|未知命令: {command}";
                }
            }
            catch (Exception ex)
            {
                return $"ERROR|{ex.Message}";
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _listenerThread?.Join(3000);
        }

        public void Dispose()
        {
            Stop();
            _pipeServer?.Dispose();
        }
    }

    public class PipeCommandEventArgs : EventArgs
    {
        public string Command { get; set; }      // 原始命令字符串
        public string[] Parameters { get; set; } // 解析后的参数
        public string ClientId { get; set; }     // 客户端标识（可选）

        public PipeCommandEventArgs(string command)
        {
            Command = command;
            Parameters = command.Split('|');
        }
    }
}
