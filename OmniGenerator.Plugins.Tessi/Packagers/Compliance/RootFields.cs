using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Tessi.Packagers.Compliance
{
    /// <summary>
    /// Strongly-typed field accessor for root-level fields in <see cref="EligibilityPackager"/>.
    /// Provides type-safe access to hierarchy root fields required for Wecheck Compliance.
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

        [FieldInfo("bankCode", "Bank code", isRequired: true)]
        public string BankCode => GetRequiredString("bankCode");

        [FieldInfo("bankUnitCode", "Bank unit code", isRequired: true)]
        public string BankUnitCode => GetRequiredString("bankUnitCode");

        [FieldInfo("providerCode", "Provider code", isRequired: true)]
        public string ProviderCode => GetRequiredString("providerCode");

        [FieldInfo("culture", "Culture/language code", isRequired: true)]
        public string Culture => GetRequiredString("culture");

        [FieldInfo("purpose", "Purpose of the transaction", isRequired: true)]
        public string Purpose => GetRequiredString("purpose");

        [FieldInfo("bankFlow", "Bank flow identifier", isRequired: true)]
        public string BankFlow => GetRequiredString("bankFlow");

        [FieldInfo("schema", "JSON schema reference", isRequired: true)]
        public string Schema => GetRequiredString("schema");

        [FieldInfo("version", "Version of the schema", isRequired: true)]
        public string Version => GetRequiredString("version");

        [FieldInfo("numlot", "Lot number", isRequired: true)]
        public Field Numlot => GetRequired("numlot");

        
    }
}
