using SeedGenerator.Lib.Tools;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorKeyCalculator : AbstractFieldGeneratorOneFieldDependant
    {
        public string KeyType { get; set; }

        public FieldGeneratorKeyCalculator(string name, string dependantUpon, string keyType)
            : base(name, dependantUpon)
        {
            KeyType = keyType;
        }

        public override string NextValue()
        {
            var depvalue = Dependances.FirstOrDefault().Value ?? "0";

            return KeyType switch
            {
                "rlmc" => ComputeRlmcKey(depvalue),
                _ => throw new Exception($"Unknown key type")
            };
        }

        private string ComputeRlmcKey(string numericString)
        {
            return KeyTools.ComputeRlmcKey(numericString);
        }
    }
}
