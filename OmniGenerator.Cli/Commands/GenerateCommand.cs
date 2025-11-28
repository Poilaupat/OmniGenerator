using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OmniGenerator.Cli.Tools;
using OmniGenerator.Cli.Widgets;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Drawers;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;
using System.Diagnostics;
using static Microsoft.ProgramSynthesis.DslLibrary.Dates.DateFormatCache;

namespace OmniGenerator.Cli.Commands
{

    /// <summary>
    /// CLI command responsible for executing the document and image generation process
    /// based on a provided configuration file and command-line settings.
    /// This command builds the data hierarchy, generates images, and packages the output,
    /// providing real-time progress updates in the console.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GenerateCommand"/> class.
    /// </remarks>
    /// <param name="appsettings">Application-level configuration settings.</param>
    /// <param name="pluginService">Service used to retrieve plugins such as packagers.</param>
    /// <param name="hierarchyBuilder">Service used to build the content hierarchy.</param>
    /// <param name="imageComposerProcessor">Service used to draw/generate images.</param>
    /// <param name="logger">Logger instance for this command.</param>
    internal class GenerateCommand(
        IOptions<AppSettings> appsettings,
        IPluginService pluginService,
        IHierarchyBuilder hierarchyBuilder,
        IDocumentDrawerManager imageComposerProcessor,
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

        /// <summary>
        /// Executes the generate command asynchronously, reading input settings,
        /// building the hierarchy, generating images, and packaging output.
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
                await GenerateOne(generatorConfig, settings);

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
        /// Core logic of the generation pipeline:
        /// - Builds data hierarchy
        /// - Generates images (if applicable)
        /// - Packages the output using the configured packager
        /// </summary>
        /// <param name="configuration">The loaded generator configuration.</param>
        /// <param name="settings">Parsed command-line settings provided by the user.</param>
        private async Task GenerateOne(OmniGeneratorConfiguration configuration, GenerateCommandSettings settings)
        {
            // Live component for real-time progress display
            await AnsiConsole
                .Live(_layout)
                .AutoClear(false)
                .Overflow(VerticalOverflow.Crop)
                .StartAsync(async ldc =>
                {
                    // Step 1: Data hierarchy generation
                    var root = await BuildHierarchyAsync(ldc, settings, configuration);

                    // Step 2: Vector images generation
                    await DrawImagesAsync(ldc, settings, root);

                    // Step 3: Output packaging
                     await PackageAsync(settings, root, configuration);
                });
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

            _layout.UpdateCell(3, 0,
                new Panel(new Markup($"[blue]Elapsed time[/] : {_watch.Elapsed.ToFluidUnitString()}"))
                    .ConfigurePanel("Execution")
            );
        }

        /// <summary>
        /// Builds the document hierarchy asynchronously and updates the UI with progress.
        /// </summary>
        /// <param name="ldc">The live display context for UI updates.</param>
        /// <param name="settings">The current command settings.</param>
        /// <param name="configuration">The generator configuration.</param>
        /// <returns>The generated <see cref="Root"/> hierarchy.</returns>
        private async Task<Root> BuildHierarchyAsync(LiveDisplayContext ldc, GenerateCommandSettings settings, OmniGeneratorConfiguration configuration)
        {
            _tasks["hierarchy"].SetProcessing();
            hierarchyBuilder.ProgressResolution = _appsettings.ProgressResolution ?? DEFAULT_PROGRESS_RESOLUTION;
            hierarchyBuilder.Progress = new Progress<HierarchyBuilderProgress>(progress =>
            {
                UpdateUI(settings, progress.ToWidget());
                ldc.Refresh();
            });

            try
            {
                var root = await hierarchyBuilder.BuildAsync(configuration);
                _tasks["hierarchy"].SetSucceeded();
                return root;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while building the hierarchy.");
                _tasks["hierarchy"].SetFailed();
                throw;
            }
        }

        /// <summary>
        /// Generates images for the documents in the hierarchy, if applicable, and updates the UI with progress.
        /// </summary>
        /// <param name="ldc">The live display context for UI updates.</param>
        /// <param name="settings">The current command settings.</param>
        /// <param name="root">The generated document hierarchy.</param>
        private async Task DrawImagesAsync(LiveDisplayContext ldc, GenerateCommandSettings settings, Root root)
        {
            if (imageComposerProcessor is null || root.GetDocuments().All(d => string.IsNullOrWhiteSpace(d.ImageComposer)))
            {
                _tasks["images"].SetSkipped();
                return;
            }

            _tasks["images"].SetProcessing();

            imageComposerProcessor.ProgressResolution = _appsettings.ProgressResolution ?? DEFAULT_PROGRESS_RESOLUTION;
            imageComposerProcessor.Progress = new Progress<DocumentDrawerManagerProgress>(progress =>
            {
                UpdateUI(settings, progress.ToWidget());
                ldc.Refresh();
            });

            try
            {
                await imageComposerProcessor.DrawImagesAsync(root);
                _tasks["images"].SetSucceeded();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while drawing images.");
                _tasks["images"].SetFailed();
                throw;
            }
        }

        /// <summary>
        /// Packages the generated output using the configured packager plugin and updates the UI with progress.
        /// </summary>
        /// <param name="settings">The current command settings.</param>
        /// <param name="root">The generated document hierarchy.</param>
        /// <param name="configuration">The generator configuration.</param>
        private async Task PackageAsync(GenerateCommandSettings settings, Root root, OmniGeneratorConfiguration configuration)
        {
            var packager = pluginService.GetPlugin<IPackager>(configuration.PackagerName);
            if (packager is null)
            {
                _tasks["package"].SetSkipped();
                return;
            }

            _tasks["package"].SetProcessing();
            try
            {
                await packager.ProcessAsync(root, settings.OutputFolderPath, configuration.RenderResolutionDPI);
                _tasks["package"].SetSucceeded();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while packaging the output.");
                _tasks["package"].SetFailed();
                throw;
            }
        }
    }
}