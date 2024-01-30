using AutoMapper;
using SeedGenerator.Lib.Param;
using System.Data;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorCollection
    {
        public List<AbstractFieldGenerator> RootFieldGenerators { get; }
        public Dictionary<string, List<AbstractFieldGenerator>> GroupFieldGenerators { get; }
        public Dictionary<string, List<AbstractFieldGenerator>> DocumentFieldGenerators { get; }

        public FieldGeneratorCollection(RootParam rootParam, IMapper mapper)
        {
            //Root field generators
            RootFieldGenerators = mapper.Map<List<AbstractFieldGenerator>>(rootParam.FieldParams);
            SetCollateralDependencies(RootFieldGenerators);

            //Document field generators
            DocumentFieldGenerators = rootParam
                .RootGroupParam
                .GetDocumentParams(true)
                .ToDictionary(x => x.Name, y => mapper.Map<List<AbstractFieldGenerator>>(y.FieldParams));

            foreach (var docFields in DocumentFieldGenerators.Values)
            {
                SetCollateralDependencies(docFields);
            }

            //Group field generators
            GroupFieldGenerators = rootParam
               .RootGroupParam
               .GetGroupParamsAndSelf(true)
               .ToDictionary(x => x.Name, y => mapper.Map<List<AbstractFieldGenerator>>(y.FieldParams));

            foreach (var grpFields in GroupFieldGenerators.Values)
            {
                SetCollateralDependencies(grpFields);
            }
        }

        private void SetCollateralDependencies(IEnumerable<AbstractFieldGenerator> generators)
        {
            foreach (var generator in generators.FilterDependantFieldGenerators())
            {
                foreach (var dependencyName in generator.DependenceNames)
                {
                    var dependency = generators.Single(x => x.Name == dependencyName);
                    generator.Dependences.Add(dependency);
                }
            }
        }

        private FieldCollection GenerateFields(IEnumerable<AbstractFieldGenerator> generators)
        {
            FieldCollection fields = new FieldCollection();

            foreach (var fieldGenerator in generators.FilterNonAggregateFieldGenerators())
            {
                fieldGenerator.SetNewValue();
                fields.Add(fieldGenerator.Name, new Field(fieldGenerator.Name, fieldGenerator.LastValue));
            }

            return fields;
        }

        public FieldCollection GenerateDocumentFields(string name)
        {
            var generators = DocumentFieldGenerators[name];
            return GenerateFields(generators);
        }

        public FieldCollection GenerateGroupFields(string name)
        {
            var generators = GroupFieldGenerators[name];
            return GenerateFields(generators);
        }

        public FieldCollection GenerateRootFields()
        {
            return GenerateFields(RootFieldGenerators);
        }

    }
}
