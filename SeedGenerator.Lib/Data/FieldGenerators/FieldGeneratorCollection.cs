using AutoMapper;
using Microsoft.ProgramSynthesis.Transformation.Text.Build.NodeTypes;
using SeedGenerator.Lib.Param;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorCollection
    {
        public Dictionary<string, List<AbstractFieldGenerator>> Generators { get; }
            = new Dictionary<string, List<AbstractFieldGenerator>>();

        public FieldGeneratorCollection(RootParam rootParam, IMapper mapper)
        {
            //Root field generators (this is why the name 'root' is reserved in param)
            Generators.Add(RootParam.Name, mapper.Map<List<AbstractFieldGenerator>>(rootParam.FieldParams));

            //Document field generators
            rootParam
                .RootGroupParam
                .GetDocumentParams(true)
                .ToDictionary(x => x.Name, y => mapper
                    .Map<List<AbstractFieldGenerator>>(y.FieldParams)
                    .OrderBy(x => x, new FieldGeneratorComparer())
                    .ToList())
                .ToList()
                .ForEach(kvp => Generators.Add(kvp.Key, kvp.Value));

            //Group field generators
            rootParam
               .RootGroupParam
               .GetGroupParamsAndSelf(true)
               .ToDictionary(x => x.Name, y => mapper
                    .Map<List<AbstractFieldGenerator>>(y.FieldParams)
                    .OrderBy(x => x, new FieldGeneratorComparer())
                    .ToList())
               .ToList()
               .ForEach(kvp => Generators.Add(kvp.Key, kvp.Value));

            SetCollateralDependencies();
        }
        public bool RootHasFields()
        {
            return Generators.ContainsKey(RootParam.Name);
        }

        public bool ElementHasFields(string name)
        {
            return Generators.ContainsKey(name);
        }

        private void SetCollateralDependencies()
        {
            foreach (var elementGenerators in Generators.Values)
            {
                foreach (var generator in elementGenerators.FilterDependantFieldGenerators())
                {
                    foreach (var dependencyName in generator.DependenceNames)
                    {
                        var dependency = elementGenerators.Single(x => x.Name == dependencyName);
                        generator.Dependences.Add(dependency);
                    }
                }
            }
        }

        private FieldCollection GenerateFields(IEnumerable<AbstractFieldGenerator> generators)
        {
            var fields = new FieldCollection();

            foreach (var fieldGenerator in generators.FilterNonAggregateFieldGenerators())
            {
                fieldGenerator.SetNewValue();
                fields.Add(fieldGenerator.Name, new Field(fieldGenerator.Name, fieldGenerator.LastValue));
            }

            return fields;
        }

        private FieldCollection GenerateAggregateFields(IEnumerable<AbstractFieldGenerator> generators, Group group)
        {
            var fields = new FieldCollection();

            foreach (var fieldGenerator in generators.FilterAggregateFieldGenerators())
            {
                fieldGenerator.Group = group;
                fieldGenerator.SetNewValue();
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
