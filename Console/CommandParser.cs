using QisToolkit3.Console.Commands;
using System;
using System.Collections.Generic;

namespace QisToolkit3.Console
{
    /// <summary>
    /// 命令解析器
    /// </summary>
    public class CommandParser
    {
        // 存储所有唯一的命令实例
        private readonly List<ICommand> _commands;

        // 命令名/别名 -> 命令实例的映射
        private readonly Dictionary<string, ICommand> _commandMap;

        private Func<List<string>> _getReturnData;

        public CommandParser(Func<List<string>> getReturnData = null)
        {
            _commands = new List<ICommand>();
            _commandMap = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase);
            _getReturnData = getReturnData ?? (() => new List<string>());

            RegisterDefaultCommands();
        }

        private void RegisterDefaultCommands()
        {
            // 创建一个临时字典来去重
            var uniqueCommands = new Dictionary<string, ICommand>();

            // 注册系统命令
            var exitCmd = new ExitCommand();
            var clearCmd = new ClearCommand();

            uniqueCommands[exitCmd.Name] = exitCmd;
            uniqueCommands[clearCmd.Name] = clearCmd;

            // 注册普通命令
            var helpCmd = new HelpCommand(this);
            var timeCmd = new TimeCommand();
            var dateCmd = new DateCommand();
            var echoCmd = new EchoCommand();
            var infoCmd = new InfoCommand();
            var biliCmd = new BilibiliCommand();
            var obCmd = new OpenBrowserCommand();
            var ffmpegCmd = new FFmpegCommand();
            var convertCmd = new ConvertCommand();
            var cmdCmd = new CmdCommand();
            var psCmd = new PowerShellCommand();
            var killCmd = new KillCommand();
            var swCmd = new SoftwareCommand();
            var webGetCmd = new WebGetCommand();
            var getGitHubCmd = new GetGitHubCommand();
            var pwdCmd = new PasswordCommand();
            var rfCmd = new RunFormCommand();
            var mbCmd = new MessageBoxCommand();

            uniqueCommands[helpCmd.Name] = helpCmd;
            uniqueCommands[timeCmd.Name] = timeCmd;
            uniqueCommands[dateCmd.Name] = dateCmd;
            uniqueCommands[echoCmd.Name] = echoCmd;
            uniqueCommands[infoCmd.Name] = infoCmd;
            uniqueCommands[biliCmd.Name] = biliCmd;
            uniqueCommands[obCmd.Name] = obCmd;
            uniqueCommands[ffmpegCmd.Name] = ffmpegCmd;
            uniqueCommands[convertCmd.Name] = convertCmd;
            uniqueCommands[cmdCmd.Name] = cmdCmd;
            uniqueCommands[psCmd.Name] = psCmd;
            uniqueCommands[killCmd.Name] = killCmd;
            uniqueCommands[swCmd.Name] = swCmd;
            uniqueCommands[webGetCmd.Name] = webGetCmd;
            uniqueCommands[getGitHubCmd.Name] = getGitHubCmd;
            uniqueCommands[pwdCmd.Name] = pwdCmd;
            uniqueCommands[rfCmd.Name] = rfCmd;
            uniqueCommands[mbCmd.Name] = mbCmd;

            // 先保存所有唯一的命令实例
            _commands.Clear();
            _commands.AddRange(uniqueCommands.Values);

            // 然后建立命令名/别名到命令实例的映射
            _commandMap.Clear();

            foreach (var cmd in _commands)
            {
                // 映射主命令名
                _commandMap[cmd.Name] = cmd;

                // 映射所有别名
                foreach (var alias in cmd.Aliases)
                {
                    _commandMap[alias] = cmd;
                }
            }

            // 最后注册DD命令（需要传入所有命令的字典和返回值访问器）
            var ddCmd = new DDCommand(_commandMap, _getReturnData);  // 传入访问器
            _commands.Add(ddCmd);
            _commandMap[ddCmd.Name] = ddCmd;
        }

        /// <summary>
        /// 注册命令（外部注册用）
        /// </summary>
        public void RegisterCommand(ICommand command)
        {
            // 检查是否已存在相同实例
            if (!_commands.Contains(command))
            {
                _commands.Add(command);
            }

            // 映射主命令名
            _commandMap[command.Name] = command;

            // 映射别名
            foreach (var alias in command.Aliases)
            {
                _commandMap[alias] = command;
            }
        }

        /// <summary>
        /// 注册动态命令（用于依赖 CommandHandler 状态的命令）
        /// </summary>
        public void RegisterDynamicCommand(ICommand command)
        {
            _commands.Add(command);
            _commandMap[command.Name] = command;
            foreach (var alias in command.Aliases)
            {
                _commandMap[alias] = command;
            }
        }

        /// <summary>
        /// 获取所有唯一的命令实例
        /// </summary>
        public IEnumerable<ICommand> GetAllCommands()
        {
            return _commands;
        }

        public CommandResult Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new CommandResult { Success = true };
            }

            string[] parts = CommandLineParser.Parse(input);

            string commandName = parts[0];
            string[] args = parts.Length > 1 ? parts[1..] : new string[0];

            if (_commandMap.TryGetValue(commandName, out var command))
            {
                try
                {
                    return command.Execute(args);
                }
                catch (Exception ex)
                {
                    return new CommandResult
                    {
                        Success = false,
                        Response = $"执行命令时出错: {ex.Message}"
                    };
                }
            }

            return new CommandResult
            {
                Success = false,
                Response = $"未知命令: '{commandName}'\n输入 'help' 查看可用命令"
            };
        }
    }


    // 系统命令：退出
    public class ExitCommand : ICommand
    {
        public string Name => "exit";
        public string Description => "退出程序";
        public string[] Aliases => new[] { "quit" };

        public CommandResult Execute(string[] args)
        {
            return new CommandResult { Exit = true, Success = true, Response = "再见！" };
        }
    }

    // 系统命令：清屏
    public class ClearCommand : ICommand
    {
        public string Name => "cls";
        public string Description => "清屏";
        public string[] Aliases => new[] { "clear" };

        public CommandResult Execute(string[] args)
        {
            return new CommandResult { Clear = true, Success = true };
        }
    }

    // 系统信息命令
    public class InfoCommand : ICommand
    {
        public string Name => "info";
        public string Description => "显示系统信息";
        public string[] Aliases => Array.Empty<string>();

        public CommandResult Execute(string[] args)
        {
            return new CommandResult
            {
                Success = true,
                Response = GetSystemInfo()
            };
        }

        private string GetSystemInfo()
        {
            return
                $"\n系统信息:\n" +
                $"  操作系统: {Environment.OSVersion}\n" +
                $"  64位系统: {Environment.Is64BitOperatingSystem}\n" +
                $"  处理器数: {Environment.ProcessorCount}\n" +
                $"  计算机名: {Environment.MachineName}\n" +
                $"  用户名: {Environment.UserName}\n" +
                $"  当前目录: {Environment.CurrentDirectory}\n" +
                $"  启动时间: {Environment.TickCount / 1000 / 60} 分钟前\n";
        }
    }
}