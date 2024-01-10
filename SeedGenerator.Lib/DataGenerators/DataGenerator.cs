using AutoMapper;
using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.DataGenerators
{
    internal class DataGenerator
    {
        private FieldGeneratorCollection _rootFieldGenerators;
        private Dictionary<string, FieldGeneratorCollection> _documentFieldGenerators;
        private Dictionary<string, FieldGeneratorCollection> _groupFieldGenerators;

        long _docId = 1;
        long _groupId = 1;

        public RootParam _rootParam { get; set; }

        public DataGenerator(RootParam rootParam, IMapper mapper)
        {
            _rootParam = rootParam;

            _rootFieldGenerators = new FieldGeneratorCollection(mapper.Map<List<AbstractFieldGenerator>>(_rootParam.FieldParams));

            _documentFieldGenerators = _rootParam
                .GetAllDocumentParams()
                .ToDictionary(x => x.Name, y => new FieldGeneratorCollection(mapper.Map<List<AbstractFieldGenerator>>(y.FieldParams)));

            _groupFieldGenerators = _rootParam
                .GetAllGroupParams()
                .ToDictionary(x => x.Name, y => new FieldGeneratorCollection(mapper.Map<List<AbstractFieldGenerator>>(y.FieldParams)));
        }

        public Root Process()
        {
            var root = new Root();

            if (_rootFieldGenerators is not null)
            {
                var fields = _rootFieldGenerators.GenerateFields();
                root.Fields.AddRange(fields);
            }

            root.Groups.AddRange(GenerateGroupsData(_rootParam.RootGroupParam));

            return root;
        }

        private IEnumerable<Group> GenerateGroupsData(GroupParam groupParam)
        {
            int occurences = GetRandomOccurence(groupParam.MinOccurs, groupParam.MaxOccurs);

            for (int i = 0; i < occurences; i++)
            {
                Group group = new Group(_groupId++, groupParam.Name);
                if (_groupFieldGenerators.ContainsKey(groupParam.Name))
                {
                    var fields = _groupFieldGenerators[groupParam.Name].GenerateFields();
                    group.Fields.AddRange(fields);
                }

                foreach (var element in groupParam.Elements)
                {
                    group.Elements.AddRange(element switch
                    {
                        DocumentParam childDocumentParam => GenerateDocumentsData(childDocumentParam, group.Fields),
                        GroupParam childGroupParam => GenerateGroupsData(childGroupParam),
                        null => throw new ArgumentNullException(),
                        _ => throw new ArgumentException("Unexpected type"),
                    });
                }

                yield return group;
            }
        }

        private IEnumerable<Element> GenerateDocumentsData(DocumentParam documentParam, FieldCollection parentGroupFields)
        {
            int occurences = GetRandomOccurence(documentParam.MinOccurs, documentParam.MaxOccurs);

            for (int i = 0; i < occurences; i++)
            {
                var document = new Document(_docId++, documentParam.Name);
                document.Fields.AddRange(parentGroupFields);
                if (_documentFieldGenerators.ContainsKey(documentParam.Name))
                {
                    var fields = _documentFieldGenerators[documentParam.Name].GenerateFields();
                    document.Fields.AddRange(fields);
                }

                yield return document;
            }
        }

        private int GetRandomOccurence(int minOccurs, int maxOccurs)
        {
            return Math.Max(new Random().Next(minOccurs, maxOccurs), 0);
        }
    }
}
