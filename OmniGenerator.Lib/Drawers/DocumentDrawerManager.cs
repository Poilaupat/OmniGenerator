using Autofac.Features.Metadata;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;

namespace OmniGenerator.Lib.Drawers
{
    /// <summary>
    /// The image composer processor.
    /// This processor takes meta data of documents, selects an <see cref="IDocumentDrawer"/> from the PluginService and generates SVG images
    /// </summary>
    internal sealed class DocumentDrawerManager : IDocumentDrawerManager
    {
        #region Interface properties

        public IProgress<DocumentDrawerManagerProgress>? Progress { get; set; }
        public int ProgressResolution { get; set; }

        #endregion

        IPluginService _pluginService;

        /// <summary>
        /// Creates a new <see cref="DocumentDrawerManager"/>
        /// </summary>
        /// <param name="composers">The available <see cref="IDocumentDrawer"/> with appropriate meta data to pick one</param>
        public DocumentDrawerManager(IPluginService pluginService)
        {
            _pluginService = pluginService;
        }

        /// <summary>
        /// Generates images for all documents in the given root
        /// If a document has no <see cref="IDocumentDrawer"/> it will be ignored
        /// </summary>
        /// <param name="root">The root containing the documents</param>
        /// <returns></returns>
        public async Task DrawImagesAsync(Root root)
        {
            var docsByComposer = root.GetDocuments()
                .GroupBy(x => x.ImageComposer);

            foreach (var docByComposer in docsByComposer.Where(g => !string.IsNullOrWhiteSpace(g.Key)))
            {
                //Getting the appropriate IDocumentDrawer implementation for the current document type
                var drawer = _pluginService.GetPlugin<IDocumentDrawer>(docByComposer.Key!);

                if (drawer is not null)
                {
                    //Compositing image(s)
                    foreach (var doc in docByComposer)
                    {
                        var recto = drawer.DrawRecto(doc);
                        doc.RectoImage = recto;

                        var verso = drawer.DrawVerso(doc);
                        doc.VersoImage = verso;
                    }
                }
            }

            await Task.CompletedTask;
        }
    }
}
