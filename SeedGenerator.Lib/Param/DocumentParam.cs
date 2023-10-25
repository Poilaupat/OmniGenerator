using SeedGenerator.Lib.Param.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
