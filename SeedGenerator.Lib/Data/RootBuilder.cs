using AutoMapper;
using SeedGenerator.Lib.Data.FieldGenerators;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Param;

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
            var rootContent = GenerateGroups(rootParam.RootGroupParam);
            var root = new Root(rootContent);

            var fgc = new FieldGeneratorCollection(rootParam, _mapper);

            GenerateFields(root, fgc);
            GenerateAggregateFields(root, fgc);

            return root;
        }

        private int GetRandomOccurence(int minOccurs, int maxOccurs)
        {
            return Math.Max(new Random().Next(minOccurs, maxOccurs), 0);
        }

        private IEnumerable<Group> GenerateGroups(GroupParam groupParam)
        {
            List<Group> groups = new List<Group>();

            int occurences = GetRandomOccurence(groupParam.MinOccurs, groupParam.MaxOccurs);
            for (int i = 0; i < occurences; i++)
            {
                Group group = new Group(_grpId++, groupParam.Name);
                groups.Add(group);

                foreach (var docParam in groupParam.GetDocumentParams(false))
                {
                    group.AddRange(GenerateDocuments(docParam));
                }

                foreach (var subGroupParam in groupParam.GetGroupParams(false))
                {
                    group.AddRange(GenerateGroups(subGroupParam));
                }

            }

            return groups;
        }

        private IEnumerable<Document> GenerateDocuments(DocumentParam documentParam)
        {
            List<Document> documents = new List<Document>();

            int occurences = GetRandomOccurence(documentParam.MinOccurs, documentParam.MaxOccurs);
            for (int i = 0; i < occurences; i++)
            {
                var document = new Document(_docId++, documentParam.Name);
                documents.Add(document);
            }
            return documents;
        }


        private void GenerateFields(Root root, FieldGeneratorCollection fgc)
        {
            // Generating root fields
            if (fgc.RootFieldGenerators is not null)
            {
                var fields = fgc.GenerateRootFields();
                root.Fields.AddRange(fields);
            }

            // Generating group fields
            var groups = root.Groups.Union(root.Groups.SelectMany(x => x.GetGroups(true)));
            foreach (var group in groups)
            {
                FieldCollection? groupFields = null;
                if (fgc.GroupFieldGenerators.ContainsKey(group.Name))
                {
                    groupFields = fgc.GenerateGroupFields(group.Name);
                    group.Fields.AddRange(groupFields);
                }

                // Generating document fields
                foreach (var document in group.GetDocuments(false))
                {
                    if (fgc.DocumentFieldGenerators.ContainsKey(document.Name))
                    {
                        var fields = fgc.GenerateDocumentFields(document.Name);
                        document.Fields.AddRange(fields);
                    }
                }
            }
        }

        private void GenerateAggregateFields(Root root, FieldGeneratorCollection holder)
        {
            foreach (var group in root.Groups)
            {
                GenerateAggregateFields(group, holder);
            }
        }

        private void GenerateAggregateFields(Group group, FieldGeneratorCollection holder)
        {
            if (holder.GroupFieldGenerators.ContainsKey(group.Name))
            {
                foreach (var generator in holder.GroupFieldGenerators[group.Name].FilterAggregateFieldGenerators())
                {
                    generator.Group = group;
                    generator.SetNewValue();
                    group.Fields.Add(generator.Name, new Field(generator.Name, generator.LastValue));
                }
            }

            foreach (var subGroup in group.GetGroups(false))
            {
                GenerateAggregateFields(subGroup, holder);
            }
        }
    }
}
