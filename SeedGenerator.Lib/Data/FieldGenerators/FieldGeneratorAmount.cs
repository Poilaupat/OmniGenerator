namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorAmount : AbstractFieldGenerator
    {
        public int Min { get; }

        public int Max { get; }

        public FieldGeneratorAmount(string name, int min, int max)
            : base(name)
        {
            Min = min;
            Max = max;
        }

        protected override object NextValue()
        {
            return new Random().Next(Min, Max);
        }
    }
}
