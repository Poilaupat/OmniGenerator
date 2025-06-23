using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public abstract class AbstractFieldConfigurationDependantBase : AbstractFieldConfigurationBase
    {
        private string _dependentUpon = default!;

        [JsonPropertyName("dependent-upon")]
        public required string DependentUpon 
        {
            get { return _dependentUpon; } 
            set { _dependentUpon = value; } 
        }
    }
}
