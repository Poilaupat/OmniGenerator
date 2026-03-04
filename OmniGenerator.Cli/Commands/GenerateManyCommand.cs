using Microsoft.Extensions.Logging;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Renderers;
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

namespace OmniGenerator.Cli.Commands
{
    internal sealed class GenerateManyCommand(
        IGenerationOrchestrator orchestrator,
        ISchedulerFactory schedulerFactory,
        ILogger<GenerateManyCommand> logger)
        : AsyncCommand<GenerateManyCommandSettings>
    {
        private readonly Stopwatch _watch = new();

        private Exception? _error;
        private HierarchyBuilderProgress? _hierarchyProgress;
        private DocumentRendererManagerProgress? _imageProgress;

        public override async Task<int> ExecuteAsync(CommandContext context, GenerateManyCommandSettings settings, CancellationToken cancellationToken)
        {
            try
            {
                _watch.Start();

                // Résolution du scheduler via DI. (L'implémentation est configurée dans `ConfigurationModule`.)
                var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

                if (!scheduler.InStandbyMode && !scheduler.IsStarted)
                {
                    await scheduler.Start(cancellationToken);
                }

                var jobKey = new JobKey(nameof(GenerateJob));
                var triggerKey = new TriggerKey($"{nameof(GenerateJob)}-trigger");

                var job = JobBuilder.Create<GenerateJob>()
                    .WithIdentity(jobKey)
                    .UsingJobData(GenerateJob.SettingsFilePathKey, settings.SettingsFilePath)
                    .UsingJobData(GenerateJob.OutputFolderPathKey, settings.OutputFolderPath)
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .WithIdentity(triggerKey)
                    .ForJob(jobKey)
                    .WithCronSchedule(settings.CronSchedule)
                    .StartNow()
                    .Build();

                var existingJob = await scheduler.CheckExists(jobKey, cancellationToken);
                if (existingJob)
                {
                    await scheduler.DeleteJob(jobKey, cancellationToken);
                }

                await scheduler.ScheduleJob(job, trigger, cancellationToken);

                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);

                return 0;
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
