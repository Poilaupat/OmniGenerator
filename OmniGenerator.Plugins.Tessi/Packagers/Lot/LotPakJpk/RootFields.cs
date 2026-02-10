using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    /// <summary>
    /// Strongly-typed field accessor for <see cref="LotPakJpkPackager"/>.
    /// Provides type-safe access to root-level fields required for LOT+PAK+JPK format export.
    /// </summary>
    [FieldEntity(EPluginFieldEntityType.Root)]
    public class RootFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the root hierarchy level.</param>
        public RootFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the name of the packet.
        /// This name is used as the base filename for the generated LOT, PAK, and JPK files.
        /// </summary>
        [FieldInfo("packet-name", "Name of the packet", isRequired: false, DefaultValue = "DefaultName")]
        public string PacketName => GetOptionalStringOrDefault("packet-name", "DefaultName");

        /// <summary>
        /// Gets the sequential number of the packet.
        /// Used for tracking and ordering multiple packets in a batch processing workflow.
        /// </summary>
        [FieldInfo("packet-number", "Number of the packet", isRequired: false, DefaultValue = "0001")]
        public string PacketNumber => GetOptionalStringOrDefault("packet-number", "0001");

        /// <summary>
        /// Gets the serial number of the scanner device.
        /// Identifies the physical or virtual scanner used to process the documents.
        /// </summary>
        [FieldInfo("scanner-serial-number", "Serial number of the scanner", isRequired: false, DefaultValue = "123456789")]
        public string MachineSerialNumber => GetOptionalStringOrDefault("scanner-serial-number", "123456789");

        /// <summary>
        /// Gets the date when the packet was created or processed.
        /// Defaults to the current date and time if not specified.
        /// </summary>
        [FieldInfo("packet-date", "Date of the packet. If not specified, defaults to the current date and time.", isRequired: false)]
        public DateTime PacketDate
        {
            get
            {
                if (GetOptionalField("packet-date") is Field field && field.Value is DateTime dateValue)
                    return dateValue;
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Gets the capture point code identifying the physical or logical location where documents are captured.
        /// </summary>
        [FieldInfo("capture-point-code", "Code of the capture point", isRequired: true)]
        public string CapturePointCode => GetRequiredString("capture-point-code");

        /// <summary>
        /// Gets the scanner code identifying the specific scanner.
        /// Distinguishes between different scanner setups at the same capture point.
        /// </summary>
        [FieldInfo("scanner-code", "Code of the scanner", isRequired: false, DefaultValue = "001")]
        public string ScannerCode => GetOptionalStringOrDefault("scanner-code", "001");

        /// <summary>
        /// Gets the organization unit code identifying the business unit or department.
        /// </summary>
        [FieldInfo("organization-unit-code", "Code of the organization unit", isRequired: true)]
        public string OrganizationUnitCode => GetRequiredString("organization-unit-code");

        /// <summary>
        /// Gets the organization code identifying the parent organization or company.
        /// </summary>
        [FieldInfo("organization-code", "Code of the organization", isRequired: true)]
        public string OrganizationCode => GetRequiredString("organization-code");

        /// <summary>
        /// Gets the process code identifying the business process or workflow type.
        /// Determines how documents in this packet should be processed downstream.
        /// </summary>
        [FieldInfo("process-code", "Process code", isRequired: false, DefaultValue = "000")]
        public string ProcessCode => GetOptionalStringOrDefault("process-code", "000");

        /// <summary>
        /// Gets the reconciliation flag indicating whether this packet requires reconciliation.
        /// </summary>
        [FieldInfo("reconciliation", "Reconciliation flag", isRequired: false, DefaultValue = "0")]
        public string Reconciliation => GetOptionalStringOrDefault("reconciliation", "0");
    }
}
