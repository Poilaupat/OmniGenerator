using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Param.FieldParams
{
    internal class FieldParamComposite : FieldParamDependantBase
    {
        [JsonPropertyName("format")]
        public string? Format { get; set; }
    }
}
