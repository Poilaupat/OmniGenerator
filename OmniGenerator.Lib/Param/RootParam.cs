using OmniGenerator.Lib.Param.FieldParams;
using OmniGenerator.Lib.Param.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Param
{
    public class RootParam
    {
        public static string Name { get; } = "root";

        [JsonPropertyName("fields")]
        public List<FieldParamBase> FieldParams { get; set; } = new List<FieldParamBase> { };

        [JsonPropertyName("root")]
        public GroupParam RootGroupParam { get; set; } = new GroupParam();

        [JsonPropertyName("field-configuration-file")]
        public string FieldConfigurationFile { get; set; } = string.Empty;
    }
}
