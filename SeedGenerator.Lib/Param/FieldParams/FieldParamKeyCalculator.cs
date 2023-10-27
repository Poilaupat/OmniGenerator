using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    internal class FieldParamKeyCalculator : FieldParamDependantBase
    {
        [JsonPropertyName("key-type")]
        public string? KeyType { get; set; }
    }
}
