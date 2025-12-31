using Microsoft.Extensions.DependencyModel;
using OmniGenerator.Lib.Tools;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The <see cref="FieldGeneratorKeyCalculator"/> produces key calculations based on the value of a dependent field.
    /// Note that this generator can throw an <see cref="ArgumentException"/> if the dependent field has a value incompatible with the key algorithm.
    /// </summary>
    internal class FieldGeneratorKeyCalculator : AbstractFieldGeneratorSingleFieldDependant<string>
    {
        /// <summary>
        /// Specifies the type of key to compute.
        /// See <see cref="EKeyType"/> for the complete list of supported key types.
        /// </summary>
        public EKeyType KeyType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGeneratorKeyCalculator"/> class.
        /// </summary>
        /// <param name="name">The name of the generator.</param>
        /// <param name="dependentUpon">The name of the dependent field whose value will be used to compute the key.</param>
        /// <param name="keyType">The type of key to compute.</param>
        public FieldGeneratorKeyCalculator(string name, string dependentUpon, EKeyType keyType)
            : base(name, dependentUpon) => KeyType = keyType;

        /// <summary>
        /// Generates the computed key value based on the dependent field's value and the specified key type.
        /// </summary>
        /// <returns>The computed key value as a string.</returns>
        /// <exception cref="Exception">Thrown if the specified <see cref="KeyType"/> is not supported.</exception>
        protected override string GenerateValue()
        {
            var value = (string)GeneratorDependencies.Single().LastValue;

            // Compute the key based on the specified key type  
            return KeyType switch
            {
                EKeyType.Dummy => KeyTools.ComputeDummyKey(value),
                EKeyType.Rlmc => KeyTools.ComputeRlmcKey(value),
                EKeyType.Rib => KeyTools.ComputeRibKey(value),
                EKeyType.Tip => KeyTools.ComputeTipKey(value),
                EKeyType.TipGroup6 => KeyTools.ComputeTipGroup6Key(value),
                EKeyType.Ics => KeyTools.ComputeIcsKey(value),
                _ => throw new Exception($"Key type {KeyType} is not supported")
            };
        }
    }
}
