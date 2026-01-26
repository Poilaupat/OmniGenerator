using OmniGenerator.Lib.FixedLengthLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    [FixedLengthLine(190)]
    internal class LotNavetteLine : FixedLengthLineBase
    {
        [FixedLengthLineField(0, 2)]
        public string Encline { get; }
        [FixedLengthLineField(3, 5)]
        public string PickupHour { get; }
        [FixedLengthLineField(9, 10, '0', PadDirection.Left)]
        public int NumDoc { get; } = 0;
        [FixedLengthLineField(17, 173, ' ', PadDirection.Right)]
        public string Filler { get; } = string.Empty;

        public LotNavetteLine()
        {
            Encline = "99";
            PickupHour = "00:00";
        }
    }
}
