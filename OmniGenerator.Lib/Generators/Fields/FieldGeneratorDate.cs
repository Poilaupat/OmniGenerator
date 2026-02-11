namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The <see cref="FieldGeneratorDate"/> produces random dates around the current date within a specified range of days.
    /// Example: If the range is 2 to 10, the generator will produce dates from 2 to 10 days prior to the current date.
    /// Bounds can be negative to produce dates ahead of the current date.
    /// </summary>
    internal class FieldGeneratorDate : AbstractFieldGenerator<DateTime>
    {
        private readonly Random _random;

        /// <summary>
        /// Gets or sets the minimum bound of the day difference range. Can be negative.
        /// </summary>
        public int DayDiffMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum bound of the day difference range. Can be negative.
        /// </summary>
        public int DayDiffMax { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGeneratorDate"/> class.
        /// Dates are generated relative to the current date.
        /// The bounds define a range that is used to randomly generate an offset (in days) which is added to the current date.
        /// The bounds can be negative.
        /// </summary>
        /// <param name="name">The name of the generator.</param>
        /// <param name="dayDiffMin">The minimum bound of the range (inclusive).</param>
        /// <param name="dayDiffMax">The maximum bound of the range (inclusive).</param>
        public FieldGeneratorDate(string name, int dayDiffMin, int dayDiffMax) : base(name)
        {
            DayDiffMin = Math.Min(dayDiffMin, dayDiffMax);
            DayDiffMax = Math.Max(dayDiffMin, dayDiffMax) + 1;
            _random = new Random();
        }

        /// <summary>
        /// Generates a random <see cref="DateTime"/> value within the specified day difference range relative to now.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> value representing a random date within the specified range.</returns>
        protected override DateTime GenerateValue()
        {
            int diff = _random.Next(DayDiffMin, DayDiffMax);
            return DateTime.Now.AddDays(diff);
        }
    }
}
