using System;

namespace QisToolkit3.Console
{
    /// <summary>
    /// 命令接口
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 命令描述
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 命令别名
        /// </summary>
        string[] Aliases { get; }

        /// <summary>
        /// 执行命令
        /// </summary>
        CommandResult Execute(string[] args);
    }
}