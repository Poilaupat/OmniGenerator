using Microsoft.Extensions.Logging;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Hierarchy.Interfaces;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Orchestration.Interfaces;
using OmniGenerator.Lib.Renderers.Interfaces;
using OmniGenerator.Lib.Reporting;

namespace OmniGenerator.Lib.Orchestration
{
    public class GenerationOrchestrator : IGenerationOrchestrator
    {
        public const string DefaultJobId = "generate-one";

        private readonly IPluginService _pluginService;
        private readonly IHierarchyBuilder _hierarchyBuilder;
        private readonly IDocumentRendererManager _imageRendererProcessor;
        private readonly IProgressHub<GenerationProgress> _hub;
        private readonly ILogger<GenerationOrchestrator> _logger;

        public GenerationOrchestrator(
            IPluginService pluginService,
            IHierarchyBuilder hierarchyBuilder,
            IDocumentRendererManager imageRendererProcessor,
            IProgressHub<GenerationProgress> hub,
            ILogger<GenerationOrchestrator> logger)
        {
            _pluginService = pluginService;
            _hierarchyBuilder = hierarchyBuilder;
            _imageRendererProcessor = imageRendererProcessor;
            _hub = hub;
            _logger = logger;
        }

        public async Task<Root> ExecuteAsync(
            OmniGeneratorConfiguration configuration,
            string outputFolderPath,
            CancellationToken cancellationToken = default,
            string jobId = DefaultJobId)
        {
            var root = await BuildHierarchyAsync(configuration, jobId, cancellationToken);
            await RenderImagesAsync(root, jobId, cancellationToken);
            await PackageAsync(root, configuration, outputFolderPath, jobId, cancellationToken);

            return root;
        }

        private async Task<Root> BuildHierarchyAsync(
            OmniGeneratorConfiguration configuration,
            string jobId,
            CancellationToken cancellationToken)
        {
            Report(jobId, GenerationStep.Hierarchy, StepStatus.Processing);

            try
            {
                var root = await _hierarchyBuilder.BuildAsync(configuration);
                Report(jobId, GenerationStep.Hierarchy, StepStatus.Succeeded);
                return root;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while building the hierarchy.");
                Report(jobId, GenerationStep.Hierarchy, StepStatus.Failed, ex);
                throw;
            }
        }

        private async Task RenderImagesAsync(Root root, string jobId, CancellationToken cancellationToken)
        {
            if (_imageRendererProcessor is null || root.GetAllDocuments().All(d => string.IsNullOrWhiteSpace(d.ImageComposer)))
            {
                Report(jobId, GenerationStep.Images, StepStatus.Skipped);
                return;
            }

            Report(jobId, GenerationStep.Images, StepStatus.Processing);

            try
            {
                await _imageRendererProcessor.RenderImagesAsync(root);
                Report(jobId, GenerationStep.Images, StepStatus.Succeeded);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while rendering images.");
                Report(jobId, GenerationStep.Images, StepStatus.Failed, ex);
                throw;
            }
        }

        private async Task PackageAsync(
            Root root,
            OmniGeneratorConfiguration configuration,
            string outputFolderPath,
            string jobId,
            CancellationToken cancellationToken)
        {
            var packager = _pluginService.GetPlugin<IPackager>(configuration.PackagerName);
            if (packager is null)
            {
                Report(jobId, GenerationStep.Package, StepStatus.Skipped);
                return;
            }

            Report(jobId, GenerationStep.Package, StepStatus.Processing);

            try
            {
                await packager.ProcessAsync(root, outputFolderPath, configuration.RenderResolutionDPI);
                Report(jobId, GenerationStep.Package, StepStatus.Succeeded);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while packaging the output.");
                Report(jobId, GenerationStep.Package, StepStatus.Failed, ex);
                throw;
            }
        }

        private void Report(string jobId, GenerationStep step, StepStatus status, Exception? error = null)
        {
            _hub.Report(jobId, new GenerationProgress
            {
                Step = step,
                Status = status,
                Error = error
            });
        }
    }
}
