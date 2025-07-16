using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.FixedLengthLine
{
    /// <summary>
    /// Base class for fixed-length line exports.
    /// Provides logic to convert an object's properties to a fixed-length string
    /// using <see cref="FixedLengthLineFieldAttribute"/> metadata.
    /// </summary>
    public class FixedLengthLineBase
    {
        /// <summary>
        /// Converts the current object to a fixed-length string representation.
        /// Each property marked with <see cref="FixedLengthLineFieldAttribute"/> is inserted
        /// at the specified offset and padded according to the attribute settings.
        /// </summary>
        /// <returns>
        /// A string of fixed length as specified by <see cref="FixedLengthLineAttribute.Length"/>,
        /// with property values inserted and padded as defined by their attributes.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the class is missing the <see cref="FixedLengthLineAttribute"/>.
        /// </exception>
        public virtual string ToFixedLengthString()
        {
            var lineAttr = GetType().GetCustomAttribute<FixedLengthLineAttribute>();
            if (lineAttr == null)
                throw new InvalidOperationException("Missing FixedLengthLine attribute on class.");

            var line = new StringBuilder(new string(' ', lineAttr.Length));

            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var attr = property.GetCustomAttribute<FixedLengthLineFieldAttribute>();
                if (attr == null)
                    continue;

                var value = property.GetValue(this) as string ?? string.Empty;

                if (value.Length > attr.Length)
                    value = value.Substring(0, attr.Length);

                string padded = attr.PadDirection switch
                {
                    PadDirection.Left => value.PadLeft(attr.Length, attr.PaddingChar),
                    PadDirection.Right => value.PadRight(attr.Length, attr.PaddingChar),
                    _ => throw new InvalidOperationException("Unknown PadDirection")
                };

                line.Remove(attr.Offset, attr.Length);
                line.Insert(attr.Offset, padded);
            }

            return line.ToString();
        }

    }
}
