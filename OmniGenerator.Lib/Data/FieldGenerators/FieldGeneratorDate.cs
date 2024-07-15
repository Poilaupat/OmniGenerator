namespace SeedGenerator.Lib.Data.FieldGenerators
{
    /// <summary>
    /// The <see cref="FieldGeneratorDate"/> produces random dates around current date within specified range of days
    /// Example : If the range is 2 to 10 the generator will produce dates from 2 to 10 days prior to the current date
    /// Bounds can be negative to produce dates ahead of the current date
    /// </summary>
    internal class FieldGeneratorDate : AbstractFieldGenerator<DateTime>
    {
        /// <summary>
        /// The range min bound. Can be negative
        /// </summary>
        public int DayDiffMin { get; set; }

        /// <summary>
        /// The range max bound. Can be negative
        /// </summary>
        public int DayDiffMax { get; set; }

        /// <summary>
        /// Creates a new <see cref="FieldGeneratorDate"/>
        /// </summary>
        /// <param name="name">The name of the generator</param>
        /// <param name="dayDiffMin">The range min bound</param>
        /// <param name="dayDiffMax">The range max bound</param>
        public FieldGeneratorDate(string name, int dayDiffMin, int dayDiffMax) : base(name)
        {
            DayDiffMin = dayDiffMin;
            DayDiffMax = dayDiffMax;
        }

        protected override DateTime GenerateValue()
        {
            int diff = new Random().Next(DayDiffMin, DayDiffMax);
            return DateTime.Today.AddDays(-diff);
        }
    }
}
