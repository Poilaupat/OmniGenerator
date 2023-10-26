using SeedGenerator.Lib.Tools;

namespace SeedGenerator.Lib.MetaData.Generators
{
    public class MetaDataGeneratorKeyCalculator : MetaDataGeneratorDependantBase
    {
        public string KeyType { get; set; }

        public MetaDataGeneratorKeyCalculator(string name, string dependantUpon, string keyType)
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
