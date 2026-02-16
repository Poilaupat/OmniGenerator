using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OmniGenerator.Cli.Tools;
using OmniGenerator.Cli.Widgets;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Renderers;
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
    /// Initializes a new instance of the <see cref="GenerateOneCommand"/> class.
    /// </remarks>
    /// <param name="orchestrator">Orchestrator responsible for the generation pipeline.</param>
    /// <param name="logger">Logger instance for this command.</param>
    internal class GenerateOneCommand(
        IGenerationOrchestrator orchestrator,
        ILogger<GenerateOneCommand> logger
        ) : AsyncCommand<GenerateOneCommandSettings>
    {
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

        private Exception? _error;
        private HierarchyBuilderProgress? _hierarchyProgress;
        private DocumentRendererManagerProgress? _imageProgress;

        /// <summary>
        /// Executes the generate command asynchronously, reading input settings,
        /// building the hierarchy, generating images, and packaging output.
        /// Displays real-time UI updates based on orchestrator progress events.
        /// </summary>
        /// <param name="context">The Spectre CLI command context.</param>
        /// <param name="settings">Command-line arguments parsed into settings.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>0 if successful, -1 if an error occurred.</returns>
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
                        try
                        {
                            orchestrator.Notifier.Progress = new Progress<GenerationProgress>(progress => HandleProgress(settings, ldc, progress));

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

        /// <summary>
        /// Updates the console UI with the current progress and status of each generation step.
        /// </summary>
        /// <param name="settings">The current command settings.</param>
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

            // Build combined progress view
            var progressWidgets = new List<IRenderable>();

            if (_hierarchyProgress is not null)
            {
                progressWidgets.Add(_hierarchyProgress.ToWidget());
            }

            if (_imageProgress is not null)
            {
                progressWidgets.Add(_imageProgress.ToWidget());
            }

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

        /// <summary>
        /// Handles progress events from the orchestrator and updates the UI accordingly.
        /// </summary>
        /// <param name="settings">The current command settings.</param>
        /// <param name="ldc">The live display context for UI updates.</param>
        /// <param name="progress">Progress information from the orchestrator.</param>
        private void HandleProgress(GenerateOneCommandSettings settings, LiveDisplayContext ldc, GenerationProgress progress)
        {
            var taskKey = progress.Step switch
            {
                GenerationStep.Hierarchy => "hierarchy",
                GenerationStep.Images => "images",
                GenerationStep.Package => "package",
                _ => throw new ArgumentOutOfRangeException($"Unknown step : {progress.Step}")
            };

            // Update progress data if provided
            switch (progress.Data)
            {
                case HierarchyBuilderProgress hbp:
                    _hierarchyProgress = hbp;
                    break;
                case DocumentRendererManagerProgress drmp:
                    _imageProgress = drmp;
                    break;
            }

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
                    {
                        _error = progress.Error;
                    }
                    break;

                case StepStatus.Skipped:
                    _tasks[taskKey].SetSkipped();
                    break;
            }

            UpdateUI(settings);
            ldc.Refresh();
        }
    }
}