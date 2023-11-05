using SeedGenerator.Lib.Data.Generators;
using SeedGenerator.Lib.Param;

namespace SeedGenerator.Lib.Data
{
    public class Packet
    {
        public FieldCollection Fields { get; set; }
        public IEnumerable<Document> Documents { get; set; }

        public Packet(FieldCollection fields, IEnumerable<Document> documents) 
        { 
            Fields = fields;
            Documents = documents;
        }
    }
}
