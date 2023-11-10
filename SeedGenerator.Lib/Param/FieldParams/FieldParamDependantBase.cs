using Microsoft.ProgramSynthesis.Transformation.Formula.Build.UnnamedConversionNodeTypes;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    internal abstract class FieldParamDependantBase : FieldParamBase
    {
        private string? _dependantUpon;

        [JsonPropertyName("dependant-upon")]
        public string? DependantUpon 
        {
            get { return _dependantUpon; } 
            set { _dependantUpon = value?.Replace("-","_"); } 
        }
    }
}
