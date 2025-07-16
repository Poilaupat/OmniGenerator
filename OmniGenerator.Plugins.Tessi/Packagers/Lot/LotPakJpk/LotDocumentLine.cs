using OmniGenerator.Lib.FixedLengthLine;
using OmniGenerator.Lib.Hierarchy;
using System;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    [FixedLengthLine(435)]
    internal class LotChequeLine : FixedLengthLineBase
    {
        [FixedLengthLineField(0, 5, ' ', PadDirection.Right)]
        public string Type { get; } = string.Empty;

        [FixedLengthLineField(6, 2, '0', PadDirection.Left)]
        public string FormType { get; } = string.Empty;

        [FixedLengthLineField(9, 2, '0', PadDirection.Left)]
        public string Priority { get; } = string.Empty;

        [FixedLengthLineField(12, 1, ' ', PadDirection.Right)]
        public string Deleted { get; } = string.Empty;

        [FixedLengthLineField(14, 5, ' ', PadDirection.Right)]
        public string RIB { get; } = string.Empty;

        [FixedLengthLineField(20, 6, '0', PadDirection.Left)]
        public string Amount { get; } = string.Empty;

        [FixedLengthLineField(27, 6, '0', PadDirection.Left)]
        public string ICRAmount { get; } = string.Empty;

        [FixedLengthLineField(34, 6, '0', PadDirection.Left)]
        public string ICRConfAmount { get; } = string.Empty;

        [FixedLengthLineField(41, 10, ' ', PadDirection.Right)]
        public string ICRCode { get; } = string.Empty;

        [FixedLengthLineField(52, 10, ' ', PadDirection.Right)]
        public string ICRConfCode { get; } = string.Empty;

        [FixedLengthLineField(63, 4, '0', PadDirection.Left)]
        public string NbCheques { get; } = string.Empty;

        [FixedLengthLineField(68, 4, '0', PadDirection.Left)]
        public string ImageOffsetRectoPak { get; } = string.Empty;

        [FixedLengthLineField(73, 4, '0', PadDirection.Left)]
        public string ImageLengthRectoPak { get; } = string.Empty;

        [FixedLengthLineField(78, 4, '0', PadDirection.Left)]
        public string ImageOffsetVersoPak { get; } = string.Empty;

        [FixedLengthLineField(83, 4, '0', PadDirection.Left)]
        public string ImageLengthVersoPak { get; } = string.Empty;

        [FixedLengthLineField(88, 4, '0', PadDirection.Left)]
        public string ImageOffsetRectoJpk { get; } = string.Empty;

        [FixedLengthLineField(93, 4, '0', PadDirection.Left)]
        public string ImageLengthRectoJpk { get; } = string.Empty;

        [FixedLengthLineField(98, 4, '0', PadDirection.Left)]
        public string ImageOffsetVersoJpk { get; } = string.Empty;

        [FixedLengthLineField(103, 4, '0', PadDirection.Left)]
        public string ImageLengthVersoJpk { get; } = string.Empty;

        [FixedLengthLineField(108, 2, '0', PadDirection.Left)]
        public string ImageQuality { get; } = string.Empty;

        [FixedLengthLineField(111, 2, '0', PadDirection.Left)]
        public string SortError { get; } = string.Empty;


        public LotChequeLine(Document document)
        {
            // Toutes les propriétés sont initialisées à string.Empty,
            // à remplir manuellement par l'appelant.
        }
    }
}
