using Fare;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The <see cref="FieldGeneratorRegex"/> produces values that match provided regular expression
    /// Use this generator with caution : Generating values can take a VERY long time depending on the pattern
    /// </summary>
    internal class FieldGeneratorRegex : AbstractFieldGenerator<string>
    {
        private readonly Xeger _xeger;

        /// <summary>
        /// Creates a new <see cref="FieldGeneratorRegex"/>
        /// </summary>
        /// <param name="name">The name of the generator</param>
        /// <param name="pattern">The regular expression pattern</param>
        public FieldGeneratorRegex(string name, string pattern)
            : base(name) => _xeger = new Xeger(pattern);

        protected override string GenerateValue()
        {
            return _xeger.Generate();
        }
    }
}
