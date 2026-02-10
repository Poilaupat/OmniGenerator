using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Renderers.ChequeRenderer
{
    /// <summary>
    /// Strongly-typed field accessor for <see cref="ChequeRenderer"/>.
    /// Provides type-safe access to all cheque-related fields.
    /// </summary>
    [FieldEntity(EPluginFieldEntityType.Document, "cheque")]
    public sealed class ChequeRendererFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChequeRendererFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the document.</param>
        public ChequeRendererFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the CMC7 line.
        /// </summary>
        [FieldInfo("dataread", "CMC7 line", isRequired: true)]
        public string Dataread => GetRequiredString("dataread");

        /// <summary>
        /// Gets the RLMC key.
        /// </summary>
        [FieldInfo("rlmc", "RLMC key", isRequired: true)]
        public string Rlmc => GetRequiredString("rlmc");

        /// <summary>
        /// Gets the bank name.
        /// </summary>
        [FieldInfo("bank-name", "Bank name", isRequired: false, DefaultValue = "OmniBank")]
        public string BankName => GetOptionalStringOrDefault("bank-name", "OmniBank");

        /// <summary>
        /// Gets the bank address.
        /// </summary>
        [FieldInfo("bank-address", "Bank address", isRequired: false, DefaultValue = "1 rue de la république")]
        public string BankAddress => GetOptionalStringOrDefault("bank-address", "1 rue de la république");

        /// <summary>
        /// Gets the bank zipcode and city.
        /// </summary>
        [FieldInfo("bank-zip-city", "Bank zipcode and city", isRequired: false, DefaultValue = "75001 Paris")]
        public string BankZipCity => GetOptionalStringOrDefault("bank-zip-city", "75001 Paris");

        /// <summary>
        /// Gets the bank phone number.
        /// </summary>
        [FieldInfo("bank-phone", "Bank phone number", isRequired: false, DefaultValue = "01 02 03 04 05")]
        public string BankPhone => GetOptionalStringOrDefault("bank-phone", "01 02 03 04 05");

        /// <summary>
        /// Gets the payor name.
        /// </summary>
        [FieldInfo("payor-name", "Payor name", isRequired: false, DefaultValue = "Bernard Pleinhausas")]
        public string PayorName => GetOptionalStringOrDefault("payor-name", "Bernard Pleinhausas");

        /// <summary>
        /// Gets the payor address.
        /// </summary>
        [FieldInfo("payor-address", "Payor address", isRequired: false, DefaultValue = "42 rue de la maille")]
        public string PayorAddress => GetOptionalStringOrDefault("payor-address", "42 rue de la maille");

        /// <summary>
        /// Gets the payor zipcode and city.
        /// </summary>
        [FieldInfo("payor-zip-city", "Payor zipcode and city", isRequired: false, DefaultValue = "75016 Paris")]
        public string PayorZipCity => GetOptionalStringOrDefault("payor-zip-city", "75016 Paris");

        /// <summary>
        /// Gets the cheque amount field (for formatting).
        /// </summary>
        [FieldInfo("amount", "Cheque amount", isRequired: true)]
        public Field Amount => GetRequiredField("amount");

        /// <summary>
        /// Gets the payee name.
        /// </summary>
        [FieldInfo("payee-name", "Payee name", isRequired: true)]
        public string PayeeName => GetRequiredString("payee-name");

        /// <summary>
        /// Gets the place where the cheque was issued.
        /// </summary>
        [FieldInfo("place", "Place where the cheque was issued", isRequired: true)]
        public string Place => GetRequiredString("place");

        /// <summary>
        /// Gets the date when the cheque was issued.
        /// </summary>
        [FieldInfo("date", "Date when the cheque was issued", isRequired: true)]
        public DateTime Date => (DateTime)GetRequiredField("date").Value;

        /// <summary>
        /// Gets the deposit account number (verso).
        /// </summary>
        [FieldInfo("deposit-account", "Deposit account number (verso)", isRequired: true)]
        public string DepositAccount => GetRequiredString("deposit-account");
    }
}
