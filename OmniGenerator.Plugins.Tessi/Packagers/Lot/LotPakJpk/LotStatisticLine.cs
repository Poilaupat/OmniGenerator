using OmniGenerator.Lib.FixedLengthLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    [FixedLengthLine(180)]
    internal class LotStatisticLine : FixedLengthLineBase
    {
        [FixedLengthLineField(0, 2)]
        public string Encline { get; }
        [FixedLengthLineField(3, 5, '0', PadDirection.Left)]
        public int NbErrors { get; set; } = 0;
        [FixedLengthLineField(9, 5, '0', PadDirection.Left)]
        public int NbDoubleFeed { get; set; } = 0;
        [FixedLengthLineField(15, 5, '0', PadDirection.Left)]
        public int NbJam { get; set; } = 0;
        [FixedLengthLineField(21, 5, '0', PadDirection.Left)]
        public int NbIntervention { get; set; } = 0;
        [FixedLengthLineField(27, 12, ' ', PadDirection.Right)]
        public string MachineSerialNumber { get; set; } = string.Empty;
        [FixedLengthLineField(40, 30, ' ', PadDirection.Right)]
        public string FirmwareVersion { get; set; } = string.Empty;
        [FixedLengthLineField(71, 20, ' ', PadDirection.Right)]
        public string MachineName { get; set; } = string.Empty;
        [FixedLengthLineField(92, 45, ' ', PadDirection.Right)]
        public string Filler1 { get; } = string.Empty;
        [FixedLengthLineField(138, 15, ' ', PadDirection.Right)]
        public string Signature { get; } = string.Empty;
        [FixedLengthLineField(154, 26, ' ', PadDirection.Right)]
        public string Filler2 { get; } = string.Empty;

        public LotStatisticLine(RootFields rootFields)
        {
            Encline = "88";
            MachineSerialNumber = rootFields.MachineSerialNumber;
            MachineName = "OmniGenerator Scan";
        }
    }
}
