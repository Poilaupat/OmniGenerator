using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Tools;

namespace OmniGenerator.Lib.Interfaces
{

    /// <summary>
    /// An interface that describes ways to create Roots
    /// </summary>
    public interface IHierarchyBuilder
    {
        /// <summary>
        /// Builds a <see cref="Root"/> using the provided configuration
        /// </summary>
        /// <param name="config">The generator configuration</param>
        /// <param name="progress">An optionnal <see cref="IProgress{T}"/> object to be notified of the build process progress</param>
        /// <returns></returns>
        Task<Root> BuildAsync(OmniGeneratorConfiguration config, IProgress<HierarchyBuilderProgressReport>? progress);
    }
}
