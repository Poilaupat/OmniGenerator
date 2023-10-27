using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    internal class FieldParamFixedValue : FieldParamBase
    {
        [JsonPropertyName("fixed-value")]
        public string? FixedValue { get; set; }
    } 
}
