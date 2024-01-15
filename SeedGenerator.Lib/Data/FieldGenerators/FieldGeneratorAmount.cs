namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorAmount : AbstractFieldGenerator
    {
        public float Min { get; }

        public float Max { get; }

        public FieldGeneratorAmount(string name, float min, float max)
            : base(name)
        {
            Min = min;
            Max = max;
        }

        public override string NextValue()
        {
            return (new Random().Next((int)(Min * 100f), (int)(Max * 100f)) / 100f).ToString();
        }
    }
}
