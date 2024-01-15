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
            var groups = GenerateGroups(rootParam.RootGroupParam);
            var root = new Root(groups);

            var holder = new FieldGeneratorHolder(rootParam, _mapper);
            GenerateFields(root, holder);
            GenerateAggregateFields(root, holder);

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
                    group.Elements.AddRange(GenerateDocuments(docParam));
                }

                foreach (var subGroupParam in groupParam.GetGroupParams(false))
                {
                    group.Elements.AddRange(GenerateGroups(subGroupParam));
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


        private void GenerateFields(Root root, FieldGeneratorHolder holder)
        {
            // Generating root fields
            if (holder.RootFieldGenerators is not null)
            {
                var fields = holder.RootFieldGenerators.GenerateFields();
                root.Fields.AddRange(fields);
            }

            // Generating group fields
            var groups = root.Groups.Union(root.Groups.SelectMany(x => x.GetGroups(true)));
            foreach (var group in groups)
            {
                FieldCollection? groupFields = null;
                if (holder.GroupFieldGenerators.ContainsKey(group.Name))
                {
                    groupFields = holder.GroupFieldGenerators[group.Name].GenerateFields();
                    group.Fields.AddRange(groupFields);
                }

                // Generating document fields (Note : There is a copy of the fields of direct parent group on each document)
                foreach (var document in group.GetDocuments(false))
                {
                    if (groupFields is not null)
                    {
                        document.Fields.AddRange(groupFields);
                    }

                    if (holder.DocumentFieldGenerators.ContainsKey(document.Name))
                    {
                        var fields = holder.DocumentFieldGenerators[document.Name].GenerateFields();
                        document.Fields.AddRange(fields);
                    }
                }
            }
        }

        private void GenerateAggregateFields(Root root, FieldGeneratorHolder holder)
        {
            foreach (var group in root.Groups)
            {
                GenerateAggregateFields(group, holder);
            }
        }

        private void GenerateAggregateFields(Group group, FieldGeneratorHolder holder)
        {
            if (holder.GroupFieldGenerators.ContainsKey(group.Name))
            {
                foreach (var generator in holder.GroupFieldGenerators[group.Name].AggregateFieldGenerators)
                {
                    generator.Group = group;
                    group.Fields.Add(generator.Name, new Field(generator.Name, generator.NextValue()));
                }
            }

            foreach (var document in group.GetDocuments(false))
            {
                if (holder.DocumentFieldGenerators.ContainsKey(document.Name))
                {
                    foreach (var generator in holder.DocumentFieldGenerators[document.Name].AggregateFieldGenerators)
                    {
                        generator.Group = group;
                        document.Fields.Add(generator.Name, new Field(generator.Name, generator.NextValue()));
                    }
                }
            }

            foreach (var subGroup in group.GetGroups(false))
            {
                GenerateAggregateFields(subGroup, holder);
            }
        }
    }
}
