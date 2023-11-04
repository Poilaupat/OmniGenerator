using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    internal abstract class FieldParamDependantBase : FieldParamBase
    {
        [JsonPropertyName("dependant-upon")]
        public string? DependantUpon { get; set; }
    }
}
