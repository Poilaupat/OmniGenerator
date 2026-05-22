using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Tessi.Packagers.Compliance
{
    /// <summary>
    /// Strongly-typed field accessor for cheque document fields in <see cref="EligibilityPackager"/>.
    /// Provides type-safe access to document-level fields for cheque processing in Wecheck Compliance.
    /// </summary>
    [FieldEntity(EPluginFieldEntityType.Document, "cheque")]
    internal class ChequeFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChequeFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the cheque document.</param>
        public ChequeFields(FieldCollection fields) : base(fields)
        {
        }

        // Document-level fields
        [FieldInfo("scanner", "Scanner identifier", isRequired: true)]
        public string Scanner => GetRequiredString("scanner");

        [FieldInfo("scanType", "Type of scan", isRequired: true)]
        public string ScanType => GetRequiredString("scanType");

        [FieldInfo("chain", "Processing chain", isRequired: true)]
        public string Chain => GetRequiredString("chain");

        [FieldInfo("z4", "MICR zone 4", isRequired: true)]
        public string Z4 => GetRequiredString("z4");

        [FieldInfo("z3", "MICR zone 3", isRequired: true)]
        public string Z3 => GetRequiredString("z3");

        [FieldInfo("z2", "MICR zone 2", isRequired: true)]
        public string Z2 => GetRequiredString("z2");

        [FieldInfo("amount", "Check amount", isRequired: true)]
        public Field Amount => GetRequiredField("amount");

        [FieldInfo("providerId", "Provider identifier", isRequired: true)]
        public string ProviderId => GetRequiredString("providerId");

        [FieldInfo("remittingBranchCode", "Remitting branch code", isRequired: true)]
        public string RemittingBranchCode => GetRequiredString("remittingBranchCode");

        [FieldInfo("deskCode", "Desk code", isRequired: true)]
        public string DeskCode => GetRequiredString("deskCode");

        [FieldInfo("accountNumber", "Account number", isRequired: true)]
        public string AccountNumber => GetRequiredString("accountNumber");
    }
}
