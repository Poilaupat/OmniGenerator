namespace SeedGenerator.Lib.Data
{
    public class PacketData
    {
        public FieldCollection Fields { get; set; }
        public IEnumerable<DocumentData> Documents { get; set; }

        public PacketData(FieldCollection fields, IEnumerable<DocumentData> documents) 
        { 
            Fields = fields;
            Documents = documents;
        }
    }
}
