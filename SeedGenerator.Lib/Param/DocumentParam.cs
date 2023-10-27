using SeedGenerator.Lib.Param.FieldParams;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    public class DocumentParam
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("metadatas")]
        public List<FieldParamBase>? FieldParams { get; set; }
    }
}
