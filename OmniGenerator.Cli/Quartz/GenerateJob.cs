using Microsoft.Extensions.Logging;
using OmniGenerator.Cli.Tools;
using OmniGenerator.Lib.Configuration;
using Quartz;
using OmniGenerator.Lib.Orchestration.Interfaces;
using OmniGenerator.Lib.Reporting;
using System.Text.Json;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Cli.Quartz
{
    internal sealed class GenerateJob(
        IGenerationOrchestrator orchestrator,
        IProgressHub<GenerationProgressNew> progressHub,
        ILogger<GenerateJob> logger) : IJob
    {
        public const string SettingsFilePathKey = "SettingsFilePath";
        public const string OutputFolderPathKey = "OutputFolderPath";

        public async Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation("Starting scheduled generation job '{JobKey}'.", context.JobDetail.Key);

            try
            {
                var data = context.MergedJobDataMap;

                var settingsFilePath = data.GetString(SettingsFilePathKey);
                var outputFolderPath = data.GetString(OutputFolderPathKey);
                var progress = (GenerationProgressNew)data["progress"];

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

                var root  = await orchestrator.ExecuteAsync(generatorConfig, outputFolderPath, context.CancellationToken, context.JobDetail.Key.Name);

                progress.BatchCount++;
                progress.DocumentCount += root.GetAllDocuments().Count();
                progress.GroupCount += root.GetAllGroups().Count();
                progress.FieldCount += root.Fields.Count + root.GetAllGroups().Sum(g => g.Fields.Count) + root.GetAllDocuments().Sum(d => d.Fields.Count);

                context.MergedJobDataMap["progress"] = progress;
                progressHub.Report(context.JobDetail.Key.Name, progress);
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
