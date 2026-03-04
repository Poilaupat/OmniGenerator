using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Interfaces;

namespace OmniGenerator.Lib.Orchestration.Interfaces
{
    public interface IGenerationOrchestrator : INotifier<GenerationProgress>
    {
        Task<Root> ExecuteAsync(
            OmniGeneratorConfiguration configuration,
            string outputFolderPath,
            CancellationToken cancellationToken = default);
    }
}
