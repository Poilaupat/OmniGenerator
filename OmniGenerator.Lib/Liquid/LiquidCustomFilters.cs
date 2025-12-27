namespace OmniGenerator.Lib.Liquid
{
    /// <summary>
    /// Custom filters for DotLiquid templates used in composite field generation.
    /// </summary>
    /// <remarks>
    /// These filters are automatically registered and converted to snake_case in templates.
    /// For example, <c>PadLeft</c> becomes <c>pad_left</c> in the template.
    /// </remarks>
    public static class LiquidCustomFilters
    {
        /// <summary>
        /// Formats an object using standard .NET format strings.
        /// </summary>
        /// <param name="input">The value to format.</param>
        /// <param name="format">The format string (e.g., "D4", "F2", "yyyy-MM-dd").</param>
        /// <returns>The formatted string.</returns>
        /// <example>
        /// Template: <c>{{packet_number | format:"D4"}}</c>
        /// Result: "0042" for input 42
        /// </example>
        public static string Format(object input, string format)
        {
            if (input == null) return string.Empty;

            if (input is IFormattable formattable)
            {
                return formattable.ToString(format, null);
            }

            return string.Format($"{{0:{format}}}", input);
        }

        /// <summary>
        /// Pads a number or string to the left with a specified character.
        /// </summary>
        /// <param name="input">The value to pad.</param>
        /// <param name="length">The total length of the resulting string.</param>
        /// <param name="paddingChar">The character to use for padding (default: "0").</param>
        /// <returns>The padded string.</returns>
        /// <example>
        /// Template: <c>{{packet_number | pad_left:4}}</c>
        /// Result: "0042" for input 42
        /// </example>
        public static string PadLeft(object input, int length, string paddingChar = "0")
        {
            if (input == null) return string.Empty.PadLeft(length, paddingChar[0]);

            return input.ToString()!.PadLeft(length, paddingChar[0]);
        }

        /// <summary>
        /// Pads a number or string to the right with a specified character.
        /// </summary>
        /// <param name="input">The value to pad.</param>
        /// <param name="length">The total length of the resulting string.</param>
        /// <param name="paddingChar">The character to use for padding (default: " ").</param>
        /// <returns>The padded string.</returns>
        /// <example>
        /// Template: <c>{{name | pad_right:20}}</c>
        /// Result: "John                " for input "John"
        /// </example>
        public static string PadRight(object input, int length, string paddingChar = " ")
        {
            if (input == null) return string.Empty.PadRight(length, paddingChar[0]);

            return input.ToString()!.PadRight(length, paddingChar[0]);
        }
    }
}
