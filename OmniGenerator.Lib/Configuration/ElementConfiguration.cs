using OmniGenerator.Lib.Configuration.Fields;
using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration
{
    public abstract class ElementConfiguration
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("min-occurs")]
        public required int MinOccurs { get; set; }

        [JsonPropertyName("max-occurs")]
        public required int MaxOccurs { get; set; }

        [JsonPropertyName("fields")]
        public List<AbstractFieldConfigurationBase> Fields { get; set; } = new List<AbstractFieldConfigurationBase>();

        [JsonPropertyName("field-configuration-file")]
        public string? FieldConfigurationFile { get; set; }
    }
}
