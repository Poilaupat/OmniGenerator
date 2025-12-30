using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    /// <summary>
    /// Strongly-typed field accessor for <see cref="LotPakJpkPackager"/>.
    /// Provides type-safe access to root-level fields required for LOT+PAK+JPK format.
    /// </summary>
    public class LotPakJpkPackagerFields : PluginFieldsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LotPakJpkPackagerFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the root.</param>
        public LotPakJpkPackagerFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the name of the packet.
        /// </summary>
        [PluginField("packet-name", "Name of the packet", isRequired: false, DefaultValue = "DefaultName")]
        [PluginFieldEntity(EPluginFieldEntityType.Root)]
        public string PacketName => GetOptionalString("packet-name", "DefaultName");

        /// <summary>
        /// Gets the number of the packet.
        /// </summary>
        [PluginField("packet-number", "Number of the packet", isRequired: false, DefaultValue = "0001")]
        [PluginFieldEntity(EPluginFieldEntityType.Root)]
        public string PacketNumber => GetOptionalString("packet-number", "0001");
    }
}
