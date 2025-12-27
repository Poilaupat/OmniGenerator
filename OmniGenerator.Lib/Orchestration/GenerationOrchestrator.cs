using Microsoft.Extensions.Logging;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Drawers;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;

namespace OmniGenerator.Lib.Orchestration
{
    public class GenerationOrchestrator : IGenerationOrchestrator
    {
        private readonly IPluginService _pluginService;
        private readonly IHierarchyBuilder _hierarchyBuilder;
        private readonly IDocumentDrawerManager _imageComposerProcessor;
        private readonly ILogger<GenerationOrchestrator> _logger;

        public Notifier<GenerationProgress> Notifier { get; }

        public GenerationOrchestrator(
            IPluginService pluginService,
            IHierarchyBuilder hierarchyBuilder,
            IDocumentDrawerManager imageComposerProcessor,
            Notifier<GenerationProgress> notifier,
            ILogger<GenerationOrchestrator> logger)
        {
            _pluginService = pluginService;
            _hierarchyBuilder = hierarchyBuilder;
            _imageComposerProcessor = imageComposerProcessor;
            _logger = logger;

            Notifier = notifier;
            Notifier.UseRegulation = true;
        }

        public async Task<Root> ExecuteAsync(
            OmniGeneratorConfiguration configuration,
            string outputFolderPath,
            CancellationToken cancellationToken = default)
        {
            var root = await BuildHierarchyAsync(configuration, cancellationToken);
            await DrawImagesAsync(root, cancellationToken);
            await PackageAsync(root, configuration, outputFolderPath, cancellationToken);

            return root;
        }

        private async Task<Root> BuildHierarchyAsync(
            OmniGeneratorConfiguration configuration,
            CancellationToken cancellationToken)
        {
            ReportProgress(GenerationStep.Hierarchy, StepStatus.Processing, force: true);

            HierarchyBuilderProgress? lastProgress = null;

            try
            {
                _hierarchyBuilder.Notifier.Progress = new Progress<HierarchyBuilderProgress>(progress =>
                {
                    lastProgress = progress;
                    ReportProgress(GenerationStep.Hierarchy, StepStatus.Processing, progress);
                });

                var root = await _hierarchyBuilder.BuildAsync(configuration);

                // Report final progress with Succeeded status
                ReportProgress(GenerationStep.Hierarchy, StepStatus.Succeeded, lastProgress, force: true);
                return root;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while building the hierarchy.");
                ReportProgress(GenerationStep.Hierarchy, StepStatus.Failed, error: ex, force: true);
                throw;
            }
        }

        private async Task DrawImagesAsync(Root root, CancellationToken cancellationToken)
        {
            if (_imageComposerProcessor is null || root.GetDocuments().All(d => string.IsNullOrWhiteSpace(d.ImageComposer)))
            {
                ReportProgress(GenerationStep.Images, StepStatus.Skipped, force: true);
                return;
            }

            ReportProgress(GenerationStep.Images, StepStatus.Processing, force: true);

            DocumentDrawerManagerProgress? lastProgress = null;

            try
            {
                //_imageComposerProcessor.ProgressResolution = ProgressResolution;
                _imageComposerProcessor.Notifier.Progress = new Progress<DocumentDrawerManagerProgress>(progress =>
                {
                    lastProgress = progress;
                    ReportProgress(GenerationStep.Images, StepStatus.Processing, progress);
                });

                await _imageComposerProcessor.DrawImagesAsync(root);

                // Report final progress with Succeeded status
                ReportProgress(GenerationStep.Images, StepStatus.Succeeded, lastProgress, force: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while drawing images.");
                ReportProgress(GenerationStep.Images, StepStatus.Failed, error: ex, force: true);
                throw;
            }
        }

        private async Task PackageAsync(
            Root root,
            OmniGeneratorConfiguration configuration,
            string outputFolderPath,
            CancellationToken cancellationToken)
        {
            var packager = _pluginService.GetPlugin<IPackager>(configuration.PackagerName);
            if (packager is null)
            {
                ReportProgress(GenerationStep.Package, StepStatus.Skipped, force: true);
                return;
            }

            ReportProgress(GenerationStep.Package, StepStatus.Processing, force: true);

            try
            {
                await packager.ProcessAsync(root, outputFolderPath, configuration.RenderResolutionDPI);
                ReportProgress(GenerationStep.Package, StepStatus.Succeeded, force: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while packaging the output.");
                ReportProgress(GenerationStep.Package, StepStatus.Failed, error: ex, force: true);
                throw;
            }
        }

        private void ReportProgress(GenerationStep step, StepStatus status, object? data = null, Exception? error = null, bool force = false)
        {
            Notifier.SendNotification(new GenerationProgress
            {
                Step = step,
                Status = status,
                Data = data,
                Error = error
            }, force);
        }
    }
}
