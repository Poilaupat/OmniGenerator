using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class FixedLengthLineAttribute : Attribute
    {
        public int Length { get; }

        public FixedLengthLineAttribute(int length)
        {
            Length = length;
        }
    }
}
