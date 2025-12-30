using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugin.Tessi.Packagers.Compliance
{
    /// <summary>
    /// Strongly-typed field accessor for <see cref="EligibilityPackager"/>.
    /// Provides type-safe access to root and document-level fields required for Wecheck Compliance.
    /// </summary>
    public class EligibilityPackagerFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EligibilityPackagerFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the root or document.</param>
        public EligibilityPackagerFields(FieldCollection fields) : base(fields)
        {
        }

        // Root-level fields
        [FieldInfo("bankCode", "Bank code", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Root)]
        public string BankCode => GetRequiredString("bankCode");

        [FieldInfo("bankUnitCode", "Bank unit code", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Root)]
        public string BankUnitCode => GetRequiredString("bankUnitCode");

        [FieldInfo("providerCode", "Provider code", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Root)]
        public string ProviderCode => GetRequiredString("providerCode");

        [FieldInfo("culture", "Culture/language code", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Root)]
        public string Culture => GetRequiredString("culture");

        [FieldInfo("purpose", "Purpose of the transaction", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Root)]
        public string Purpose => GetRequiredString("purpose");

        [FieldInfo("bankFlow", "Bank flow identifier", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Root)]
        public string BankFlow => GetRequiredString("bankFlow");

        [FieldInfo("schema", "JSON schema reference", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Root)]
        public string Schema => GetRequiredString("schema");

        [FieldInfo("version", "Version of the schema", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Root)]
        public string Version => GetRequiredString("version");

        [FieldInfo("numlot", "Lot number", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Root)]
        public Field Numlot => GetRequired("numlot");

        // Document-level fields
        [FieldInfo("scanner", "Scanner identifier", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Document)]
        public string Scanner => GetRequiredString("scanner");

        [FieldInfo("scanType", "Type of scan", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Document)]
        public string ScanType => GetRequiredString("scanType");

        [FieldInfo("chain", "Processing chain", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Document)]
        public string Chain => GetRequiredString("chain");

        [FieldInfo("z4", "MICR zone 4", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Document)]
        public string Z4 => GetRequiredString("z4");

        [FieldInfo("z3", "MICR zone 3", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Document)]
        public string Z3 => GetRequiredString("z3");

        [FieldInfo("z2", "MICR zone 2", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Document)]
        public string Z2 => GetRequiredString("z2");

        [FieldInfo("amount", "Check amount", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Document)]
        public Field Amount => GetRequired("amount");

        [FieldInfo("providerId", "Provider identifier", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Document)]
        public string ProviderId => GetRequiredString("providerId");

        [FieldInfo("remittingBranchCode", "Remitting branch code", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Document)]
        public string RemittingBranchCode => GetRequiredString("remittingBranchCode");

        [FieldInfo("deskCode", "Desk code", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Document)]
        public string DeskCode => GetRequiredString("deskCode");

        [FieldInfo("accountNumber", "Account number", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Document)]
        public string AccountNumber => GetRequiredString("accountNumber");
    }
}
