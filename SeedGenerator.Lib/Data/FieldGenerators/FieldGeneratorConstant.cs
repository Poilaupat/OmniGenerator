namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorConstant : AbstractFieldGenerator<string>
    {
        public string Constant { get; }

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
