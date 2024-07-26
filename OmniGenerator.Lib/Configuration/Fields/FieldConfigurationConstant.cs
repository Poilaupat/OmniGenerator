using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationConstant : FieldConfigurationBase
    {
        [JsonPropertyName("value")]
        public string? Constant { get; set; }
    } 
}
