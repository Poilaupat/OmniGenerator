using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OmniGenerator.Cli.Quartz;
using OmniGenerator.Cli.Tools;
using OmniGenerator.Cli.Widgets;
using OmniGenerator.Lib.Reporting;
using Quartz;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;

namespace OmniGenerator.Cli.Commands
{
    internal sealed class GenerateManyCommand(
        ISchedulerFactory schedulerFactory,
        IProgressHub<JobExecutionStats> progressHub,
        JobStateListener jobStateListener,
        IConfiguration configuration,
        ILogger<GenerateManyCommand> logger)
        : AsyncCommand<GenerateManyCommandSettings>
    {
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        private readonly int _uiResolutionMs = Math.Max(500, configuration.GetValue<int>("AppSettings:progress-resolution"));
        private readonly Stopwatch _watch = new();

        private readonly Table _layout = new Table()
            .Border(TableBorder.None)
            .AddColumns("column")
            .HideHeaders()
            .AddEmptyRow()
            .AddEmptyRow()
            .AddEmptyRow();

        private readonly ConcurrentDictionary<string, DateTimeOffset?> _nextFireTimes = new();

        private SchedulePlan? _plan;
        private IScheduler? _scheduler;
        private GenerateManyCommandSettings? _settings;

        protected override async Task<int> ExecuteAsync(CommandContext context, GenerateManyCommandSettings settings, CancellationToken cancellationToken)
        {
            try
            {
                _watch.Start();
                _settings = settings;

                _plan = await LoadSchedulePlanAsync(settings.SchedulePlanFilePath, cancellationToken);

                _scheduler = await schedulerFactory.GetScheduler(cancellationToken);
                _scheduler.ListenerManager.AddJobListener(jobStateListener);
                await _scheduler.Start(cancellationToken);

                foreach (var entry in _plan.Jobs)
                {
                    await ScheduleJobAsync(_scheduler, entry, cancellationToken);
                    _nextFireTimes[entry.Name] = null;
                    logger.LogInformation("Scheduled job '{JobName}' with cron '{Cron}'.", entry.Name, entry.CronSchedule);
                }

                await AnsiConsole
                    .Live(_layout)
                    .AutoClear(false)
                    .Overflow(VerticalOverflow.Crop)
                    .StartAsync(async ldc =>
                    {
                        using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(_uiResolutionMs));
                        try
                        {
                            while (await timer.WaitForNextTickAsync(cancellationToken))
                            {
                                RefreshNextFireTimes();
                                UpdateUI();
                                ldc.Refresh();
                            }
                        }
                        catch (OperationCanceledException) { }
                    });

                return 0;
            }
            catch (OperationCanceledException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while scheduling generation jobs.");
                return 1;
            }
            finally
            {
                _watch.Stop();
            }
        }

        private void RefreshNextFireTimes()
        {
            if (_plan is null) return;

            foreach (var entry in _plan.Jobs)
            {
                try
                {
                    var cron = new CronExpression(entry.CronSchedule);
                    _nextFireTimes[entry.Name] = cron.GetNextValidTimeAfter(DateTimeOffset.UtcNow);
                }
                catch
                {
                    _nextFireTimes[entry.Name] = null;
                }
            }
        }

        private void UpdateUI()
        {
            if (_plan is null || _settings is null) return;

            _layout.UpdateCell(0, 0,
                new Panel(new TextPath(Path.GetFullPath(_settings.SchedulePlanFilePath)).LeafColor(Color.Red))
                    .ConfigurePanel("Configuration")
            );

            _layout.UpdateCell(1, 0,
                new Panel(BuildJobsTable())
                    .ConfigurePanel("Jobs")
            );

            var executionLines = new List<string>
            {
                $"[blue]Elapsed time[/] : {_watch.Elapsed.ToFluidUnitString()}"
            };

            foreach (var (jobName, errorMessage) in jobStateListener.Errors)
                executionLines.Add($"[red]{jobName.EscapeMarkup()}:[/] {errorMessage.EscapeMarkup()}");

            _layout.UpdateCell(2, 0,
                new Panel(new Markup(string.Join("\n", executionLines)))
                    .ConfigurePanel("Execution")
            );
        }

        private Table BuildJobsTable()
        {
            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[blue]Name[/]"))
                .AddColumn(new TableColumn("[blue]State[/]"))
                .AddColumn(new TableColumn("[blue]Paths[/]"))
                .AddColumn(new TableColumn("[blue]Stats[/]"));

            foreach (var job in _plan!.Jobs)
            {
                IRenderable stateCell;
                if (jobStateListener.IsRunning(job.Name))
                {
                    stateCell = new Markup("[yellow]:gear:  Running[/]");
                }
                else
                {
                    _nextFireTimes.TryGetValue(job.Name, out var nextFire);
                    stateCell = nextFire.HasValue
                        ? new Markup($"[grey]:hourglass_not_done:  Idle[/]\nNext: {nextFire.Value.ToLocalTime():HH:mm:ss}")
                        : new Markup("[grey]Idle[/]");
                }

                var pathsMarkup = $"{TruncatePath(job.SettingsFilePath).EscapeMarkup()}\n[grey]{TruncatePath(job.OutputFolderPath).EscapeMarkup()}[/]";

                progressHub.TryGetLatest(job.Name, out var progress);
                IRenderable statsCell = progress is null
                    ? new Markup("[grey]-[/]")
                    : BuildStatsCell(progress);

                table.AddRow(
                    new Markup(job.Name.EscapeMarkup()),
                    stateCell,
                    new Markup(pathsMarkup),
                    statsCell
                );
            }

            return table;
        }

        private static IRenderable BuildStatsCell(JobExecutionStats progress)
        {
            var elapsed = DateTime.UtcNow - progress.StartTime;
            var dpm = elapsed.TotalMinutes > 0
                ? (int)(progress.DocumentCount / elapsed.TotalMinutes)
                : 0;

            return new Markup(
                $"[blue]Batches[/] {progress.BatchCount}   [blue]Docs[/] {progress.DocumentCount}   [blue]Speed[/] {dpm} dpm"
            );
        }

        private static string TruncatePath(string path, int maxLength = 45)
        {
            var full = Path.GetFullPath(path);
            return full.Length <= maxLength ? full : $"…{full[^(maxLength - 1)..]}";
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
