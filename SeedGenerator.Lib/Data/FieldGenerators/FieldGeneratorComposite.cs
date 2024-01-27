using Mustache;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorComposite : AbstractFieldGeneratorDependant
    {
        public string Format { get; set; }

        public FieldGeneratorComposite(string name, string dependantUpon, string format)
            : base(name, dependantUpon)
        {
            Format = format;
        }

        public override object NextValue()
        {
            FormatCompiler compiler = new FormatCompiler();
            Generator generator = compiler.Compile(Format);
            string result = generator.Render(Dependances);

            return result;
        }
    }
}
