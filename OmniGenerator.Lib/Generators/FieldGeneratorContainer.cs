using AutoMapper;
using Microsoft.ProgramSynthesis.Utils.Interactive;
using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Configuration;
using System.Data;
using OmniGenerator.Lib.Interfaces.FieldGenerators;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Generators.Fields;

namespace OmniGenerator.Lib.Generators
{
    /// <summary>
    /// Manages <see cref="AbstractFieldGenerator{T}"/> by <see cref="Document"/> or <see cref="Group"/>
    /// </summary>
    public class FieldGeneratorContainer
    {
        /// <summary>
        /// The generators by <see cref="Document"/> or <see cref="Group"/>
        /// </summary>
        private Dictionary<string, FieldGeneratorCollection> _generators = new();

        /// <summary>
        /// Creates a new <see cref="FieldGeneratorContainer"/> from configuration classes
        /// </summary>
        /// <param name="rootConfiguration">The root configuration</param>
        /// <param name="mapper">The mapper that projects configuration to fields</param>
        public FieldGeneratorContainer(HierarchyConfiguration rootConfiguration, IMapper mapper)
        {
            //Root field generators (this is why the name 'root' is reserved in param)
            _generators.Add(
                HierarchyConfiguration.Name,
                new FieldGeneratorCollection(HierarchyConfiguration.Name, mapper.Map<List<IFieldGenerator>>(rootConfiguration.Fields)));

            //Document field generators
            rootConfiguration
                .Root
                .GetDocumentsConfiguration(true)
                .Select(x => new { x.Name, Fields = mapper.Map<List<IFieldGenerator>>(x.Fields) })
                .ForEach(x => _generators.Add(x.Name, new FieldGeneratorCollection(x.Name, x.Fields)));

            //Group field generators
            rootConfiguration
               .Root
               .GetGroupsAndSelfConfiguration(true)
               .Select(x => new { x.Name, Fields = mapper.Map<List<IFieldGenerator>>(x.Fields) })
               .ForEach(x => _generators.Add(x.Name, new FieldGeneratorCollection(x.Name, x.Fields)));
        }

        /// <summary>
        /// Indicates if the root element has fields generators
        /// </summary>
        /// <returns>True if root has at least one field. False if not.</returns>
        public bool RootHasFields() => _generators.ContainsKey(HierarchyConfiguration.Name);

        /// <summary>
        /// Indicates if the element specified by its name has fields generators
        /// </summary>
        /// <returns>True if the element has at least one field. False if not.</returns>
        public bool ElementHasFields(string name) => _generators.ContainsKey(name);

        /// <summary>
        /// Refreshes the regular fields of the element specified by its name, uses those values to build a <see cref="FieldCollection"/>
        /// </summary>
        /// <param name="name">The name of the element to refresh</param>
        /// <returns>A <see cref="FieldCollection"/></returns>
        public IDictionary<string, Field> GenerateRegularFields(string name) => _generators[name].GenerateRegularFields();

        /// <summary>
        /// Refreshes the aggregate fields of the specified scope of the element specified by its name, uses those values to build a <see cref="FieldCollection"/>
        /// </summary>
        /// <param name="name">The name of the element to refresh</param>
        /// <returns>A <see cref="FieldCollection"/></returns>
        public IDictionary<string, Field> GenerateAggregateFields(string name, Group group) => _generators[name].GenerateAggregateFields(group);

        /// <summary>
        /// Refreshes the regular fields of the root element, uses those values to build a <see cref="FieldCollection"/>
        /// Note that root element cannot contain aggregate fields
        /// </summary>
        /// <returns>A <see cref="FieldCollection"/></returns>
        public IDictionary<string, Field> GenerateRootFields() => GenerateRegularFields(HierarchyConfiguration.Name);
    }
}
