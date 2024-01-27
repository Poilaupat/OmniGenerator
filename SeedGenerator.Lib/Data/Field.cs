namespace SeedGenerator.Lib.Data
{
    public class Field
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string StringValue => Value?.ToString() ?? string.Empty;

        public Field(string name, object value)
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
