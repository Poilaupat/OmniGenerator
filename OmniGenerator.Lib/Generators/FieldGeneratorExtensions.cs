using Mustache;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Interfaces.FieldGenerators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Generators
{
    /// <summary>
    /// Provides usefull extensions to <see cref="IFieldGenerator"/> collections
    /// </summary>
    internal static class FieldGeneratorExtensions
    {
        /// <summary>
        /// Filters the provided <see cref="IFieldGenerator"/> list an returns all fields but <see cref="FieldGeneratorAggregate"/>
        /// </summary>
        /// <param name="fields">The list of <see cref="IFieldGenerator"/> to filter</param>
        /// <returns>The filtered list</returns>
        public static IEnumerable<IFieldGenerator> FilterRegularFieldGenerators(this IEnumerable<IFieldGenerator> fields)
        {
            return fields
               .Where(x => x is not FieldGeneratorAggregate)
               .OrderBy(x => x, new FieldGeneratorComparer());
        }

        /// <summary>
        /// Filters the provided <see cref="IFieldGenerator"/> list an returns only <see cref="IFieldGeneratorDependent"/>
        /// </summary>
        /// <param name="fields">The list of <see cref="IFieldGenerator"/> to filter</param>
        /// <returns>The filtered list</returns>
        public static IEnumerable<IFieldGeneratorDependent> FilterDependantFieldGenerators(this IEnumerable<IFieldGenerator> fields)
        {
            return fields
                .Where(x => x is not FieldGeneratorAggregate && x is IFieldGeneratorDependent)
                .Cast<IFieldGeneratorDependent>()
                .OrderBy(x => x, new FieldGeneratorComparer());
        }

        /// <summary>
        /// Filters the provided <see cref="IFieldGenerator"/> list an returns only <see cref="FieldGeneratorAggregate"/>
        /// </summary>
        /// <param name="fields">The list of <see cref="IFieldGenerator"/> to filter</param>
        /// <returns>The filtered list</returns>
        public static IEnumerable<FieldGeneratorAggregate> FilterAggregateFieldGenerators(this IEnumerable<IFieldGenerator> fields)
        {
            return fields
                .Where(x => x.GetType() == typeof(FieldGeneratorAggregate))
                .Cast<FieldGeneratorAggregate>();
        }

        /// <summary>
        /// Sets the Dependencies (from dependency names) of <see cref="AbstractFieldGeneratorDependant{T}"/> generators in the provided list of field generators
        /// </summary>
        /// <param name="generatorsByElements">The list of field generators</param>
        /// <returns>The enriched list</returns>
        public static IEnumerable<IFieldGenerator> SetCollateralDependencies(this IEnumerable<IFieldGenerator> generators)
        {
            foreach (var generator in generators.FilterDependantFieldGenerators())
            {
                foreach (var dependencyName in generator.DependenceNames)
                {
                    var dependency = generators.Single(x => x.Name == dependencyName);
                    generator.GeneratorDependencies.Add(dependency);
                }
            }

            return generators;
        }
    }
}
