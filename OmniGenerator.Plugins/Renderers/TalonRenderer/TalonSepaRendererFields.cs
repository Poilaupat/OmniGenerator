using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Renderers.TalonRenderer
{
    /// <summary>
    /// Strongly-typed field accessor for <see cref="TalonSepaRenderer"/>.
    /// Provides type-safe access to all TIP SEPA related fields.
    /// </summary>
    [FieldEntity(EPluginFieldEntityType.Document, "talon")]
    public class TalonSepaRendererFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TalonSepaRendererFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the document.</param>
        public TalonSepaRendererFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the amount to be debited.
        /// </summary>
        [FieldInfo("amount", "Amount to be debited", isRequired: true)]
        public Field Amount => GetRequired("amount");

        /// <summary>
        /// Gets the low OCRB line.
        /// </summary>
        [FieldInfo("low-line", "The low OCRB line", isRequired: true)]
        public string LowLine => GetRequiredString("low-line");
    }
}
