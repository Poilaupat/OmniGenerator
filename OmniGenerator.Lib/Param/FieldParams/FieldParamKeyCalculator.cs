using OmniGenerator.Lib.Data.FieldGenerators;
using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Param.FieldParams
{
    public class FieldParamKeyCalculator : FieldParamDependantBase
    {
        [JsonPropertyName("key-type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EKeyType KeyType { get; set; } = EKeyType.Rlmc;
    }
}
