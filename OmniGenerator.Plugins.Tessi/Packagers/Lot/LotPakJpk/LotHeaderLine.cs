using OmniGenerator.Lib.FixedLengthLine;
using OmniGenerator.Lib.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    [FixedLengthLine(435)]
    internal class LotHeaderLine : FixedLengthLineBase
    {
        [FixedLengthLineField(0, 5, ' ', PadDirection.Right)]
        public string Copyright { get; }

        [FixedLengthLineField(6, 12, ' ', PadDirection.Left)]
        public string Database { get; }

        [FixedLengthLineField(19, 8, '0', PadDirection.Left)]
        public string Date { get; }

        [FixedLengthLineField(28, 5, '0', PadDirection.Left)]
        public string BranchID { get; }

        [FixedLengthLineField(34, 3, '0', PadDirection.Left)]
        public string MachineID { get; }

        [FixedLengthLineField(38, 70, ' ', PadDirection.Right)]
        public string ImagePath { get; }

        [FixedLengthLineField(109, 20, ' ', PadDirection.Right)]
        public string SeedVersion { get; }

        [FixedLengthLineField(130, 20, ' ', PadDirection.Right)]
        public string AppVersion { get; }

        [FixedLengthLineField(151, 5, '0', PadDirection.Left)]
        public string BranchOrigID { get; }

        [FixedLengthLineField(157, 8, '0', PadDirection.Left)]
        public string OperationDate { get; }

        [FixedLengthLineField(166, 50, ' ', PadDirection.Right)]
        public string UserName { get; }

        [FixedLengthLineField(217, 128, ' ', PadDirection.Right)]
        public string SignatureID { get; }

        [FixedLengthLineField(357, 3, '0', PadDirection.Left)]
        public string DotPerMillimeterRectoPak { get; }

        [FixedLengthLineField(361, 3, '0', PadDirection.Left)]
        public string DotPerMillimeterVersoPak { get; }

        [FixedLengthLineField(365, 3, '0', PadDirection.Left)]
        public string DotPerMillimeterRectoJpk { get; }

        [FixedLengthLineField(369, 3, '0', PadDirection.Left)]
        public string DotPerMillimeterVersoJpk { get; }

        [FixedLengthLineField(373, 12, '0', PadDirection.Left)]
        public string FileTimeStamp { get; }


        [FixedLengthLineField(386, 5, '0', PadDirection.Left)]
        public string BankID { get; }

        [FixedLengthLineField(392, 9, ' ', PadDirection.Left)]
        public string SerialNumber { get; }

        [FixedLengthLineField(402, 4, '0', PadDirection.Left)]
        public string SequenceNumber { get; }

        [FixedLengthLineField(408, 3, ' ', PadDirection.Right)]
        public string Free { get; }

        [FixedLengthLineField(411, 24, ' ', PadDirection.Right)]
        public string Identifiant { get; }

        public LotHeaderLine(RootFields rootFields)
        {
            Copyright = "ATHIC";
            Database = string.Empty;
            Date = rootFields.PacketDate.ToString("ddMMyyyy");
            BranchID = rootFields.CapturePointCode;
            MachineID = rootFields.ScannerCode;
            ImagePath = "\\No\\Path\\Image";
            SeedVersion = "Format A V4.10";
            AppVersion = "OG Tessi Plugin V0.X";
            BranchOrigID = rootFields.OrganizationUnitCode;
            OperationDate = Date;
            UserName = "OmniGenerator";
            SignatureID = string.Empty;
            DotPerMillimeterRectoPak = string.Empty;
            DotPerMillimeterVersoPak = string.Empty;
            DotPerMillimeterRectoJpk = string.Empty;
            DotPerMillimeterVersoJpk = string.Empty;
            FileTimeStamp = DateTime.Now.ToString("yyMMddHHmmss");
            Free = string.Empty;
            BankID = rootFields.OrganizationCode;
            SerialNumber = rootFields.MachineSerialNumber;
            SequenceNumber = rootFields.PacketNumber;
            Identifiant = string.Empty;
        }
    }
}
