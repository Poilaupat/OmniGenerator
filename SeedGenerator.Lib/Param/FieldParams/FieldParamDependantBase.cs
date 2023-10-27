using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    public abstract class FieldParamDependantBase : FieldParamBase
    {
        [JsonPropertyName("dependant-upon"), JsonPropertyOrder(1)]
        public string? DependantUpon { get; set; }
    }
}
