using SeedGenerator.Lib.Tools;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorKeyCalculator : AbstractFieldGeneratorOneFieldDependant
    {
        public EKeyType KeyType { get; set; }

        public FieldGeneratorKeyCalculator(string name, string dependentUpon, EKeyType keyType)
            : base(name, dependentUpon)
        {
            KeyType = keyType;
        }

        protected override object NextValue()
        {
            var dependency = Dependences.Single();

            return KeyType switch
            {
                EKeyType.Rlmc => KeyTools.ComputeRlmcKey((string)dependency.LastValue),
                EKeyType.Rib => KeyTools.ComputeRibKey((string)dependency.LastValue),
                EKeyType.Tip => KeyTools.ComputeTipKey((string)dependency.LastValue),
                EKeyType.TipGroup6 => KeyTools.ComputeTipGroup6Key((string)dependency.LastValue),
                _ => throw new Exception($"Key type {KeyType} is not supported")
            };
        }
    }
}
