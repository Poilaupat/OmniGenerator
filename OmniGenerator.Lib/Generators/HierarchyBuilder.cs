using AutoMapper;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Configuration;
using OmniGenerator.Lib.Tools;
using System.Diagnostics;
using System.Collections.Concurrent;
using Microsoft.ProgramSynthesis.Utils.Interactive;
using Microsoft.Extensions.Configuration;
using System.Buffers;

namespace OmniGenerator.Lib.Generators
{
    /// <summary>
    /// Builds a new <see cref="Root"/> using provided configuration
    /// </summary>
    internal sealed class HierarchyBuilder : IHierarchyBuilder
    {
        private readonly Random _random;
        private IMapper _mapper;
        private IConfiguration _config;

        private DateTime _lastNotification = DateTime.Now;

        private long _docCount = 0;
        private long _countProcessedDoc = 0;
        private long _countGroup = 0;
        private long _countProcessedGroup = 0;
        private long _countField = 0;
        private DateTime? _startHierarchyBuild;
        private DateTime? _endHierarchyBuild;

        public IProgress<BuilderProgressReport>? Progress { get; set; }
        public int ProgressResolution { get; set; } = 5;


        /// <summary>
        /// Creates a new <see cref="HierarchyBuilder"/>
        /// </summary>
        /// <param name="mapper">A mapper configured to map field configuration classes to field generator classes</param>
        public HierarchyBuilder(IMapper mapper, IConfiguration config)
        {
            _mapper = mapper;
            _config = config;
            _random = new Random();
        }

        /// <summary>
        /// Builds a new <see cref="Root"/> using provided configuration
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public Root Build(OmniGeneratorConfiguration config)
        {
            _startHierarchyBuild = DateTime.Now;

            var fgc = new FieldGeneratorContainer(config.Root, _mapper);

            var groups = GenerateGroups(config.Root.Group, fgc);
            var root = new Root(groups);

            if (fgc.RootHasFields())
            {
                var fields = fgc.GenerateRootFields();
                root.Fields.AddRange(fields);
            }

            _endHierarchyBuild = DateTime.Now;

            Notify(true);
            return root;
        }

        private int GetRandomOccurence(int minOccurs, int maxOccurs)
        {
            // If occurence is negative (due to negative min-occurs or max-occurs), occurence is set to 0 
            return Math.Max(_random.Next(minOccurs, maxOccurs + 1), 0);
        }

        private Group[] GenerateGroups(GroupConfiguration groupConfiguration, FieldGeneratorContainer fgc)
        {
            int groupCount = GetRandomOccurence(groupConfiguration.MinOccurs, groupConfiguration.MaxOccurs);
            Interlocked.Add(ref _countGroup, groupCount);

            //ConcurrentQueue<Group> groups = new();
            Group[] groups = new Group[groupCount];

            var partitioner = Partitioner.Create(0, groupCount, 5000);
            Parallel.ForEach(partitioner, range =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    Document[] subdocuments = Array.Empty<Document>();
                    Group[] subGroups = Array.Empty<Group>();

                    foreach (var subGroupConfiguration in groupConfiguration.GetGroupsConfiguration(false))
                    {
                        subGroups = GenerateGroups(subGroupConfiguration, fgc);
                    }

                    foreach (var docConfiguration in groupConfiguration.GetDocumentsConfiguration(false))
                    {
                        subdocuments = GenerateDocuments(docConfiguration, fgc);
                    }

                    Group group = new Group(groupConfiguration.Name, subGroups, subdocuments);
                    group.GenerateFields(fgc);
                    Interlocked.Increment(ref _countProcessedGroup);
                    Interlocked.Add(ref _countField, group.Fields.FieldCount);
                    groups[i] = group;

                    Notify();
                }
            });

            return groups;
        }

        private Document[] GenerateDocuments(DocumentConfiguration documentConfiguration, FieldGeneratorContainer fgc)
        {
            var docCount = GetRandomOccurence(documentConfiguration.MinOccurs, documentConfiguration.MaxOccurs);
            Interlocked.Add(ref _docCount, docCount);

            //ConcurrentQueue<Document> documents = new();
            Document[] documents = new Document[docCount];

            var partitioner = Partitioner.Create(0, docCount, 5000);
            Parallel.ForEach(partitioner, range =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    var document = new Document(documentConfiguration.Name, documentConfiguration.ImageComposer);
                    document.GenerateFields(fgc);
                    Interlocked.Increment(ref _countProcessedDoc);
                    Interlocked.Add(ref _countField, document.Fields.FieldCount);
                    documents[i] = document;

                    Notify();
                }
            });

            return documents;
        }

        private void Notify(bool force = false)
        {
            var now = DateTime.Now;
            if (Progress is not null && ((now - _lastNotification).Seconds > ProgressResolution || force))
            {
                Progress.Report(new BuilderProgressReport
                {
                    CountField = _countField,
                    CountGroup = _countGroup,
                    CountDocument = _docCount,
                    CountProcessedGroup = _countProcessedGroup,
                    CountProcessedDocument = _countProcessedDoc,
                    StartHierarchyBuild = _startHierarchyBuild,
                    EndHierarchyBuild = _endHierarchyBuild,
                });

                _lastNotification = now;
            }
        }
    }
}
