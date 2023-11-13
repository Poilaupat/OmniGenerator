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
    internal class PacketDataGenerator
    {
        private FieldGeneratorCollection _packetFieldGenerators;
        private Dictionary<string, FieldGeneratorCollection> _documentFieldGenerators;
        private Dictionary<string, FieldGeneratorCollection> _groupFieldGenerators;

        public PacketParam Param { get; set; }

        public PacketDataGenerator(PacketParam param, IMapper mapper)
        {
            Param = param;

            _packetFieldGenerators = new FieldGeneratorCollection(mapper.Map<List<FieldGeneratorBase>>(Param.FieldParams));

            _documentFieldGenerators = Param
                .RootParams
                .GetAllDocuments()
                .ToDictionary(x => x.Name, y => new FieldGeneratorCollection(mapper.Map<List<FieldGeneratorBase>>(y.FieldParams)));

            _groupFieldGenerators = Param
                .RootParams
                .GetAllGroups()
                .ToDictionary(x => x.Name, y => new FieldGeneratorCollection(mapper.Map<List<FieldGeneratorBase>>(y.FieldParams)));
        }

        public PacketData GeneratePacketData()
        {
            var packetFields = _packetFieldGenerators.GenerateFields();
            var documents = GenerateDocumentsData(Param.RootParams).ToList();
            return new PacketData(packetFields, documents);
        }

        private IEnumerable<DocumentData> GenerateDocumentsData(GroupParam groupParam)
        {
            int occurences = new Random().Next(groupParam.MinOccurs, groupParam.MaxOccurs);

            for (int i = 0; i < occurences; i++)
            {
                FieldCollection grpFields = new FieldCollection();
                if (_groupFieldGenerators.ContainsKey(groupParam.Name))
                    grpFields.AddRange(_groupFieldGenerators[groupParam.Name].GenerateFields());

                foreach (var element in groupParam.Elements)
                {
                    var docs = element switch
                    {
                        DocumentParam childDocumentParam => GenerateDocumentsData(childDocumentParam, grpFields),
                        GroupParam childGroupParam => GenerateDocumentsData(childGroupParam),
                        null => throw new ArgumentNullException(),
                        _ => throw new ArgumentException("Unexpected type"),
                    };

                    foreach (var doc in docs)
                        yield return doc;
                }
            }
        }

        private IEnumerable<DocumentData> GenerateDocumentsData(DocumentParam documentParam, FieldCollection parentGroupFields)
        {
            int occurences = new Random().Next(documentParam.MinOccurs, documentParam.MaxOccurs);

            for (int i = 0; i < occurences; i++)
            {
                var document = new DocumentData(documentParam.Name);
                document.Fields.AddRange(parentGroupFields);
                if (_documentFieldGenerators.ContainsKey(documentParam.Name))
                    document.Fields.AddRange(_documentFieldGenerators[documentParam.Name].GenerateFields());

                yield return document;
            }
        }
    }
}
