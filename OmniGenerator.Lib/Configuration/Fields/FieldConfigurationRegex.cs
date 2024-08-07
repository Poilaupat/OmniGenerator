using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationRegex : AbstractFieldConfigurationBase
    {
        [JsonPropertyName("pattern")]
        public required string Pattern { get; set; }
    }
}
