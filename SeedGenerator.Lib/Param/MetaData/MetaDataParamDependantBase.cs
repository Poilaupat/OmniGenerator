using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.MetaData
{
    public abstract class MetaDataParamDependantBase : MetaDataParamBase
    {
        [JsonPropertyName("dependant-upon"), JsonPropertyOrder(1)]
        public string? DependantUpon { get; set; }
    }
}
