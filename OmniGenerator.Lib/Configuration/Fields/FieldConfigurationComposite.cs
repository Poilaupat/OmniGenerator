using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationComposite : FieldConfigurationDependantBase
    {
        private string? _format;

        [JsonPropertyName("format")]
        public string? Format 
        { 
            get { return _format; }
            set { _format = value?.Replace("-", "_"); }
        }
    }
}
