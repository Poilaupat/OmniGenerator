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

        private GroupParam? _param;

        public SeedBuilderApplication(IConfiguration configuration, IMapper mapper, IPackager packager, IImageComposerProcessor imageComposerProcessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _packager = packager;
            _imageComposerProcessor = imageComposerProcessor;
        }

        public async Task Run(string paramFilePath, string outputPath)
        {
            await LoadParam(paramFilePath);

            if (_param is not null)
            {
                //Packet data generation
                var packetData = new PacketDataGenerator(_param, _mapper).GeneratePacketData();

                //Packet images generation
                await _imageComposerProcessor.ProcessAsync(packetData);

                //Packet files generation
                await _packager.GenerateFilesAsync(packetData, outputPath);
            }
        }

        private async Task LoadParam(string paramFilePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                TypeInfoResolver = new PolymorphicTypeResolver(),
            };

            string jsonparam = await File.ReadAllTextAsync(paramFilePath);
            _param = JsonSerializer.Deserialize<GroupParam>(jsonparam, options);

            if(_param is null)
            {
                throw new Exception("Invalid configuration");
            }
        }
    }
}
