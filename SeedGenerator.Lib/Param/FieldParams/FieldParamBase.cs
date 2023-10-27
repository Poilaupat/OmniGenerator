using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    internal abstract class FieldParamBase
    {
        [JsonPropertyName("name"), JsonPropertyOrder(0)]
        public string? Name {  get; set; }
    }
}
