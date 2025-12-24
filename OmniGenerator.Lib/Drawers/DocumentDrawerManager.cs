using Autofac.Features.Metadata;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
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

        public Notifier<DocumentDrawerManagerProgress> Notifier { get; }

        #endregion

        private readonly IPluginService _pluginService;
        private long _totalDocuments = 0;
        private long _processedDocuments = 0;

        /// <summary>
        /// Creates a new <see cref="DocumentDrawerManager"/>
        /// </summary>
        /// <param name="pluginService">The plugin service used to retrieve document drawers</param>
        public DocumentDrawerManager(IPluginService pluginService, Notifier<DocumentDrawerManagerProgress> notifier)
        {
            _pluginService = pluginService;
            Notifier = notifier;
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

            // Count total documents to process
            _totalDocuments = docsByComposer
                .Where(g => !string.IsNullOrWhiteSpace(g.Key))
                .Sum(g => g.Count());
            _processedDocuments = 0;

            Notifier.SendNotification(GetNotificationData());

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
                        doc.RectoVectorImage = recto;

                        var verso = drawer.DrawVerso(doc);
                        doc.VersoVectorImage = verso;

                        Interlocked.Increment(ref _processedDocuments);
                        Notifier.SendNotification(GetNotificationData());
                    }
                }
            }

            Notifier.SendNotification(GetNotificationData());
            await Task.CompletedTask;
        }

        private DocumentDrawerManagerProgress GetNotificationData()
        {
            return new DocumentDrawerManagerProgress()
            {
                TotalDocuments = Interlocked.Read(ref _totalDocuments),
                ProcessedDocuments = Interlocked.Read(ref _processedDocuments)
            };
        }
    }
}
