using System.Collections.Generic;

namespace OmniGenerator.Lib.Mapping.Interfaces
{
    /// <summary>
    /// Interface for loading and parsing collection items from files
    /// </summary>
    /// <typeparam name="TCollection">The type of collection items</typeparam>
    internal interface IListFileLoader<TCollection>
        where TCollection : notnull
    {
        /// <summary>
        /// Loads a collection from a file
        /// </summary>
        /// <param name="filepath">The path to the file to load</param>
        /// <returns>A collection of parsed items</returns>
        IEnumerable<TCollection> LoadFromFile(string filepath);
    }
}
