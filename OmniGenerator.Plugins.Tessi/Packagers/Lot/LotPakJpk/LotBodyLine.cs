using OmniGenerator.Lib.FixedLengthLine;
using OmniGenerator.Lib.Hierarchy;
using System;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    [FixedLengthLine(435)]
    internal class LotBodyLine : FixedLengthLineBase
    {
        [FixedLengthLineField(0, 2, '0', PadDirection.Left)]
        public string DocType { get; } = string.Empty;

        [FixedLengthLineField(3, 5, '0', PadDirection.Left)]
        public string RefEndos { get; } = string.Empty;

        [FixedLengthLineField(10, 64, ' ', PadDirection.Right)]
        public string Dataread { get; } = string.Empty;
        
        [FixedLengthLineField(74, 12, ' ', PadDirection.Right)]
        public string LotID { get; } = string.Empty;

        [FixedLengthLineField(87, 8, ' ', PadDirection.Right)]
        public string RemittanceID { get; } = string.Empty;

        [FixedLengthLineField(96, 1, '0', PadDirection.Left)]
        public string QualityCode { get; } = string.Empty;

        [FixedLengthLineField(98, 7, '0', PadDirection.Left)]
        public string NumDoc { get; } = string.Empty;

        [FixedLengthLineField(106, 1, '0', PadDirection.Left)]
        public string SendGrayLevel { get; } = string.Empty;

        [FixedLengthLineField(108, 1, '0', PadDirection.Left)]
        public string SendRear { get; } = string.Empty;

        [FixedLengthLineField(110, 5, ' ', PadDirection.Right)]
        public string TimeStamp { get; } = string.Empty;

        [FixedLengthLineField(116, 24, ' ', PadDirection.Right)]
        public string RefDoc { get; } = string.Empty;

        [FixedLengthLineField(141, 15, ' ', PadDirection.Right)]
        public string Signature { get; } = string.Empty;

        [FixedLengthLineField(157, 5, '0', PadDirection.Left)]
        public string BankCode { get; } = string.Empty;

        [FixedLengthLineField(165, 3, '0', PadDirection.Left)]
        public string ProcessCode { get; } = string.Empty;

        [FixedLengthLineField(167, 1, '0', PadDirection.Left)]
        public string Reconciliation { get; } = string.Empty;

        [FixedLengthLineField(169, 1, '0', PadDirection.Left)]
        public string Status { get; } = string.Empty;

        [FixedLengthLineField(171, 3, ' ', PadDirection.Right)]
        public string Priority { get; } = string.Empty;
        
        [FixedLengthLineField(175, 23, ' ', PadDirection.Right)]
        public string RIB { get; } = string.Empty;

        [FixedLengthLineField(199, 3, ' ', PadDirection.Right)]
        public string NbChecks { get; } = string.Empty;

        [FixedLengthLineField(203, 3, ' ', PadDirection.Right)]
        public string ICRConfAmount { get; } = string.Empty;

        [FixedLengthLineField(207, 12, ' ', PadDirection.Right)]
        public string ICRAmount { get; } = string.Empty;

        [FixedLengthLineField(220, 34, ' ', PadDirection.Right)]
        public string Free { get; } = string.Empty;

        [FixedLengthLineField(255, 9, '0', PadDirection.Left)]
        public string LengthRectoPak { get; } = string.Empty;

        [FixedLengthLineField(265, 10, '0', PadDirection.Left)]
        public string OffsetRectoPak { get; } = string.Empty;

        [FixedLengthLineField(276, 9, '0', PadDirection.Left)]
        public string LengthVersoPak { get; } = string.Empty;

        [FixedLengthLineField(286, 10, '0', PadDirection.Left)]
        public string OffsetVersoPak { get; } = string.Empty;

        [FixedLengthLineField(299, 9, '0', PadDirection.Left)]
        public string LengthRectoJpk { get; } = string.Empty;

        [FixedLengthLineField(307, 10, '0', PadDirection.Left)]
        public string OffsetRectoJpk { get; } = string.Empty;

        [FixedLengthLineField(318, 9, '0', PadDirection.Left)]
        public string LengthVersoJpk { get; } = string.Empty;

        [FixedLengthLineField(329, 10, '0', PadDirection.Left)]
        public string OffsetVersoJpk { get; } = string.Empty;

        [FixedLengthLineField(339, 5, '0', PadDirection.Left)]
        public string SortError { get; } = string.Empty;

        [FixedLengthLineField(345, 6, '0', PadDirection.Left)]
        public string ImageQuality { get; } = string.Empty;

        [FixedLengthLineField(352, 1, '0', PadDirection.Left)]
        public string Deleted { get; } = string.Empty;

        [FixedLengthLineField(354, 10, '0', PadDirection.Left)]
        public string Amount { get; } = string.Empty;

        public LotBodyLine(int index, Document document, OffsetLengthImage bwRecto, OffsetLengthImage bwVerso, OffsetLengthImage gsRecto, OffsetLengthImage gsVerso)
        {
            var remittance = document.Parent;
            var root = remittance.Parent;

            DocType = document.Fields.GetStringValue("encline");
            RefEndos = index.ToString();
            Dataread = document.Fields.GetStringValueOrDefault("dataread", string.Empty);
            LotID = root.Fields.GetStringValueOrDefault("packet-number", "1");
            RemittanceID = remittance.Fields.GetStringValueOrDefault("remittance-id", string.Empty);
            QualityCode = document.Fields.GetStringValueOrDefault("quality-code", "0");
            NumDoc = index.ToString();
            SendGrayLevel = gsRecto.Length > 0 ? "1" : "0";
            SendRear = gsVerso.Length > 0 ? "1" : "0";
            TimeStamp = DateTime.Now.TimeOfDay.TotalSeconds.ToString();
            RefDoc = document.Fields.GetStringValueOrDefault("ref-doc", string.Empty);
            Signature = document.Fields.GetStringValueOrDefault("signature", "---SIGNATURE---");
            BankCode = root.Fields.GetStringValueOrDefault("bank-code", "00001");
            ProcessCode = root.Fields.GetStringValueOrDefault("process-code", "000");
            Reconciliation = root.Fields.GetStringValueOrDefault("reconciliation", "0");
            Status = document.Fields.GetStringValueOrDefault("status", "0");
            Priority = document.Fields.GetStringValueOrDefault("priority", string.Empty);
            RIB = document.Fields.GetStringValueOrDefault("rib", string.Empty);
            NbChecks = document.Fields.GetStringValueOrDefault("nb-checks", string.Empty);
            ICRConfAmount = document.Fields.GetStringValueOrDefault("icr-conf-amount", string.Empty);
            ICRAmount = document.Fields.GetStringValueOrDefault("icr-amount", string.Empty);
            Free = string.Empty;
            LengthRectoPak = bwRecto.Length.ToString();
            OffsetRectoPak = bwRecto.Offset.ToString();
            LengthVersoPak = bwVerso.Length.ToString();
            OffsetVersoPak = bwVerso.Offset.ToString();
            LengthRectoJpk = gsRecto.Length.ToString();
            OffsetRectoJpk = gsRecto.Offset.ToString();
            LengthVersoJpk = gsVerso.Length.ToString();
            OffsetVersoJpk = gsVerso.Offset.ToString();
            SortError = document.Fields.GetStringValueOrDefault("sort-error", "0");
            ImageQuality = document.Fields.GetStringValueOrDefault("image-quality", "0");
            Deleted = document.Fields.GetStringValueOrDefault("deleted", "0");
        }
    }
}
