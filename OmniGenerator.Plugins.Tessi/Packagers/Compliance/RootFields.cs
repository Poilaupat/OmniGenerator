using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Tessi.Packagers.Compliance
{
    /// <summary>
    /// Strongly-typed field accessor for root-level fields in <see cref="EligibilityPackager"/>.
    /// Provides type-safe access to hierarchy root fields required for Wecheck Compliance export.
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
        /// Gets the bank code identifying the financial institution.
        /// This is typically a standardized code used in banking systems.
        /// </summary>
        [FieldInfo("bankCode", "Bank code", isRequired: true)]
        public string BankCode => GetRequiredString("bankCode");

        /// <summary>
        /// Gets the bank unit code identifying a specific branch or operational unit within the bank.
        /// </summary>
        [FieldInfo("bankUnitCode", "Bank unit code", isRequired: true)]
        public string BankUnitCode => GetRequiredString("bankUnitCode");

        /// <summary>
        /// Gets the provider code identifying the service provider or partner organization.
        /// </summary>
        [FieldInfo("providerCode", "Provider code", isRequired: true)]
        public string ProviderCode => GetRequiredString("providerCode");

        /// <summary>
        /// Gets the culture/language code (e.g., "fr-FR", "en-US")
        /// </summary>
        [FieldInfo("culture", "Culture/language code", isRequired: true)]
        public string Culture => GetRequiredString("culture");

        /// <summary>
        /// Gets the purpose or reason code for the transaction batch.
        /// Describes the business context or intent of the processing operation.
        /// </summary>
        [FieldInfo("purpose", "Purpose of the transaction", isRequired: true)]
        public string Purpose => GetRequiredString("purpose");

        /// <summary>
        /// Gets the bank flow identifier indicating the type of banking operation or workflow.
        /// </summary>
        [FieldInfo("bankFlow", "Bank flow identifier", isRequired: true)]
        public string BankFlow => GetRequiredString("bankFlow");

        /// <summary>
        /// Gets the JSON schema reference URL or identifier.
        /// Specifies the schema used for validating the compliance output format.
        /// </summary>
        [FieldInfo("schema", "JSON schema reference", isRequired: true)]
        public string Schema => GetRequiredString("schema");

        /// <summary>
        /// Gets the version number of the schema being used.
        /// Ensures compatibility and proper validation of the compliance data structure.
        /// </summary>
        [FieldInfo("version", "Version of the schema", isRequired: true)]
        public string Version => GetRequiredString("version");

        /// <summary>
        /// Gets the lot number (numéro de lot) uniquely identifying this batch of documents.
        /// Used for tracking and traceability in the compliance system.
        /// </summary>
        [FieldInfo("numlot", "Lot number", isRequired: true)]
        public Field Numlot => GetRequiredField("numlot");
    }
}
