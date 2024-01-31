using SeedGenerator.Lib.Data.FieldGenerators;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    public class FieldParamAmount : FieldParamBase
    {
        [JsonPropertyName("min")]
        public float Min { get; set; } = 0.01f;
        
        [JsonPropertyName("max")]
        public float Max { get; set; } = 10000000f;
    }
}
