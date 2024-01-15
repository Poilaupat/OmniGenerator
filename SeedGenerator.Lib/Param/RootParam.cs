using SeedGenerator.Lib.Param.FieldParams;
using SeedGenerator.Lib.Param.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    public class RootParam
    {
        [JsonPropertyName("fields")]
        public List<FieldParamBase> FieldParams { get; set; } = new List<FieldParamBase> { };

        [JsonPropertyName("root")]
        public GroupParam RootGroupParam { get; set; } = new GroupParam();

        public static async Task<RootParam> FromFileAsync(string filepath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                TypeInfoResolver = new PolymorphicTypeResolver(),
            };

            string json = await File.ReadAllTextAsync(filepath);
            var param = JsonSerializer.Deserialize<RootParam>(json, options);

            if (param is null)
            {
                throw new Exception($"Invalid parameter file ({filepath})");
            }

            return (RootParam)param;
        }
    }
}
