using System.Diagnostics;

namespace OmniGenerator.Lib.Hierarchy
{
    /// <summary>
    /// Modelize an element's field
    /// </summary>
    [DebuggerDisplay("Name={Name} Value={Value}")]
    public readonly struct Field
    {
        /// <summary>
        /// The field name
        /// </summary>
        public readonly string Name { get; }

        /// <summary>
        /// The field value
        /// </summary>
        public readonly object Value { get; }

        /// <summary>
        /// The field value as a string representation
        /// </summary>
        public readonly string StringValue => Convert.ToString(Value, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty;

        /// <summary>
        /// Creates a new <see cref="Field"/>
        /// </summary>
        /// <param name="name">The name of the field</param>
        /// <param name="value">The value of the field</param>
        public Field(string name, object? value)
        {
            Name = name;
            Value = value!;
        }
    }
}
