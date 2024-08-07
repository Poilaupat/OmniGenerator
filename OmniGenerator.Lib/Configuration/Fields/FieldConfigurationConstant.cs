using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationConstant : AbstractFieldConfigurationBase
    {
        [JsonPropertyName("value")]
        public required string Constant { get; set; }
    } 
}
