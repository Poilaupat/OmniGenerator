using Microsoft.Extensions.Logging;
using OmniGenerator.Cli.Quartz;
using Quartz;
using Spectre.Console.Cli;
using System.Diagnostics;
using System.Text.Json;

namespace OmniGenerator.Cli.Commands
{
    internal sealed class GenerateManyCommand(
        ISchedulerFactory schedulerFactory,
        ILogger<GenerateManyCommand> logger)
        : AsyncCommand<GenerateManyCommandSettings>
    {
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        private readonly Stopwatch _watch = new();

        public override async Task<int> ExecuteAsync(CommandContext context, GenerateManyCommandSettings settings, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting generation (many mode)");
            try
            {
                _watch.Start();

                var plan = await LoadSchedulePlanAsync(settings.SchedulePlanFilePath, cancellationToken);

                var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
                await scheduler.Start(cancellationToken);

                foreach (var entry in plan.Jobs)
                {
                    await ScheduleJobAsync(scheduler, entry, cancellationToken);
                    logger.LogInformation("Scheduled job '{JobName}' with cron '{Cron}'.", entry.Name, entry.CronSchedule);
                }

                logger.LogInformation("{Count} job(s) scheduled. Waiting for executions. Press Ctrl+C to stop.", plan.Jobs.Count);

                // Keep the command running until the user cancels.
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);

                return -1; // Not supposed to hit this point.
            }
            catch (OperationCanceledException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while scheduling generation jobs.");
                return -1;
            }
            finally
            {
                _watch.Stop();
            }
        }

        private static async Task<SchedulePlan> LoadSchedulePlanAsync(string filePath, CancellationToken cancellationToken)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Schedule plan file not found: '{filePath}'.");

            var json = await File.ReadAllTextAsync(filePath, cancellationToken);
            var plan = JsonSerializer.Deserialize<SchedulePlan>(json, _jsonOptions)
                ?? throw new InvalidOperationException($"Failed to deserialize schedule plan from '{filePath}'.");

            var duplicates = plan.Jobs
                .GroupBy(j => j.Name)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicates.Count > 0)
                throw new InvalidOperationException($"Duplicate job names in schedule plan: {string.Join(", ", duplicates)}.");

            return plan;
        }

        private static async Task ScheduleJobAsync(IScheduler scheduler, ScheduledJobEntry entry, CancellationToken cancellationToken)
        {
            var jobKey = new JobKey(entry.Name);
            var triggerKey = new TriggerKey($"{entry.Name}-trigger");

            var job = JobBuilder.Create<GenerateJob>()
                .WithIdentity(jobKey)
                .UsingJobData(GenerateJob.SettingsFilePathKey, entry.SettingsFilePath)
                .UsingJobData(GenerateJob.OutputFolderPathKey, entry.OutputFolderPath)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .ForJob(jobKey)
                .WithCronSchedule(entry.CronSchedule)
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(job, trigger, cancellationToken);
        }
    }
}

