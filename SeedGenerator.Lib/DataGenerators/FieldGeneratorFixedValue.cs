namespace SeedGenerator.Lib.Builders
{
    internal class FieldGeneratorFixedValue : FieldGeneratorBase
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
