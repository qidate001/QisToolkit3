using EasyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QisDefense
{
    public class ReadBookInjector
    {
        private const string TARGET_PROCESS = "ReadBook";  // 目标进程名（不含 .exe）
        private string _dllPath;  // ReadBookProxy.dll 的完整路径

        public ReadBookInjector()
        {
            // 获取 DLL 路径（假设放在注入器同目录下）
            _dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReadBookProxy.dll");

            MessageBox.Show(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReadBookProxy.dll"));
            if (!File.Exists(_dllPath))
            {
                throw new FileNotFoundException($"找不到 ReadBookProxy.dll，路径: {_dllPath}");
            }
        }

        /// <summary>
        /// 注入到指定进程 ID
        /// </summary>
        public void Inject(int processId)
        {
            try
            {
                // 检查目标进程是否存在
                Process process = Process.GetProcessById(processId);
                if (process == null || process.HasExited)
                {
                    throw new Exception($"目标进程不存在或已退出 (PID: {processId})");
                }

                MessageBox.Show($"正在注入到进程: {process.ProcessName} (PID: {processId})");

                // EasyHook 注入（RemoteHooking.Inject 是同步方法，会阻塞直到注入完成）
                //RemoteHooking.Inject(
                //    processId,                    // 目标进程 ID
                //    _dllPath,                     // 要注入的托管 DLL 路径
                //    _dllPath,                     // 参数（注入后传递给 DLL 的配置文件，这里传相同路径即可）
                //                                  // 注意：这里的参数会传递给 DLL 中 IEntryPoint 的构造函数
                //                                  // 如果需要传递额外参数，可以在这里添加，例如：
                //                                  // "UTF-8", "GBK"              // 可以传递多个参数
                //    IntPtr.Zero                   // 如果不需要额外参数，传 IntPtr.Zero
                //);

                RemoteHooking.Inject(
                    processId,                    // 目标进程 ID
                    _dllPath,                     // 要注入的托管 DLL 路径
                    _dllPath,                     // 参数（传递给 DLL 构造函数的第一个参数）
                    "UTF-8 to GBK Hook"          // 第二个参数（传递给 Run 方法的 channelName）
                );

                MessageBox.Show($"✅ 注入成功！PID: {processId}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ 注入失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 等待 ReadBook.exe 启动并自动注入（监控模式）
        /// </summary>
        public void StartMonitoring(int checkInterval = 2000)
        {
            Console.WriteLine("开始监控 ReadBook.exe 启动...");

            while (true)
            {
                try
                {
                    // 查找 ReadBook.exe 进程
                    Process[] processes = Process.GetProcessesByName(TARGET_PROCESS);

                    foreach (Process proc in processes)
                    {
                        try
                        {
                            // 检查是否已经注入（通过检查进程模块中是否包含我们的 DLL）
                            if (!IsInjected(proc))
                            {
                                MessageBox.Show($"检测到 {TARGET_PROCESS}.exe 启动 (PID: {proc.Id})");
                                Inject(proc.Id);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"处理进程 PID {proc.Id} 时出错: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"监控异常: {ex.Message}");
                }

                Thread.Sleep(checkInterval);
            }
        }

        /// <summary>
        /// 检查目标进程是否已被注入（通过检查模块列表）
        /// </summary>
        private bool IsInjected(Process process)
        {
            try
            {
                foreach (ProcessModule module in process.Modules)
                {
                    if (module.FileName.EndsWith("ReadBookProxy.dll", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // 访问进程模块可能需要权限，忽略异常
            }
            return false;
        }

        /// <summary>
        /// 扫描所有 ReadBook 进程并注入（适用于程序启动时的批量注入）
        /// </summary>
        public void InjectAllExisting()
        {
            Process[] processes = Process.GetProcessesByName(TARGET_PROCESS);

            if (processes.Length == 0)
            {
                Console.WriteLine($"当前没有运行中的 {TARGET_PROCESS}.exe");
                return;
            }

            Console.WriteLine($"发现 {processes.Length} 个 {TARGET_PROCESS}.exe 进程");

            foreach (Process proc in processes)
            {
                try
                {
                    if (!IsInjected(proc))
                    {
                        Inject(proc.Id);
                    }
                    else
                    {
                        Console.WriteLine($"进程 PID {proc.Id} 已注入，跳过");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"注入进程 PID {proc.Id} 失败: {ex.Message}");
                }
            }
        }
    }
}
