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
        public List<FieldConfigurationBase> Fields { get; set; } = new List<FieldConfigurationBase> { };

        [JsonPropertyName("group")]
        public GroupConfiguration Group { get; set; } = new GroupConfiguration();

        [JsonPropertyName("field-configuration-file")]
        public string FieldConfigurationFile { get; set; } = string.Empty;
    }
}
