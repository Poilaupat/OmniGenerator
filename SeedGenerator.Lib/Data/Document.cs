namespace SeedGenerator.Lib.Data
{
    public class Document
    {
        public string Name { get; set; }

        public FieldCollection? Fields { get; set; }

        public Document(string name)
        {
            Name = name;
        }
    }
}
