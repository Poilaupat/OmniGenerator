using Autofac.Features.Metadata;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Renderers.Interfaces;
using OmniGenerator.Lib.Reporting;

namespace OmniGenerator.Lib.Renderers
{
    /// <summary>
    /// The image renderer processor.
    /// This processor takes meta data of documents, selects an <see cref="IDocumentRenderer"/> from the PluginService and generates SVG images
    /// </summary>
    internal sealed class DocumentRendererManager : IDocumentRendererManager
    {
        public const string HubKey = nameof(DocumentRendererManager);

        private readonly IPluginService _pluginService;
        private readonly IProgressHub<RenderingProgress> _hub;
        private long _totalDocuments = 0;
        private long _processedDocuments = 0;

        public DocumentRendererManager(IPluginService pluginService, IProgressHub<RenderingProgress> hub)
        {
            _pluginService = pluginService;
            _hub = hub;
        }

        /// <summary>
        /// Generates images for all documents in the given root
        /// If a document has no <see cref="IDocumentRenderer"/> it will be ignored
        /// </summary>
        public Task RenderImagesAsync(Root root)
        {
            var docsByComposer = root.GetAllDocuments()
                .Where(d => !string.IsNullOrWhiteSpace(d.ImageComposer))
                .ToList();

            // Count total documents to process
            _totalDocuments = docsByComposer.Count;
            _processedDocuments = 0;

            _hub.Report(HubKey, GetProgress());

            // Group documents by composer type for efficient parallel processing
            var groupedDocs = docsByComposer
                .GroupBy(x => x.ImageComposer)
                .ToList();

            // Process each composer type sequentially, but documents within each type in parallel
            foreach (var docGroup in groupedDocs)
            {
                var composerName = docGroup.Key!;
                var documents = docGroup.ToList();

                // Resolve a single renderer instance for this composer group.
                // Reusing one instance is safe only if the renderer implementation is thread-safe.
                var renderer = _pluginService.GetPlugin<IDocumentRenderer>(composerName);
                if (renderer is not null)
                {
                    // Parallel processing of documents with the same composer
                    Parallel.ForEach(documents, doc =>
                    {
                        var recto = renderer.RenderRecto(doc);
                        doc.RectoVectorImage = recto;

                        var verso = renderer.RenderVerso(doc);
                        doc.VersoVectorImage = verso;

                        Interlocked.Increment(ref _processedDocuments);
                        _hub.Report(HubKey, GetProgress());
                    });
                }
            }

            _hub.Report(HubKey, GetProgress());
            return Task.CompletedTask;
        }

        private RenderingProgress GetProgress() => new()
        {
            TotalDocuments = Interlocked.Read(ref _totalDocuments),
            ProcessedDocuments = Interlocked.Read(ref _processedDocuments)
        };
    }
}
