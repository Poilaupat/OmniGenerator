using SeedGenerator.Lib.Param.FieldParams;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    public abstract class ElementParam
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("min-occurs")]
        public int MinOccurs { get; set; } = 1;

        [JsonPropertyName("max-occurs")]
        public int MaxOccurs { get; set; } = 100;

        [JsonPropertyName("fields")]
        public List<FieldParamBase> Fields { get; set; } = new List<FieldParamBase>();

        [JsonPropertyName("field-configuration-file")]
        public string FieldConfigurationFile { get; set; } = string.Empty;

        public void MergeFields(IEnumerable<FieldParamBase> fields)
        {
            foreach (var field in fields)
            {
                if (!Fields.Any(x => x.Name == field.Name))
                {
                    Fields.Add(field);
                }
            }
        }
    }
}
