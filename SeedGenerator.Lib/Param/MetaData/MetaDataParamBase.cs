using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Param.MetaData
{
    public abstract class MetaDataParamBase
    {
        [JsonPropertyName("name"), JsonPropertyOrder(0)]
        public string? Name {  get; set; }
    }
}
