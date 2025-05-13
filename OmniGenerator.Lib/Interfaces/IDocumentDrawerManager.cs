using OmniGenerator.Lib.Drawers;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Interfaces.Infrastructure;

namespace OmniGenerator.Lib.Interfaces
{
    /// <summary>
    /// The interface that defines objects responsible of image generation
    /// </summary>
    public interface IDocumentDrawerManager : IProgressReporter<DocumentDrawerManagerProgress>
    {
        /// <summary>
        /// Generates all the images of the documents of a root
        /// </summary>
        /// <param name="root">The root</param>
        /// <returns></returns>
        Task DrawImagesAsync(Root root);
    }
}
