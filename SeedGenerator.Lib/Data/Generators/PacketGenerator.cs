using AutoMapper;
using SeedGenerator.Lib.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Data.Generators
{
    internal class PacketGenerator
    {
        private FieldGeneratorCollection _packetFieldGenerators;
        private Dictionary<string, FieldGeneratorCollection> _documentFieldGenerators;    
        
        public PacketParam Param { get; set; }

        public PacketGenerator(PacketParam param, IMapper mapper) 
        {
            Param = param;

            _packetFieldGenerators = new FieldGeneratorCollection(mapper.Map<List<FieldGeneratorBase>>(Param.FieldParams));
            
            _documentFieldGenerators = Param
                    .RootParams
                    .GetAllDocuments()
                    .ToDictionary(x => x.Name, y => new FieldGeneratorCollection(mapper.Map<List<FieldGeneratorBase>>(y.FieldParams)));
        }

        public Packet GeneratePacket()
        {
            var packetFields = _packetFieldGenerators.GenerateFields();
            var documents = GenerateDocuments(Param.RootParams).ToList();
            return new Packet(packetFields, documents);
        }

        private IEnumerable<Document> GenerateDocuments(GroupParam groupParam)
        {
            int occurences = new Random().Next(groupParam.MinOccurs, groupParam.MaxOccurs);

            for (int i = 0; i < occurences; i++)
            {
                foreach (var element in groupParam.Elements)
                {
                    var docs = element switch
                    {
                        DocumentParam childDocumentParam => GenerateDocuments(childDocumentParam),
                        GroupParam childGroupParam => GenerateDocuments(childGroupParam),
                        null => throw new ArgumentNullException(),
                        _ => throw new ArgumentException("Unexpected type"),
                    };

                    foreach (var doc in docs)
                        yield return doc;
                }
            }
        }

        private IEnumerable<Document> GenerateDocuments(DocumentParam documentParam)
        {
            int occurences = new Random().Next(documentParam.MinOccurs, documentParam.MaxOccurs);

            for (int i = 0; i < occurences; i++)
            {
                var document = new Document(documentParam.Name);
                if (_documentFieldGenerators.ContainsKey(documentParam.Name))
                    document.Fields = _documentFieldGenerators[documentParam.Name].GenerateFields();
                
                yield return document;
            }
        }
    }
}
