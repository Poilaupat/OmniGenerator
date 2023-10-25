using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Param.MetaData
{
    public abstract class MetaDataParamDependantBase : MetaDataParamBase
    {
        [JsonPropertyName("dependant-upon"), JsonPropertyOrder(1)]
        public string? DependantUpon { get; set; }
    }
}
