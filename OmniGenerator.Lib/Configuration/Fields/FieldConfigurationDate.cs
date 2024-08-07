using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public class FieldConfigurationDate : AbstractFieldConfigurationBase
    {
        [JsonPropertyName("day-diff-min")]
        public required int DayDiffMin { get; set; }

        [JsonPropertyName("day-diff-max")]
        public required int DayDiffMax { get; set; }
    }
}
