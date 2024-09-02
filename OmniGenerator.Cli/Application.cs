using Microsoft.Extensions.Configuration;
using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Interfaces.Infrastructure;
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
            var progress = new Progress<ProgressReport>(pr => 
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(pr); 
            });

            //Configuration reading
            var config = await ConfigurationReader.ReadConfigurationAsync(configurationFilePath);
            ConfigurationReader.CheckConfiguration(config);

            //Data generation
            var root = _hierarchyBuilder.Build(config, progress);

            //Images generation
            if (_imageComposerProcessor is not null)
                await _imageComposerProcessor.DrawImagesAsync(root);

            //Files generation
            var packager = _pluginService.GetPackager(config.PackagerName);
            if (packager is not null)
                await packager.ProcessAsync(root, outputPath);
        }
    }
}
