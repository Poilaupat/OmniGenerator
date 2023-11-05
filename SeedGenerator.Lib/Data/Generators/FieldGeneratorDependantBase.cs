namespace SeedGenerator.Lib.Data.Generators
{
    internal abstract class FieldGeneratorDependantBase : FieldGeneratorBase
    {
        public Dictionary<string, string?> Dependances { get; set; } = new Dictionary<string, string?>();


        public FieldGeneratorDependantBase(string name, string dependantUpon)
            : base(name)
        {
            string[] dependanceNames = dependantUpon.Split(new[] { ',', ';', '|' });

            foreach (var dependanceName in dependanceNames)
            {
                Dependances[dependanceName] = null;
            }
        }
    }
}
