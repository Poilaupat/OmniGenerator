using System.Text.RegularExpressions;

namespace SeedGenerator.Lib.DataGenerators
{
    internal abstract class AbstractFieldGeneratorDependant : AbstractFieldGenerator
    {
        public Dictionary<string, string?> Dependances { get; set; } = new Dictionary<string, string?>();


        public AbstractFieldGeneratorDependant(string name, string dependantUpon)
            : base(name)
        {
            string[] dependanceNames = Regex.Replace(dependantUpon, @"\s", string.Empty).Split(new[] { ',', ';', '|' });

            foreach (var dependanceName in dependanceNames)
            {
                Dependances[dependanceName] = null;
            }
        }
    }
}
