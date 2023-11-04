using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.ProgramSynthesis.Utils;
using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Data.Fields.Generators;
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
                Dictionary<string, FieldGeneratorCollection> generators = param
                    .RootParams
                    .GetAllDocuments()
                    .ToDictionary(x => x.Name, y => new FieldGeneratorCollection(_mapper.Map<List<FieldGeneratorBase>>(y.FieldParams)));

                generators.Add("packet", new FieldGeneratorCollection(_mapper.Map<List<FieldGeneratorBase>>(param.FieldParams)));

                var packet = new Packet(param, generators);
            }
        }
    }
}
