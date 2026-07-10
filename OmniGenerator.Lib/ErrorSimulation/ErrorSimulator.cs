using Microsoft.Extensions.Logging;
using OmniGenerator.Lib.Configuration.ErrorSimulation;
using OmniGenerator.Lib.ErrorSimulation.Mutators;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.ErrorSimulation
{
    /// <summary>
    /// Default <see cref="IErrorSimulator"/> implementation.
    /// Iterates the configured rules against every matching document, draws a per-document
    /// probability, and applies the relevant mutator to the field's data or image channel.
    /// A field can undergo at most one error type per document; the first triggered rule wins.
    /// </summary>
    public sealed class ErrorSimulator : IErrorSimulator
    {
        private readonly IReadOnlyDictionary<EErrorSimulationType, IFieldErrorMutator> _mutators;
        private readonly ILogger<ErrorSimulator> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorSimulator"/> class.
        /// </summary>
        /// <param name="mutators">The available field error mutators, one per error type.</param>
        /// <param name="logger">The logger used for structured debug traces.</param>
        public ErrorSimulator(IEnumerable<IFieldErrorMutator> mutators, ILogger<ErrorSimulator> logger)
        {
            _mutators = mutators.ToDictionary(m => m.Type);
            _logger = logger;
        }

        /// <inheritdoc />
        public void Apply(Root root, ErrorSimulationConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(root);
            ArgumentNullException.ThrowIfNull(configuration);

            if (!configuration.Enabled || configuration.Rules.Count == 0)
            {
                _logger.LogDebug("Error simulation skipped (enabled={Enabled}, ruleCount={RuleCount}).",
                    configuration.Enabled, configuration.Rules.Count);
                return;
            }

            var disabledErrors = configuration.DisabledErrors.ToHashSet();
            var documentsByName = root
                .GetAllDocuments()
                .GroupBy(d => d.Name)
                .ToDictionary(g => g.Key, g => g.ToList());

            var appliedCount = 0;

            foreach (var document in root.GetAllDocuments())
            {
                // Tracks fields already mutated on this document instance to enforce one error per field.
                var mutatedFields = new HashSet<string>();

                foreach (var rule in configuration.Rules)
                {
                    if (!rule.TargetDocument.Equals(document.Name))
                        continue;

                    if (disabledErrors.Contains(rule.Type))
                        continue;

                    if (mutatedFields.Contains(rule.TargetField))
                        continue;

                    if (!document.Fields.TryGetValue(rule.TargetField, out var field) || field is null)
                        continue;

                    if (Random.Shared.NextDouble() >= rule.Probability)
                        continue;

                    if (!_mutators.TryGetValue(rule.Type, out var mutator))
                        continue;

                    mutator.Mutate(field);
                    mutatedFields.Add(rule.TargetField);
                    appliedCount++;

                    _logger.LogDebug(
                        "Applied {ErrorType} on document '{Document}' field '{Field}' (probability={Probability}).",
                        rule.Type, document.Name, rule.TargetField, rule.Probability);
                }
            }

            _logger.LogDebug("Error simulation completed: {AppliedCount} error(s) applied across {DocumentTypeCount} document type(s).",
                appliedCount, documentsByName.Count);
        }
    }
}
