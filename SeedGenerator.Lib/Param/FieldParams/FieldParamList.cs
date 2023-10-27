using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    internal class FieldParamList : FieldParamBase
    {
        [JsonPropertyName("list-path")]
        public string? ListPath { get; set; }
    } 
}
