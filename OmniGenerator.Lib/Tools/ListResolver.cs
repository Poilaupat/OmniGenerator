using OmniGenerator.Lib.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OmniGenerator.Lib.Tools
{
    /// <summary>
    /// Helper class to resolve lists from configuration and/or file
    /// </summary>
    internal static class ListResolver
    {
        /// <summary>
        /// Resolves a list from configuration and/or file path.
        /// Priority is given to the file if it exists.
        /// </summary>
        /// <typeparam name="TCollection">The type of collection items</typeparam>
        /// <param name="configuredList">The list from configuration</param>
        /// <param name="listFilePath">The path to the list file</param>
        /// <param name="fileLoader">The file loader implementation</param>
        /// <returns>The resolved list (from file if exists, otherwise from configuration, or empty)</returns>
        public static IEnumerable<TCollection> ResolveList<TCollection>(
            IEnumerable<TCollection>? configuredList,
            string? listFilePath,
            IListFileLoader<TCollection> fileLoader)
            where TCollection : notnull
        {
            // Priority to items from file if specified
            if (!string.IsNullOrEmpty(listFilePath) && File.Exists(listFilePath))
            {
                var fileItems = fileLoader.LoadFromFile(listFilePath);
                if (fileItems.Any())
                    return fileItems;
            }

            // Otherwise, use list from configuration, or empty list
            return configuredList ?? Enumerable.Empty<TCollection>();
        }
    }
}
