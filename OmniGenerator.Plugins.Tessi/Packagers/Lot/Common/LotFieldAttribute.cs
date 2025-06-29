using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class LotFieldAttribute : Attribute
    {
        public int Offset { get; }
        public int Length { get; }
        public char PaddingChar { get; }
        public PadDirection PadDirection { get; }

        public LotFieldAttribute(int offset, int length, char paddingChar, PadDirection padDirection = PadDirection.Right)
        {
            Offset = offset;
            Length = length;
            PaddingChar = paddingChar;
            PadDirection = padDirection;
        }
    }
}
