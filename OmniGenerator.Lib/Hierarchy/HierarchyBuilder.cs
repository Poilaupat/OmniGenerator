using OmniGenerator.Lib.Configuration;
using System.Collections.Concurrent;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Tools;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces.Infrastructure;

/// <summary>
/// Provides functionality to build a document generation hierarchy (a <see cref="Root"/>)
/// from a given <see cref="OmniGeneratorConfiguration"/>.
/// </summary>
internal sealed class HierarchyBuilder : IHierarchyBuilder
{
    private readonly IFieldMapper _mapper;
    private readonly Random _random;
    private long _countDoc = 0;
    private long _countProcessedDoc = 0;
    private long _countGroup = 0;
    private long _countProcessedGroup = 0;
    private long _countField = 0;

    public Notifier<HierarchyBuilderProgress> Notifier { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HierarchyBuilder"/> class.
    /// </summary>
    /// <param name="mapper">
    /// An instance of <see cref="IFieldMapper"/> used to map field configurations to field generators.
    /// </param>
    public HierarchyBuilder(IFieldMapper mapper, Notifier<HierarchyBuilderProgress> notifier)
    {
        _mapper = mapper;
        _random = new Random();
        Notifier = notifier;
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

        Notifier.SendNotification(GetHierarchyBuilderProgress());
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
        Interlocked.Add(ref _countGroup, groupCount);

        ConcurrentQueue<Group> groups = new();
#if DEBUG
        for (int i = 0; i < groupCount; i++)
#else
        Parallel.For(0, groupCount, i =>
#endif
        {
            List<Document> subdocuments = new();
            List<Group> subGroups = new();

            foreach (var subGroupConfiguration in groupConfiguration.GetGroupsConfiguration(false))
                subGroups.AddRange(GenerateGroups(subGroupConfiguration, fgc));

            foreach (var docConfiguration in groupConfiguration.GetDocumentsConfiguration(false))
                subdocuments.AddRange(GenerateDocuments(docConfiguration, fgc));

            var group = new Group(groupConfiguration.Name, subGroups, subdocuments);
            group.GenerateFields(fgc);
            Interlocked.Increment(ref _countProcessedGroup);
            Interlocked.Add(ref _countField, group.Fields.Count);
            groups.Enqueue(group);

            Notifier.SendNotification(GetHierarchyBuilderProgress());
        }
#if !DEBUG
        );
#endif
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
        Interlocked.Add(ref _countDoc, docCount);

        ConcurrentQueue<Document> documents = new();
#if DEBUG
        for (int i = 0; i < docCount; i++)
#else
        Parallel.For(0, docCount, i =>
#endif
        {
            var document = new Document(documentConfiguration.Name, documentConfiguration.ImageRenderer);
            document.GenerateFields(fgc);
            Interlocked.Increment(ref _countProcessedDoc);
            Interlocked.Add(ref _countField, document.Fields.Count);
            documents.Enqueue(document);

            Notifier.SendNotification(GetHierarchyBuilderProgress());
        }
#if !DEBUG
        );
#endif

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
        return Math.Max(_random.Next(minOccurs, maxOccurs + 1), 0);
    }

    private HierarchyBuilderProgress GetHierarchyBuilderProgress()
    {
        return new HierarchyBuilderProgress()
        {
            CountField = Interlocked.Read(ref _countField),
            CountGroup = Interlocked.Read(ref _countGroup),
            CountDocument = Interlocked.Read(ref _countDoc),
            CountProcessedGroup = Interlocked.Read(ref _countProcessedGroup),
            CountProcessedDocument = Interlocked.Read(ref _countProcessedDoc),
        };
    }
}
