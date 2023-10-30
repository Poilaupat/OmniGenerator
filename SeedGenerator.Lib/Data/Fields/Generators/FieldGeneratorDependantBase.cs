namespace SeedGenerator.Lib.Data.Fields.Generators
{
    internal abstract class FieldGeneratorDependantBase : FieldGeneratorBase
    {
        public string DependantUpon { get; }

        public string? DependantValue { get; set; }


        public FieldGeneratorDependantBase(string name, string dependantUpon)
            : base(name)
        {
            DependantUpon = dependantUpon;
        }
    }
}
