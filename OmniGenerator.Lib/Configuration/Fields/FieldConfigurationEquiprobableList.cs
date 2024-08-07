using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationEquiprobableList : AbstractFieldConfigurationCollectionBase
    {
        [JsonPropertyName("list")]
        public List<string>? List { get; set; }
    }
}
