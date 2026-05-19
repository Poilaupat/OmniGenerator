using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Generators.Interfaces;
using OmniGenerator.Lib.Hierarchy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Generators
{
    /// <summary>
    /// Represents a collection of field generators and provides methods to generate fields
    /// using regular, aggregate, and dependent generators.
    /// </summary>
    internal class FieldGeneratorCollection
    {
        private readonly object _lock = new object();
        private readonly IReadOnlyList<IFieldGenerator> _generators;

        /// <summary>
        /// Gets the name of the field generator collection. This is the name of the element owning the field generators
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGeneratorCollection"/> class.
        /// </summary>
        /// <param name="name">The name of the collection.</param>
        /// <param name="generators">The field generators to include in the collection.</param>
        public FieldGeneratorCollection(string name, IEnumerable<IFieldGenerator> generators)
        {
            Name = name;

            _generators = generators
                .SetCollateralDependencies()
                .TopologicalSort()
                .ToList();
        }

        /// <summary>
        /// Refreshes the value of the regular generators and uses those values to build a <see cref="FieldCollection"/>.
        /// By "regular" generators, this means all generators except aggregates. Aggregates must be generated separately and after all other generators.
        /// </summary>
        /// <returns>
        /// A dictionary containing the generated fields, where the key is the field name and the value is the <see cref="Field"/> instance.
        /// </returns>
        public IDictionary<string, Field> GenerateFields()
        {
            lock (_lock)
            {
                var fields = new Dictionary<string, Field>();
                foreach (var fieldGenerator in _generators.Where(x => x is not FieldGeneratorAggregate))
                {
                    var value = fieldGenerator.GenerateNextValue();
                    fields.Add(fieldGenerator.Name, new Field(fieldGenerator.Name, value));
                }
                return fields;
            }
        }

        /// <summary>
        /// Refreshes the value of the aggregate generators and uses those values to build a <see cref="FieldCollection"/>.
        /// </summary>
        /// <param name="group">The group context to use for aggregate field generation.</param>
        /// <returns>
        /// A dictionary containing the generated aggregate fields, where the key is the field name and the value is the <see cref="Field"/> instance.
        /// </returns>
        public IDictionary<string, Field> GenerateAggregateFields(Group group)
        {
            // IMPORTANT: The lock is mandatory here and must never be removed.
            // FieldGeneratorAggregate instances are shared across all groups of the same type
            // (one FieldGeneratorCollection per element name in FieldGeneratorContainer).
            // In Release mode, groups are generated in parallel (Parallel.For in HierarchyBuilder).
            // The lock serializes access so that Group is set and consumed atomically,
            // preventing a race condition where one thread overwrites Group before another thread reads it.
            lock (_lock)
            {
                var fields = new Dictionary<string, Field>();

                foreach (var fieldGenerator in _generators.OfType<FieldGeneratorAggregate>())
                {
                    fieldGenerator.Group = group;
                    var value = fieldGenerator.GenerateNextValue();
                    fields.Add(fieldGenerator.Name, new Field(fieldGenerator.Name, value));
                }

                return fields;
            }
        }


    }
}
