using Microsoft.Extensions.Configuration;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Tools;
using OmniGenerator.Cli.Options;
using Microsoft.Extensions.Options;

namespace OmniGenerator.Cli
{
    public class OmniGeneratorCliApplication
    {
        private readonly ApplicationSettings _configuration;
        private readonly IHierarchyBuilder _hierarchyBuilder;
        private readonly IDocumentDrawerManager _imageComposerProcessor;
        private readonly IPluginService _pluginService;
        private readonly ICommandLineOptions _options;

        public OmniGeneratorCliApplication(
            IOptions<ApplicationSettings> configuration, 
            IPluginService pluginService, 
            IHierarchyBuilder hierarchyBuilder, 
            IDocumentDrawerManager imageComposerProcessor, 
            ICommandLineOptions options)
        {
            _configuration = configuration.Value;
            _pluginService = pluginService;
            _hierarchyBuilder = hierarchyBuilder;
            _imageComposerProcessor = imageComposerProcessor;
            _options = options;
        }

        public async Task RunAsync()
        {
            //Configuration reading
            var generationConfig = await ConfigurationReader.ReadConfigurationAsync(_options.ParamFilePath);
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
                await packager.ProcessAsync(root, _options.OutputFolderPath);
            ConsoleWriter.WriteLine("Packet generation finished");
        }
    }
}
