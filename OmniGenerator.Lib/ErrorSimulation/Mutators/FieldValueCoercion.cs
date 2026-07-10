using System.Globalization;

namespace OmniGenerator.Lib.ErrorSimulation.Mutators
{
    /// <summary>
    /// Helper turning a mutated string back into the original value's type when possible.
    /// Keeps the simulation field-agnostic while preserving typed reads (int, DateTime, ...)
    /// whenever the corrupted value is still convertible.
    /// </summary>
    internal static class FieldValueCoercion
    {
        /// <summary>
        /// Attempts to convert <paramref name="mutated"/> back to the runtime type of <paramref name="original"/>.
        /// Falls back to the raw string when the conversion is not possible (for example a '?' inside a number).
        /// </summary>
        /// <param name="original">The original channel value, used only to infer the target type.</param>
        /// <param name="mutated">The mutated string representation.</param>
        /// <returns>The coerced value, or the raw string when coercion fails.</returns>
        public static object Coerce(object? original, string mutated)
        {
            if (original is null || original is string)
                return mutated;

            try
            {
                return Convert.ChangeType(mutated, original.GetType(), CultureInfo.InvariantCulture);
            }
            catch (Exception ex) when (ex is InvalidCastException or FormatException or OverflowException)
            {
                return mutated;
            }
        }
    }
}
