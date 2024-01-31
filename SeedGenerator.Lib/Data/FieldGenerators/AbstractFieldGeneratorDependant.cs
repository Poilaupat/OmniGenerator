using System.Text.RegularExpressions;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal abstract class AbstractFieldGeneratorDependant : AbstractFieldGenerator
    {
        public List<string> DependenceNames { get; private set; } = new List<string>();

        public List<AbstractFieldGenerator> Dependences { get; } = new List<AbstractFieldGenerator>();


        protected AbstractFieldGeneratorDependant(string name, string dependentUpon)
            : base(name)
        {
            string[] dependenceNames = Regex.Replace(dependentUpon, @"\s", string.Empty).Split(new[] { ',', ';', '|' });

            foreach (var dependenceName in dependenceNames)
            {
                DependenceNames.Add(dependenceName);
            }
        }

        public bool IsDependentUpon(AbstractFieldGenerator generator)
        {
            foreach(var dependence in Dependences)
            {
                if(generator.Name == dependence.Name)
                    return true;

                if(dependence is AbstractFieldGeneratorDependant afgd)
                {
                    return afgd.IsDependentUpon(generator);
                }
            }

            return false;
        }
    }
}
