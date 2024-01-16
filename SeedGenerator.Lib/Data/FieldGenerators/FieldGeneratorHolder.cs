using AutoMapper;
using Microsoft.ProgramSynthesis.Transformation.Text.Build.NodeTypes;
using SeedGenerator.Lib.Param;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorHolder
    {
        private FieldGeneratorCollection _rootFieldGenerators;
        private Dictionary<string, FieldGeneratorCollection> _groupFieldGenerators;
        private Dictionary<string, FieldGeneratorCollection> _documentFieldGenerators;

        public FieldGeneratorCollection RootFieldGenerators => _rootFieldGenerators;
        public Dictionary<string, FieldGeneratorCollection> GroupFieldGenerators => _groupFieldGenerators;
        public Dictionary<string, FieldGeneratorCollection> DocumentFieldGenerators => _documentFieldGenerators;


        public FieldGeneratorHolder(RootParam rootParam, IMapper mapper)
        {
            // Root field generators
            _rootFieldGenerators = new FieldGeneratorCollection(mapper.Map<List<AbstractFieldGenerator>>(rootParam.FieldParams));

            // Document field generators
            _documentFieldGenerators = rootParam
                .RootGroupParam
                .GetDocumentParams(true)
                .ToDictionary(x => x.Name, y => new FieldGeneratorCollection(mapper.Map<List<AbstractFieldGenerator>>(y.FieldParams)));

            // Group field generators
            _groupFieldGenerators = rootParam
               .RootGroupParam
               .GetGroupParamsAndSelf(true)
               .ToDictionary(x => x.Name, y => new FieldGeneratorCollection(mapper.Map<List<AbstractFieldGenerator>>(y.FieldParams)));
        }
    }
}
