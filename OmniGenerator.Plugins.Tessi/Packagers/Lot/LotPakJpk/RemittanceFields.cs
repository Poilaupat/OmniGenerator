using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    /// <summary>
    /// Strongly-typed field accessor for remittance-level fields in <see cref="LotPakJpkPackager"/>.
    /// Provides type-safe access to remittance (group) fields required for LOT+PAK+JPK format.
    /// </summary>
    [FieldEntity(EPluginFieldEntityType.Group, "remittance")]
    public class RemittanceFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemittanceFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the remittance.</param>
        public RemittanceFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the remittance identifier.
        /// </summary>
        [FieldInfo("remittance-id", "Remittance identifier", isRequired: false, DefaultValue = "")]
        public string RemittanceId => GetOptionalStringOrDefault("remittance-id", string.Empty);
    }
}
