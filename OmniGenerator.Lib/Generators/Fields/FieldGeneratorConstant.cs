namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The <see cref="FieldGeneratorConstant"/> is a kind of generator that returns constant values.
    /// </summary>
    internal class FieldGeneratorConstant : AbstractFieldGenerator<string>
    {
        /// <summary>
        /// Gets the constant value to return.
        /// </summary>
        public string Constant { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGeneratorConstant"/> class.
        /// </summary>
        /// <param name="name">The name of the field generator.</param>
        /// <param name="constant">The constant value to return for each generation.</param>
        public FieldGeneratorConstant(string name, string constant)
            : base(name)
        {
            Constant = constant;
        }

        /// <summary>
        /// Generates the constant value.
        /// </summary>
        /// <returns>The constant value specified at construction.</returns>
        protected override string GenerateValue()
        {
            return Constant;
        }
    }
}
