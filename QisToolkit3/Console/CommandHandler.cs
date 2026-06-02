using QisToolkit3.Console.Commands;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace QisToolkit3.Console
{
    public class CommandHandler
    {
        private bool _running = true;
        private CommandParser _parser;
        private List<string> _lastReturnData = new List<string>();

        // Windows API 用于控制控制台滚动
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleCursorPosition(IntPtr hConsoleOutput, COORD dwCursorPosition);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleWindowInfo(IntPtr hConsoleOutput, bool bAbsolute, ref SMALL_RECT lpConsoleWindow);

        private const int STD_OUTPUT_HANDLE = -11;

        [StructLayout(LayoutKind.Sequential)]
        struct COORD
        {
            public short X;
            public short Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;
            public COORD dwCursorPosition;
            public int wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
        }

        public CommandHandler()
        {
            _parser = new CommandParser(() => _lastReturnData);

            // 在解析器创建后，注册依赖 _lastReturnData 的命令
            var lrdCommand = new LastReturnDataCommand(
                () => _lastReturnData,
                (data) => _lastReturnData = data
            );
            _parser.RegisterDynamicCommand(lrdCommand);
        }

        public void Run()
        {
            //_lastReturnData.Add(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe");
            // 确保控制台编码正确
            //System.Console.OutputEncoding = Encoding.UTF8;
            //System.Console.InputEncoding = Encoding.UTF8;

            ShowWelcomeScreen();

            while (_running)
            {
                string input = Qi.CmdInput()?.Trim() ?? "";

                // 第一步：先解析命令和参数（处理引号）
                string[] parts = CommandLineParser.Parse(input);

                if (parts.Length == 0)
                {
                    continue;
                }

                // 第二步：对参数替换 & 符号（跳过第一个，第一个是命令名）
                for (int i = 1; i < parts.Length; i++)
                {
                    parts[i] = ReplaceAmpersand(parts[i]);
                }

                // 第三步：重新组合成命令行
                string processedInput = string.Join(" ", parts.Select(p =>
                {
                    //System.Console.WriteLine(p);
                    // 检查是否需要加引号
                    if ((p.Contains(' ') || p.Contains('|') || p.Contains('>') || p.Contains('<')) && !p.Contains('"'))
                    {
                        return $"\"{p}\"";
                    }
                    return p;
                }));

                var result = _parser.Parse(processedInput);

                if (result.Clear)
                {
                    System.Console.Clear();
                    ShowWelcomeScreen();
                    continue;
                }

                if (result.Exit)
                {
                    _running = false;
                }

                // 存储返回值数据
                if (result.ReturnData != null && result.ReturnData.Count > 0)
                {
                    _lastReturnData = result.ReturnData;
                }

                if (!string.IsNullOrEmpty(result.Response))
                {
                    if (result.Success)
                    {
                        System.Console.WriteLine(result.Response);
                    }
                    else
                    {
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine(result.Response);
                        System.Console.ResetColor();
                    }
                }
            }
        }

        /// <summary>
        /// 执行单条命令（用于 -C 模式）
        /// </summary>
        public CommandResult ExecuteCommand(string commandLine)
        {
            // 第一步：先解析命令和参数（处理引号）
            string[] parts = CommandLineParser.Parse(commandLine);

            if (parts.Length == 0)
            {
                return new CommandResult { Success = true };
            }

            // 第二步：对参数替换 & 符号（跳过第一个，第一个是命令名）
            for (int i = 1; i < parts.Length; i++)
            {
                parts[i] = ReplaceAmpersand(parts[i]);
            }

            // 第三步：重新组合成命令行
            string processedInput = string.Join(" ", parts.Select(p =>
            {
                if ((p.Contains(' ') || p.Contains('|') || p.Contains('>') || p.Contains('<')) && !p.Contains('"'))
                {
                    return $"\"{p}\"";
                }
                return p;
            }));

            var result = _parser.Parse(processedInput);

            // 存储返回值数据
            if (result.ReturnData != null && result.ReturnData.Count > 0)
            {
                _lastReturnData = result.ReturnData;
            }

            return result;
        }

        /// <summary>
        /// 替换参数中的 & 符号
        /// </summary>
        private string ReplaceAmpersand(string arg)
        {
            if (string.IsNullOrEmpty(arg)) return arg;

            // 匹配 & 或 &数字 的模式
            var pattern = @"&(\d*)";

            return Regex.Replace(arg, pattern, match =>
            {
                string indexStr = match.Groups[1].Value;

                if (string.IsNullOrEmpty(indexStr))
                {
                    // & 单独使用，取第一条数据
                    if (_lastReturnData.Count > 0)
                    {
                        return _lastReturnData[0];
                    }
                    return "&";
                }
                else
                {
                    // &数字 使用，取对应索引
                    if (int.TryParse(indexStr, out int index) && index >= 0 && index < _lastReturnData.Count)
                    {
                        return _lastReturnData[index];
                    }
                    return match.Value;
                }
            });
        }

        /// <summary>
        /// 处理 & 符号，替换为存储的数据
        /// </summary>
        private string ProcessAmpersand(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // 匹配 & 或 &数字 的模式
            var pattern = @"&(\d*)";

            return Regex.Replace(input, pattern, match =>
            {
                string indexStr = match.Groups[1].Value;
                string replacement;

                if (string.IsNullOrEmpty(indexStr))
                {
                    // & 单独使用，取第一条数据
                    if (_lastReturnData.Count > 0)
                    {
                        replacement = _lastReturnData[0];
                    }
                    else
                    {
                        return "&";
                    }
                }
                else
                {
                    // &数字 使用，取对应索引
                    if (int.TryParse(indexStr, out int index) && index >= 0 && index < _lastReturnData.Count)
                    {
                        replacement = _lastReturnData[index];
                    }
                    else
                    {
                        return match.Value;
                    }
                }

                // 如果替换的内容包含空格或特殊字符，且不在引号内，则需要处理
                if (NeedsQuoting(replacement, match.Index, input))
                {
                    // 如果内容本身已经有引号，就不再加
                    if (replacement.StartsWith("\"") && replacement.EndsWith("\""))
                    {
                        return replacement;
                    }
                    return $"\"{replacement}\"";
                }

                return replacement;
            });
        }

        /// <summary>
        /// 判断替换的内容是否需要加引号
        /// </summary>
        private bool NeedsQuoting(string replacement, int matchIndex, string originalInput)
        {
            // 检查是否已经在引号内
            bool inQuotes = false;
            for (int i = 0; i < matchIndex; i++)
            {
                if (originalInput[i] == '"')
                {
                    inQuotes = !inQuotes;
                }
            }

            // 如果已经在引号内，不需要再加
            if (inQuotes)
            {
                return false;
            }

            // 如果包含空格或特殊字符，需要加引号
            return replacement.Contains(' ') ||
                   replacement.Contains('&') ||
                   replacement.Contains('|') ||
                   replacement.Contains('>') ||
                   replacement.Contains('>') ||
                   replacement.Contains('(') ||
                   replacement.Contains(')') ||
                   replacement.Contains(';') ||
                   replacement.Contains('^') ||
                   replacement.Contains('=');
        }

        /// <summary>
        /// 滚动控制台到底部
        /// </summary>
        private void ScrollToBottom()
        {
            try
            {
                IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);
                if (handle == IntPtr.Zero || handle == new IntPtr(-1))
                    return;

                CONSOLE_SCREEN_BUFFER_INFO csbi;
                if (!GetConsoleScreenBufferInfo(handle, out csbi))
                    return;

                // 获取当前缓冲区大小和光标位置
                short bufferHeight = csbi.dwSize.Y;
                short cursorY = csbi.dwCursorPosition.Y;
                short windowHeight = (short)(csbi.srWindow.Bottom - csbi.srWindow.Top + 1);

                // 如果光标已经超出窗口底部，需要滚动
                if (cursorY >= csbi.srWindow.Bottom)
                {
                    // 计算需要滚动的行数
                    short scrollRows = (short)(cursorY - csbi.srWindow.Bottom + 1);

                    // 新的窗口位置
                    SMALL_RECT newWindow = csbi.srWindow;
                    newWindow.Top += scrollRows;
                    newWindow.Bottom += scrollRows;

                    // 确保不超出缓冲区
                    if (newWindow.Bottom >= bufferHeight)
                    {
                        newWindow.Top = (short)(bufferHeight - windowHeight);
                        newWindow.Bottom = (short)(bufferHeight - 1);
                    }

                    // 滚动窗口
                    SetConsoleWindowInfo(handle, true, ref newWindow);
                }
            }
            catch
            {
                // 忽略滚动错误
            }
        }

        private void ShowWelcomeScreen()
        {
            if (Qi.QisToolkit3_Datas.isSystem)
                System.Console.ForegroundColor = ConsoleColor.DarkRed;
            else
                System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("==========================================");
            System.Console.WriteLine("    Qis Toolkit 3 - 命令行模式 v1.0");
            System.Console.WriteLine("==========================================");
            System.Console.WriteLine();

            if (Qi.QisToolkit3_Datas.isSystem)
            {
                System.Console.WriteLine("【当前处于系统级权限运行，本模式仅适合需高权限场景！】");
                System.Console.WriteLine("【部分需要依赖用户配置的命令将不可用！如ob、b等】");
                System.Console.WriteLine();
            }

            System.Console.ResetColor();

            // 初始滚动到底部
            ScrollToBottom();
        }
    }
}