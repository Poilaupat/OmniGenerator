using OmniGenerator.Lib.Configuration.Fields;
using OmniGenerator.Lib.Interfaces.FieldGenerators;

namespace OmniGenerator.Lib.Interfaces
{
    /// <summary>
    /// Interface for mapping field configurations to field generators.
    /// </summary>
    internal interface IFieldMapper
    {
        /// <summary>
        /// Maps a single field configuration to its corresponding field generator.
        /// </summary>
        /// <param name="config">The field configuration to map.</param>
        /// <returns>The mapped field generator.</returns>
        IFieldGenerator Map(AbstractFieldConfigurationBase config);

        /// <summary>
        /// Maps a list of field configurations to their corresponding field generators.
        /// </summary>
        /// <param name="configs">The list of field configurations to map.</param>
        /// <returns>A list of mapped field generators.</returns>
        List<IFieldGenerator> MapList(List<AbstractFieldConfigurationBase> configs);
    }
}
