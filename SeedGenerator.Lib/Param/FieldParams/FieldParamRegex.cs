using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    internal class FieldParamRegex : FieldParamBase
    {
        [JsonPropertyName("pattern")]
        public string? Pattern { get; set; }
    }
}
