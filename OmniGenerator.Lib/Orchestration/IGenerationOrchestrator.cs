using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Interfaces.Infrastructure;

namespace OmniGenerator.Lib.Orchestration
{
    public interface IGenerationOrchestrator : INotifier<GenerationProgress>
    {
        Task<Root> ExecuteAsync(
            OmniGeneratorConfiguration configuration,
            string outputFolderPath,
            CancellationToken cancellationToken = default);
    }
}
