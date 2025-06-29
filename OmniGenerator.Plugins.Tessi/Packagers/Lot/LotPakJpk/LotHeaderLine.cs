using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Plugins.Tessi.Packagers.Lot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    [FixedLengthLine(435)]
    internal class LotHeaderLine : LotLineBase
    {
        [LotField(0, 5, ' ', PadDirection.Right)]
        public string Copyright { get; }

        [LotField(6, 12, ' ', PadDirection.Left)]
        public string Database { get; }

        [LotField(19, 8, '0', PadDirection.Left)]
        public string Date { get; }

        [LotField(28, 5, '0', PadDirection.Left)]
        public string BranchID { get; }

        [LotField(34, 3, '0', PadDirection.Left)]
        public string MachineID { get; }

        [LotField(38, 70, ' ', PadDirection.Right)]
        public string ImagePath { get; }

        [LotField(109, 20, ' ', PadDirection.Right)]
        public string SeedVersion { get; }

        [LotField(130, 20, ' ', PadDirection.Right)]
        public string AppVersion { get; }

        [LotField(151, 5, '0', PadDirection.Left)]
        public string BranchOrigID { get; }

        [LotField(157, 8, '0', PadDirection.Left)]
        public string OperationDate { get; }

        [LotField(166, 50, ' ', PadDirection.Right)]
        public string UserName { get; }

        [LotField(217, 128, ' ', PadDirection.Right)]
        public string SignatureID { get; }

        [LotField(357, 3, '0', PadDirection.Left)]
        public string DotPerMillimeterRectoPak { get; }

        [LotField(361, 3, '0', PadDirection.Left)]
        public string DotPerMillimeterVersoPak { get; }

        [LotField(365, 3, '0', PadDirection.Left)]
        public string DotPerMillimeterRectoJpk { get; }

        [LotField(369, 3, '0', PadDirection.Left)]
        public string DotPerMillimeterVersoJpk { get; }

        [LotField(373, 12, '0', PadDirection.Left)]
        public string FileTimeStamp { get; }

        [LotField(380, 3, ' ', PadDirection.Right)]
        public string Free { get; }

        [LotField(386, 5, '0', PadDirection.Left)]
        public string BankID { get; }

        [LotField(392, 9, ' ', PadDirection.Left)]
        public string SerialNumber { get; }

        [LotField(402, 4, '0', PadDirection.Left)]
        public string SequenceNumber { get; }

        [LotField(411, 24, ' ', PadDirection.Right)]
        public string Identifiant { get; }

        public LotHeaderLine(Root root, int packetNumber)
        {
            Copyright = "ATHIC";
            Database = "NoDatabase";
            Date = root.Fields.ContainsKey("seed-date") ? ((DateTime)(root.Fields["seed-date"].Value)).ToString("ddMMyyyy") : DateTime.Now.ToString("ddMMyyyy");
            BranchID = root.Fields["code-cp"].StringValue;
            MachineID = root.Fields.ContainsKey("code-scanner") ? root.Fields["code-scanner"].StringValue : "001";
            ImagePath = "\\No\\Path\\Image";
            SeedVersion = "Format A V4.10";
            AppVersion = "OG Tessi Plugin V0.X";
            BranchOrigID = root.Fields["code-ou"].StringValue;
            OperationDate = Date;
            UserName = "OmniGenerator";
            SignatureID = string.Empty;
            DotPerMillimeterRectoPak = string.Empty;
            DotPerMillimeterVersoPak = string.Empty;
            DotPerMillimeterRectoJpk = string.Empty;
            DotPerMillimeterVersoJpk = string.Empty;
            FileTimeStamp = DateTime.Now.ToString("yyMMddHHmmss");
            Free = string.Empty;
            BankID = root.Fields["code-org"].StringValue;
            SerialNumber = root.Fields.ContainsKey("scanner-serial-number") ? root.Fields["scanner-serial-number"].StringValue : "123456789";
            SequenceNumber = packetNumber.ToString();
            Identifiant = string.Empty;
        }
    }
}
