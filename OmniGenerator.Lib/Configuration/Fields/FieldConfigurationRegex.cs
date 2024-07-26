using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationRegex : FieldConfigurationBase
    {
        [JsonPropertyName("pattern")]
        public string? Pattern { get; set; }
    }
}
