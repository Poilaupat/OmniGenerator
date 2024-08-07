using AutoMapper;
using Microsoft.ProgramSynthesis.Utils.Interactive;
using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Configuration;
using System.Data;
using OmniGenerator.Lib.Interfaces.FieldGenerators;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// Manages <see cref="AbstractFieldGenerator{T}"/> by <see cref="Document"/> or <see cref="Group"/>
    /// </summary>
    internal class FieldGeneratorCollection
    {
        /// <summary>
        /// The generators by <see cref="Document"/> or <see cref="Group"/>
        /// </summary>
        private Dictionary<string, List<IFieldGenerator>> _generators = new Dictionary<string, List<IFieldGenerator>>();

        /// <summary>
        /// Creates a new <see cref="FieldGeneratorCollection"/> from configuration classes
        /// </summary>
        /// <param name="rootConfiguration">The root configuration</param>
        /// <param name="mapper">The mapper that projects configuration to fields</param>
        public FieldGeneratorCollection(RootConfiguration rootConfiguration, IMapper mapper)
        {
            //Root field generators (this is why the name 'root' is reserved in param)
            _generators.Add(RootConfiguration.Name, mapper.Map<List<IFieldGenerator>>(rootConfiguration.Fields));

            //Document field generators
            rootConfiguration
                .Group
                .GetDocumentsConfiguration(true)
                .ToDictionary(x => x.Name, y => mapper
                    .Map<List<IFieldGenerator>>(y.Fields)
                    .ToList())
                .ToList()
                .ForEach(kvp => _generators.Add(kvp.Key, kvp.Value));
            
            //Group field generators
            rootConfiguration
               .Group
               .GetGroupsAndSelfConfiguration(true)
               .ToDictionary(x => x.Name, y => mapper
                    .Map<List<IFieldGenerator>>(y.Fields)
                    .ToList())
               .ToList()
               .ForEach(kvp => _generators.Add(kvp.Key, kvp.Value));

            SetCollateralDependencies();
        }

        /// <summary>
        /// Indicates if the root element has fields generators
        /// </summary>
        /// <returns>True if root has at least one field. False if not.</returns>
        public bool RootHasFields()
        {
            return _generators.ContainsKey(RootConfiguration.Name);
        }

        /// <summary>
        /// Indicates if the element specified by its name has fields generators
        /// </summary>
        /// <returns>True if the element has at least one field. False if not.</returns>
        public bool ElementHasFields(string name)
        {
            return _generators.ContainsKey(name);
        }

        /// <summary>
        /// Refresh the value of the regular generators in the specified list, uses those values to build a <see cref="FieldCollection"/>
        /// By "regular" generators, we mean all generators but aggregates. The aggregate must be generated separately and after all other generators
        /// </summary>
        /// <param name="generators">The generators to refresh</param>
        /// <returns>A field collection</returns>
        private FieldCollection GenerateFields(IEnumerable<IFieldGenerator> generators)
        {
            var fields = new FieldCollection();

            foreach (var fieldGenerator in generators.FilterNonAggregateFieldGenerators())
            {
                fieldGenerator.RefreshValue();
                fields.Add(fieldGenerator.Name, fieldGenerator.LastValue);
            }

            return fields;
        }

        /// <summary>
        /// Refresh the value of the aggregates generators is the specified list, uses those values to build a <see cref="FieldCollection"/>
        /// </summary>
        /// <param name="generators">The generators to refresh</param>
        /// <returns>A <see cref="FieldCollection"/></returns>
        private FieldCollection GenerateAggregateFields(IEnumerable<IFieldGenerator> generators, Group group)
        {
            var fields = new FieldCollection();

            foreach (var fieldGenerator in generators.FilterAggregateFieldGenerators())
            {
                fieldGenerator.Group = group;
                fieldGenerator.RefreshValue();
                fields.Add(fieldGenerator.Name, fieldGenerator.LastValue);
            }

            return fields;
        }


        /// <summary>
        /// Refreshes the regular fields of the element specified by its name, uses those values to build a <see cref="FieldCollection"/>
        /// </summary>
        /// <param name="name">The name of the element to refresh</param>
        /// <returns>A <see cref="FieldCollection"/></returns>
        public FieldCollection GenerateFields(string name)
        {
            var generators = _generators[name];
            return GenerateFields(generators);
        }

        /// <summary>
        /// Refreshes the aggregate fields of the element specified by its name, uses those values to build a <see cref="FieldCollection"/>
        /// </summary>
        /// <param name="name">The name of the element to refresh</param>
        /// <returns>A <see cref="FieldCollection"/></returns>
        public FieldCollection GenerateAggregateFields(string name, Group group)
        {
            var generators = _generators[name];
            return GenerateAggregateFields(generators, group);
        }

        /// <summary>
        /// Refreshes the regular fields of the root element, uses those values to build a <see cref="FieldCollection"/>
        /// Note that root element cannot contain aggregate fields
        /// </summary>
        /// <returns>A <see cref="FieldCollection"/></returns>
        public FieldCollection GenerateRootFields()
        {
            return GenerateFields(RootConfiguration.Name);
        }

        /// <summary>
        /// Sets the Dependencies (from dependency names) of all <see cref="AbstractFieldGeneratorDependant{T}"/> generators 
        /// </summary>
        public void SetCollateralDependencies()
        {
            foreach (var generators in _generators.Values)
            {
                generators.SetCollateralDependencies();
            }
        }
    }
}
