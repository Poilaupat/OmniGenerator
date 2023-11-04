using SeedGenerator.Lib.Data.Fields;
using SeedGenerator.Lib.Data.Fields.Generators;
using SeedGenerator.Lib.Param;

namespace SeedGenerator.Lib.Data
{
    internal class Packet
    {
        public FieldCollection Fields { get; set; }
        public List<Document> Documents { get; set; } = new List<Document>();

        public Packet(PacketParam packetParam, Dictionary<string, FieldGeneratorCollection> generators)
        {
            Fields = generators["packet"].GenerateFields();
            GenerateDocuments(packetParam.RootParams, generators);
        }

        private void GenerateDocuments(GroupParam groupParam, Dictionary<string, FieldGeneratorCollection> generators)
        {
            int occurences = new Random().Next(groupParam.MinOccurs, groupParam.MaxOccurs);

            for (int i = 0; i < occurences; i++)
            {
                foreach (var element in groupParam.Elements)
                {
                    switch (element)
                    {
                        case DocumentParam childDocumentParam:
                            GenerateDocuments(childDocumentParam, generators);
                            break;

                        case GroupParam childGroupParam:
                            GenerateDocuments(childGroupParam, generators);
                            break;

                        default:
                            throw new ArgumentException("Unexpected type");

                        case null:
                            throw new ArgumentNullException();
                    }
                }
            }
        }

        private void GenerateDocuments(DocumentParam documentParam, Dictionary<string, FieldGeneratorCollection> generators)
        {
            int occurences = new Random().Next(documentParam.MinOccurs, documentParam.MaxOccurs);

            for (int i = 0; i < occurences; i++)
            {
                Documents.Add(new Document(documentParam.Name, generators[documentParam.Name]));
            }
        }
    }
}
