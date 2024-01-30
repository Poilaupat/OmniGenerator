using Fare;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorRegex : AbstractFieldGenerator
    {
        public string Pattern { get; set; }

        public FieldGeneratorRegex(string name, string pattern)
            : base(name)
        {
            Pattern = pattern;
        }

        protected override object NextValue()
        {
            var xeger = new Xeger(Pattern, new Random());
            return xeger.Generate();
        }
    }
}
