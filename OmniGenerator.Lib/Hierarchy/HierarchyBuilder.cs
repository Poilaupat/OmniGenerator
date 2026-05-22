using OmniGenerator.Lib.Configuration;
using System.Collections.Concurrent;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Hierarchy.Interfaces;
using OmniGenerator.Lib.Mapping.Interfaces;
using OmniGenerator.Lib.Reporting;

/// <summary>
/// Provides functionality to build a document generation hierarchy (a <see cref="Root"/>)
/// from a given <see cref="OmniGeneratorConfiguration"/>.
/// </summary>
internal sealed class HierarchyBuilder : IHierarchyBuilder
{
    public const string HubKey = nameof(HierarchyBuilder);

    private readonly IFieldMapper _mapper;
    private readonly IProgressHub<HierarchyBuildingProgress> _hub;
    private readonly int _maxParallelism;
    private long _docCount = 0;
    private long _processedDocCount = 0;
    private long _groupCount = 0;
    private long _processedGroupCount = 0;
    private long _fieldCount = 0;

    public HierarchyBuilder(IFieldMapper mapper, IProgressHub<HierarchyBuildingProgress> hub, int maxParallelism = -1)
    {
        _mapper = mapper;
        _hub = hub;
        _maxParallelism = maxParallelism;
    }

    /// <summary>
    /// Builds a new <see cref="Root"/> object using the provided generator configuration.
    /// </summary>
    /// <param name="config">The document generation configuration.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is the generated <see cref="Root"/> object containing the document hierarchy.
    /// </returns>
    public async Task<Root> BuildAsync(OmniGeneratorConfiguration config)
    {
        var fgc = new FieldGeneratorContainer(config.Hierarchy, _mapper);
        var groups = GenerateGroups(config.Hierarchy.Root, fgc);
        var root = new Root(groups);

        if (fgc.RootHasFields())
        {
            var fields = fgc.GenerateRootFields();
            root.AddFields(fields);
        }

        _hub.Report(HubKey, GetHierarchyBuilderProgress());
        return await Task.FromResult(root);
    }

    /// <summary>
    /// Generates a list of <see cref="Group"/> instances recursively based on the configuration.
    /// </summary>
    /// <param name="groupConfiguration">The configuration for the group to generate.</param>
    /// <param name="fgc">The field generator container.</param>
    /// <returns>An array of generated <see cref="Group"/> objects.</returns>
    private Group[] GenerateGroups(GroupConfiguration groupConfiguration, FieldGeneratorContainer fgc)
    {
        int groupCount = GetRandomOccurence(groupConfiguration.MinOccurs, groupConfiguration.MaxOccurs);
        Interlocked.Add(ref _groupCount, groupCount);

        ConcurrentQueue<Group> groups = new();
        Parallel.For(0, groupCount, new ParallelOptions { MaxDegreeOfParallelism = _maxParallelism }, i =>
        {
            List<Document> subdocuments = new();
            List<Group> subGroups = new();

            foreach (var subGroupConfiguration in groupConfiguration.GetGroupsConfiguration(false))
                subGroups.AddRange(GenerateGroups(subGroupConfiguration, fgc));

            foreach (var docConfiguration in groupConfiguration.GetDocumentsConfiguration(false))
                subdocuments.AddRange(GenerateDocuments(docConfiguration, fgc));

            var group = new Group(groupConfiguration.Name, subGroups, subdocuments);
            group.GenerateFields(fgc);
            Interlocked.Increment(ref _processedGroupCount);
            Interlocked.Add(ref _fieldCount, group.Fields.Count);
            groups.Enqueue(group);

            _hub.Report(HubKey, GetHierarchyBuilderProgress());
        });
        return groups.ToArray();
    }

    /// <summary>
    /// Generates a list of <see cref="Document"/> instances based on the configuration.
    /// </summary>
    /// <param name="documentConfiguration">The configuration for the documents to generate.</param>
    /// <param name="fgc">The field generator container.</param>
    /// <returns>An array of generated <see cref="Document"/> objects.</returns>
    private Document[] GenerateDocuments(DocumentConfiguration documentConfiguration, FieldGeneratorContainer fgc)
    {
        int docCount = GetRandomOccurence(documentConfiguration.MinOccurs, documentConfiguration.MaxOccurs);
        Interlocked.Add(ref _docCount, docCount);

        ConcurrentQueue<Document> documents = new();
        Parallel.For(0, docCount, new ParallelOptions { MaxDegreeOfParallelism = _maxParallelism }, i =>
        {
            var document = new Document(documentConfiguration.Name, documentConfiguration.ImageRenderer);
            document.GenerateFields(fgc);
            Interlocked.Increment(ref _processedDocCount);
            Interlocked.Add(ref _fieldCount, document.Fields.Count);
            documents.Enqueue(document);

            _hub.Report(HubKey, GetHierarchyBuilderProgress());
        });

        return documents.ToArray();
    }

    /// <summary>
    /// Generates a random occurrence count between min and max, inclusive.
    /// Returns 0 if the computed value is negative.
    /// </summary>
    /// <param name="minOccurs">Minimum number of occurrences.</param>
    /// <param name="maxOccurs">Maximum number of occurrences.</param>
    /// <returns>A random number of occurrences between min and max.</returns>
    private int GetRandomOccurence(int minOccurs, int maxOccurs)
    {
        return Math.Max(Random.Shared.Next(minOccurs, maxOccurs + 1), 0);
    }

    private HierarchyBuildingProgress GetHierarchyBuilderProgress()
    {
        return new HierarchyBuildingProgress()
        {
            FieldCount = Interlocked.Read(ref _fieldCount),
            GroupCount = Interlocked.Read(ref _groupCount),
            DocumentCount = Interlocked.Read(ref _docCount),
            ProcessedGroupCount = Interlocked.Read(ref _processedGroupCount),
            ProcessedDocumentCount = Interlocked.Read(ref _processedDocCount),
        };
    }
}
