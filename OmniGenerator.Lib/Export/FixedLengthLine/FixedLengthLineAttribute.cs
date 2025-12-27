using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.FixedLengthLine
{
    /// <summary>
    /// Attribute to mark a class as representing a fixed-length line export.
    /// Specifies the total length of the line.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class FixedLengthLineAttribute : Attribute
    {
        /// <summary>
        /// Gets the total length of the fixed-length line.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedLengthLineAttribute"/> class
        /// with the specified line length.
        /// </summary>
        /// <param name="length">The total length of the fixed-length line.</param>
        public FixedLengthLineAttribute(int length) => Length = length;
    }
}
