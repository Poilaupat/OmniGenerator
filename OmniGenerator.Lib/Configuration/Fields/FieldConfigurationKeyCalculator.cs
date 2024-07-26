using OmniGenerator.Lib.Generators.Fields;
using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationKeyCalculator : FieldConfigurationDependantBase
    {
        [JsonPropertyName("key-type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EKeyType KeyType { get; set; } = EKeyType.Rlmc;
    }
}
