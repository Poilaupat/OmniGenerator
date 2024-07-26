using Microsoft.Extensions.Configuration;
using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Configuration;

namespace OmniGenerator.Lib
{
    public class Application
    {
        private readonly IConfiguration _configuration;
        private readonly IHierarchyBuilder _hierarchyBuilder;
        private readonly IPackager _packager;
        private readonly IImageComposerProcessor _imageComposerProcessor;

        public Application(IConfiguration configuration, IHierarchyBuilder hierarchyBuilder, IPackager packager, IImageComposerProcessor imageComposerProcessor)
        {
            _configuration = configuration;
            _hierarchyBuilder = hierarchyBuilder;
            _packager = packager;
            _imageComposerProcessor = imageComposerProcessor;
        }

        public async Task Run(string configurationFilePath, string outputPath)
        {
            //Configuration reading
            var config = await ConfigurationReader.ReadConfigurationAsync(configurationFilePath);
            ConfigurationReader.CheckConfiguration(config);

            //Data generation
            var root = _hierarchyBuilder.Build(config);

            //Images generation
            if(_imageComposerProcessor is not null)
                await _imageComposerProcessor.ProcessAsync(root);

            //Files generation
            if(_packager is not null)
                await _packager.ProcessAsync(root, outputPath);
        }

        

        
    }
}
