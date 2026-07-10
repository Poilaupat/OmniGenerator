using System.Diagnostics;
using System.Globalization;

namespace OmniGenerator.Lib.Hierarchy
{
    /// <summary>
    /// Modelize an element's field.
    /// A field carries three values on independent output channels so that error
    /// simulation can make the image diverge from the metadata (and vice versa):
    /// <list type="bullet">
    /// <item><see cref="Value"/>: the original generated value (source of truth, used for traceability/logging).</item>
    /// <item><see cref="DataValue"/>: the value read by packagers (data package output).</item>
    /// <item><see cref="ImageValue"/>: the value read by renderers (image output).</item>
    /// </list>
    /// At creation time all three values are equal. The error simulation pass mutates
    /// only the relevant channel, leaving <see cref="Value"/> untouched.
    /// </summary>
    [DebuggerDisplay("Name={Name} Value={Value} Data={DataValue} Image={ImageValue}")]
    public sealed class Field
    {
        /// <summary>
        /// The field name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The original generated value. This is the source of truth and is never mutated
        /// by the error simulation, so it can be used for traceability and logging.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// The value read by packagers (data package output). Defaults to <see cref="Value"/>
        /// and may be mutated by data-channel error simulations (e.g. MISREAD, SUBSTITUTION).
        /// </summary>
        public object DataValue { get; set; }

        /// <summary>
        /// The value read by renderers (image output). Defaults to <see cref="Value"/>
        /// and may be mutated by image-channel error simulations (e.g. INCONSISTENCY).
        /// </summary>
        public object ImageValue { get; set; }

        /// <summary>
        /// The original value as a string representation (invariant culture).
        /// </summary>
        public string StringValue => ToInvariantString(Value);

        /// <summary>
        /// The data-channel value as a string representation (invariant culture).
        /// </summary>
        public string DataStringValue => ToInvariantString(DataValue);

        /// <summary>
        /// The image-channel value as a string representation (invariant culture).
        /// </summary>
        public string ImageStringValue => ToInvariantString(ImageValue);

        /// <summary>
        /// Creates a new <see cref="Field"/>. All channels are initialized to <paramref name="value"/>.
        /// </summary>
        /// <param name="name">The name of the field</param>
        /// <param name="value">The value of the field</param>
        public Field(string name, object? value)
        {
            Name = name;
            Value = value!;
            DataValue = value!;
            ImageValue = value!;
        }

        /// <summary>
        /// Gets the value carried by the specified <paramref name="channel"/>.
        /// </summary>
        /// <param name="channel">The output channel to read.</param>
        /// <returns>The value for the requested channel.</returns>
        public object GetValue(FieldChannel channel) => channel switch
        {
            FieldChannel.Image => ImageValue,
            _ => DataValue,
        };

        /// <summary>
        /// Gets the string representation of the value carried by the specified <paramref name="channel"/>.
        /// </summary>
        /// <param name="channel">The output channel to read.</param>
        /// <returns>The invariant-culture string for the requested channel.</returns>
        public string GetStringValue(FieldChannel channel) => channel switch
        {
            FieldChannel.Image => ImageStringValue,
            _ => DataStringValue,
        };

        private static string ToInvariantString(object? value) =>
            Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
    }
}
