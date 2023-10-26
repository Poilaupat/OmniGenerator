using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.MetaData
{
    public class MetaDataParamFixedValue : MetaDataParamBase
    {
        [JsonPropertyName("fixed-value")]
        public string? FixedValue { get; set; }
    } 
}
