using AutoMapper;
using Microsoft.Extensions.Configuration;
using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Data.Fields.Generators;
using SeedGenerator.Lib.Param;
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

            using (var fs = new FileStream(@"C:\Users\Ruben\source\repos\SeedGenerator\ParamFiles\param.json", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                DocumentParam? chequeParam = JsonSerializer.Deserialize<DocumentParam>(fs, options);

                if (chequeParam is not null)
                {
                    var generators = new FieldGeneratorCollection(_mapper.Map<List<FieldGeneratorBase>>(chequeParam.FieldParams));

                    var docs = new List<Document>();
                    for (int i = 0; i < 100; i++)
                    {
                        docs.Add(new Document(generators));
                    }
                }
            }
            await Task.CompletedTask;
        }
    }
}
