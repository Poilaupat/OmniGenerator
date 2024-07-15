namespace OmniGenerator.Lib.Data
{
    /// <summary>
    /// Modelize an element's field
    /// </summary>
    public class Field
    {
        /// <summary>
        /// The field name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The field value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// The field value as a string representation
        /// </summary>
        public string StringValue => Value?.ToString() ?? string.Empty;

        /// <summary>
        /// Creates a new <see cref="Field"/>
        /// </summary>
        /// <param name="name">The name of the field</param>
        /// <param name="value">The value of the field</param>
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
