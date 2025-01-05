using AutoMapper;
using OmniGenerator.Lib.Interfaces.FieldGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Generators.Fields
{
    internal class FieldGeneratorCollection
    {
        private IEnumerable<IFieldGenerator> _generators;

        public string Name { get; }

        public bool HasRegularGenerators { get; } = false;

        public bool HasAggregateGenerators { get; } = false;

        public bool HasDependentGenerators { get; } = false;

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
        /// Refresh the value of the regular generators, uses those values to build a <see cref="FieldCollection"/>
        /// By "regular" generators, we mean all generators but aggregates. The aggregate must be generated separately and after all other generators
        /// </summary>
        /// <returns>A field collection</returns>
        public FieldCollection GenerateRegularFields()
        {
            var fields = new FieldCollection();
            if (HasRegularGenerators)
            {
                foreach (var fieldGenerator in _generators.FilterRegularFieldGenerators())
                {
                    fieldGenerator.RefreshValue();
                    fields.Add(fieldGenerator.Name, fieldGenerator.LastValue);
                }
            }
            return fields;
        }

        /// <summary>
        /// Refresh the value of the aggregates generators, uses those values to build a <see cref="FieldCollection"/>
        /// </summary>
        /// <returns>A <see cref="FieldCollection"/></returns>
        public FieldCollection GenerateAggregateFields(Group group)
        {
            var fields = new FieldCollection();

            if (HasAggregateGenerators)
            {
                foreach (var fieldGenerator in _generators.FilterAggregateFieldGenerators())
                {
                    fieldGenerator.Group = group;
                    fieldGenerator.RefreshValue();
                    fields.Add(fieldGenerator.Name, fieldGenerator.LastValue);
                }
            }

            return fields;
        }
    }
}
