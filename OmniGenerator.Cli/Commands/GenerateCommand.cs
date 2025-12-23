using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OmniGenerator.Cli.Tools;
using OmniGenerator.Cli.Widgets;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Drawers;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using OmniGenerator.Lib.Orchestration;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;
using System.Diagnostics;

namespace OmniGenerator.Cli.Commands
{

    /// <summary>
    /// CLI command responsible for executing the document and image generation process
    /// based on a provided configuration file and command-line settings.
    /// This command provides real-time progress updates in the console and delegates
    /// the business logic to the <see cref="IGenerationOrchestrator"/>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GenerateCommand"/> class.
    /// </remarks>
    /// <param name="appsettings">Application-level configuration settings.</param>
    /// <param name="orchestrator">Orchestrator responsible for the generation pipeline.</param>
    /// <param name="logger">Logger instance for this command.</param>
    internal class GenerateCommand(
        IOptions<AppSettings> appsettings,
        IGenerationOrchestrator orchestrator,
        ILogger<GenerateCommand> logger
        ) : AsyncCommand<GenerateCommandSettings>
    {
        private readonly int DEFAULT_PROGRESS_RESOLUTION = 1000; // 1 second

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

        private readonly AppSettings _appsettings = appsettings.Value;

        private Exception? _currentError;

        /// <summary>
        /// Executes the generate command asynchronously, reading input settings,
        /// building the hierarchy, generating images, and packaging output.
        /// Displays real-time UI updates based on orchestrator progress events.
        /// </summary>
        /// <param name="context">The Spectre CLI command context.</param>
        /// <param name="settings">Command-line arguments parsed into settings.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>0 if successful, -1 if an error occurred.</returns>
        public override async Task<int> ExecuteAsync(CommandContext context, GenerateCommandSettings settings, CancellationToken ct)
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
                        try
                        {
                            orchestrator.ProgressResolution = _appsettings.ProgressResolution ?? DEFAULT_PROGRESS_RESOLUTION;
                            orchestrator.Progress = new Progress<GenerationProgress>(progress =>
                            {
                                HandleProgress(settings, ldc, progress);
                            });

                            await orchestrator.ExecuteAsync(generatorConfig, settings.OutputFolderPath, ct);
                        }
                        catch (Exception ex)
                        {
                            _currentError = ex;
                            UpdateUI(settings, new Markup(string.Empty));
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

        /// <summary>
        /// Updates the console UI with the current progress and status of each generation step.
        /// </summary>
        /// <param name="settings">The current command settings.</param>
        /// <param name="progress">A renderable progress widget.</param>
        private void UpdateUI(GenerateCommandSettings settings, IRenderable progress)
        {
            _layout.UpdateCell(0, 0,
                new Panel(new TextPath(Path.GetFullPath(settings.SettingsFilePath)).LeafColor(Color.Red))
                    .ConfigurePanel("Configuration")
            );

            _layout.UpdateCell(1, 0,
                new Panel(_tasks)
                    .ConfigurePanel("Tasks")
            );

            _layout.UpdateCell(2, 0,
                progress
            );

            var executionContent = _currentError is null
                ? new Markup($"[blue]Elapsed time[/] : {_watch.Elapsed.ToFluidUnitString()}")
                : new Markup($"[blue]Elapsed time[/] : {_watch.Elapsed.ToFluidUnitString()}\n[red]Error:[/] {_currentError.Message.EscapeMarkup()}");

            _layout.UpdateCell(3, 0,
                new Panel(executionContent)
                    .ConfigurePanel("Execution")
            );
        }

        /// <summary>
        /// Handles progress events from the orchestrator and updates the UI accordingly.
        /// </summary>
        /// <param name="settings">The current command settings.</param>
        /// <param name="ldc">The live display context for UI updates.</param>
        /// <param name="progress">Progress information from the orchestrator.</param>
        private void HandleProgress(GenerateCommandSettings settings, LiveDisplayContext ldc, GenerationProgress progress)
        {
            var taskKey = progress.Step switch
            {
                GenerationStep.Hierarchy => "hierarchy",
                GenerationStep.Images => "images",
                GenerationStep.Package => "package",
                _ => throw new ArgumentOutOfRangeException()
            };

            switch (progress.Status)
            {
                case StepStatus.Processing:
                    _tasks[taskKey].SetProcessing();
                    if (progress.Data is HierarchyBuilderProgress hbProgress)
                    {
                        UpdateUI(settings, hbProgress.ToWidget());
                    }
                    else if (progress.Data is DocumentDrawerManagerProgress ddmProgress)
                    {
                        UpdateUI(settings, ddmProgress.ToWidget());
                    }
                    break;

                case StepStatus.Succeeded:
                    _tasks[taskKey].SetSucceeded();
                    break;

                case StepStatus.Failed:
                    _tasks[taskKey].SetFailed();
                    if (progress.Error is not null)
                    {
                        _currentError = progress.Error;
                    }
                    break;

                case StepStatus.Skipped:
                    _tasks[taskKey].SetSkipped();
                    break;
            }

            ldc.Refresh();
        }
    }
}