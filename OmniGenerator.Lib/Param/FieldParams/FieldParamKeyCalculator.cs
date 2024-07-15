using SeedGenerator.Lib.Data.FieldGenerators;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    public class FieldParamKeyCalculator : FieldParamDependantBase
    {
        [JsonPropertyName("key-type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EKeyType KeyType { get; set; } = EKeyType.Rlmc;
    }
}
