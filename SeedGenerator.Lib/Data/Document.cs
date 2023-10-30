using SeedGenerator.Lib.Data.Fields;
using SeedGenerator.Lib.Data.Fields.Generators;

namespace SeedGenerator.Lib.Data
{
    internal class Document
    {
        public FieldCollection Fields { get; set; }

        public Document(FieldGeneratorCollection generators)
        {
            Fields = generators.GenerateFields();
        }
    }
}
