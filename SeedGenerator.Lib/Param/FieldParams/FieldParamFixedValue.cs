using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    public class FieldParamFixedValue : FieldParamBase
    {
        [JsonPropertyName("value")]
        public string? FixedValue { get; set; }
    } 
}
