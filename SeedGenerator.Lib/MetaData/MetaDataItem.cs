namespace SeedGenerator.Lib.MetaData
{
    public class MetaDataItem
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public MetaDataItem(string name, string value) 
        { 
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"Name={Name} Value={Value}";
        }
    }
}
