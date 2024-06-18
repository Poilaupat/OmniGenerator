using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Param;

namespace SeedGenerator.Lib.Interfaces
{

    /// <summary>
    /// An interface that describes ways to create Roots
    /// </summary>
    public interface IRootBuilder
    {
        /// <summary>
        /// Builds a <see cref="Root"/> using the provided configuration
        /// </summary>
        /// <param name="param">The configuration</param>
        /// <returns></returns>
        Root Build(RootParam param);
    }
}
