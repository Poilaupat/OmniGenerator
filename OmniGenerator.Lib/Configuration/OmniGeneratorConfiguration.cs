using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Configuration
{
    public class OmniGeneratorConfiguration
    {
        [JsonPropertyName("packager")]
        public required string PackagerName { get; set; }
 
        [JsonPropertyName("root")]
        public required RootConfiguration Root { get; set; }
    }
}
