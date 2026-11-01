using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace QisToolkit3
{
    public static class SingleInstanceManager
    {
        private static readonly string MutexName = "Global\\QisToolkit3_SingleInstance_Mutex";
        private static readonly string PipeName = "QisToolkit3_SingleInstance_Pipe";
        private static Mutex _mutex;

        // 收到其他实例发来的参数时触发
        public static event Action<string[]> OnArgumentsReceived;

        public static bool IsFirstInstance(string[] args)
        {
            _mutex = new Mutex(true, MutexName, out bool createdNew);
            if (createdNew)
            {
                StartPipeServer();
                return true;
            }

            // 不是第一个实例，把参数发给老实例
            SendArgumentsToFirstInstance(args);
            return false;
        }

        private static void SendArgumentsToFirstInstance(string[] args)
        {
            try
            {
                using var pipeClient = new NamedPipeClientStream(".", PipeName, PipeDirection.Out);
                pipeClient.Connect(1000);
                using var writer = new StreamWriter(pipeClient);
                writer.WriteLine(string.Join("\n", args));
                writer.Flush();
            }
            catch { /* 忽略连接失败 */ }
        }

        private static void StartPipeServer()
        {
            var server = new NamedPipeServerStream(PipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
            server.WaitForConnectionAsync().ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    using var reader = new StreamReader(server);
                    string content = reader.ReadToEnd();
                    string[] args = content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    OnArgumentsReceived?.Invoke(args);
                }
                StartPipeServer(); // 循环监听下一次
            });
        }
    }
}