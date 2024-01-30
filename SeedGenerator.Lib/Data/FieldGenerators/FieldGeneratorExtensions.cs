using Mustache;
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
        public static IEnumerable<AbstractFieldGenerator> FilterNonAggregateFieldGenerators(this IEnumerable<AbstractFieldGenerator> fields)
        {
            return fields
               .Where(x => x is not FieldGeneratorAggregate)
               .OrderBy(x => x, new FieldGeneratorComparer())
               .ToList();
        }

        public static IEnumerable<AbstractFieldGeneratorDependant> FilterDependantFieldGenerators(this IEnumerable<AbstractFieldGenerator> fields)
        {
            return fields
                .Where(x => x is not FieldGeneratorAggregate && x.GetType().IsSubclassOf(typeof(AbstractFieldGeneratorDependant)))
                .Cast<AbstractFieldGeneratorDependant>()
                .OrderBy(x => x, new FieldGeneratorComparer())
                .ToList();
        }

        public static IEnumerable<FieldGeneratorAggregate> FilterAggregateFieldGenerators(this IEnumerable<AbstractFieldGenerator> fields)
        {
            return fields
                .Where(x => x.GetType() == typeof(FieldGeneratorAggregate))
                .Cast<FieldGeneratorAggregate>()
                .ToList();
        }
    }
}
