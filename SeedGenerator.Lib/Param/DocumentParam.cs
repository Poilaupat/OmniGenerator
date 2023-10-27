using SeedGenerator.Lib.Param.FieldParams;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    internal class DocumentParam
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("metadatas")]
        public List<FieldParamBase>? FieldParams { get; set; }
    }
}
