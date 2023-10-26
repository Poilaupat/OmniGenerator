using SeedGenerator.Lib.Param.MetaData;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    public class DocumentParam
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("metadatas")]
        public List<MetaDataParamBase>? MetaDatas { get; set; }
    }
}
