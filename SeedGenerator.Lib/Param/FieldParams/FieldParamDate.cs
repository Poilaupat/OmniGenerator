using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Param.FieldParams
{
    internal class FieldParamDate : FieldParamBase
    {
        [JsonPropertyName("day-diff-min")]
        public int DayDiffMin { get; set; }

        [JsonPropertyName("day-diff-max")]
        public int DayDiffMax { get; set; }
    }
}
