using OmniGenerator.Lib.Configuration.Fields;
using OmniGenerator.Lib.Configuration.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration
{
    public class RootConfiguration
    {
        public static string Name { get; } = "root";

        [JsonPropertyName("fields")]
        public List<AbstractFieldConfigurationBase> Fields { get; set; } = new List<AbstractFieldConfigurationBase> { };

        [JsonPropertyName("group")]
        public required GroupConfiguration Group { get; set; }

        [JsonPropertyName("field-configuration-file")]
        public string? FieldConfigurationFile { get; set; }
    }
}
