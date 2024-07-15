using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Param.FieldParams
{
    public class FieldParamRegex : FieldParamBase
    {
        [JsonPropertyName("pattern")]
        public string? Pattern { get; set; }
    }
}
