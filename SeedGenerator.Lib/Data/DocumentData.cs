namespace SeedGenerator.Lib.Data
{
    public class DocumentData
    {
        public string Name { get; set; }

        public FieldCollection? Fields { get; set; }

        public DocumentData(string name)
        {
            Name = name;
        }
    }
}
