namespace SeedGenerator.Lib.MetaData.Generators
{
    public class MetaDataGeneratorFixedValue : MetaDataGeneratorBase
    {
        public string FixedValue { get; }

        public MetaDataGeneratorFixedValue(string name, string fixedvalue)
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
