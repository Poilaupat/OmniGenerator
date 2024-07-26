using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationList : FieldConfigurationBase
    {
        [JsonPropertyName("list-path")]
        public string? ListPath { get; set; }
    } 
}
