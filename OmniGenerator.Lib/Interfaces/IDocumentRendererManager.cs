using OmniGenerator.Lib.Renderers;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Interfaces.Infrastructure;

namespace OmniGenerator.Lib.Interfaces
{
    /// <summary>
    /// The interface that defines objects responsible of image generation
    /// </summary>
    public interface IDocumentRendererManager : INotifier<DocumentRendererManagerProgress>
    {
        /// <summary>
        /// Generates all the images of the documents of a root
        /// </summary>
        /// <param name="root">The root</param>
        /// <returns></returns>
        Task RenderImagesAsync(Root root);
    }
}
