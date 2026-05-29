using System;

namespace QisToolkit3.Console.Commands
{
    public class PasswordCommand : ICommand
    {
        public string Name => "setwindowspassword";
        public string Description => "设置或清除Windows用户密码";
        public string[] Aliases => new[] { "pwd" };

        public CommandResult Execute(string[] args)
        {
            if (args.Length < 2)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = GetUsageHelp()
                };
            }

            string userName = args[0];
            string action = args[1].ToLower();
            string password = args.Length > 2 ? args[2] : "";

            // 处理 "_" 表示当前用户
            if (userName == "_")
            {
                userName = "__Auto__";
            }

            // 检查权限
            if (!Qi.QisToolkit3_Datas.isSystem && userName != "__Auto__")
            {
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine("警告: 修改其他用户密码可能需要管理员权限");
                System.Console.ResetColor();
            }

            try
            {
                switch (action)
                {
                    case "set":
                        if (string.IsNullOrEmpty(password))
                        {
                            return new CommandResult
                            {
                                Success = false,
                                Response = "设置密码时不能为空，请提供新密码"
                            };
                        }
                        return SetPassword(userName, password);

                    case "clear":
                        return ClearPassword(userName);

                    default:
                        return new CommandResult
                        {
                            Success = false,
                            Response = $"未知操作: {action}\n{GetUsageHelp()}"
                        };
                }
            }
            catch (Exception ex)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = $"操作失败: {ex.Message}"
                };
            }
        }

        private CommandResult SetPassword(string userName, string password)
        {
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            if (userName == "_") userName = "__Auto__";
            if (userName == "__Auto__")
            {
                if (string.IsNullOrEmpty(Environment.UserName) || Environment.UserName.EndsWith('$'))
                {
                    var localUsers = Qi.GetLocalUsers();
                    if (localUsers.Count > 0)
                    {
                        if (localUsers.Count == 1)
                            userName = localUsers[0];

                        else
                        {
                            System.Console.WriteLine($"当前未登录任何用户，请输入要执行的用户：");

                            for (int i = 1; i <= localUsers.Count; i++)
                                System.Console.WriteLine($"{i}: {localUsers[i - 1]}");

                            System.Console.WriteLine();
                            userName = Qi.GetLocalUsers()[Qi.StrToInt(Qi.CmdInput("QisToolkit3", "SetWindowsPassword")) - 1];
                            System.Console.WriteLine($"正在设置{userName}的密码...");
                        }
                    }
                }
                else
                {
                    System.Console.WriteLine($"正在设置当前用户密码...");
                }

            }
            else
            {
                System.Console.WriteLine($"正在设置用户 '{userName}' 密码...");
            }
            System.Console.ResetColor();

            // 调用现有的方法
            Qi.SetWindowsUserPassword(password, userName);

            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("密码设置成功！");
            System.Console.ResetColor();

            return new CommandResult
            {
                Success = true,
                Response = ""
            };
        }

        private CommandResult ClearPassword(string userName)
        {
            System.Console.ForegroundColor = ConsoleColor.Cyan;

            if (userName == "_") userName = "__Auto__";
            if (userName == "__Auto__")
            {
                if (string.IsNullOrEmpty(Environment.UserName) || Environment.UserName.EndsWith('$'))
                {
                    var localUsers = Qi.GetLocalUsers();
                    if (localUsers.Count > 0)
                    {
                        if (localUsers.Count == 1)
                            userName = localUsers[0];

                        else
                        {
                            System.Console.WriteLine($"当前未登录任何用户，请输入要执行的用户：");

                            for (int i = 1; i <= localUsers.Count; i++)
                                System.Console.WriteLine($"{i}: {localUsers[i - 1]}");

                            System.Console.WriteLine();
                            userName = Qi.GetLocalUsers()[Qi.StrToInt(Qi.CmdInput("QisToolkit3", "SetWindowsPassword")) - 1];
                            System.Console.WriteLine($"正在清除{userName}的密码...");
                        }
                    }
                }
                else
                {
                    System.Console.WriteLine($"正在清除当前用户密码...");
                }
            }
            else
            {
                System.Console.WriteLine($"正在清除用户 '{userName}' 密码...");
            }
            System.Console.ResetColor();

            // 清除密码：传入空字符串
            Qi.SetWindowsUserPassword("", userName);

            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("密码已清除！");
            System.Console.ResetColor();

            return new CommandResult
            {
                Success = true,
                Response = ""
            };
        }

        private string GetUsageHelp()
        {
            return
                "用法: pwd <用户名> <set/clear> [密码]\n\n" +
                "示例:\n" +
                "  pwd _ clear                 - 清除当前用户密码\n" +
                "  pwd Administrator set 12345 - 设置 Administrator 密码为 12345\n" +
                "  pwd _ set MyNewPassword     - 设置当前用户密码\n\n" +
                "说明:\n" +
                "  • _ 表示当前登录用户\n" +
                "  • 清除密码时不需要提供密码参数\n" +
                "  • 修改其他用户密码可能需要管理员权限";
        }
    }
}