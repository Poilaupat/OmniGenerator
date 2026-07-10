using OmniGenerator.Lib.Configuration.ErrorSimulation;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.ErrorSimulation.Mutators
{
    /// <summary>
    /// Applies a single kind of error simulation to a field, mutating only the relevant channel.
    /// Implementations must never overwrite <see cref="Field.Value"/> (the source of truth).
    /// </summary>
    public interface IFieldErrorMutator
    {
        /// <summary>
        /// The error type handled by this mutator.
        /// </summary>
        EErrorSimulationType Type { get; }

        /// <summary>
        /// Mutates the appropriate channel of the given <paramref name="field"/>.
        /// </summary>
        /// <param name="field">The field to mutate in place.</param>
        void Mutate(Field field);
    }
}
