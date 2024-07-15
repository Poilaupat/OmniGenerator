using SeedGenerator.Lib.Tools;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    /// <summary>
    /// The <see cref="FieldGeneratorKeyCalculator"/> produces key calculation upon the dependant field value
    /// Note that this generator can throw <see cref="ArgumentException"/> if the dependant field has a value incompatible with the key algorithm
    /// </summary>
    internal class FieldGeneratorKeyCalculator : AbstractFieldGeneratorOneFieldDependant<string>
    {
        /// <summary>
        /// Specifies the key to compute 
        /// See <see cref="EKeyType"/> for the complete list of keys
        /// </summary>
        public EKeyType KeyType { get; set; }

        /// <summary>
        /// Creates a new <see cref="FieldGeneratorKeyCalculator"/>
        /// </summary>
        /// <param name="name">The name of the generator</param>
        /// <param name="dependentUpon">The dependant field whose value will be used to compute the key</param>
        /// <param name="keyType">The key type</param>
        public FieldGeneratorKeyCalculator(string name, string dependentUpon, EKeyType keyType)
            : base(name, dependentUpon)
        {
            KeyType = keyType;
        }

        protected override string GenerateValue()
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
