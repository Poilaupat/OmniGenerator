// See https://aka.ms/new-console-template for more information
using Microsoft.ProgramSynthesis.Transformation.Formula.Build.RuleNodeTypes;
using SeedGenerator.Lib.Document;
using SeedGenerator.Lib.MetaData;
using SeedGenerator.Lib.Serialization;
using System.Text.Json;


Console.WriteLine("Hello, World!");

//ChequeComposer composer = new ChequeComposer(96);
//composer.Compose(new Dictionary<string, string>());

MetaDataGeneratorCollection generators = new MetaDataGeneratorCollection();
generators.Add(new MetaDataGeneratorRegex("TéléphoneFixe", @"(+33 |0)(1|2|3|4|5) \d{2} \d{2} \d{2} \d{2}"));
generators.Add(new MetaDataGeneratorKeyRlmc("Rlmc2", "Rlmc"));
generators.Add(new MetaDataGeneratorRegex("TéléphoneMobile", @"(+33 |0)(6|9) \d{2} \d{2} \d{2} \d{2}"));
generators.Add(new MetaDataGeneratorKeyRlmc("Rlmc", "Cmc7"));
generators.Add(new MetaDataGeneratorList("Prénom", @".\Resources\Lists\first_names.txt"));
generators.Add(new MetaDataGeneratorRegex("Cmc7", @"\d{7} \d{9}908 \d{12}"));
generators.Add(new MetaDataGeneratorList("Nom", @".\Resources\Lists\last_names.txt"));
generators.Add(new MetaDataGeneratorFixedValue("Banque", "Crédit du Nord"));


var options = new JsonSerializerOptions
{
    WriteIndented = true,
    TypeInfoResolver = new PolymorphicTypeResolver(),
};
string json = JsonSerializer.Serialize(generators, options);

var docs = new List<Document>();
for (int i = 0; i < 100; i++)
{
    docs.Add(new Document(generators));
}
;
