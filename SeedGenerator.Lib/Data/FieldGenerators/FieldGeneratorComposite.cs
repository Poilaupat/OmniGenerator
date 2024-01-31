using Mustache;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorComposite : AbstractFieldGeneratorDependant<string>
    {
        public string Format { get; set; }

        public FieldGeneratorComposite(string name, string dependentUpon, string format)
            : base(name, dependentUpon)
        {
            Format = format;
        }

        protected override string GenerateValue()
        {
            var data = Dependences.ToDictionary(x => x.Name, y => y.LastValue);
            FormatCompiler compiler = new FormatCompiler();
            Generator generator = compiler.Compile(Format);
            string result = generator.Render(data);

            return result;
        }
    }
}
