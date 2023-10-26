using SeedGenerator.Lib.MetaData;
using SeedGenerator.Lib.MetaData.Generators;

namespace SeedGenerator.Lib
{
    public class Document
    {
        public MetaDataCollection MetaDatas { get; set; }

        public Document(MetaDataGeneratorCollection generators)
        {
            MetaDatas = generators.GenerateMetaData();
        }
    }
}
