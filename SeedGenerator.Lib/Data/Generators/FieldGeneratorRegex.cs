using Fare;

namespace SeedGenerator.Lib.Data.Generators
{
    internal class FieldGeneratorRegex : FieldGeneratorBase
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
