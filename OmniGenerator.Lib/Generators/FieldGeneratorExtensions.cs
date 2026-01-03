using OmniGenerator.Lib.Exceptions;
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
        /// Sets the Dependencies (from dependency names) of <see cref="AbstractFieldGeneratorDependant{T}"/> generators in the provided list of field generators
        /// </summary>
        /// <param name="generators">The list of field generators</param>
        /// <returns>The enriched list</returns>
        public static IEnumerable<IFieldGenerator> SetCollateralDependencies(this IEnumerable<IFieldGenerator> generators)
        {
            foreach (var generator in generators
                .Where(x => x is IFieldGeneratorDependent && x is not FieldGeneratorAggregate)
                .Cast<IFieldGeneratorDependent>())
            {
                foreach (var dependencyName in generator.DependenceNames)
                {
                    var dependency = generators.Single(x => x.Name == dependencyName);
                    generator.GeneratorDependencies.Add(dependency);
                }
            }
            return generators;
        }

        /// <summary>
        /// Performs a topological sort on the field generators using Kahn's algorithm.
        /// </summary>
        /// <param name="generators">The collection of field generators to sort.</param>
        /// <returns>An ordered list where all dependencies come before their dependents.</returns>
        /// <exception cref="ConfigurationException">Thrown if a circular dependency is detected.</exception>
        public static IList<IFieldGenerator> TopologicalSort(this IEnumerable<IFieldGenerator> generators)
        {
            var generatorList = generators.ToList();
            var result = new List<IFieldGenerator>();

            // Calculate in-degree (number of incoming edges) for each node
            var inDegree = new Dictionary<string, int>();
            var adjacencyList = new Dictionary<string, List<IFieldGenerator>>();
            var generatorMap = generatorList.ToDictionary(g => g.Name);

            // Initialize
            foreach (var gen in generatorList)
            {
                inDegree[gen.Name] = 0;
                adjacencyList[gen.Name] = new List<IFieldGenerator>();
            }

            // Build adjacency list and calculate in-degrees
            foreach (var gen in generatorList.OfType<IFieldGeneratorDependent>())
            {
                foreach (var dep in gen.GeneratorDependencies)
                {
                    // dep -> gen (dependency points to dependent)
                    adjacencyList[dep.Name].Add(gen);
                    inDegree[gen.Name]++;
                }
            }

            // Queue all nodes with in-degree 0 (independent generators)
            var queue = new Queue<IFieldGenerator>(
                generatorList.Where(g => inDegree[g.Name] == 0)
            );

            // Process queue
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                result.Add(current);

                // For each generator that depends on current
                foreach (var dependent in adjacencyList[current.Name])
                {
                    inDegree[dependent.Name]--;

                    // If all dependencies are resolved, add to queue
                    if (inDegree[dependent.Name] == 0)
                    {
                        queue.Enqueue(dependent);
                    }
                }
            }

            // Check for cycles
            if (result.Count != generatorList.Count)
            {
                var remaining = generatorList.Except(result).Select(g => g.Name);
                throw new ConfigurationException(
                    $"Circular dependency detected among fields: {string.Join(", ", remaining)}"
                );
            }

            return result;
        }

    }
}
