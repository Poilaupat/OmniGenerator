using SeedGenerator.Lib.Interfaces;
using System.Text.RegularExpressions;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal abstract class AbstractFieldGeneratorDependant<T> : AbstractFieldGenerator<T>, IFieldGeneratorDependent
        where T : notnull
    {
        public List<string> DependenceNames { get; private set; } = new List<string>();

        public List<IFieldGenerator> Dependences { get; } = new List<IFieldGenerator>();


        protected AbstractFieldGeneratorDependant(string name, string dependentUpon)
            : base(name)
        {
            string[] dependenceNames = Regex.Replace(dependentUpon, @"\s", string.Empty).Split(new[] { ',', ';', '|' });

            foreach (var dependenceName in dependenceNames)
            {
                DependenceNames.Add(dependenceName);
            }
        }

        public bool IsDependentUpon(IFieldGenerator generator)
        {
            foreach(var dependence in Dependences)
            {
                if(generator.Name == dependence.Name)
                    return true;

                if(dependence is IFieldGeneratorDependent dependentDependence)
                {
                    return dependentDependence.IsDependentUpon(generator);
                }
            }

            return false;
        }
    }
}
