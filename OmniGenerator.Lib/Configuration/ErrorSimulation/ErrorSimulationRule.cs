using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.ErrorSimulation
{
    /// <summary>
    /// Declares a single error simulation rule targeting a document field.
    /// Rules are centralized in <see cref="ErrorSimulationConfiguration"/>; inline configuration is not supported.
    /// </summary>
    public class ErrorSimulationRule
    {
        /// <summary>
        /// The name of the document the rule targets.
        /// </summary>
        [JsonPropertyName("target-document")]
        public required string TargetDocument { get; set; }

        /// <summary>
        /// The name of the field the rule targets. Any field, including composite fields, can be targeted.
        /// </summary>
        [JsonPropertyName("target-field")]
        public required string TargetField { get; set; }

        /// <summary>
        /// The type of error to simulate.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required EErrorSimulationType Type { get; set; }

        /// <summary>
        /// The per-document probability the rule triggers, expressed in the [0, 1] range (for example 0.02 for 2%).
        /// </summary>
        [JsonPropertyName("probability")]
        public required double Probability { get; set; }
    }
}
