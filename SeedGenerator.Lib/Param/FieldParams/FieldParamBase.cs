using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    public abstract class FieldParamBase
    {
        private string? _name;

        [JsonPropertyName("name"), JsonPropertyOrder(0)]
        public string? Name { get { return _name; } set { _name = value?.Replace("-", "_"); } }
    }
}
