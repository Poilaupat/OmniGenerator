using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Param.FieldParams
{
    public class FieldParamList : FieldParamBase
    {
        [JsonPropertyName("list-path")]
        public string? ListPath { get; set; }
    } 
}
