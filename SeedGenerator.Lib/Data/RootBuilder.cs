using AutoMapper;
using Mustache;
using SeedGenerator.Lib.DataGenerators;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Data
{
    public class RootBuilder : IRootBuilder
    {
        private long _docId = 1;
        private long _grpId = 1;

        private IMapper _mapper;

        public RootBuilder(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Root Build(RootParam rootParam)
        {
            var root = new Root();

            var groups = GenerateGroups(rootParam.RootGroupParam);
            root.Groups.AddRange(groups);

            GenerateFields(root, rootParam);

            return root;
        }

        private int GetRandomOccurence(int minOccurs, int maxOccurs)
        {
            return Math.Max(new Random().Next(minOccurs, maxOccurs), 0);
        }

        private IEnumerable<Group> GenerateGroups(GroupParam groupParam)
        {
            int occurences = GetRandomOccurence(groupParam.MinOccurs, groupParam.MaxOccurs);
            for (int i = 0; i < occurences; i++)
            {
                Group group = new Group(_grpId++, groupParam.Name);

                foreach (var docParam in groupParam.GetDocumentParams(false))
                {
                    group.Elements.AddRange(GenerateDocuments(docParam));
                }

                foreach (var subGroupParam in groupParam.GetGroupParams(false))
                {
                    group.Elements.AddRange(GenerateGroups(subGroupParam));
                }

                yield return group;
            }
        }

        private IEnumerable<Document> GenerateDocuments(DocumentParam documentParam)
        {
            int occurences = GetRandomOccurence(documentParam.MinOccurs, documentParam.MaxOccurs);
            for (int i = 0; i < occurences; i++)
            {
                var document = new Document(_docId++, documentParam.Name);
                yield return document;
            }
        }


        private void GenerateFields(Root root, RootParam rootParam)
        {
            // Root field generators
            var rootFieldGenerators = new FieldGeneratorCollection(_mapper.Map<List<AbstractFieldGenerator>>(rootParam.FieldParams));

            // Document field generators
            var documentFieldGenerators = rootParam
                .RootGroupParam
                .GetDocumentParams(true)
                .ToDictionary(x => x.Name, y => new FieldGeneratorCollection(_mapper.Map<List<AbstractFieldGenerator>>(y.FieldParams)));

            // Group field generators
            var groupFieldGenerators = rootParam
               .RootGroupParam
               .GetGroupParams(true)
               .ToDictionary(x => x.Name, y => new FieldGeneratorCollection(_mapper.Map<List<AbstractFieldGenerator>>(y.FieldParams)));

            // Root fields
            if (rootFieldGenerators is not null)
            {
                var fields = rootFieldGenerators.GenerateFields();
                root.Fields.AddRange(fields);
            }

            // Group fields
            var groups = root.Groups.Union(root.Groups.SelectMany(x => x.GetGroups(true)));
            foreach(var group in groups)
            {
                FieldCollection? groupFields = null;
                if (groupFieldGenerators.ContainsKey(group.Name))
                {
                    groupFields = groupFieldGenerators[group.Name].GenerateFields();
                    group.Fields.AddRange(groupFields);
                }

                // Document fields (Note : There is a copy of the fields of direct parent group on each document)
                foreach (var document in group.GetDocuments(false))
                {
                    if (groupFields is not null)
                    {
                        document.Fields.AddRange(groupFields);
                    }

                    if (documentFieldGenerators.ContainsKey(document.Name))
                    {
                        var fields = documentFieldGenerators[document.Name].GenerateFields();
                        document.Fields.AddRange(fields);
                    }
                }
            }
        }
    }
}
