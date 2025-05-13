using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OmniGenerator.Cli.Widgets;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using OmniGenerator.Lib.Tools;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;
using System.Diagnostics;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// CLI command responsible for executing a document/image generation process
    /// based on a provided configuration file and command-line settings.
    /// </summary>
    internal class GenerateCommand : CancellableAsyncCommand<GenerateCommandSettings>
    {
        private Table _layout = new Table()
                .Border(TableBorder.None)
                .AddColumns("column")
                .HideHeaders()
                .AddEmptyRow()
                .AddEmptyRow()
                .AddEmptyRow()
                .AddEmptyRow();

        private TaskList _tasks = new TaskList()
                .AddTask(new TaskItem("hierarchy", new Markup($"[blue]Data Generation[/]")))
                .AddTask(new TaskItem("images", new Markup($"[blue]Vector images Generation[/]")))
                .AddTask(new TaskItem("package", new Markup($"[blue]Package Generation[/]")));

        private Stopwatch _watch = new Stopwatch();

        private readonly AppSettings _appsettings;
        private readonly IHierarchyBuilder _hierarchyBuilder;
        private readonly IDocumentDrawerManager _imageComposerProcessor;
        private readonly IPluginService _pluginService;
        private readonly ILogger<GenerateCommand> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateCommand"/> class.
        /// </summary>
        /// <param name="appsettings">Application-level configuration settings.</param>
        /// <param name="pluginService">Service used to retrieve plugins such as packagers.</param>
        /// <param name="hierarchyBuilder">Service used to build the content hierarchy.</param>
        /// <param name="imageComposerProcessor">Service used to draw/generate images.</param>
        /// <param name="logger">Logger instance for this command.</param>
        /// <param name="baselogger">Logger used by the base cancellable command class.</param>
        public GenerateCommand(
            IOptions<AppSettings> appsettings,
            IPluginService pluginService,
            IHierarchyBuilder hierarchyBuilder,
            IDocumentDrawerManager imageComposerProcessor,
            ILogger<GenerateCommand> logger,
            ILogger<CancellableAsyncCommand> baselogger
        ) : base(baselogger)
        {
            _appsettings = appsettings.Value;
            _pluginService = pluginService;
            _hierarchyBuilder = hierarchyBuilder;
            _imageComposerProcessor = imageComposerProcessor;
            _logger = logger;
        }

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
                _logger.LogError(ex, "An error occurred during the generation process.");
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Core logic of the generation pipeline:
        /// - Reads and validates configuration
        /// - Builds data hierarchy
        /// - Generates images (if applicable)
        /// - Packages the output using the configured packager
        /// </summary>
        /// <param name="settings">Parsed command-line settings provided by the user.</param>
        private async Task GenerateOne(OmniGeneratorConfiguration configuration, GenerateCommandSettings settings)
        {
            //Live component
            await AnsiConsole
                .Live(_layout)
                .AutoClear(false)
                .Overflow(VerticalOverflow.Crop)
                .StartAsync(async liveDisplay =>
                {
                    // Step 1: Data hierarchy generation
                    _tasks["hierarchy"].SetProcessing();
                    _hierarchyBuilder.ProgressResolution = _appsettings.ProgressResolution ?? 1000;
                    _hierarchyBuilder.Progress = new Progress<HierarchyBuilderProgress>(progress =>
                    {
                        UpdateUI(settings, progress);
                        liveDisplay.Refresh();
                    });
                    var root = await _hierarchyBuilder.BuildAsync(configuration);
                    _tasks["hierarchy"].SetSucceeded();


                    // Step 2: Vector images generation
                    _tasks["images"].SetProcessing();
                    if (_imageComposerProcessor is not null)
                        await _imageComposerProcessor.DrawImagesAsync(root);
                    _tasks["images"].SetSucceeded();


                    // Step 3: Output packaging
                    _tasks["package"].SetProcessing();
                    var packager = _pluginService.GetPackager(configuration.PackagerName);
                    if (packager is not null)
                        await packager.ProcessAsync(root, settings.OutputFolderPath);
                    _tasks["package"].SetSucceeded();
                });
        }

        private void UpdateUI(GenerateCommandSettings settings, HierarchyBuilderProgress progress)
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
                progress.ToWidget()
            );

            _layout.UpdateCell(3, 0,
                new Panel(new Markup($"[blue]Elapsed time[/] : {_watch.ElapsedMilliseconds}ms"))
                    .ConfigurePanel("Execution")
            );
        }
    }
}