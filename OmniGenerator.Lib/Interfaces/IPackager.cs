using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.Interfaces
{
    /// <summary>
    /// Defines a contract for packaging and persisting generated files, such as images and metadata,
    /// from a document hierarchy. Implementations of this interface are responsible for determining
    /// how and where the files are stored (e.g., file system, archive, cloud storage).
    /// </summary>
    public interface IPackager : IOmniGeneratorPlugin
    {
        /// <summary>
        /// Processes the specified <see cref="Root"/> hierarchy and generates all associated files,
        /// persisting them to the given base directory.
        /// </summary>
        /// <param name="root">
        /// The <see cref="Root"/> object representing the top-level document hierarchy to be packaged.
        /// This includes all groups, documents, fields, and associated images or metadata.
        /// </param>
        /// <param name="basepath">
        /// The absolute or relative path to the directory where the generated files should be written.
        /// The directory is guaranteed to exist when this method is called.
        /// </param>
        /// <param name="imageRenderingResolution">
        /// The resolution (in DPI or pixels per inch) to use when rendering images associated with the documents.
        /// Implementations should use this value to control the quality and size of generated image files.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous packaging operation.
        /// </returns>
        Task ProcessAsync(Root root, string basepath, int imageRenderingResolution);
    }
}
