using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Renderers.PaymentSlipRenderer
{
    /// <summary>
    /// Strongly-typed field accessor for <see cref="PaymentSlipRenderer"/>.
    /// Provides type-safe access to all TIP SEPA (Titre Interbancaire de Paiement SEPA) related fields
    /// required for rendering payment authorization slips.
    /// </summary>
    [FieldEntity(EPluginFieldEntityType.Document, "talon")]
    public sealed class PaymentSlipFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentSlipFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the document.</param>
        public PaymentSlipFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the amount to be debited from the payer's account.
        /// The amount is typically stored in cents (e.g., 12345 for 123.45 EUR).
        /// </summary>
        [FieldInfo("amount", "Amount to be debited", isRequired: true)]
        public Field Amount => GetRequiredField("amount");

        /// <summary>
        /// Gets the low OCRB (Optical Character Recognition-B) machine-readable line.
        /// This line contains encoded payment information for automated processing.
        /// </summary>
        [FieldInfo("low-line", "The low OCRB line", isRequired: true)]
        public string LowLine => GetRequiredString("low-line");

        /// <summary>
        /// Gets the high OCRB (Optical Character Recognition-B) machine-readable line.
        /// This line typically contains additional encoded information such as IBAN and reference numbers.
        /// </summary>
        [FieldInfo("high-line", "The high OCRB line", isRequired: true)]
        public string? HighLine => GetRequiredString("high-line");

        /// <summary>
        /// Gets the IBAN (International Bank Account Number) of the payer.
        /// If not provided, the form will display "JOIGNEZ UN RIB" (attach a bank statement).
        /// </summary>
        [FieldInfo("iban", "The payer's bank account", isRequired: false)]
        public string? Iban => GetOptionalString("iban");

        /// <summary>
        /// Gets the ICS (Identifiant Créancier SEPA) - the SEPA Creditor Identifier.
        /// This unique identifier is assigned to creditors who collect SEPA direct debits.
        /// </summary>
        [FieldInfo("ics", "Identifiant créancier SEPA", isRequired: false)]
        public string Ics => GetOptionalStringOrDefault("ics", string.Empty);

        /// <summary>
        /// Gets the RUM (Référence Unique de Mandat) - the Unique Mandate Reference.
        /// This reference uniquely identifies the SEPA direct debit mandate between the creditor and debtor.
        /// </summary>
        [FieldInfo("rum", "Référence Unique de Mandat", isRequired: false)]
        public string Rum => GetOptionalStringOrDefault("rum", string.Empty);

        /// <summary>
        /// Gets the full name of the payer (the person or entity making the payment).
        /// </summary>
        [FieldInfo("payor-name", "The payer's name", isRequired: false)]
        public string? PayorName => GetOptionalString("payor-name");

        /// <summary>
        /// Gets the payer's postal code and city name (e.g., "75001 Paris").
        /// </summary>
        [FieldInfo("payor-zip-city", "The payer's zip code and city name", isRequired: false)]
        public string? PayorZipCity => GetOptionalString("payor-zip-city");

        /// <summary>
        /// Gets the payer's street address.
        /// </summary>
        [FieldInfo("payor-address", "The payer's address", isRequired: false)]
        public string? PayorAddress => GetOptionalString("payor-address");

        /// <summary>
        /// Gets the name of the payee/creditor (the entity receiving the payment).
        /// </summary>
        [FieldInfo("payee-name", "The payee's name", isRequired: false)]
        public string? PayeeName => GetOptionalString("payee-name");

        /// <summary>
        /// Gets the payee's postal code and city name (e.g., "75001 Paris").
        /// </summary>
        [FieldInfo("payee-zip-city", "The payee's zip code and city name", isRequired: false)]
        public string? PayeeZipCity => GetOptionalString("payee-zip-city");

        /// <summary>
        /// Gets the payee's street address.
        /// </summary>
        [FieldInfo("payee-address", "The payee's address", isRequired: false)]
        public string? PayeeAddress => GetOptionalString("payee-address");

        /// <summary>
        /// Gets the content to be encoded in a DataMatrix 2D barcode.
        /// DataMatrix codes can store additional payment information in a compact, machine-readable format.
        /// </summary>
        [FieldInfo("datamatrix", "DataMatrix content", isRequired: false)]
        public string? Datamatrix => GetOptionalString("datamatrix");
    }
}