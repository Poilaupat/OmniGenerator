using OmniGenerator.Lib.Configuration.ErrorSimulation;
using OmniGenerator.Lib.Configuration.Fields;
using OmniGenerator.Lib.Configuration.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration
{
    public class HierarchyConfiguration
    {
        public static string Name { get; } = "omni.generator.hierarchy";

        [JsonPropertyName("fields")]
        public List<AbstractFieldConfigurationBase> Fields { get; set; } = new List<AbstractFieldConfigurationBase> { };

        [JsonPropertyName("root")]
        public required GroupConfiguration Root { get; set; }

        [JsonPropertyName("field-configuration-file")]
        public string? FieldConfigurationFile { get; set; }

        [JsonPropertyName("error-simulations")]
        public ErrorSimulationConfiguration? ErrorSimulations { get; set; }
    }
}
