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
        private IConfiguration _appconfig;

        private DateTime _lastNotification = DateTime.Now;

        private long _countDoc = 0;
        private long _countProcessedDoc = 0;
        private long _countGroup = 0;
        private long _countProcessedGroup = 0;
        private long _countField = 0;


        /// <summary>
        /// Creates a new <see cref="HierarchyBuilder"/>
        /// </summary>
        /// <param name="mapper">A mapper configured to map field configuration classes to field generator classes</param>
        public HierarchyBuilder(IMapper mapper, IConfiguration appconfig)
        {
            _mapper = mapper;
            _appconfig = appconfig;
            _random = new Random();
        }

        /// <summary>
        /// Builds a new <see cref="Root"/> using provided configuration
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<Root> BuildAsync(OmniGeneratorConfiguration config, IProgress<HierarchyBuilderProgressReport>? progress)
        {
            var fgc = new FieldGeneratorContainer(config.Hierarchy, _mapper);
            var groups = GenerateGroups(config.Hierarchy.Root, fgc, progress);
            var root = new Root(groups);

            if (fgc.RootHasFields())
            {
                var fields = fgc.GenerateRootFields();
                root.Fields.AddRange(fields);
            }

            NotifyProgress(progress, true);
            return await Task.FromResult(root);
        }

        private int GetRandomOccurence(int minOccurs, int maxOccurs)
        {
            // If occurence is negative (due to negative min-occurs or max-occurs), occurence is set to 0 
            return Math.Max(_random.Next(minOccurs, maxOccurs + 1), 0);
        }

        private Group[] GenerateGroups(GroupConfiguration groupConfiguration, FieldGeneratorContainer fgc, IProgress<HierarchyBuilderProgressReport>? progress)
        {
            int groupCount = GetRandomOccurence(groupConfiguration.MinOccurs, groupConfiguration.MaxOccurs);
            Interlocked.Add(ref _countGroup, groupCount);

            ConcurrentQueue<Group> groups = new();

            Parallel.For(0, groupCount, i =>
            {
                Document[] subdocuments = Array.Empty<Document>();
                Group[] subGroups = Array.Empty<Group>();

                foreach (var subGroupConfiguration in groupConfiguration.GetGroupsConfiguration(false))
                {
                    subGroups = GenerateGroups(subGroupConfiguration, fgc, progress);
                }

                foreach (var docConfiguration in groupConfiguration.GetDocumentsConfiguration(false))
                {
                    subdocuments = GenerateDocuments(docConfiguration, fgc, progress);
                }

                Group group = new Group(groupConfiguration.Name, subGroups, subdocuments);
                group.GenerateFields(fgc);
                Interlocked.Increment(ref _countProcessedGroup);
                Interlocked.Add(ref _countField, group.Fields.FieldCount);
                groups.Enqueue(group);

                NotifyProgress(progress);
            });

            return groups.ToArray();
        }

        private Document[] GenerateDocuments(DocumentConfiguration documentConfiguration, FieldGeneratorContainer fgc, IProgress<HierarchyBuilderProgressReport>? progress)
        {
            var docCount = GetRandomOccurence(documentConfiguration.MinOccurs, documentConfiguration.MaxOccurs);
            Interlocked.Add(ref _countDoc, docCount);

            ConcurrentQueue<Document> documents = new();

            Parallel.For(0, docCount, i =>
            {
                var document = new Document(documentConfiguration.Name, documentConfiguration.ImageComposer);
                document.GenerateFields(fgc);
                Interlocked.Increment(ref _countProcessedDoc);
                Interlocked.Add(ref _countField, document.Fields.FieldCount);
                documents.Enqueue(document);

                NotifyProgress(progress);
            });

            return documents.ToArray();
        }

        private void NotifyProgress(IProgress<HierarchyBuilderProgressReport>? progress, bool force = false)
        {
            var now = DateTime.Now;
            if (progress is not null && ((now - _lastNotification).Seconds > 1 || force))
            {
                progress.Report(new HierarchyBuilderProgressReport
                {
                    CountField = Interlocked.Read(ref _countField),
                    CountGroup = Interlocked.Read(ref _countGroup),
                    CountDocument = Interlocked.Read(ref _countDoc),
                    CountProcessedGroup = Interlocked.Read(ref _countProcessedGroup),
                    CountProcessedDocument = Interlocked.Read(ref _countProcessedDoc),
                });

                _lastNotification = now;
            }
        }
    }
}
