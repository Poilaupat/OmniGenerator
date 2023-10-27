using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    public abstract class FieldParamBase
    {
        [JsonPropertyName("name"), JsonPropertyOrder(0)]
        public string? Name {  get; set; }
    }
}
