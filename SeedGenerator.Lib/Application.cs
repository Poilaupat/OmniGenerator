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
            RootParam param = await ReadParamAsync(paramFilePath);
            ParamTools.CheckParam(param);

            //Data generation
            var root = _rootBuilder.Build(param);

            //Images generation
            await _imageComposerProcessor.ProcessAsync(root);

            //Seed files generation
            await _packager.ProcessAsync(root, outputPath);
        }

        private async Task<RootParam> ReadParamAsync(string paramFilePath)
        {
            try
            {
                var param = await ParamTools.ReadParamFromFileAsync<RootParam>(paramFilePath);

                foreach (var elementParam in param.RootGroupParam.GetElementParams(true))
                {
                    if (!string.IsNullOrWhiteSpace(elementParam.FieldConfigurationFile))
                    {
                        var directory = Path.GetDirectoryName(paramFilePath);
                        var filefields = await ParamTools.ReadParamFromFileAsync<List<FieldParamBase>>(directory, elementParam.FieldConfigurationFile);
                        elementParam.MergeFields(filefields);
                    }
                }

                return param;
            }
            catch (ParamException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        
    }
}
