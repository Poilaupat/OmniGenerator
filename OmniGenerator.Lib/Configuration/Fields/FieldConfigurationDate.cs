using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationDate : FieldConfigurationBase
    {
        [JsonPropertyName("day-diff-min")]
        public int DayDiffMin { get; set; }

        [JsonPropertyName("day-diff-max")]
        public int DayDiffMax { get; set; }
    }
}
