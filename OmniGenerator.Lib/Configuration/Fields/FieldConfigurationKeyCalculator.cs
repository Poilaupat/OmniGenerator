using OmniGenerator.Lib.Generators;
using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationKeyCalculator : AbstractFieldConfigurationDependantBase
    {
        [JsonPropertyName("key-type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required EKeyType KeyType { get; set; }
    }
}
