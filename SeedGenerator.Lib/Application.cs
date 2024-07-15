using Microsoft.Extensions.Configuration;
using SeedGenerator.Lib.Exceptions;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Param;
using SeedGenerator.Lib.Param.FieldParams;

namespace SeedGenerator.Lib
{
    public class Application
    {
        private readonly IConfiguration _configuration;
        private readonly IRootBuilder _rootBuilder;
        private readonly IPackager _packager;
        private readonly IImageComposerProcessor _imageComposerProcessor;

        public Application(IConfiguration configuration, IRootBuilder rootBuilder, IPackager packager, IImageComposerProcessor imageComposerProcessor)
        {
            _configuration = configuration;
            _rootBuilder = rootBuilder;
            _packager = packager;
            _imageComposerProcessor = imageComposerProcessor;
        }

        public async Task Run(string paramFilePath, string outputPath)
        {
            //Param reading
            RootParam param = await ParamReader.ReadParamAsync(paramFilePath);
            ParamReader.CheckParam(param);

            //Data generation
            var root = _rootBuilder.Build(param);

            //Images generation
            if(_imageComposerProcessor is not null)
                await _imageComposerProcessor.ProcessAsync(root);

            //Seed files generation
            if(_packager is not null)
                await _packager.ProcessAsync(root, outputPath);
        }

        

        
    }
}
