using OmniGenerator.Lib.FixedLengthLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    [FixedLengthLine(114)]
    internal class LotScannerStatisticsLine : FixedLengthLineBase
    {
        [FixedLengthLineField(0, 2)]
        public string Encline { get; }
        [FixedLengthLineField(3, 10, '0', PadDirection.Left)]
        public int PowerOnHours { get; } = 0;
        [FixedLengthLineField(14, 10, '0', PadDirection.Left)]
        public int PowerOnHoursSinceReset { get; } = 0;
        [FixedLengthLineField(25, 10, '0', PadDirection.Left)]
        public int DocCount { get; } = 0;
        [FixedLengthLineField(36, 10, '0', PadDirection.Left)]
        public int NumDocSinceReset { get; } = 0;
        [FixedLengthLineField(47, 10, '0', PadDirection.Left)]
        public int NumDoubleDoc { get; } = 0;
        [FixedLengthLineField(58, 10, '0', PadDirection.Left)]
        public int NumJamDoc { get; } = 0;
        [FixedLengthLineField(69, 10, '0', PadDirection.Left)]
        public int NumFeederJams { get; } = 0;
        [FixedLengthLineField(80, 10, '0', PadDirection.Left)]
        public int NulSkewedDocs { get; } = 0;
        [FixedLengthLineField(91, 10, '0', PadDirection.Left)]
        public int NumIncompleteImage { get; } = 0;
        [FixedLengthLineField(102, 10, '0', PadDirection.Left)]
        public int EndorseDotCount { get; } = 0;

        public LotScannerStatisticsLine()
        {
            Encline = "89";
        }
    }
}
