using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugin.Tessi.Packagers.Compliance
{
    /// <summary>
    /// Strongly-typed field accessor for <see cref="EligibilityPackager"/>.
    /// Provides type-safe access to root and document-level fields required for Wecheck Compliance.
    /// </summary>
    public class EligibilityPackagerFields : PluginFieldsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EligibilityPackagerFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the root or document.</param>
        public EligibilityPackagerFields(FieldCollection fields) : base(fields)
        {
        }

        // Root-level fields
        [PluginField("bankCode", "Bank code", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Root)]
        public string BankCode => GetRequiredString("bankCode");

        [PluginField("bankUnitCode", "Bank unit code", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Root)]
        public string BankUnitCode => GetRequiredString("bankUnitCode");

        [PluginField("providerCode", "Provider code", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Root)]
        public string ProviderCode => GetRequiredString("providerCode");

        [PluginField("culture", "Culture/language code", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Root)]
        public string Culture => GetRequiredString("culture");

        [PluginField("purpose", "Purpose of the transaction", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Root)]
        public string Purpose => GetRequiredString("purpose");

        [PluginField("bankFlow", "Bank flow identifier", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Root)]
        public string BankFlow => GetRequiredString("bankFlow");

        [PluginField("schema", "JSON schema reference", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Root)]
        public string Schema => GetRequiredString("schema");

        [PluginField("version", "Version of the schema", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Root)]
        public string Version => GetRequiredString("version");

        [PluginField("numlot", "Lot number", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Root)]
        public Field Numlot => GetRequired("numlot");

        // Document-level fields
        [PluginField("scanner", "Scanner identifier", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Document)]
        public string Scanner => GetRequiredString("scanner");

        [PluginField("scanType", "Type of scan", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Document)]
        public string ScanType => GetRequiredString("scanType");

        [PluginField("chain", "Processing chain", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Document)]
        public string Chain => GetRequiredString("chain");

        [PluginField("z4", "MICR zone 4", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Document)]
        public string Z4 => GetRequiredString("z4");

        [PluginField("z3", "MICR zone 3", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Document)]
        public string Z3 => GetRequiredString("z3");

        [PluginField("z2", "MICR zone 2", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Document)]
        public string Z2 => GetRequiredString("z2");

        [PluginField("amount", "Check amount", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Document)]
        public Field Amount => GetRequired("amount");

        [PluginField("providerId", "Provider identifier", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Document)]
        public string ProviderId => GetRequiredString("providerId");

        [PluginField("remittingBranchCode", "Remitting branch code", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Document)]
        public string RemittingBranchCode => GetRequiredString("remittingBranchCode");

        [PluginField("deskCode", "Desk code", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Document)]
        public string DeskCode => GetRequiredString("deskCode");

        [PluginField("accountNumber", "Account number", isRequired: true)]
        [PluginFieldEntity(EPluginFieldEntityType.Document)]
        public string AccountNumber => GetRequiredString("accountNumber");
    }
}
