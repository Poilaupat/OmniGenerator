using Microsoft.Extensions.Logging;
using OmniGenerator.Cli.Tools;
using OmniGenerator.Lib.Configuration;
using Quartz;
using OmniGenerator.Lib.Orchestration.Interfaces;

namespace OmniGenerator.Cli.Quartz
{
    internal sealed class GenerateJob(
        IGenerationOrchestrator orchestrator,
        ILogger<GenerateJob> logger) : IJob
    {
        public const string SettingsFilePathKey = "SettingsFilePath";
        public const string OutputFolderPathKey = "OutputFolderPath";

        public async Task Execute(IJobExecutionContext context)
        {
            var ct = context.CancellationToken;

            try
            {
                var data = context.MergedJobDataMap;

                var settingsFilePath = data.GetString(SettingsFilePathKey);
                var outputFolderPath = data.GetString(OutputFolderPathKey);

                if (string.IsNullOrWhiteSpace(settingsFilePath))
                {
                    throw new JobExecutionException($"Missing job data '{SettingsFilePathKey}'.");
                }

                if (string.IsNullOrWhiteSpace(outputFolderPath))
                {
                    throw new JobExecutionException($"Missing job data '{OutputFolderPathKey}'.");
                }

                var generatorConfig = await ConfigurationReader.ReadConfigurationAsync(settingsFilePath);
                ConfigurationReader.CheckConfiguration(generatorConfig);

                await orchestrator.ExecuteAsync(generatorConfig, outputFolderPath, ct);
            }
            catch (OperationCanceledException)
            {
                // let Quartz treat cancellation as a graceful stop
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during scheduled generation.");
                throw new JobExecutionException(ex, refireImmediately: false);
            }
        }
    }
}
