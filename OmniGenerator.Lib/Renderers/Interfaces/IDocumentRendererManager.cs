using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.Renderers.Interfaces
{
    /// <summary>
    /// The interface that defines objects responsible of image generation
    /// </summary>
    public interface IDocumentRendererManager
    {
        /// <summary>
        /// Generates all the images of the documents of a root
        /// </summary>
        Task RenderImagesAsync(Root root);
    }
}
