using SeedGenerator.Lib.Param.FieldParams;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    internal class DocumentParam : ElementParam
    {
        [JsonPropertyName("fields")]
        public List<FieldParamBase>? FieldParams { get; set; }
    }
}
