using Mustache;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The <see cref="FieldGeneratorComposite"/> is a kind of generator that can put together a format other field values of the same <see cref="Element"/>
    /// </summary>
    internal class FieldGeneratorComposite : AbstractFieldGeneratorDependant<string>
    {
        /// <summary>
        /// The formatting pattern. It must follow the Mustache for C# pattern.
        /// See https://danielescipioni.github.io/Mustache/ for more details
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Creates a new <see cref="FieldGeneratorComposite"/>
        /// </summary>
        /// <param name="name">The name of the generator</param>
        /// <param name="dependentUpon">The names of the field dependences to build this composite field</param>
        /// <param name="format">The Mustache format</param>
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
