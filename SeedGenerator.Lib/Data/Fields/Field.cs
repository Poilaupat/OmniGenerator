namespace SeedGenerator.Lib.Data.Fields
{
    internal class Field
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Field(string name, string value)
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
