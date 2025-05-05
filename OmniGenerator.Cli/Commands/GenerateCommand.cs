using OmniGenerator.Lib.Interfaces.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Tools;
using Microsoft.Extensions.Logging;

namespace OmniGenerator.Cli.Commands
{
    internal class GenerateCommand : CancellableAsyncCommand<GenerateCommandSettings>
    {
        private readonly CliAppSettings _configuration;
        private readonly IHierarchyBuilder _hierarchyBuilder;
        private readonly IDocumentDrawerManager _imageComposerProcessor;
        private readonly IPluginService _pluginService;
        private readonly ILogger _logger;

        public GenerateCommand(
            IOptions<CliAppSettings> configuration,
            IPluginService pluginService,
            IHierarchyBuilder hierarchyBuilder,
            IDocumentDrawerManager imageComposerProcessor,
            ILogger logger)
        {
            _configuration = configuration.Value;
            _pluginService = pluginService;
            _hierarchyBuilder = hierarchyBuilder;
            _imageComposerProcessor = imageComposerProcessor;
            _logger = logger;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, GenerateCommandSettings settings, CancellationToken ct)
        {
            try
            {
                await GenerateOne(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"beurk");
                return -1;
            }

            return 0;
        }

        private async Task GenerateOne(GenerateCommandSettings settings)
        {
            //Configuration reading
            var generationConfig = await ConfigurationReader.ReadConfigurationAsync(settings.SettingsFilePath);
            ConfigurationReader.CheckConfiguration(generationConfig);

            //Data generation
            var root = await _hierarchyBuilder.BuildAsync(
                generationConfig,
                new Progress<HierarchyBuilderProgressReport>(pr =>
                {
                    ConsoleWriter.WriteLine(pr);
                }));

            //Images generation
            ConsoleWriter.WriteLine("Starting image generation");
            if (_imageComposerProcessor is not null)
                await _imageComposerProcessor.DrawImagesAsync(root);
            ConsoleWriter.WriteLine("Image generation finished");

            //Files generation
            ConsoleWriter.WriteLine("Starting packet generation");
            var packager = _pluginService.GetPackager(generationConfig.PackagerName);
            if (packager is not null)
                await packager.ProcessAsync(root, settings.OutputFolderPath);
            ConsoleWriter.WriteLine("Packet generation finished");
        }
    }
}
