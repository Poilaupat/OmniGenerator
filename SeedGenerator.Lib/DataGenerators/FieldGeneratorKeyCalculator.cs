using SeedGenerator.Lib.Tools;

namespace SeedGenerator.Lib.DataGenerators
{
    internal class FieldGeneratorKeyCalculator : FieldGeneratorDependantBase
    {
        public string KeyType { get; set; }

        public FieldGeneratorKeyCalculator(string name, string dependantUpon, string keyType)
            : base(name, dependantUpon)
        {
            KeyType = keyType;

            int dependances = dependantUpon.Split(';').Count();
            if (dependances > 1)
            {
                throw new ArgumentException($"{keyType} generator support only one dependance");
            }
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
