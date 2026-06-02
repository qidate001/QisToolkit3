using System.Collections.Generic;

namespace QisToolkit3.Console
{
    public class CommandResult
    {
        public bool Exit { get; set; }
        public bool Clear { get; set; }
        public string Response { get; set; }
        public bool Success { get; set; }

        /// <summary>
        /// 命令返回的数据列表（用于 & 符号）
        /// </summary>
        public List<string> ReturnData { get; set; } = new List<string>();

        /// <summary>
        /// 创建带返回数据的成功结果
        /// </summary>
        public static CommandResult Ok(string response, params string[] returnData)
        {
            return new CommandResult
            {
                Success = true,
                Response = response,
                ReturnData = new List<string>(returnData)
            };
        }

        /// <summary>
        /// 创建带返回数据的成功结果（无输出消息）
        /// </summary>
        public static CommandResult OkWithData(params string[] returnData)
        {
            return new CommandResult
            {
                Success = true,
                Response = "",
                ReturnData = new List<string>(returnData)
            };
        }

        /// <summary>
        /// 创建错误结果
        /// </summary>
        public static CommandResult Error(string response, params string[] returnData)
        {
            return new CommandResult
            {
                Success = false,
                Response = response,
                ReturnData = new List<string>()
            };
        }

        /// <summary>
        /// 创建带返回数据的错误结果（无输出消息）
        /// </summary>
        public static CommandResult ErrorWithData(params string[] returnData)
        {
            return new CommandResult
            {
                Success = false,
                Response = "",
                ReturnData = new List<string>()
            };
        }

        /// <summary>
        /// 创建清屏结果
        /// </summary>
        public static CommandResult ClearScreen()
        {
            return new CommandResult
            {
                Clear = true,
                Success = true
            };
        }

        /// <summary>
        /// 创建退出结果
        /// </summary>
        public static CommandResult ExitApp(string response = "再见！")
        {
            return new CommandResult
            {
                Exit = true,
                Success = true,
                Response = response
            };
        }
    }
}