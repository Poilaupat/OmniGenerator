using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Param.FieldParams
{
    public abstract class FieldParamDependantBase : FieldParamBase
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
