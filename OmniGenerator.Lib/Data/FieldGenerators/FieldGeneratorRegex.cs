using Fare;

namespace OmniGenerator.Lib.Data.FieldGenerators
{
    /// <summary>
    /// The <see cref="FieldGeneratorRegex"/> produces values that match provided regular expression
    /// Use this generator with caution : Generating values can take a VERY long time depending on the pattern
    /// </summary>
    internal class FieldGeneratorRegex : AbstractFieldGenerator<string>
    {
        /// <summary>
        /// The regular expression pattern
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Creates a new <see cref="FieldGeneratorRegex"/>
        /// </summary>
        /// <param name="name">The name of the generator</param>
        /// <param name="pattern">The regular expression pattern</param>
        public FieldGeneratorRegex(string name, string pattern)
            : base(name)
        {
            Pattern = pattern;
        }

        protected override string GenerateValue()
        {
            var xeger = new Xeger(Pattern, new Random());
            return xeger.Generate();
        }
    }
}
