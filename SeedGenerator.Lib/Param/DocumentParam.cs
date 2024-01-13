using SeedGenerator.Lib.Param.FieldParams;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    public class DocumentParam : ElementParam
    {
        [JsonPropertyName("fields")]
        public List<FieldParamBase> FieldParams { get; set; } = new List<FieldParamBase>();
    }
}
