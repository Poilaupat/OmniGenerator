using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    public class FieldParamConstant : FieldParamBase
    {
        [JsonPropertyName("value")]
        public string? Constant { get; set; }
    } 
}
