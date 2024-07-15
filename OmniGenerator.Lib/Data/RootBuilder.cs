using AutoMapper;
using OmniGenerator.Lib.Data.FieldGenerators;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Param;

namespace OmniGenerator.Lib.Data
{
    /// <summary>
    /// Builds a new <see cref="Root"/> using provided configuration
    /// </summary>
    public class RootBuilder : IRootBuilder
    {
        private long _docId = 1;
        private long _grpId = 1;

        private IMapper _mapper;

        /// <summary>
        /// Creates a new <see cref="RootBuilder"/>
        /// </summary>
        /// <param name="mapper">A mapper configured to map field configuration classes to field generator classes</param>
        public RootBuilder(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Builds a new <see cref="Root"/> using provided configuration
        /// </summary>
        /// <param name="rootParam"></param>
        /// <returns></returns>
        public Root Build(RootParam rootParam)
        {
            var fgc = new FieldGeneratorCollection(rootParam, _mapper);


            var rootContent = GenerateGroups(rootParam.RootGroupParam);
            var root = new Root(rootContent);


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
            if (fgc.RootHasFields())
            {
                var fields = fgc.GenerateRootFields();
                root.Fields.AddRange(fields);
            }

            // Generating group fields
            var groups = root.Groups.Union(root.Groups.SelectMany(x => x.GetGroups(true)));
            foreach (var group in groups)
            {
                if (fgc.ElementHasFields(group.Name))
                {
                    var groupFields = fgc.GenerateFields(group.Name);
                    group.Fields.AddRange(groupFields);
                }

                // Generating document fields
                foreach (var document in group.GetDocuments(false))
                {
                    if (fgc.ElementHasFields(document.Name))
                    {
                        var fields = fgc.GenerateFields(document.Name);
                        document.Fields.AddRange(fields);
                    }
                }
            }
        }

        private void GenerateAggregateFields(Root root, FieldGeneratorCollection fgc)
        {
            var groups = root.Groups.Union(root.Groups.SelectMany(x => x.GetGroups(true)));
            foreach (var group in groups)
            {
                if (fgc.ElementHasFields(group.Name))
                {
                    var aggregateGroupFields = fgc.GenerateAggregateFields(group.Name, group);
                    group.Fields.AddRange(aggregateGroupFields);
                }
            }
        }
    }
}
