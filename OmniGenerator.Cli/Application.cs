using Microsoft.Extensions.Configuration;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Tools;

namespace OmniGenerator.Cli
{
    public class Application
    {
        private readonly IConfiguration _configuration;
        private readonly IHierarchyBuilder _hierarchyBuilder;
        private readonly IDocumentDrawerManager _imageComposerProcessor;
        private readonly IPluginService _pluginService;

        public Application(IConfiguration configuration, IPluginService pluginService, IHierarchyBuilder hierarchyBuilder, IDocumentDrawerManager imageComposerProcessor)
        {
            _configuration = configuration;
            _pluginService = pluginService;
            _hierarchyBuilder = hierarchyBuilder;
            _imageComposerProcessor = imageComposerProcessor;
        }

        public async Task Run(string configurationFilePath, string outputPath)
        {
            //Configuration reading
            var generationConfig = await ConfigurationReader.ReadConfigurationAsync(configurationFilePath);
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
                await packager.ProcessAsync(root, outputPath);
            ConsoleWriter.WriteLine("Packet generation finished");
        }
    }
}
