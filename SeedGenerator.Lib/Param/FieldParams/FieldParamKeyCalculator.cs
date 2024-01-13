using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    public class FieldParamKeyCalculator : FieldParamDependantBase
    {
        [JsonPropertyName("key-type")]
        public string? KeyType { get; set; }
    }
}
