using System;
using System.Collections.Generic;
using System.Text;

namespace QisToolkit3.Console
{
    /// <summary>
    /// 命令行参数解析器（支持引号）
    /// </summary>
    public static class CommandLineParser
    {
        /// <summary>
        /// 解析命令行，支持引号包裹的参数
        /// </summary>
        /// <param name="commandLine">原始命令行字符串</param>
        /// <returns>解析后的参数数组</returns>
        public static string[] Parse(string commandLine)
        {
            if (string.IsNullOrWhiteSpace(commandLine))
                return Array.Empty<string>();

            var args = new List<string>();
            var currentArg = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < commandLine.Length; i++)
            {
                char c = commandLine[i];

                // 处理引号
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    //continue; // 不添加引号本身
                }

                // 处理空格（只在引号外分割）
                if (c == ' ' && !inQuotes)
                {
                    if (currentArg.Length > 0)
                    {
                        args.Add(currentArg.ToString());
                        currentArg.Clear();
                    }
                    continue;
                }

                // 普通字符（包括反斜杠）都原样添加
                currentArg.Append(c);
            }

            // 添加最后一个参数
            if (currentArg.Length > 0)
            {
                args.Add(currentArg.ToString());
            }

            // 如果还有未关闭的引号，说明输入格式有问题
            if (inQuotes)
            {
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine("警告: 引号未闭合，可能影响参数解析");
                System.Console.ResetColor();
            }

            return args.ToArray();
        }

    }
}