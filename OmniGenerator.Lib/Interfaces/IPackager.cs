using SeedGenerator.Lib.Data;

namespace SeedGenerator.Lib.Interfaces
{
    /// <summary>
    /// An interface that defines the packaging of a seed.
    /// Packaging is the way files (images, metadata, etc) are persisted  
    /// </summary>
    public interface IPackager
    {
        /// <summary>
        /// Generates the files for the given root
        /// </summary>
        /// <param name="root">The rrot</param>
        /// <param name="path">The directory where the files must be written</param>
        /// <returns></returns>
        Task ProcessAsync(Root root, string path);
    }
}
