using OmniGenerator.Lib.Generators;

namespace OmniGenerator.Lib.Interfaces
{
    /// <summary>
    /// The interface that defines objects responsible of image generation
    /// </summary>
    public interface IImageComposerProcessor
    {
        /// <summary>
        /// Generates all the images of the documents of a root
        /// </summary>
        /// <param name="root">The root</param>
        /// <returns></returns>
        Task ProcessAsync(Root root);
    }
}
