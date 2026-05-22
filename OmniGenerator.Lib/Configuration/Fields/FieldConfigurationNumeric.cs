using OmniGenerator.Lib.Generators.Fields;
using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationNumeric : AbstractFieldConfigurationBase
    {
        [JsonPropertyName("min")]
        public required float Min { get; set; } = 0.01f;

        [JsonPropertyName("max")]
        public required float Max { get; set; } = 10000000f;
    }
}
