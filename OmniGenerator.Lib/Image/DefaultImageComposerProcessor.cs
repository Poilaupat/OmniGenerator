using Autofac.Features.Metadata;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Interfaces;

namespace OmniGenerator.Lib.Image
{
    /// <summary>
    /// The default image composer processor.
    /// This processor takes meta data of documents, selects an <see cref="IImageComposer"/> from the DI container and generates SVG images
    /// </summary>
    public class DefaultImageComposerProcessor : IImageComposerProcessor
    {
        IEnumerable<Meta<IImageComposer>> _composers;

        /// <summary>
        /// Creates a new <see cref="DefaultImageComposerProcessor"/>
        /// </summary>
        /// <param name="composers">The available <see cref="IImageComposer"/> with appropriate meta data to pick one</param>
        public DefaultImageComposerProcessor(IEnumerable<Meta<IImageComposer>> composers)
        {
            _composers = composers;
        }

        /// <summary>
        /// Generates images for all documents in the given root
        /// If a document has no <see cref="IImageComposer"/> it will be ignored
        /// </summary>
        /// <param name="root">The root containing the documents</param>
        /// <returns></returns>
        public async Task ProcessAsync(Root root)
        {
            var docByNamesGrp = root.GetDocuments(true)
                .GroupBy(x => x.Name);

            foreach (var docByName in docByNamesGrp)
            {
                //Getting the appropriate IImageComposer implementation from DI container for the current document type
                var composer = _composers.SingleOrDefault(x => (x.Metadata["DocumentName"] ?? string.Empty).Equals(docByName.Key));

                if (composer is not null)
                {
                    //Compositing image(s)
                    foreach (var doc in docByName)
                    {
                        var recto = composer.Value.ComposeImageRecto(doc);
                        doc.RectoImage = recto;

                        var verso = composer.Value.ComposeImageVerso(doc);
                        doc.VersoImage = verso;
                    }
                }
            }

            await Task.CompletedTask;
        }
    }
}
