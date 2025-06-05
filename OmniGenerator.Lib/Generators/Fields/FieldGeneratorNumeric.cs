namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The <see cref="FieldGeneratorNumeric"/> is a kind of generator that returns numeric values between a specified range.
    /// </summary>
    internal class FieldGeneratorNumeric : AbstractFieldGenerator<int>
    {
        private readonly Random _random;

        /// <summary>
        /// Gets the lower bound of the range.
        /// </summary>
        public int Min { get; }

        /// <summary>
        /// Gets the higher bound of the range (exclusive).
        /// </summary>
        public int Max { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGeneratorNumeric"/> class.
        /// </summary>
        /// <param name="name">The generator name.</param>
        /// <param name="min">The lower bound of the range (inclusive).</param>
        /// <param name="max">The higher bound of the range (exclusive).</param>
        public FieldGeneratorNumeric(string name, int min, int max)
            : base(name)
        {
            Min = min;
            Max = max;
            _random = new Random();
        }

        /// <summary>
        /// Generates a new random integer value within the specified range.
        /// </summary>
        /// <returns>
        /// A random integer greater than or equal to <see cref="Min"/> and less than <see cref="Max"/>.
        /// </returns>
        protected override int GenerateValue()
        {
            return _random.Next(Min, Max);
        }
    }
}
