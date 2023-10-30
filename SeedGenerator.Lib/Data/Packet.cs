using SeedGenerator.Lib.Data.Fields;

namespace SeedGenerator.Lib.Data
{
    internal class Packet
    {
        public FieldCollection Fields { get; set; }
        public List<Document> Documents { get; set; } = new List<Document>();
    }
}
