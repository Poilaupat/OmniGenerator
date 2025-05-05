using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using OmniGenerator.Lib.Tools;
using Spectre.Console.Cli;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// CLI command responsible for executing a document/image generation process
    /// based on a provided configuration file and command-line settings.
    /// </summary>
    internal class GenerateCommand : CancellableAsyncCommand<GenerateCommandSettings>
    {
        private readonly AppSettings _appeettings;
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
            _appeettings = appsettings.Value;
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
                await GenerateOne(settings);
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
        private async Task GenerateOne(GenerateCommandSettings settings)
        {
            // Step 1: Configuration reading and validation
            var generationConfig = await ConfigurationReader.ReadConfigurationAsync(settings.SettingsFilePath);
            ConfigurationReader.CheckConfiguration(generationConfig);

            // Step 2: Data hierarchy generation
            _hierarchyBuilder.ProgressResolution = _appeettings.ProgressResolution ?? 1000;
            _hierarchyBuilder.Progress = new Progress<HierarchyBuilderProgressReport>(pr =>
            {
                ConsoleWriter.WriteLine(pr);
            });
            var root = await _hierarchyBuilder.BuildAsync(generationConfig);
                

            // Step 3: Image generation
            ConsoleWriter.WriteLine("Starting image generation");
            if (_imageComposerProcessor is not null)
                await _imageComposerProcessor.DrawImagesAsync(root);
            ConsoleWriter.WriteLine("Image generation finished");

            // Step 4: Output packaging
            ConsoleWriter.WriteLine("Starting packet generation");
            var packager = _pluginService.GetPackager(generationConfig.PackagerName);
            if (packager is not null)
                await packager.ProcessAsync(root, settings.OutputFolderPath);
            ConsoleWriter.WriteLine("Packet generation finished");
        }
    }
}
