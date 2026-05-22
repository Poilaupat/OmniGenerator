using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.FixedLengthLine
{
    /// <summary>
    /// Specifies the direction in which padding should be applied to a fixed-length field.
    /// </summary>
    public enum PadDirection
    {
        /// <summary>
        /// Pad on the left side of the value.
        /// </summary>
        Left,
        /// <summary>
        /// Pad on the right side of the value.
        /// </summary>
        Right
    }
}
