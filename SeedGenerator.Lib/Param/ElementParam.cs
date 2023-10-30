using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    internal class ElementParam
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("min-occurs")]
        public int MinOccurs { get; set; } = 1;

        [JsonPropertyName("max-occurs")]
        public int MaxOccurs { get; set; } = 100;
    }
}
