using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    internal class GroupParam : ElementParam
    {
        [JsonPropertyName("documents")]
        public List<DocumentParam> Documents { get; set; } = new List<DocumentParam>();
    }
}
