using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.Interfaces
{

    /// <summary>
    /// Defines a contract for building a document generation hierarchy represented by a <see cref="Root"/> object.
    /// </summary>
    public interface IHierarchyBuilder : INotifier<HierarchyBuilderProgress>
    {
        /// <summary>
        /// Asynchronously builds a <see cref="Root"/> hierarchy based on the provided generator configuration.
        /// </summary>
        /// <param name="config">The configuration describing how the hierarchy should be built.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the generated <see cref="Root"/> object representing the built hierarchy.
        /// </returns>
        Task<Root> BuildAsync(OmniGeneratorConfiguration config);
    }

}
