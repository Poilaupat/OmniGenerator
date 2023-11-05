using SeedGenerator.Lib.Param.FieldParams;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    internal class PacketParam
    {
        [JsonPropertyName("fields")]
        public List<FieldParamBase> FieldParams { get; set; } = new List<FieldParamBase>();

        [JsonPropertyName("root")]
        public GroupParam RootParams { get; set; } = new GroupParam();
    }
}
