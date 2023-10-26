using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.MetaData
{
    public abstract class MetaDataParamBase
    {
        [JsonPropertyName("name"), JsonPropertyOrder(0)]
        public string? Name {  get; set; }
    }
}
