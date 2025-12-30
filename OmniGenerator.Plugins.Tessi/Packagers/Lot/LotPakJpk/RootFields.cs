using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    /// <summary>
    /// Strongly-typed field accessor for <see cref="LotPakJpkPackager"/>.
    /// Provides type-safe access to root-level fields required for LOT+PAK+JPK format.
    /// </summary>
    [FieldEntity(EPluginFieldEntityType.Root)]
    public class RootFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the root.</param>
        public RootFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the name of the packet.
        /// </summary>
        [FieldInfo("packet-name", "Name of the packet", isRequired: false, DefaultValue = "DefaultName")]
        public string PacketName => GetOptionalString("packet-name", "DefaultName");

        /// <summary>
        /// Gets the number of the packet.
        /// </summary>
        [FieldInfo("packet-number", "Number of the packet", isRequired: false, DefaultValue = "0001")]
        public string PacketNumber => GetOptionalString("packet-number", "0001");
    }
}
