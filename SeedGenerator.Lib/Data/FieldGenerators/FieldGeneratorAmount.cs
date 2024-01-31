namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorAmount : AbstractFieldGenerator<int>
    {
        public int Min { get; }

        public int Max { get; }

        public FieldGeneratorAmount(string name, int min, int max)
            : base(name)
        {
            Min = min;
            Max = max;
        }

        protected override int GenerateValue()
        {
            return new Random().Next(Min, Max);
        }
    }
}
