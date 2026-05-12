using Microsoft.Extensions.Logging;
using OmniGenerator.Cli.Quartz;
using Quartz;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniGenerator.Lib.Orchestration.Interfaces;
using OmniGenerator.Lib.Reporting;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace OmniGenerator.Cli.Commands
{
    internal sealed class GenerateManyCommand(
        IGenerationOrchestrator orchestrator,
        ISchedulerFactory schedulerFactory,
        IProgressHub<GenerationProgressNew> progressHub,
        ILogger<GenerateManyCommand> logger)
        : AsyncCommand<GenerateManyCommandSettings>
    {
        private readonly Stopwatch _watch = new();

        private Exception? _error;
        private HierarchyBuildingProgress? _hierarchyProgress;
        private RenderingProgress? _imageProgress;

        public override async Task<int> ExecuteAsync(CommandContext context, GenerateManyCommandSettings settings, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting generation (many mode)");
            try
            {
                _watch.Start();

                var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
                await scheduler.Start(cancellationToken);

                var jobKey = new JobKey(nameof(GenerateJob));
                var triggerKey = new TriggerKey($"{nameof(GenerateJob)}-trigger");

                var job = JobBuilder.Create<GenerateJob>()
                    .WithIdentity(jobKey)
                    .UsingJobData(GenerateJob.SettingsFilePathKey, settings.SettingsFilePath)
                    .UsingJobData(GenerateJob.OutputFolderPathKey, settings.OutputFolderPath)
                    .Build();

                job.JobDataMap["progress"] = new GenerationProgressNew
                {
                    StartTime = DateTime.UtcNow,
                    BatchCount = 0,
                };

                var trigger = TriggerBuilder.Create()
                    .WithIdentity(triggerKey)
                    .ForJob(jobKey)
                    .WithCronSchedule(settings.CronSchedule)
                    .StartNow()
                    .Build();

                await scheduler.ScheduleJob(job, trigger, cancellationToken);

                // Keeping the command running to allow the scheduled job(s) to execute. Command can be stopped by hitting Ctrl+C.
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);

                return -1; // Not supposed to hit this point. 
            }
            catch (OperationCanceledException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while scheduling the generation job.");
                return -1;
            }
            finally
            {
                _watch.Stop();
            }
        }
    }
}
