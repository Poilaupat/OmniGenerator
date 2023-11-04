using SeedGenerator.Lib.Data.Fields;
using SeedGenerator.Lib.Data.Fields.Generators;

namespace SeedGenerator.Lib.Data
{
    internal class Document
    {
        public string Name { get; set; }

        public FieldCollection Fields { get; set; }

        public Document(string name, FieldGeneratorCollection generators)
        {
            Name = name;
            Fields = generators.GenerateFields();
        }
    }
}
