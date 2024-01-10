using Fare;

namespace SeedGenerator.Lib.DataGenerators
{
    internal class FieldGeneratorRegex : AbstractFieldGenerator
    {
        public string Pattern { get; set; }

        public FieldGeneratorRegex(string name, string pattern)
            : base(name)
        {
            Pattern = pattern;
        }

        public override string NextValue()
        {
            var xeger = new Xeger(Pattern, new Random());
            return xeger.Generate();
        }
    }
}
