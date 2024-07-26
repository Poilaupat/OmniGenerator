using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public abstract class FieldConfigurationBase
    {
        private string? _name;

        [JsonPropertyName("name"), JsonPropertyOrder(0)]
        public string? Name { get { return _name; } set { _name = value?.Replace("-", "_"); } }
    }
}
