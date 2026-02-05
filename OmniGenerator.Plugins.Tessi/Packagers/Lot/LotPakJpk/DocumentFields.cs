using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    /// <summary>
    /// Strongly-typed field accessor for document-level fields in <see cref="LotPakJpkPackager"/>.
    /// Provides type-safe access to document fields required for LOT+PAK+JPK format.
    /// </summary>
    [FieldEntity(EPluginFieldEntityType.Document)]
    public class DocumentFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the document.</param>
        public DocumentFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the encoding line type.
        /// </summary>
        [FieldInfo("encline", "Encoding line type", isRequired: true)]
        public string Encline => GetRequiredString("encline");

        /// <summary>
        /// Gets the data read from the document.
        /// </summary>
        [FieldInfo("dataread", "Data read from the document", isRequired: false, DefaultValue = "")]
        public string Dataread => GetOptionalStringOrDefault("dataread", string.Empty);

        /// <summary>
        /// Gets the quality code.
        /// </summary>
        [FieldInfo("quality-code", "Quality code", isRequired: false, DefaultValue = "0")]
        public string QualityCode => GetOptionalStringOrDefault("quality-code", "0");

        /// <summary>
        /// Gets the document reference.
        /// </summary>
        [FieldInfo("ref-doc", "Document reference", isRequired: false, DefaultValue = "")]
        public string RefDoc => GetOptionalStringOrDefault("ref-doc", string.Empty);

        /// <summary>
        /// Gets the signature.
        /// </summary>
        [FieldInfo("signature", "Signature", isRequired: false, DefaultValue = "---SIGNATURE---")]
        public string Signature => GetOptionalStringOrDefault("signature", "---SIGNATURE---");

        /// <summary>
        /// Gets the status.
        /// </summary>
        [FieldInfo("status", "Status", isRequired: false, DefaultValue = "0")]
        public string Status => GetOptionalStringOrDefault("status", "0");

        /// <summary>
        /// Gets the priority.
        /// </summary>
        [FieldInfo("priority", "Priority", isRequired: false, DefaultValue = "")]
        public string Priority => GetOptionalStringOrDefault("priority", string.Empty);

        /// <summary>
        /// Gets the RIB (Bank account identifier).
        /// </summary>
        [FieldInfo("rib", "Bank account identifier", isRequired: false, DefaultValue = "")]
        public string RIB => GetOptionalStringOrDefault("rib", string.Empty);

        /// <summary>
        /// Gets the number of checks.
        /// </summary>
        [FieldInfo("nb-checks", "Number of checks", isRequired: false, DefaultValue = "")]
        public string NbChecks => GetOptionalStringOrDefault("nb-checks", string.Empty);

        /// <summary>
        /// Gets the ICR confidence amount.
        /// </summary>
        [FieldInfo("icr-conf-amount", "ICR confidence amount", isRequired: false, DefaultValue = "")]
        public string ICRConfAmount => GetOptionalStringOrDefault("icr-conf-amount", string.Empty);

        /// <summary>
        /// Gets the ICR amount.
        /// </summary>
        [FieldInfo("icr-amount", "ICR amount", isRequired: false, DefaultValue = "")]
        public string ICRAmount => GetOptionalStringOrDefault("icr-amount", string.Empty);

        /// <summary>
        /// Gets the sort error.
        /// </summary>
        [FieldInfo("sort-error", "Sort error", isRequired: false, DefaultValue = "0")]
        public string SortError => GetOptionalStringOrDefault("sort-error", "0");

        /// <summary>
        /// Gets the image quality.
        /// </summary>
        [FieldInfo("image-quality", "Image quality", isRequired: false, DefaultValue = "0")]
        public string ImageQuality => GetOptionalStringOrDefault("image-quality", "0");

        /// <summary>
        /// Gets the deleted flag.
        /// </summary>
        [FieldInfo("deleted", "Deleted flag", isRequired: false, DefaultValue = "0")]
        public string Deleted => GetOptionalStringOrDefault("deleted", "0");
    }
}
