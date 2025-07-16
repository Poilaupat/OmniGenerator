using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.FixedLengthLine
{
    /// <summary>
    /// Attribute to mark a property as a fixed-length field in a line export.
    /// Specifies the offset, length, padding character, and padding direction.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FixedLengthLineFieldAttribute : Attribute
    {
        /// <summary>
        /// Gets the zero-based offset of the field in the line.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Gets the length of the field.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets the character used for padding the field.
        /// </summary>
        public char PaddingChar { get; }

        /// <summary>
        /// Gets the direction in which padding is applied.
        /// </summary>
        public PadDirection PadDirection { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedLengthLineFieldAttribute"/> class
        /// with the specified offset, length, padding character, and padding direction.
        /// </summary>
        /// <param name="offset">The zero-based offset of the field in the line.</param>
        /// <param name="length">The length of the field.</param>
        /// <param name="paddingChar">The character used for padding.</param>
        /// <param name="padDirection">The direction in which padding is applied. Defaults to <see cref="PadDirection.Right"/>.</param>
        public FixedLengthLineFieldAttribute(int offset, int length, char paddingChar, PadDirection padDirection = PadDirection.Right)
        {
            Offset = offset;
            Length = length;
            PaddingChar = paddingChar;
            PadDirection = padDirection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedLengthLineFieldAttribute"/> class
        /// with the specified offset, length, and padding direction. Uses a space character for padding.
        /// </summary>
        /// <param name="offset">The zero-based offset of the field in the line.</param>
        /// <param name="length">The length of the field.</param>
        /// <param name="padDirection">The direction in which padding is applied. Defaults to <see cref="PadDirection.Right"/>.</param>
        public FixedLengthLineFieldAttribute(int offset, int length, PadDirection padDirection = PadDirection.Right)
            : this(offset, length, ' ', padDirection)
        {
        }
    }
}
