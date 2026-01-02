using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Interfaces.FieldGenerators;
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
        private IEnumerable<IFieldGenerator> _generators;

        /// <summary>
        /// Gets the name of the field generator collection. This is the name of the element owning the field generators
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the collection contains any regular generators.
        /// </summary>
        public bool HasRegularGenerators { get; } = false;

        /// <summary>
        /// Gets a value indicating whether the collection contains any aggregate generators.
        /// </summary>
        public bool HasAggregateGenerators { get; } = false;

        /// <summary>
        /// Gets a value indicating whether the collection contains any dependent generators.
        /// </summary>
        public bool HasDependentGenerators { get; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGeneratorCollection"/> class.
        /// </summary>
        /// <param name="name">The name of the collection.</param>
        /// <param name="generators">The field generators to include in the collection.</param>
        public FieldGeneratorCollection(string name, IEnumerable<IFieldGenerator> generators)
        {
            _generators = generators;
            Name = name;
            HasRegularGenerators = _generators.FilterRegularFieldGenerators().Any();
            HasAggregateGenerators = _generators.FilterAggregateFieldGenerators().Any();
            HasDependentGenerators = _generators.FilterDependantFieldGenerators().Any();
            _generators.SetCollateralDependencies();
        }

        /// <summary>
        /// Refreshes the value of the regular generators and uses those values to build a <see cref="FieldCollection"/>.
        /// By "regular" generators, this means all generators except aggregates. Aggregates must be generated separately and after all other generators.
        /// </summary>
        /// <returns>
        /// A dictionary containing the generated fields, where the key is the field name and the value is the <see cref="Field"/> instance.
        /// </returns>
        public IDictionary<string, Field> GenerateRegularFields()
        {
            lock (_lock)
            {
                var fields = new Dictionary<string, Field>();
                if (HasRegularGenerators)
                {
                    Console.WriteLine($"**********************************************************");
                    foreach (var fieldGenerator in _generators.FilterRegularFieldGenerators())
                    {
                        var value = fieldGenerator.GenerateNextValue();
                        fields.Add(fieldGenerator.Name, new Field(fieldGenerator.Name, value));
                    }
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
            lock (_lock)
            {
                var fields = new Dictionary<string, Field>();

                if (HasAggregateGenerators)
                {
                    foreach (var fieldGenerator in _generators.FilterAggregateFieldGenerators())
                    {
                        fieldGenerator.Group = group;
                        var value = fieldGenerator.GenerateNextValue();
                        fields.Add(fieldGenerator.Name, new Field(fieldGenerator.Name, value));
                    }
                }

                return fields;
            }
        }
    }
}
