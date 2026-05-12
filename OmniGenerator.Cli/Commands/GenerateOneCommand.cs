using Microsoft.Extensions.Logging;
using OmniGenerator.Cli.Tools;
using OmniGenerator.Cli.Widgets;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Orchestration;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;
using System.Diagnostics;
using OmniGenerator.Lib.Orchestration.Interfaces;
using OmniGenerator.Lib.Reporting;
using Microsoft.Extensions.Configuration;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// CLI command responsible for executing the document and image generation process
    /// based on a provided configuration file and command-line settings.
    /// This command provides real-time progress updates in the console and delegates
    /// the business logic to the <see cref="IGenerationOrchestrator"/>.
    /// </summary>
    internal class GenerateOneCommand(
        IGenerationOrchestrator orchestrator,
        IProgressHub<GenerationProgress> generationHub,
        IProgressHub<HierarchyBuildingProgress> hierarchyHub,
        IProgressHub<RenderingProgress> renderingHub,
        IConfiguration configuration,
        ILogger<GenerateOneCommand> logger
        ) : AsyncCommand<GenerateOneCommandSettings>
    {
        private readonly int _uiResolutionMs = configuration.GetValue<int>("AppSettings:progress-resolution");

        private readonly Table _layout = new Table()
                .Border(TableBorder.None)
                .AddColumns("column")
                .HideHeaders()
                .AddEmptyRow()
                .AddEmptyRow()
                .AddEmptyRow()
                .AddEmptyRow();

        private readonly TaskList _tasks = new TaskList()
                .AddTask(new TaskItem("hierarchy", new Markup("[blue]Data Generation[/]")))
                .AddTask(new TaskItem("images", new Markup("[blue]Vector images Generation[/]")))
                .AddTask(new TaskItem("package", new Markup("[blue]Package Generation[/]")));

        private readonly Stopwatch _watch = new();
        private DateTime _lastUiRefresh = DateTime.MinValue;

        private Exception? _error;
        private HierarchyBuildingProgress? _hierarchyProgress;
        private RenderingProgress? _imageProgress;

        public override async Task<int> ExecuteAsync(CommandContext context, GenerateOneCommandSettings settings, CancellationToken ct)
        {
            try
            {
                _watch.Start();

                var generatorConfig = await ConfigurationReader.ReadConfigurationAsync(settings.SettingsFilePath);
                ConfigurationReader.CheckConfiguration(generatorConfig);

                await AnsiConsole
                    .Live(_layout)
                    .AutoClear(false)
                    .Overflow(VerticalOverflow.Crop)
                    .StartAsync(async ldc =>
                    {
                        generationHub.DataChanged += (_, progress) => HandleGenerationProgress(settings, ldc, progress);
                        hierarchyHub.DataChanged += (_, progress) => { _hierarchyProgress = progress; ThrottledRefresh(settings, ldc); };
                        renderingHub.DataChanged += (_, progress) => { _imageProgress = progress; ThrottledRefresh(settings, ldc); };

                        try
                        {
                            await orchestrator.ExecuteAsync(generatorConfig, settings.OutputFolderPath, ct);

                            // Final UI refresh to ensure all updates are visible
                            UpdateUI(settings);
                            ldc.Refresh();
                        }
                        catch (Exception ex)
                        {
                            _error = ex;
                            UpdateUI(settings);
                            ldc.Refresh();
                        }
                    });

                _watch.Stop();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during the generation process.");
                return -1;
            }

            return 0;
        }

        private void ThrottledRefresh(GenerateOneCommandSettings settings, LiveDisplayContext ldc)
        {
            var now = DateTime.UtcNow;
            if ((now - _lastUiRefresh).TotalMilliseconds < _uiResolutionMs) return;
            _lastUiRefresh = now;
            UpdateUI(settings);
            ldc.Refresh();
        }

        private void HandleGenerationProgress(GenerateOneCommandSettings settings, LiveDisplayContext ldc, GenerationProgress progress)
        {
            var taskKey = progress.Step switch
            {
                GenerationStep.Hierarchy => "hierarchy",
                GenerationStep.Images => "images",
                GenerationStep.Package => "package",
                _ => throw new ArgumentOutOfRangeException($"Unknown step : {progress.Step}")
            };

            switch (progress.Status)
            {
                case StepStatus.Processing:
                    _tasks[taskKey].SetProcessing();
                    break;

                case StepStatus.Succeeded:
                    _tasks[taskKey].SetSucceeded();
                    break;

                case StepStatus.Failed:
                    _tasks[taskKey].SetFailed();
                    if (progress.Error is not null)
                        _error = progress.Error;
                    break;

                case StepStatus.Skipped:
                    _tasks[taskKey].SetSkipped();
                    break;
            }

            UpdateUI(settings);
            ldc.Refresh();
        }

        private void UpdateUI(GenerateOneCommandSettings settings)
        {
            _layout.UpdateCell(0, 0,
                new Panel(new TextPath(Path.GetFullPath(settings.SettingsFilePath)).LeafColor(Color.Red))
                    .ConfigurePanel("Configuration")
            );

            _layout.UpdateCell(1, 0,
                new Panel(_tasks)
                    .ConfigurePanel("Tasks")
            );

            var progressWidgets = new List<IRenderable>();

            if (_hierarchyProgress is not null)
                progressWidgets.Add(_hierarchyProgress.ToWidget());

            if (_imageProgress is not null)
                progressWidgets.Add(_imageProgress.ToWidget());

            IRenderable progressContent = progressWidgets.Count switch
            {
                0 => new Markup(string.Empty),
                1 => progressWidgets[0],
                _ => new Rows(progressWidgets)
            };

            _layout.UpdateCell(2, 0, progressContent);

            var executionContent = _error is null
                ? new Markup($"[blue]Elapsed time[/] : {_watch.Elapsed.ToFluidUnitString()}")
                : new Markup($"[blue]Elapsed time[/] : {_watch.Elapsed.ToFluidUnitString()}\n[red]Error:[/] {_error.Message.EscapeMarkup()}");

            _layout.UpdateCell(3, 0,
                new Panel(executionContent)
                    .ConfigurePanel("Execution")
            );
        }
    }
}
