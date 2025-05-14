using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationIncrement : AbstractFieldConfigurationBase
    {
        [JsonPropertyName("start")]
        public required long Start { get; set; } = 0;

        [JsonPropertyName("increment")]
        public required long Increment { get; set; } = 1;
    }
}
