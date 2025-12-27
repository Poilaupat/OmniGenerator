using DotLiquid;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// Represents a field generator that composes its value from other field values of the same <see cref="Element"/>,
    /// using a Mustache template format.
    /// </summary>
    internal class FieldGeneratorComposite : AbstractFieldGeneratorDependant<string>
    {
        /// <summary>
        /// Gets or sets the formatting pattern for the composite field.
        /// The pattern must follow the Liquid for C# (DotLiquid) syntax.
        /// See https://github.com/dotliquid/dotliquid for more details.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGeneratorComposite"/> class.
        /// </summary>
        /// <param name="name">The name of the generator.</param>
        /// <param name="dependentUpon">The names of the field dependencies to build this composite field.</param>
        /// <param name="format">The Mustache format string.</param>
        public FieldGeneratorComposite(string name, string dependentUpon, string format)
            : base(name, dependentUpon) => Format = format.Replace("-", "_");

        /// <summary>
        /// Generates the composite field value by rendering the Liquid template with the current field dependencies.
        /// </summary>
        /// <returns>The generated composite string value.</returns>
        protected override string GenerateValue()
        {
            var data = GeneratorDependencies.ToDictionary(x => x.Name.Replace("-", "_"), y => y.LastValue);
            Template template = Template.Parse(Format);
            return template.Render(Hash.FromDictionary(data));
        }
    }
}
