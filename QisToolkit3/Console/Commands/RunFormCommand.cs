using System;
using System.Windows.Forms;
using QisToolkit3.Console.Services;

namespace QisToolkit3.Console.Commands
{
    public class RunFormCommand : ICommand
    {
        private readonly MultiLevelAliasService _aliasService;

        public string Name => "runform";
        public string Description => "运行指定Form（支持别名）";
        public string[] Aliases => new[] { "rf" };

        public RunFormCommand()
        {
            // 为 openbrowser 命令创建多级别名服务
            _aliasService = new MultiLevelAliasService("runform");
        }

        public CommandResult Execute(string[] args)
        {
            if (args.Length == 0)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = GetUsageHelp()
                };
            }

            string target = args[0];

            // 别名管理命令
            if (target.Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                return ListAliases();
            }

            if (target.Equals("reload", StringComparison.OrdinalIgnoreCase))
            {
                _aliasService.Reload();
                return new CommandResult
                {
                    Success = true,
                    Response = "别名配置已重新加载"
                };
            }

            // 解析别名
            string resolvedTarget = _aliasService.Resolve(target);

            // 如果解析后和原来不同，说明使用了别名
            if (resolvedTarget != target)
            {
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.WriteLine($"[别名] '{target}' 解析为: {resolvedTarget}");
                System.Console.ResetColor();
            }

            // 确保URL格式正确
            string FormName = resolvedTarget;


            try
            {
                System.Console.WriteLine($"开始启动窗体 '{FormName}'");
                Application.Run(Program.CreateFormByName(FormName));
                return new CommandResult
                {
                    Success = true,
                    Response = $"启动窗体: {FormName}"
                };
            }
            catch (Exception ex)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = $"启动窗体失败: {ex.Message}"
                };
            }
        }

        private CommandResult ListAliases()
        {
            var allAliases = _aliasService.GetAllAliases();
            var globalAliases = _aliasService.GetGlobalAliases();
            var commandAliases = _aliasService.GetCommandAliases();

            string response = "\n=== OpenForm 别名配置 ===\n";

            // 显示命令专用别名
            if (commandAliases.Count > 0)
            {
                response += "\n[命令专用别名] (优先级高)\n";
                foreach (var kv in commandAliases)
                {
                    string desc = string.IsNullOrEmpty(kv.Value.Description) ? "" : $" ({kv.Value.Description})";
                    response += $"  {kv.Key,-10} -> {kv.Value.Value}{desc}\n";
                }
            }

            // 显示通用别名
            if (globalAliases.Count > 0)
            {
                response += "\n[通用别名] (优先级低)\n";
                foreach (var kv in globalAliases)
                {
                    // 如果被命令专用覆盖，标记出来
                    if (commandAliases.ContainsKey(kv.Key))
                    {
                        response += $"  {kv.Key,-10} -> {kv.Value.Value} (被命令专用覆盖)\n";
                    }
                    else
                    {
                        string desc = string.IsNullOrEmpty(kv.Value.Description) ? "" : $" ({kv.Value.Description})";
                        response += $"  {kv.Key,-10} -> {kv.Value.Value}{desc}\n";
                    }
                }
            }

            if (allAliases.Count == 0)
            {
                response += "暂无已配置的别名\n";
            }

            response += $"\n使用方式:\n";
            response += $"  {Aliases[0]} <别名/URL>     - 打开网址\n";
            response += $"  {Aliases[0]} list          - 查看所有别名\n";
            response += $"  {Aliases[0]} reload        - 重新加载配置\n";

            return new CommandResult
            {
                Success = true,
                Response = response
            };
        }

        private string GetUsageHelp()
        {
            return "用法: rf <窗体名/别名>\n" +
                   "示例: rf Main\n" +
                   "      rf main (如果配置了别名)\n" +
                   "      rf list (查看所有别名)\n\n" +
                   "窗体名与 -o 参数通用一套，详见 '启动参数说明.txt'\n" +
                   "或使用rf list查看所有自带别名配置";
        }
    }
}