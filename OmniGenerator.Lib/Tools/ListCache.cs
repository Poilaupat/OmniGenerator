using System.Text;
using OmniGenerator.Lib.Generators.Fields;

namespace OmniGenerator.Lib.Tools
{
    /// <summary>
    /// Provides tools for loading and caching text list used by <see cref="FieldGeneratorList"/>
    /// </summary>
    public static class ListCache
    {
        private static Dictionary<string, string[]> _cache = new Dictionary<string, string[]>();

        /// <summary>
        /// Gets the list specified by the name.
        /// If the list is already loaded, its will be returned from the cache. If not it is loaded in the cache
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string[] GetList(string filepath)
        {
            if (!_cache.ContainsKey(filepath))
            {
                _cache[filepath] = LoadList(filepath);
            }
            return _cache[filepath];
        }

        /// <summary>
        /// Loads a list into the cache.
        /// There is an attempt to automatically detect the file encoding.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
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
