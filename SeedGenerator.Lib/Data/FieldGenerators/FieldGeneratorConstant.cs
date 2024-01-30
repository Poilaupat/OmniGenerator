namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorConstant : AbstractFieldGenerator
    {
        public string Constant { get; }

        public FieldGeneratorConstant(string name, string constant)
            : base(name)
        {
            Constant = constant;
        }

        protected override object NextValue()
        {
            return Constant;
        }
    }
}
