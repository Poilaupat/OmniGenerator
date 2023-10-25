using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Tools
{
    public static class ListCache
    {
        private static Dictionary<string, string[]> _cache = new Dictionary<string, string[]>();

        public static string[] GetList(string filepath)
        {
            if (!_cache.ContainsKey(filepath))
            {
                _cache[filepath] = LoadList(filepath);
            }
            return _cache[filepath];
        }

        private static string[] LoadList(string filepath)
        {
            var list = new List<string>();

            if (File.Exists(filepath))
            {
                using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var encoding = StreamTools.DetectEncoding(stream);
                    using (var sr = new StreamReader(stream, encoding ?? Encoding.UTF8))
                    {
                        string? line;
                        while ((line = sr.ReadLine()) is not null)
                        {
                            list.Add(line);
                        }
                    }
                }
            }

            return list.ToArray();
        }
    }
}
