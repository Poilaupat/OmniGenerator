using SeedGenerator.Lib.Tools;

namespace SeedGenerator.Lib.Fields.Generators
{
    internal class FieldGeneratorKeyCalculator : FieldGeneratorDependantBase
    {
        public string KeyType { get; set; }

        public FieldGeneratorKeyCalculator(string name, string dependantUpon, string keyType)
            : base(name, dependantUpon)
        {
            KeyType = keyType;
        }

        public override string NextValue()
        {
            return KeyType switch
            {
                "rlmc" => ComputeRlmcKey(DependantValue ?? "0"),
                _ => throw new Exception($"Unknown key type")
            };
        }

        private string ComputeRlmcKey(string numericString)
        {
            return KeyTools.ComputeRlmcKey(numericString);
        }
    }
}
