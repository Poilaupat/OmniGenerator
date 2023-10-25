using SeedGenerator.Lib.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Document
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
