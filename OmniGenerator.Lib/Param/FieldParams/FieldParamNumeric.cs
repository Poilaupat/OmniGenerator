using OmniGenerator.Lib.Data.FieldGenerators;
using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Param.FieldParams
{
    public class FieldParamNumeric : FieldParamBase
    {
        [JsonPropertyName("min")]
        public float Min { get; set; } = 0.01f;
        
        [JsonPropertyName("max")]
        public float Max { get; set; } = 10000000f;
    }
}
