using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.MetaData
{
    public class MetaDataParamRegex : MetaDataParamBase
    {
        [JsonPropertyName("pattern")]
        public string? Pattern { get; set; }
    }
}
