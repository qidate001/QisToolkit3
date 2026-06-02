using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QisToolkit3.Console.Models
{
    /// <summary>
    /// 单个别名配置
    /// </summary>
    public class BiliBiliAlias
    {
        [JsonPropertyName("alias")]  // 添加这个特性
        public string Alias { get; set; }        // 别名
        
        [JsonPropertyName("value")]  // 添加这个特性
        public string Value { get; set; }         // 实际值
        
        [JsonPropertyName("description")]  // 添加这个特性
        public string Description { get; set; }   // 描述
    }

    /// <summary>
    /// 别名配置文件根对象
    /// </summary>
    public class BiliBiliAliasConfig
    {
        public List<BiliBiliAlias> Aliases { get; set; } = new();
    }
}