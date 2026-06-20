using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Qi;

namespace QisToolkit3
{
    public class PipeClient
    {
        private static readonly string _pipeName = "QisDefensePipe";
        private static readonly int _timeoutMs = 5000;
        
        public static string SendCommand(string command, int timeoutMs = 5000)
        {
            try
            {
                using (var client = new NamedPipeClientStream(".", _pipeName, PipeDirection.InOut))
                {
                    client.Connect(timeoutMs);

                    byte[] commandBytes = Encoding.UTF8.GetBytes(command);
                    client.Write(commandBytes, 0, commandBytes.Length);
                    client.Flush();

                    byte[] buffer = new byte[4096];
                    int bytesRead = client.Read(buffer, 0, buffer.Length);
                    return Encoding.UTF8.GetString(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                return $"ERROR|{ex.Message}";
            }
        }

        /// <summary>
        /// 检查齐之防御服务是否运行（通过命名管道探测）
        /// </summary>
        public static bool IsServiceRunning()
        {
            try
            {
                string response = SendCommand("PING", 1000);
                return response.StartsWith("PONG|");
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检查齐之防御服务是否运行并提示启动
        /// </summary>
        public static bool CheckServiceRunning()
        {
            if (!IsServiceRunning())
            {
                DialogResult result = MessageBox.Show(
                    "齐之防御未运行，无法执行操作。\n是否启动服务？",
                    "服务未运行",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Process.Start(Path.Combine(QisToolkit3_Datas.actualDirectory, "QisDefense.exe"));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static bool LockFile(string filePath, int mode)
        {
            string response = SendCommand($"LOCK|{filePath}|{mode}");
            return response.StartsWith("OK|");
        }

        public static bool UnlockFile(string filePath)
        {
            string response = SendCommand($"UNLOCK|{filePath}");
            return response.StartsWith("OK|");
        }

        public static bool CheckStatus(string filePath)
        {
            string response = SendCommand($"STATUS|{filePath}");
            return response.StartsWith("LOCKED|true");
        }

        public static bool AddCriticalProcess(int processId)
        {
            string response = SendCommand($"ADD_CRITICAL|{processId}");
            return response.StartsWith("OK|");
        }

        public static bool RemoveCriticalProcess(int processId)
        {
            string response = SendCommand($"REMOVE_CRITICAL|{processId}");
            return response.StartsWith("OK|");
        }

        public static bool IsCriticalProcess(int processId)
        {
            string response = SendCommand($"CHECK_CRITICAL|{processId}");
            return response.StartsWith("OK|true");
        }

        public static bool KillProcess(int processId)
        {
            string response = SendCommand($"KILL_PROCESS|{processId}");
            return response.StartsWith("OK|true");
        }

        public static bool JobKillProcess(int processId)
        {
            string response = SendCommand($"JOB_KILL_PROCESS|{processId}");
            return response.StartsWith("OK|true");
        }
    }
}
