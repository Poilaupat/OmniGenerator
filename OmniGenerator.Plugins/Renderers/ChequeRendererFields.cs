using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Renderers
{
    /// <summary>
    /// Strongly-typed field accessor for <see cref="ChequeRenderer"/>.
    /// Provides type-safe access to all cheque-related fields.
    /// </summary>
    public class ChequeRendererFields : PluginFieldsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChequeRendererFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the document.</param>
        public ChequeRendererFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the CMC7 code with separator chars.
        /// </summary>
        [PluginField("dataread", "CMC7 code with separator chars", isRequired: true)]
        public string Dataread => GetRequiredString("dataread");

        /// <summary>
        /// Gets the RLMC key.
        /// </summary>
        [PluginField("rlmc", "RLMC key", isRequired: true)]
        public string Rlmc => GetRequiredString("rlmc");

        /// <summary>
        /// Gets the bank name.
        /// </summary>
        [PluginField("bank-name", "Bank name", isRequired: false, DefaultValue = "Default Bank Name")]
        public string BankName => GetOptionalString("bank-name", "Default Bank Name");

        /// <summary>
        /// Gets the bank address.
        /// </summary>
        [PluginField("bank-address", "Bank address", isRequired: false, DefaultValue = "")]
        public string BankAddress => GetOptionalString("bank-address", string.Empty);

        /// <summary>
        /// Gets the bank zipcode and city.
        /// </summary>
        [PluginField("bank-zip-city", "Bank zipcode and city", isRequired: false, DefaultValue = "")]
        public string BankZipCity => GetOptionalString("bank-zip-city", string.Empty);

        /// <summary>
        /// Gets the bank phone number.
        /// </summary>
        [PluginField("bank-phone", "Bank phone number", isRequired: false, DefaultValue = "")]
        public string BankPhone => GetOptionalString("bank-phone", string.Empty);

        /// <summary>
        /// Gets the payor name.
        /// </summary>
        [PluginField("payor-name", "Payor name", isRequired: true)]
        public string PayorName => GetRequiredString("payor-name");

        /// <summary>
        /// Gets the payor address.
        /// </summary>
        [PluginField("payor-address", "Payor address", isRequired: true)]
        public string PayorAddress => GetRequiredString("payor-address");

        /// <summary>
        /// Gets the payor zipcode and city.
        /// </summary>
        [PluginField("payor-zip-city", "Payor zipcode and city", isRequired: true)]
        public string PayorZipCity => GetRequiredString("payor-zip-city");

        /// <summary>
        /// Gets the cheque amount field (for formatting).
        /// </summary>
        [PluginField("amount", "Cheque amount", isRequired: true)]
        public Field Amount => GetRequired("amount");

        /// <summary>
        /// Gets the cheque amount as a string.
        /// </summary>
        public string AmountString => GetRequiredString("amount");

        /// <summary>
        /// Gets the payee name.
        /// </summary>
        [PluginField("payee-name", "Payee name", isRequired: true)]
        public string PayeeName => GetRequiredString("payee-name");

        /// <summary>
        /// Gets the place where the cheque was issued.
        /// </summary>
        [PluginField("place", "Place where the cheque was issued", isRequired: true)]
        public string Place => GetRequiredString("place");

        /// <summary>
        /// Gets the date when the cheque was issued.
        /// </summary>
        [PluginField("date", "Date when the cheque was issued", isRequired: true)]
        public string Date => GetRequiredString("date");

        /// <summary>
        /// Gets the deposit account number (verso).
        /// </summary>
        [PluginField("deposit-account", "Deposit account number (verso)", isRequired: true)]
        public string DepositAccount => GetRequiredString("deposit-account");
    }
}
