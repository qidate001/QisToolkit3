using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisToolkit3.Forms
{
    public class IdNameMapper
    {
        private Dictionary<string, string> _idNameMap;
        private readonly string _mapFilePath;

        public IdNameMapper(string mapFilePath)
        {
            _mapFilePath = mapFilePath;
            _idNameMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 读取映射文件
        /// 文件格式：每行一个 [ID] [名称]，用空格或制表符分隔
        /// </summary>
        public async Task LoadMapAsync()
        {
            if (!File.Exists(_mapFilePath))
            {
                throw new FileNotFoundException($"映射文件不存在: {_mapFilePath}");
            }

            _idNameMap.Clear();

            var lines = await File.ReadAllLinesAsync(_mapFilePath);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue; // 跳过空行和注释行

                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 2)
                {
                    string id = parts[0].Trim();
                    string name = string.Join(" ", parts.Skip(1)).Trim(); // 处理名称中包含空格的情况

                    if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name))
                    {
                        _idNameMap[id] = name;
                    }
                }
            }
        }

        /// <summary>
        /// 根据ID获取名称，如果不存在则返回ID本身
        /// </summary>
        public string GetNameOrDefault(string id)
        {
            if (string.IsNullOrEmpty(id))
                return id;

            return _idNameMap.TryGetValue(id, out var name) ? name : id;
        }

        /// <summary>
        /// 清理文件名中的非法字符
        /// </summary>
        public static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return fileName;

            var invalidChars = Path.GetInvalidFileNameChars();
            return string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        }

        // 添加到IdNameMapper类中
        public int GetMapCount() => _idNameMap.Count;

        /// <summary>
        /// 重新加载映射文件
        /// </summary>
        public async Task ReloadMapAsync()
        {
            await LoadMapAsync();
        }

        /// <summary>
        /// 获取所有ID列表
        /// </summary>
        public IEnumerable<string> GetAllIds() => _idNameMap.Keys;

        /// <summary>
        /// 获取所有映射
        /// </summary>
        public IReadOnlyDictionary<string, string> GetAllMappings() => _idNameMap;
    }
}
