using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Param.FieldParams
{
    internal class FieldParamAmount : FieldParamBase
    {
        [JsonPropertyName("min")]
        public float Min { get; set; } = 0.01f;
        
        [JsonPropertyName("max")]
        public float Max { get; set; } = 10000000f;
    }
}
