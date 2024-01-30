namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorDate : AbstractFieldGenerator
    {
        public int DayDiffMin { get; set; }
        public int DayDiffMax { get; set; }

        public FieldGeneratorDate(string name, int dayDiffMin, int dayDiffMax) : base(name)
        {
            DayDiffMin = dayDiffMin;
            DayDiffMax = dayDiffMax;
        }

        protected override object NextValue()
        {
            int diff = new Random().Next(DayDiffMin, DayDiffMax);
            return DateTime.Today.AddDays(-diff).ToString("dd/MM/yyyy");
        }
    }
}
