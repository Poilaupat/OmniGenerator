using Microsoft.Extensions.Logging;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Drawers;
using OmniGenerator.Lib.Hierarchy;
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

        public IProgress<GenerationProgress>? Progress { get; set; }
        public int ProgressResolution { get; set; } = 1000;

        public GenerationOrchestrator(
            IPluginService pluginService,
            IHierarchyBuilder hierarchyBuilder,
            IDocumentDrawerManager imageComposerProcessor,
            ILogger<GenerationOrchestrator> logger)
        {
            _pluginService = pluginService;
            _hierarchyBuilder = hierarchyBuilder;
            _imageComposerProcessor = imageComposerProcessor;
            _logger = logger;
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
            ReportProgress(GenerationStep.Hierarchy, StepStatus.Processing);

            try
            {
                _hierarchyBuilder.ProgressResolution = ProgressResolution;
                _hierarchyBuilder.Progress = new Progress<HierarchyBuilderProgress>(progress =>
                {
                    ReportProgress(GenerationStep.Hierarchy, StepStatus.Processing, progress);
                });

                var root = await _hierarchyBuilder.BuildAsync(configuration);
                ReportProgress(GenerationStep.Hierarchy, StepStatus.Succeeded);
                return root;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while building the hierarchy.");
                ReportProgress(GenerationStep.Hierarchy, StepStatus.Failed, error: ex);
                throw;
            }
        }

        private async Task DrawImagesAsync(Root root, CancellationToken cancellationToken)
        {
            if (_imageComposerProcessor is null || root.GetDocuments().All(d => string.IsNullOrWhiteSpace(d.ImageComposer)))
            {
                ReportProgress(GenerationStep.Images, StepStatus.Skipped);
                return;
            }

            ReportProgress(GenerationStep.Images, StepStatus.Processing);

            try
            {
                _imageComposerProcessor.ProgressResolution = ProgressResolution;
                _imageComposerProcessor.Progress = new Progress<DocumentDrawerManagerProgress>(progress =>
                {
                    ReportProgress(GenerationStep.Images, StepStatus.Processing, progress);
                });

                await _imageComposerProcessor.DrawImagesAsync(root);
                ReportProgress(GenerationStep.Images, StepStatus.Succeeded);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while drawing images.");
                ReportProgress(GenerationStep.Images, StepStatus.Failed, error: ex);
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
                ReportProgress(GenerationStep.Package, StepStatus.Skipped);
                return;
            }

            ReportProgress(GenerationStep.Package, StepStatus.Processing);

            try
            {
                await packager.ProcessAsync(root, outputFolderPath, configuration.RenderResolutionDPI);
                ReportProgress(GenerationStep.Package, StepStatus.Succeeded);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while packaging the output.");
                ReportProgress(GenerationStep.Package, StepStatus.Failed, error: ex);
                throw;
            }
        }

        private void ReportProgress(GenerationStep step, StepStatus status, object? data = null, Exception? error = null)
        {
            Progress?.Report(new GenerationProgress
            {
                Step = step,
                Status = status,
                Data = data,
                Error = error
            });
        }
    }
}
