using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.ErrorSimulation
{
    /// <summary>
    /// Centralized configuration for realistic scanner and human error simulation.
    /// All rules are declared here; inline configuration on fields is not supported.
    /// </summary>
    public class ErrorSimulationConfiguration
    {
        /// <summary>
        /// Enables or disables the whole error simulation pass. Defaults to <c>false</c>.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// The set of error simulation rules to evaluate for each generated document.
        /// </summary>
        [JsonPropertyName("rules")]
        public List<ErrorSimulationRule> Rules { get; set; } = new();

        /// <summary>
        /// Optional list of error types disabled globally without removing their rules.
        /// </summary>
        [JsonPropertyName("disabled-errors")]
        public List<EErrorSimulationType> DisabledErrors { get; set; } = new();
    }
}
