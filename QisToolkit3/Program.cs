using QisToolkit3.Console;
using QisToolkit3.Forms;
using QisToolkit3.Forms.SoftwareFunctionForms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Xml.Linq;
using static BufferedLogger;

namespace QisToolkit3
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCP(uint wCodePageID);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleOutputCP(uint wCodePageID);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_MAXIMIZE = 3;
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_RESTORE = 9;
        private const int SW_HIDE = 0;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // 检查 -H 或 -Hide 参数
            bool hideWindow = args.Contains("-H", StringComparer.OrdinalIgnoreCase) ||
                              args.Contains("-Hide", StringComparer.OrdinalIgnoreCase);

            if (!IsRunAsAdmin())
            {
                Log.Warn("此软件需要管理员权限才可运行。");
                var tmp = MessageBox.Show("此软件需要管理员权限才可运行。\n是否尝试提权？", "需要管理员权限",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (tmp == DialogResult.Yes)
                {
                    RestartAsAdmin();
                }
                return;
            }
            


            Log.Info("=== 启动信息 ===");
            Log.Info($"EXE文件: {Qi.QisToolkit3_Datas.exePath}");
            Log.Info($"文件夹路径: {Qi.QisToolkit3_Datas.actualDirectory}");
            Log.Info($"进程PID: {Qi.QisToolkit3_Datas.processId} 用户名: {Qi.QisToolkit3_Datas.owner}");
            Log.Info($"系统级权限判断: {Qi.QisToolkit3_Datas.isSystem}");
            Log.Info($"隐藏模式: {hideWindow}");
            
            // 加载
            Qi.QisToolkit3_Datas.LoadDatas();

            Log.Info("=== 加载数据 ===");
            Log.Info($"使用MinSudo: {Qi.QisToolkit3_Datas.IsRunMinSudo}");

            bool UseSPLArg = false;
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i].Equals("-SPL", StringComparison.OrdinalIgnoreCase))
                {
                    string program = args[i + 1];
                    Qi.RunNSudo(program);
                    UseSPLArg = true;
                    i++; // 跳过已处理的程序名
                }
            }

            if (UseSPLArg) return;

            // 注册编码提供程序
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();

            // 检查是否是 -C 系列参数（命令执行模式）
            int cmdIndex = Array.FindIndex(
                args,
                x => IsCommandOnlyMode(x)
            );

            if (cmdIndex >= 0)
            {
                RunCommandOnlyMode(args, hideWindow);
                return;
            }

            // 解析启动参数
            Form startupForm = ParseStartupArgs(args);

            const bool DeBug_Command = false;

            if (startupForm != null && !DeBug_Command)
            {
                if (hideWindow)
                {
                    // 隐藏窗口模式：不显示主窗体
                    // 静默运行，不调用 Application.Run
                    // 可以选择在这里执行其他后台任务，或者直接退出
                    // 由于没有窗口需要显示，直接返回
                    BufferedLogger.Shutdown();
                    return;
                }
                Application.Run(startupForm);
            }

            // 命令行模式
            else
            {
                RunCommandLineMode(hideWindow);
            }
        }

        public static void RestartAsAdmin(string arguments = "")
        {
            var process = new ProcessStartInfo
            {
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                Arguments = arguments,
                UseShellExecute = true,
                Verb = "runas"  // 触发 UAC 提权
            };
            Process.Start(process);
            Environment.Exit(0);
        }

        /// <summary>
        /// 判断是否是命令执行参数（-C 系列）
        /// </summary>
        static bool IsCommandOnlyMode(string arg)
        {
            string[] commandModes = { "-AC", "-ArgC", "-ArgCmd", "-ArgCommand" };
            return commandModes.Contains(arg, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 命令执行模式（执行后自动退出）
        /// </summary>
        static void RunCommandOnlyMode(string[] args, bool hideWindow)
        {
            // 获取参数（跳过第一个命令模式参数）
            int cmdIndex = Array.FindIndex(
                args,
                x => IsCommandOnlyMode(x)
            );

            string commandArg = "";

            if (cmdIndex >= 0 && cmdIndex + 1 < args.Length)
            {
                commandArg = args[cmdIndex + 1];
            }

            if (string.IsNullOrEmpty(commandArg))
            {
                System.Console.WriteLine("错误: 请提供要执行的命令");
                System.Console.WriteLine("用法: QisToolkit3.exe -AC \"命令1 | 命令2 | 命令3\"");
                return;
            }

            // 分配控制台（用于显示输出）
            AllocConsole();

            // 如果需要隐藏窗口，隐藏控制台
            if (hideWindow)
            {
                IntPtr consoleWindow = GetConsoleWindow();
                if (consoleWindow != IntPtr.Zero)
                {
                    ShowWindow(consoleWindow, SW_HIDE);
                }
            }

            try
            {
                // 设置控制台编码
                System.Console.OutputEncoding = Encoding.GetEncoding("GBK");
                System.Console.InputEncoding = Encoding.GetEncoding("GBK");
                System.Console.Title = "QisToolkit3 - 命令执行模式";

                // 如果不隐藏窗口，最大化显示
                if (!hideWindow)
                {
                    IntPtr consoleWindow = GetConsoleWindow();
                    if (consoleWindow != IntPtr.Zero)
                    {
                        ShowWindow(consoleWindow, SW_MAXIMIZE);
                    }
                }

                // 解析多命令（用 | 分割，支持引号内的 | 不被分割）
                string[] commands = SplitCommands(commandArg);

                // 创建命令处理器
                var handler = new CommandHandler();

                System.Console.WriteLine("==========================================");
                System.Console.WriteLine("    Qis Toolkit 3 - 命令执行模式");
                System.Console.WriteLine("==========================================");
                System.Console.WriteLine();

                // 依次执行每条命令
                int successCount = 0;
                for (int i = 0; i < commands.Length; i++)
                {
                    string cmd = commands[i].Trim();
                    if (string.IsNullOrEmpty(cmd)) continue;

                    string text = $"执行 {i + 1}/{commands.Length} {cmd}";
                    Log.Info($"[命令执行模式] [执行命令] {text}");
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    System.Console.WriteLine(text);
                    System.Console.ResetColor();

                    var result = handler.ExecuteCommand(cmd);

                    if (result.Success)
                    {
                        successCount++;
                    }
                    else
                    {
                        Log.Err($"命令执行失败: {result.Response}");
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine($"命令执行失败: {result.Response}");
                        System.Console.ResetColor();
                    }

                    // 输出命令的响应
                    if (!string.IsNullOrEmpty(result.Response))
                    {
                        Log.Info($"[命令执行模式] {result.Response}");
                        System.Console.WriteLine(result.Response);
                    }
                    System.Console.WriteLine();
                }

                System.Console.WriteLine($"==========================================");
                System.Console.WriteLine($"执行完成: 成功 {successCount}/{commands.Length} 条命令");
                System.Console.WriteLine("==========================================");

                // 自动退出，不等待按键
                BufferedLogger.Shutdown();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"执行出错: {ex.Message}");
                Log.Err($"执行出错: {ex.Message}");
            }
            finally
            {
                FreeConsole();
            }
        }

        /// <summary>
        /// 分割命令（支持引号内的 | 不被分割）
        /// </summary>
        static string[] SplitCommands(string input)
        {
            var commands = new List<string>();
            var currentCmd = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                // 处理引号
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    currentCmd.Append(c);
                    continue;
                }

                // 处理 | 分隔符（只在引号外）
                if (c == '|' && !inQuotes)
                {
                    if (currentCmd.Length > 0)
                    {
                        commands.Add(currentCmd.ToString());
                        currentCmd.Clear();
                    }
                    continue;
                }

                currentCmd.Append(c);
            }

            if (currentCmd.Length > 0)
            {
                commands.Add(currentCmd.ToString());
            }

            return commands.ToArray();
        }

        private static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static Form ParseStartupArgs(string[] args)
        {
            // 如果没有参数，默认启动 Main 窗体
            if (args.Length == 0)
            {
                Log.Info($"无启动参数。");
                return new Main();
            }

            else
            {
                Log.Info($"启动参数：\n{string.Join("\n", args.Select(arg => $"[ARGS] {arg}"))}");
            }

            // 解析参数（跳过 -H 参数）
            for (int i = 0; i < args.Length; i++)
            {
                // 跳过 -H 和 -Hide
                if (args[i].Equals("-H", StringComparison.OrdinalIgnoreCase) ||
                    args[i].Equals("-Hide", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                switch (args[i])
                {
                    case "-o":
                        if (i + 1 < args.Length)
                        {
                            string formName = args[i + 1];
                            return CreateFormByName(formName);
                        }

                        return new Main();

                    case "-cmd":
                    case "-command":
                    case "-c":
                        return null;
                }
            }

            // 如果没有找到 -o 参数，默认启动 Main 窗体
            return new Main();
        }

        public static Form CreateFormByName(string formName)
        {
            switch (formName)
            {
                case "Main":
                    return new Main();
                case "DCQAA":
                    return new DCQAA();
                case "AdvancedModificationSystemTools":
                    return new AdvancedModificationSystemTools();
                case "Calculator":
                    return new Calculator();
                case "CleaningUpTrash":
                    return new CleaningUpTrash();
                case "CmdInQisToolkit3":
                    return new CmdInQisToolkit3();
                case "ExtendedFeatures":
                    return new ExtendedFeatures();
                case "FFmpegTool":
                    return new FFmpegTool();
                case "FileList":
                    return new FileList();
                case "FilesOperation":
                    return new FilesOperation();
                case "FilesOperationPlus":
                    return new FilesOperationPlus();
                case "GameTools":
                    return new GameTools();
                case "ImageHijackingTool":
                    return new ImageHijackingTool();
                case "MediumAutoStartTool":
                    return new MediumAutoStartTool();
                case "MinecraftProjectE":
                    return new MinecraftProjectE();
                case "MinecraftTools":
                    return new MinecraftTools();
                case "MyComputerNameSpaceTool":
                    return new MyComputerNameSpaceTool();
                case "Options":
                    return new Options();
                case "ScanRogueSoftwareTool":
                    return new ScanRogueSoftwareTool();
                case "SendMessageToWindowsTool":
                    return new SendMessageToWindowsTool();
                case "SoftwareDownload":
                    return new SoftwareDownload();
                case "SoftwareFunctionPage":
                    return new SoftwareFunctionPage();
                case "SoftwareLogParser":
                    return new SoftwareLogParser();
                case "StrangeFoods":
                    return new StrangeFoods();
                case "StrangeQuestionAndAnswer":
                    return new StrangeQuestionAndAnswer();
                case "StrangeQuestionAndAnswerMain":
                    return new StrangeQuestionAndAnswerMain();
                case "SurvivalChallengeGame":
                    return new SurvivalChallengeGame();
                case "SystemErrorCheck":
                    return new SystemErrorCheck();
                case "SystemServiceTools":
                    return new SystemServiceTools();
                case "TextGeneration":
                    return new TextGeneration();
                case "TextGenerationPlus":
                    return new TextGenerationPlus();
                case "TextProcessingTools":
                    return new TextProcessingTools();
                case "TextProcessorForm":
                    return new TextProcessorForm();
                case "Tools":
                    return new Tools();
                case "UnicodeTool":
                    return new UnicodeTool();
                case "UninstallRegistryKeysTool":
                    return new UninstallRegistryKeysTool();
                case "WhatToEatToday":
                    return new WhatToEatToday();
                case "WindowNesterTool":
                    return new WindowNesterTool();
                case "YtDlpTool":
                    return new YtDlpTool();
                case "PCLFunction":
                    return new PCLFunction();
                case "QQFunction":
                    return new QQFunction();
                case "WeChatFunction":
                    return new WeChatFunction();
                case "CommonFunctionalTools":
                    return new CommonFunctionalTools();
                case "TaskManager":
                    return new TaskManager();
                case "SystemPermissionLauncher":
                case "spl":
                    return new SystemPermissionLauncher();
                default:
                    // 如果找不到对应的窗体，显示错误信息并返回主窗体
                    MessageBox.Show($"未找到名为 {formName} 的窗体，将启动主窗体。",
                                    "参数错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return new Main();
            }
        }

        private static bool IsCommandLineMode(string[] args)
        {
            return args != null && args.Length >= 1 &&
                   args[0].Equals("-command", StringComparison.OrdinalIgnoreCase);
        }

        static void RunCommandLineMode(bool hideWindow = false)
        {
            AllocConsole();

            // 如果需要隐藏窗口，隐藏控制台
            if (hideWindow)
            {
                IntPtr consoleWindow = GetConsoleWindow();
                if (consoleWindow != IntPtr.Zero)
                {
                    ShowWindow(consoleWindow, SW_HIDE);
                }
            }

            try
            {
                System.Console.Title = "QisToolkit3 - 命令行模式";
                System.Console.BufferHeight = 3000;

                var handler = new CommandHandler();
                handler.Run();
            }
            finally
            {
                FreeConsole();
            }
        }
    }
}