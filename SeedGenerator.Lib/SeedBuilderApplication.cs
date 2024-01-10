using AutoMapper;
using Microsoft.Extensions.Configuration;
using SeedGenerator.Lib.DataGenerators;
using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Param;
using SeedGenerator.Lib.Param.Serialization;
using SeedGenerator.Lib.Tools;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SeedGenerator.Lib
{
    public class SeedBuilderApplication
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IPackager _packager;
        private readonly IImageComposerProcessor _imageComposerProcessor;

        public SeedBuilderApplication(IConfiguration configuration, IMapper mapper, IPackager packager, IImageComposerProcessor imageComposerProcessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _packager = packager;
            _imageComposerProcessor = imageComposerProcessor;
        }

        public async Task Run(string paramFilePath, string outputPath)
        {
            var param = await RootParam.FromFileAsync(paramFilePath);

            if (param is not null)
            {
                //Data generation
                var root = new DataGenerator(param, _mapper).Process();

                //Images generation
                await _imageComposerProcessor.ProcessAsync(root);

                //Seed files generation
                await _packager.ProcessAsync(root, outputPath);
            }
        }
    }
}
