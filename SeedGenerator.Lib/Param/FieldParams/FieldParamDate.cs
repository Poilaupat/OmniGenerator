using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    public class FieldParamDate : FieldParamBase
    {
        [JsonPropertyName("day-diff-min")]
        public int DayDiffMin { get; set; }

        [JsonPropertyName("day-diff-max")]
        public int DayDiffMax { get; set; }
    }
}
