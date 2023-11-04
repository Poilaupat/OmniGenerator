using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    internal class GroupParam : ElementParam
    {
        [JsonPropertyName("elements")]
        public List<ElementParam> Elements { get; set; } = new List<ElementParam>();
    }
}
