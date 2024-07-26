using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public abstract class FieldConfigurationDependantBase : FieldConfigurationBase
    {
        private string? _dependentUpon;

        [JsonPropertyName("dependent-upon")]
        public string? DependentUpon 
        {
            get { return _dependentUpon; } 
            set { _dependentUpon = value?.Replace("-","_"); } 
        }
    }
}
