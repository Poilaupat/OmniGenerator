using Mustache;
using SeedGenerator.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal static class FieldGeneratorExtensions
    {
        public static IEnumerable<IFieldGenerator> FilterNonAggregateFieldGenerators(this IEnumerable<IFieldGenerator> fields)
        {
            return fields
               .Where(x => x is not FieldGeneratorAggregate)
               .OrderBy(x => x, new FieldGeneratorComparer())
               .ToList();
        }

        public static IEnumerable<IFieldGeneratorDependent> FilterDependantFieldGenerators(this IEnumerable<IFieldGenerator> fields)
        {
            return fields
                .Where(x => x is not FieldGeneratorAggregate && x is IFieldGeneratorDependent)
                .Cast<IFieldGeneratorDependent>()
                .OrderBy(x => x, new FieldGeneratorComparer())
                .ToList();
        }

        public static IEnumerable<FieldGeneratorAggregate> FilterAggregateFieldGenerators(this IEnumerable<IFieldGenerator> fields)
        {
            return fields
                .Where(x => x.GetType() == typeof(FieldGeneratorAggregate))
                .Cast<FieldGeneratorAggregate>()
                .ToList();
        }

        public static Dictionary<string, List<IFieldGenerator>> SetCollateralDependencies(this Dictionary<string, List<IFieldGenerator>> generatorsByElements)
        {
            foreach(var generators in generatorsByElements.Values)
            {
                SetCollateralDependencies(generators);
            }

            return generatorsByElements;
        }

        public static IEnumerable<IFieldGenerator> SetCollateralDependencies(this IEnumerable<IFieldGenerator> generators)
        {
            foreach (var generator in generators.FilterDependantFieldGenerators())
            {
                foreach (var dependencyName in generator.DependenceNames)
                {
                    var dependency = generators.Single(x => x.Name == dependencyName);
                    generator.Dependences.Add(dependency);
                }
            }

            return generators;
        }
    }
}
