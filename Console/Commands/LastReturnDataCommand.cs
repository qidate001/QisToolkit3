using System;
using System.Linq;

namespace QisToolkit3.Console.Commands
{
    public class LastReturnDataCommand : ICommand
    {
        private readonly Func<List<string>> _getData;
        private readonly Action<List<string>> _setData;

        public string Name => "lastreturndata";
        public string Description => "查看或设置命令返回值数据";
        public string[] Aliases => new[] { "&", "lrd" };

        public LastReturnDataCommand(Func<List<string>> getData, Action<List<string>> setData)
        {
            _getData = getData;
            _setData = setData;
        }

        public CommandResult Execute(string[] args)
        {
            // 无参数：显示当前所有数据
            if (args.Length == 0)
            {
                var data = _getData();
                if (data.Count == 0)
                {
                    return CommandResult.Ok("暂无数据");
                }

                string response = "";
                for (int i = 0; i < data.Count; i++)
                {
                    // 如果数据包含空格，加引号显示
                    //string display = data[i].Contains(" ") ? $"\"{data[i]}\"" : data[i];
                    response += $"{i} : {data[i]}\n";
                }
                return CommandResult.Ok(response.TrimEnd());
            }

            // 有参数：设置新数据
            var newData = args.ToList();
            _setData(newData);

            // 显示设置结果
            string result = "";
            for (int i = 0; i < newData.Count; i++)
            {
                //string display = newData[i].Contains(" ") ? $"\"{newData[i]}\"" : newData[i];
                result += $"{i} : {newData[i]}\n";
            }

            return CommandResult.Ok($"已设置 {newData.Count} 条数据:\n{result.TrimEnd()}");
        }
    }
}