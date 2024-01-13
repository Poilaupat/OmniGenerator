using SeedGenerator.Lib.Tools;

namespace SeedGenerator.Lib.DataGenerators
{
    internal class FieldGeneratorKeyCalculator : AbstractFieldGeneratorDependant
    {
        public string KeyType { get; set; }

        public FieldGeneratorKeyCalculator(string name, string dependantUpon, string keyType)
            : base(name, dependantUpon)
        {
            KeyType = keyType;

            if (Dependances.Count > 1)
            {
                throw new ArgumentException($"{name} generator support only one dependance");
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
