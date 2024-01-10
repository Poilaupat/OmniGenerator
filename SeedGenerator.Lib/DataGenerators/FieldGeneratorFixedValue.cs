namespace SeedGenerator.Lib.DataGenerators
{
    internal class FieldGeneratorFixedValue : AbstractFieldGenerator
    {
        public string FixedValue { get; }

        public FieldGeneratorFixedValue(string name, string fixedvalue)
            : base(name)
        {
            FixedValue = fixedvalue;
        }

        public override string NextValue()
        {
            return FixedValue;
        }
    }
}
