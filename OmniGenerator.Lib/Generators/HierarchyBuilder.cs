using AutoMapper;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Tools;
using System.Diagnostics;

namespace OmniGenerator.Lib.Generators
{
    /// <summary>
    /// Builds a new <see cref="Root"/> using provided configuration
    /// </summary>
    internal sealed class HierarchyBuilder : IHierarchyBuilder
    {
        private readonly Random _random;
        private IMapper _mapper;

        public long CountDoc { get; private set; } = 0;
        public long CountGroup { get; private set; } = 0;


        /// <summary>
        /// Creates a new <see cref="HierarchyBuilder"/>
        /// </summary>
        /// <param name="mapper">A mapper configured to map field configuration classes to field generator classes</param>
        public HierarchyBuilder(IMapper mapper)
        {
            _mapper = mapper;
            _random = new Random();
        }

        /// <summary>
        /// Builds a new <see cref="Root"/> using provided configuration
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public Root Build(OmniGeneratorConfiguration config, IProgress<ProgressReport> progress)
        {
            var rootContent = GenerateGroups(config.Root.Group);
            var root = new Root(rootContent);

            var fgc = new FieldGeneratorCollection(config.Root, _mapper);

            GenerateFields(root, fgc, progress);
            GenerateAggregateFields(root, fgc);

            return root;
        }

        private int GetRandomOccurence(int minOccurs, int maxOccurs)
        {
            // If occurence is negative (due to negative min-occurs or max-occurs), occurence is set to 0 
            return Math.Max(_random.Next(minOccurs, maxOccurs + 1), 0);
        }

        private IEnumerable<Group> GenerateGroups(GroupConfiguration groupConfiguration)
        {
            List<Group> groups = new List<Group>();

            int occurences = GetRandomOccurence(groupConfiguration.MinOccurs, groupConfiguration.MaxOccurs);
            for (int i = 0; i < occurences; i++)
            {
                Group group = new Group(++CountGroup, groupConfiguration.Name);
                groups.Add(group);

                foreach (var docParam in groupConfiguration.GetDocumentsConfiguration(false))
                {
                    group.AddRange(GenerateDocuments(docParam));
                }

                foreach (var subGroupConfiguration in groupConfiguration.GetGroupsConfiguration(false))
                {
                    group.AddRange(GenerateGroups(subGroupConfiguration));
                }
            }

            return groups;
        }

        private IEnumerable<Document> GenerateDocuments(DocumentConfiguration documentConfiguration)
        {
            List<Document> documents = new List<Document>();

            int occurences = GetRandomOccurence(documentConfiguration.MinOccurs, documentConfiguration.MaxOccurs);
            for (int i = 0; i < occurences; i++)
            {
                var document = new Document(++CountDoc, documentConfiguration.Name, documentConfiguration.ImageComposer);
                documents.Add(document);
            }
            return documents;
        }


        private void GenerateFields(Root root, FieldGeneratorCollection fgc, IProgress<ProgressReport> progress)
        {
            Stopwatch watch = Stopwatch.StartNew();

            // Generating root fields
            if (fgc.RootHasFields())
            {
                var fields = fgc.GenerateRootFields();
                root.Fields.AddRange(fields);
            }


            long processedGroups = 0, processedDocs = 0;

            // Generating group fields
            var groups = root
                .Groups
                .Union(root.Groups.SelectMany(x => x.GetGroups(true)));

            Parallel.ForEach(groups, group =>
            {
                if (fgc.ElementHasFields(group.Name))
                {
                    var groupFields = fgc.GenerateFields(group.Name);
                    group.Fields.AddRange(groupFields);
                }
                processedGroups++;
                progress.Report(new ProgressReport
                {
                    Topic = "Field Generation",
                    TotalGroups = CountGroup,
                    TotalDocuments = CountDoc,
                    ProcessedGroups = processedGroups,
                    ProcessedDocuments = processedDocs,
                });
            });

            // Generating document fields
            var documents = root.GetDocuments(true);
            Parallel.ForEach(documents, document =>
            {
                if (fgc.ElementHasFields(document.Name))
                {
                    var fields = fgc.GenerateFields(document.Name);
                    document.Fields.AddRange(fields);
                }
                processedDocs++;
                progress.Report(new ProgressReport
                {
                    Topic = "Field Generation",
                    TotalGroups = CountGroup,
                    TotalDocuments = CountDoc,
                    ProcessedGroups = processedGroups,
                    ProcessedDocuments = processedDocs,
                });
            });

            watch.Stop();
            Console.WriteLine($"Elapsed time {watch.Elapsed.TotalSeconds}s");
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
