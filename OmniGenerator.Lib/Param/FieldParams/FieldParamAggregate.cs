using OmniGenerator.Lib.Data.FieldGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static OmniGenerator.Lib.Data.FieldGenerators.FieldGeneratorAggregate;

namespace OmniGenerator.Lib.Param.FieldParams
{
    public class FieldParamAggregate : FieldParamDependantBase
    {
        [JsonPropertyName("aggregate-type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EFFieldAggregateType AggregateType { get; set; }

        [JsonPropertyName("scope")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EScope Scope { get; set; }

        [JsonPropertyName("target-element")]
        public string? TargetElement { get; set; }
    }
}
