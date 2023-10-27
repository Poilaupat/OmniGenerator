using SeedGenerator.Lib.Fields;
using SeedGenerator.Lib.Fields.Generators;

namespace SeedGenerator.Lib
{
    internal class Document
    {
        public FieldCollection Fields { get; set; }

        public Document(FieldGeneratorCollection generators)
        {
            Fields = generators.GenerateMetaData();
        }
    }
}
