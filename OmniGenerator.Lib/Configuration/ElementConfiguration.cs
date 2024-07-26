using OmniGenerator.Lib.Configuration.Fields;
using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration
{
    public abstract class ElementConfiguration
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("min-occurs")]
        public int MinOccurs { get; set; } = 1;

        [JsonPropertyName("max-occurs")]
        public int MaxOccurs { get; set; } = 100;

        [JsonPropertyName("fields")]
        public List<FieldConfigurationBase> Fields { get; set; } = new List<FieldConfigurationBase>();

        [JsonPropertyName("field-configuration-file")]
        public string FieldConfigurationFile { get; set; } = string.Empty;
    }
}
