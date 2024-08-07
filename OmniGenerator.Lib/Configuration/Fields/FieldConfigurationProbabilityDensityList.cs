using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationProbabilityDensityList : AbstractFieldConfigurationCollectionBase
    {
        [JsonPropertyName("list")]
        public Dictionary<string, int>? List { get; set; }
    }
}
