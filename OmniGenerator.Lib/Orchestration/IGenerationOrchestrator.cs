using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.Orchestration
{
    public interface IGenerationOrchestrator
    {
        IProgress<GenerationProgress>? Progress { get; set; }
        int ProgressResolution { get; set; }

        Task<Root> ExecuteAsync(
            OmniGeneratorConfiguration configuration,
            string outputFolderPath,
            CancellationToken cancellationToken = default);
    }
}
