using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.MetaData
{
    public class MetaDataParamList : MetaDataParamBase
    {
        [JsonPropertyName("list-path")]
        public string? ListPath { get; set; }
    } 
}
