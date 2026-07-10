using OmniGenerator.Lib.Configuration.ErrorSimulation;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.ErrorSimulation
{
    /// <summary>
    /// Applies realistic scanner and human error simulations to a generated hierarchy.
    /// The simulation mutates only the data or image channel of targeted fields, leaving the
    /// canonical <see cref="Field.Value"/> untouched for traceability.
    /// </summary>
    public interface IErrorSimulator
    {
        /// <summary>
        /// Applies the configured error simulation rules to every matching document in the hierarchy.
        /// </summary>
        /// <param name="root">The generated hierarchy to mutate in place.</param>
        /// <param name="configuration">The centralized error simulation configuration.</param>
        void Apply(Root root, ErrorSimulationConfiguration configuration);
    }
}
