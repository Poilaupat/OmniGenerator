using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    public class FieldParamFixedValue : FieldParamBase
    {
        [JsonPropertyName("fixed-value")]
        public string? FixedValue { get; set; }
    } 
}
