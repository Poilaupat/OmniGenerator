using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Renderers
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
        /// Gets the Group 3 part of OCRB code.
        /// </summary>
        [FieldInfo("group3", "Group 3 part of OCRB code", isRequired: true)]
        public string Group3 => GetRequiredString("group3");

        /// <summary>
        /// Gets the Group 2 part of OCRB code.
        /// </summary>
        [FieldInfo("group2", "Group 2 part of OCRB code", isRequired: true)]
        public string Group2 => GetRequiredString("group2");

        /// <summary>
        /// Gets the Group 1 part of OCRB code.
        /// </summary>
        [FieldInfo("group1", "Group 1 part of OCRB code", isRequired: true)]
        public string Group1 => GetRequiredString("group1");
    }
}
