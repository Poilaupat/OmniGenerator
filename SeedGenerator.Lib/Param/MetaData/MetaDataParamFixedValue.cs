using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Param.MetaData
{
    public class MetaDataParamFixedValue : MetaDataParamBase
    {
        [JsonPropertyName("fixed-value")]
        public string? FixedValue { get; set; }
    } 
}
