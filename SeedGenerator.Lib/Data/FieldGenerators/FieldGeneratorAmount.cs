namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorAmount : AbstractFieldGenerator
    {
        public float Min { get; }

        public float Max { get; }

        public EAmountFormat Format { get; }

        public FieldGeneratorAmount(string name, float min, float max, EAmountFormat format)
            : base(name)
        {
            Min = min;
            Max = max;
            Format = format;
        }

        protected override object NextValue()
        {
            return Format switch
            {
                EAmountFormat.Euro => NextFloatValue(),
                EAmountFormat.Cent => NextIntValue(),
                _ => throw new InvalidOperationException($"The value {Format} is not supported")
            };
        }

        private float NextFloatValue()
        {
            return new Random().Next((int)(Min * 100f), (int)(Max * 100f)) / 100f;
        }

        private int NextIntValue()
        {
            return new Random().Next((int)(Min) * 100, (int)(Max) * 100);
        }
    }
}
