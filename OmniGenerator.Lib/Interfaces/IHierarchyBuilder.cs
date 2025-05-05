using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Tools;

namespace OmniGenerator.Lib.Interfaces
{

    /// <summary>
    /// Defines a contract for building a document generation hierarchy represented by a <see cref="Root"/> object.
    /// </summary>
    public interface IHierarchyBuilder
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

        /// <summary>
        /// Gets or sets the optional progress reporter used to receive updates about the build process.
        /// Can be set to <c>null</c> if progress reporting is not needed.
        /// </summary>
        IProgress<HierarchyBuilderProgressReport>? Progress { get; set; }

        /// <summary>
        /// Gets or sets the minimum time interval, in milliseconds, between two progress updates.
        /// This value controls how frequently progress notifications should be emitted. 
        /// </summary>
        int ProgressResolution { get; set; }
    }

}
