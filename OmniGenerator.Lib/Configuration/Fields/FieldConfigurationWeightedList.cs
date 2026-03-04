using OmniGenerator.Lib.Configuration.Serialization;
using OmniGenerator.Lib.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationWeightedList : AbstractFieldConfigurationCollectionBase
    {
        [JsonPropertyName("list")]
        [JsonConverter(typeof(ConfigurationListConverter))]
        public IEnumerable<WeightedValue>? List { get; set; }
    }
}
