using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    internal abstract class FieldParamDependantBase : FieldParamBase
    {
        [JsonPropertyName("dependant-upon"), JsonPropertyOrder(1)]
        public string? DependantUpon { get; set; }
    }
}
