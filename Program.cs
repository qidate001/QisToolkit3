using QisToolkit3.Forms;
using QisToolkit3.Forms.SoftwareFunctionForms;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using QisToolkit3.Console;
using static BufferedLogger;
using System.Linq;
using System.Collections.Generic;

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

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!IsRunAsAdmin())
            {
                Log.Warn("此软件需要管理员权限才可运行。");
                var tmp = MessageBox.Show("此软件需要管理员权限才可运行。\n是否尝试提权？", "需要管理员权限",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (tmp == DialogResult.Yes)
                    Qi.RunNSudo(Path.Combine(Qi.QisToolkit3_Datas.actualDirectory, "QisToolkit3.exe"));

                else
                    return; // 退出程序
            }


            Log.Info("=== 启动信息 ===");
            Log.Info($"EXE文件: {Qi.QisToolkit3_Datas.exePath}");
            Log.Info($"文件夹路径: {Qi.QisToolkit3_Datas.actualDirectory}");
            Log.Info($"进程PID: {Qi.QisToolkit3_Datas.processId} 用户名: {Qi.QisToolkit3_Datas.owner}");
            Log.Info($"系统级权限判断: {Qi.QisToolkit3_Datas.isSystem}");

            // 加载
            Qi.QisToolkit3_Datas.LoadDatas();

            Log.Info($"使用MinSudo: {Qi.QisToolkit3_Datas.IsRunMinSudo}");

            // 注册编码提供程序
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();

            // 检查是否是 -C 系列参数（命令执行模式）
            if (args.Length > 0 && IsCommandOnlyMode(args[0]))
            {
                // 命令行执行模式（执行后自动退出）
                RunCommandOnlyMode(args);
                return;
            }

            // 解析启动参数
            Form startupForm = ParseStartupArgs(args);

            const bool DeBug_Command = false;

            if (startupForm != null && !DeBug_Command)
            {
                Application.Run(startupForm);
            }

            // 命令行模式
            else
            {
                RunCommandLineMode();
            }
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
        static void RunCommandOnlyMode(string[] args)
        {
            // 获取参数（跳过第一个命令模式参数）
            string commandArg = args.Length > 1 ? args[1] : "";

            if (string.IsNullOrEmpty(commandArg))
            {
                System.Console.WriteLine("错误: 请提供要执行的命令");
                System.Console.WriteLine("用法: QisToolkit3.exe -C \"命令1 | 命令2 | 命令3\"");
                return;
            }

            // 分配控制台（用于显示输出）
            AllocConsole();

            try
            {
                // 设置控制台编码
                System.Console.OutputEncoding = Encoding.GetEncoding("GBK");
                System.Console.InputEncoding = Encoding.GetEncoding("GBK");
                System.Console.Title = "QisToolkit3 - 命令执行模式";

                // 最大化窗口
                IntPtr consoleWindow = GetConsoleWindow();
                if (consoleWindow != IntPtr.Zero)
                {
                    ShowWindow(consoleWindow, SW_MAXIMIZE);
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

                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    System.Console.WriteLine($"[执行 {i + 1}/{commands.Length}] {cmd}");
                    System.Console.ResetColor();

                    var result = handler.ExecuteCommand(cmd);

                    if (result.Success)
                    {
                        successCount++;
                    }
                    else
                    {
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine($"命令执行失败: {result.Response}");
                        System.Console.ResetColor();
                    }

                    // 输出命令的响应
                    if (!string.IsNullOrEmpty(result.Response))
                    {
                        System.Console.WriteLine(result.Response);
                    }
                    System.Console.WriteLine();
                }

                System.Console.WriteLine($"==========================================");
                System.Console.WriteLine($"执行完成: 成功 {successCount}/{commands.Length} 条命令");
                System.Console.WriteLine("==========================================");

                // 自动退出，不等待按键
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"执行出错: {ex.Message}");
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

            // 解析参数
            for (int i = 0; i < args.Length; i++)
            {
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

        static void RunCommandLineMode()
        {
            AllocConsole();

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