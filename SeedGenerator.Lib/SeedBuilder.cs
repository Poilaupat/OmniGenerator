using AutoMapper;
using Microsoft.Extensions.Configuration;
using SeedGenerator.Lib.Builders;
using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Param;
using SeedGenerator.Lib.Param.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SeedGenerator.Lib
{
    public class SeedBuilder
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IPackager _packager;
        
        private PacketParam? _param;

        public SeedBuilder(IConfiguration configuration, IMapper mapper, IPackager packager)
        {
            _configuration = configuration;
            _mapper = mapper;
            _packager = packager;
        }

        public async Task LoadParam(string paramFilePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                TypeInfoResolver = new PolymorphicTypeResolver(),
            };

            string jsonparam = await File.ReadAllTextAsync(paramFilePath);
            _param = JsonSerializer.Deserialize<PacketParam>(jsonparam, options);
        }

        public async Task BuildSeed(string outputPath)
        {
            var packetData = GenerateSeedData();
            await _packager.GenerateFilesAsync(packetData, outputPath);
        }

        private PacketData GenerateSeedData()
        {
            if (_param is null)
                throw new NullReferenceException($"No param was provided");


            var packetDataBuilder = new PacketDataGenerator(_param, _mapper);
            return packetDataBuilder.GeneratePacketData();
        }
    }
}
