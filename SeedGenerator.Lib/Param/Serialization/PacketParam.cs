using SeedGenerator.Lib.Param.FieldParams;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.Serialization
{
    internal class PacketParam
    {
        [JsonPropertyName("fields")]
        public List<FieldParamBase> FieldParams = new List<FieldParamBase>();

        public List<DocumentParam> DocumentParams = new List<DocumentParam>();
        public List<GroupParam> GroupParams = new List<DocumentParam>();
    }
}
