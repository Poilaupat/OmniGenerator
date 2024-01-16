using Microsoft.Extensions.Configuration;
using SeedGenerator.Lib.Exceptions;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Param;

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
            RootParam param;
            try
            {
                param = await ParamTools.GetParamFromFileAsync(paramFilePath);               
            }
            catch (ParamException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            //Data generation
            var root = _rootBuilder.Build(param);

            //Images generation
            await _imageComposerProcessor.ProcessAsync(root);

            //Seed files generation
            await _packager.ProcessAsync(root, outputPath);
        }
    }
}
