using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.ProgramSynthesis.Utils;
using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Data.Generators;
using SeedGenerator.Lib.Param;
using SeedGenerator.Lib.Param.FieldParams;
using SeedGenerator.Lib.Param.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SeedGenerator.Lib
{
    public class Application
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public Application(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task Run(string[] args)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                TypeInfoResolver = new PolymorphicTypeResolver(),
            };

            string jsonparam = await File.ReadAllTextAsync(@"C:\Users\Ruben\source\repos\SeedGenerator\ParamFiles\param.json");
            PacketParam? param = JsonSerializer.Deserialize<PacketParam>(jsonparam, options);


            if (param is not null)
            {
                var generator = new PacketGenerator(param, _mapper);
                var packet = generator.GeneratePacket();
            }
        }
    }
}
