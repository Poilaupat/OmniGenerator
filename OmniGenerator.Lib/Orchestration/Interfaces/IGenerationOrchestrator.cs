using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.Orchestration.Interfaces
{
    public interface IGenerationOrchestrator
    {
        Task<Root> ExecuteAsync(
            OmniGeneratorConfiguration configuration,
            string outputFolderPath,
            CancellationToken cancellationToken = default,
            string jobId = GenerationOrchestrator.DefaultJobId);
    }
}
