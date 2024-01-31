using SeedGenerator.Lib.Param.FieldParams;
using SeedGenerator.Lib.Param.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    public class RootParam
    {
        public static string Name { get; } = "root";

        [JsonPropertyName("fields")]
        public List<FieldParamBase> FieldParams { get; set; } = new List<FieldParamBase> { };

        [JsonPropertyName("root")]
        public GroupParam RootGroupParam { get; set; } = new GroupParam();
    }
}
