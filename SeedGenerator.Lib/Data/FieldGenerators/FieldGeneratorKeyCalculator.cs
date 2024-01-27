using SeedGenerator.Lib.Tools;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorKeyCalculator : AbstractFieldGeneratorOneFieldDependant
    {
        public EKeyType KeyType { get; set; }

        public FieldGeneratorKeyCalculator(string name, string dependantUpon, EKeyType keyType)
            : base(name, dependantUpon)
        {
            KeyType = keyType;
        }

        public override object NextValue()
        {
            var depvalue = Dependances.SingleOrDefault().Value?.ToString() ?? "0";

            return KeyType switch
            {
                EKeyType.Rlmc => KeyTools.ComputeRlmcKey(depvalue),
                EKeyType.Rib => KeyTools.ComputeRibKey(depvalue),
                EKeyType.Tip => KeyTools.ComputeTipKey(depvalue),
                EKeyType.TipGroup6 => KeyTools.ComputeTipGroup6Key(depvalue),
                _ => throw new Exception($"Key type {KeyType} is not supported")
            };
        }
    }
}
