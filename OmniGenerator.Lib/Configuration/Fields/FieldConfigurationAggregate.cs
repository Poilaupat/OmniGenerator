using OmniGenerator.Lib.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static OmniGenerator.Lib.Generators.Fields.FieldGeneratorAggregate;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationAggregate : AbstractFieldConfigurationDependantBase
    {
        [JsonPropertyName("aggregate-type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required EFFieldAggregateType AggregateType { get; set; }

        [JsonPropertyName("scope")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required EScope Scope { get; set; }

        [JsonPropertyName("target-element")]
        public required string TargetElement { get; set; }
    }
}
