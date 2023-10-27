using SeedGenerator.Lib.Fields;
using SeedGenerator.Lib.Fields.Generators;

namespace SeedGenerator.Lib
{
    public class Document
    {
        public FieldCollection Fields { get; set; }

        public Document(FieldGeneratorCollection generators)
        {
            Fields = generators.GenerateMetaData();
        }
    }
}
