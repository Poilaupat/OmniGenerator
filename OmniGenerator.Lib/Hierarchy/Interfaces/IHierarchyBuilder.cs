using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.Hierarchy.Interfaces
{
    /// <summary>
    /// Defines a contract for building a document generation hierarchy represented by a <see cref="Root"/> object.
    /// </summary>
    public interface IHierarchyBuilder
    {
        /// <summary>
        /// Asynchronously builds a <see cref="Root"/> hierarchy based on the provided generator configuration.
        /// </summary>
        Task<Root> BuildAsync(OmniGeneratorConfiguration config);
    }
}
