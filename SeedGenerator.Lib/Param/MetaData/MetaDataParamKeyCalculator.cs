using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.MetaData
{
    public class MetaDataParamKeyCalculator : MetaDataParamDependantBase
    {
        [JsonPropertyName("key-type")]
        public string? KeyType { get; set; }
    }
}
