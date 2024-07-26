namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The <see cref="FieldGeneratorNumeric"/> is kind of generator that returns numeric values between a range
    /// </summary>
    internal class FieldGeneratorNumeric : AbstractFieldGenerator<int>
    {
        /// <summary>
        /// The lower bound of the range
        /// </summary>
        public int Min { get; }

        /// <summary>
        /// The higher bound of the range
        /// </summary>
        public int Max { get; }

        /// <summary>
        /// Creates a new <see cref="FieldGeneratorNumeric"/>
        /// </summary>
        /// <param name="name">The generator name</param>
        /// <param name="min">The lower bound of the range</param>
        /// <param name="max">The higher bound of the range</param>
        public FieldGeneratorNumeric(string name, int min, int max)
            : base(name)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Generates a new value
        /// </summary>
        /// <returns></returns>
        protected override int GenerateValue()
        {
            return new Random().Next(Min, Max);
        }
    }
}
