namespace OmniGenerator.Lib.Data.FieldGenerators
{
    /// <summary>
    /// The <see cref="FieldGeneratorConstant"/> is a king of generator that return constant values
    /// </summary>
    internal class FieldGeneratorConstant : AbstractFieldGenerator<string>
    {
        /// <summary>
        /// The constant value to return 
        /// </summary>
        public string Constant { get; }

        /// <summary>
        /// Creates a new <see cref="FieldGeneratorConstant"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="constant"></param>
        public FieldGeneratorConstant(string name, string constant)
            : base(name)
        {
            Constant = constant;
        }

        protected override string GenerateValue()
        {
            return Constant;
        }
    }
}
