using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
