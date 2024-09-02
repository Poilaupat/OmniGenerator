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
        /// <param name="param">The configuration</param>
        /// <returns></returns>
        Root Build(OmniGeneratorConfiguration config, IProgress<ProgressReport>progress);
    }
}
