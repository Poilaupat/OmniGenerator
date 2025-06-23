using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationComposite : AbstractFieldConfigurationDependantBase
    {
        private string _format = default!;

        [JsonPropertyName("format")]
        public required string Format 
        { 
            get { return _format; }
            set { _format = value; }
        }
    }
}
