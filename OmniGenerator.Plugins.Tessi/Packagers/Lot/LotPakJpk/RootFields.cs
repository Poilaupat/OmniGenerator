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

    /// <summary>
    /// Gets the serial number of the scanner.
    /// </summary>
    [FieldInfo("scanner-serial-number", "Serial number of the scanner", isRequired: false, DefaultValue = "123456789")]
    public string MachineSerialNumber => GetOptionalString("scanner-serial-number", "123456789");

    /// <summary>
    /// Gets the packet date.
    /// </summary>
    [FieldInfo("packet-date", "Date of the packet", isRequired: false)]
    public DateTime PacketDate
    {
        get
        {
            if (TryGet("packet-date", out var field) && field.Value is DateTime dateValue)
                return dateValue;
            return DateTime.Now;
        }
    }

    /// <summary>
    /// Gets the capture point code.
    /// </summary>
    [FieldInfo("capture-point-code", "Code of the capture point", isRequired: true)]
    public string CapturePointCode => GetRequiredString("capture-point-code");

    /// <summary>
    /// Gets the scanner code.
    /// </summary>
    [FieldInfo("scanner-code", "Code of the scanner", isRequired: false, DefaultValue = "001")]
    public string ScannerCode => GetOptionalString("scanner-code", "001");

    /// <summary>
    /// Gets the organization unit code.
    /// </summary>
    [FieldInfo("organization-unit-code", "Code of the organization unit", isRequired: true)]
    public string OrganizationUnitCode => GetRequiredString("organization-unit-code");

    /// <summary>
    /// Gets the organization code.
    /// </summary>
    [FieldInfo("organization-code", "Code of the organization", isRequired: true)]
    public string OrganizationCode => GetRequiredString("organization-code");

    /// <summary>
    /// Gets the process code.
    /// </summary>
    [FieldInfo("process-code", "Process code", isRequired: false, DefaultValue = "000")]
    public string ProcessCode => GetOptionalString("process-code", "000");

    /// <summary>
    /// Gets the reconciliation flag.
    /// </summary>
    [FieldInfo("reconciliation", "Reconciliation flag", isRequired: false, DefaultValue = "0")]
    public string Reconciliation => GetOptionalString("reconciliation", "0");
}
}
