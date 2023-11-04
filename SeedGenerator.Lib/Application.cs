using AutoMapper;
using Microsoft.Extensions.Configuration;
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

            //PacketParam pp = new PacketParam();
            //pp.FieldParams.Add(new FieldParamRegex { Name = "numlot", Pattern = "\\d{9}" });
            //pp.RootParams.Name = "root";
            //pp.RootParams.MinOccurs = 15;
            //pp.RootParams.MaxOccurs = 25;
            //var remittance = new DocumentParam { Name = "slip", MinOccurs = 1, MaxOccurs = 1 };
            //remittance.FieldParams.Add(new FieldParamRegex { Name = "numremise", Pattern = "\\d{7}" });
            //pp.RootParams.Elements.Add(remittance);
            //var grpRemittance = new GroupParam { Name = "remittance", MinOccurs = 1, MaxOccurs = 1 };
            //var coupon = new DocumentParam { Name = "coupon", MinOccurs = 1, MaxOccurs = 3 };
            //coupon.FieldParams.Add(new FieldParamRegex { Name = "numcoupon", Pattern = "\\d{8} \\d{25} \\d{3}" });
            //var cheque = new DocumentParam { Name = "cheque", MinOccurs = 1, MaxOccurs = 3 };
            //cheque.FieldParams.Add(new FieldParamRegex { Name = "cmc7", Pattern = "\\d{7} \\d{9}908 \\d{12}" });
            //grpRemittance.Elements.Add(coupon);
            //grpRemittance.Elements.Add(cheque);
            //pp.RootParams.Elements.Add(grpRemittance);

            //string result = JsonSerializer.Serialize(pp, options);

            //using (var fs = new FileStream(@"C:\Users\Ruben\source\repos\SeedGenerator\ParamFiles\param.json", FileMode.Open, FileAccess.Read, FileShare.Read))
            string content = File.ReadAllText(@"C:\Users\Ruben\source\repos\SeedGenerator\ParamFiles\param.json");
            {
                PacketParam? packetParam = JsonSerializer.Deserialize<PacketParam>(content, options);

                //if (packetParam is not null)
                //{
                //    var generators = new FieldGeneratorCollection(_mapper.Map<List<FieldGeneratorBase>>(packetParam.FieldParams));

                //    var docs = new List<Document>();
                //    for (int i = 0; i < 100; i++)
                //    {
                //        docs.Add(new Document(generators));
                //    }
                //}
            }
            await Task.CompletedTask;
        }
    }
}
