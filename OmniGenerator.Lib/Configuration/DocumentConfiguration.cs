using OmniGenerator.Lib.Configuration.Fields;
using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration
{
    public class DocumentConfiguration : ElementConfiguration
    {
        [JsonPropertyName("drawer")]
        public string? ImageComposer { get; set; }
    }
}
