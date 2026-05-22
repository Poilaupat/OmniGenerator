using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Renderers.BatchTicketRenderer
{
    /// <summary>
    /// Strongly-typed field accessor for generic deposit slip document fields.
    /// Provides type-safe access to fields required for rendering bank deposit slips (bordereaux de remise).
    /// A deposit slip is a document that accompanies cheques being deposited at a bank,
    /// summarizing the deposit transaction details.
    /// </summary>
    [FieldEntity(EPluginFieldEntityType.Document, "batch-ticket")]
    public sealed class BatchTicketFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchTicketFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the batch ticket document.</param>
        public BatchTicketFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the title displayed on the batch ticket.
        /// Defaults to "TICKET LOT" if not specified.
        /// </summary>
        [FieldInfo("title", "The title of the batch ticket", isRequired: false, DefaultValue = "TICKET LOT")]
        public string Title => GetOptionalStringOrDefault("title", "TICKET LOT");

        /// <summary>
        /// Gets the CMC7 line data.
        /// CMC7 is a magnetic ink character recognition format used on French cheques and banking documents
        /// for automated processing. This line typically contains the bank code, branch code, account number,
        /// and cheque number in a machine-readable format.
        /// </summary>
        [FieldInfo("dataread", "The CMC7 line", isRequired: true)]
        public string Dataread => GetRequiredString("dataread");
    }
}
