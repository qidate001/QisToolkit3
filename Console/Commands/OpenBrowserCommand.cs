using System;
using QisToolkit3.Console.Services;

namespace QisToolkit3.Console.Commands
{
    public class OpenBrowserCommand : ICommand
    {
        private readonly MultiLevelAliasService _aliasService;

        public string Name => "openbrowser";
        public string Description => "用默认浏览器打开URL（支持别名）";
        public string[] Aliases => new[] { "ob" };

        public OpenBrowserCommand()
        {
            // 为 openbrowser 命令创建多级别名服务
            _aliasService = new MultiLevelAliasService("openbrowser");
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
                System.Console.WriteLine($"别名 '{target}' 解析为: {resolvedTarget}");
                System.Console.ResetColor();
            }

            // 确保URL格式正确
            string url = resolvedTarget;
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = "https://" + url;
            }

            try
            {
                Qi.OpenBrowser(url);

                return CommandResult.Ok(
                    $"正在打开: {url}",
                    url,                                // 数据0: URL
                    "True"                              // 数据1: 是否成功
                );
            }
            catch (Exception ex)
            {
                return CommandResult.Error(
                    $"打开浏览器失败: {ex.Message}",
                    url,                                // 数据0: URL
                    "False",                            // 数据1: 是否成功
                    ex.Message                          // 数据2: 错误信息
                );
            }
        }

        private CommandResult ListAliases()
        {
            var allAliases = _aliasService.GetAllAliases();
            var globalAliases = _aliasService.GetGlobalAliases();
            var commandAliases = _aliasService.GetCommandAliases();

            string response = "\n=== OpenBrowser 别名配置 ===\n";

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
            return "用法: ob <URL/别名>\n" +
                   "示例: ob https://www.bilibili.com\n" +
                   "      ob bilibili.com\n" +
                   "      ob bili (如果配置了别名)\n" +
                   "      ob list (查看所有别名)";
        }
    }
}