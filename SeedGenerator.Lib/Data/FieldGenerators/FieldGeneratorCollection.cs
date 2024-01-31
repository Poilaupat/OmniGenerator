using AutoMapper;
using Microsoft.ProgramSynthesis.Transformation.Text.Build.NodeTypes;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Param;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorCollection
    {
        public Dictionary<string, List<IFieldGenerator>> Generators { get; }
            = new Dictionary<string, List<IFieldGenerator>>();

        public FieldGeneratorCollection(RootParam rootParam, IMapper mapper)
        {
            //Root field generators (this is why the name 'root' is reserved in param)
            Generators.Add(RootParam.Name, mapper.Map<List<IFieldGenerator>>(rootParam.FieldParams));

            //Document field generators
            rootParam
                .RootGroupParam
                .GetDocumentParams(true)
                .ToDictionary(x => x.Name, y => mapper
                    .Map<List<IFieldGenerator>>(y.FieldParams)
                    .OrderBy(x => x, new FieldGeneratorComparer())
                    .ToList())
                .ToList()
                .ForEach(kvp => Generators.Add(kvp.Key, kvp.Value));

            //Group field generators
            rootParam
               .RootGroupParam
               .GetGroupParamsAndSelf(true)
               .ToDictionary(x => x.Name, y => mapper
                    .Map<List<IFieldGenerator>>(y.FieldParams)
                    .OrderBy(x => x, new FieldGeneratorComparer())
                    .ToList())
               .ToList()
               .ForEach(kvp => Generators.Add(kvp.Key, kvp.Value));

            Generators.SetCollateralDependencies();
        }

        public bool RootHasFields()
        {
            return Generators.ContainsKey(RootParam.Name);
        }

        public bool ElementHasFields(string name)
        {
            return Generators.ContainsKey(name);
        }

        private FieldCollection GenerateFields(IEnumerable<IFieldGenerator> generators)
        {
            var fields = new FieldCollection();

            foreach (var fieldGenerator in generators.FilterNonAggregateFieldGenerators())
            {
                fieldGenerator.RefreshValue();
                fields.Add(fieldGenerator.Name, new Field(fieldGenerator.Name, fieldGenerator.LastValue));
            }

            return fields;
        }

        private FieldCollection GenerateAggregateFields(IEnumerable<IFieldGenerator> generators, Group group)
        {
            var fields = new FieldCollection();

            foreach (var fieldGenerator in generators.FilterAggregateFieldGenerators())
            {
                fieldGenerator.Group = group;
                fieldGenerator.RefreshValue();
                fields.Add(fieldGenerator.Name, new Field(fieldGenerator.Name, fieldGenerator.LastValue));
            }

            return fields;
        }

        public FieldCollection GenerateFields(string name)
        {
            var generators = Generators[name];
            return GenerateFields(generators);
        }

        public FieldCollection GenerateAggregateFields(string name, Group group)
        {
            var generators = Generators[name];
            return GenerateAggregateFields(generators, group);
        }

        public FieldCollection GenerateRootFields()
        {
            return GenerateFields(RootParam.Name);
        }

    }
}
