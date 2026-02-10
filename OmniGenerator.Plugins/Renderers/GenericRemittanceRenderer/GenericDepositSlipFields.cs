using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Renderers.GenericDepositSlipRenderer
{
    /// <summary>
    /// Strongly-typed field accessor for generic deposit slip document fields.
    /// Provides type-safe access to fields required for rendering bank deposit slips (bordereaux de remise).
    /// A deposit slip is a document that accompanies cheques being deposited at a bank,
    /// summarizing the deposit transaction details.
    /// </summary>
    [FieldEntity(EPluginFieldEntityType.Document, "deposit-slip")]
    public sealed class GenericDepositSlipFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDepositSlipFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the deposit slip document.</param>
        public GenericDepositSlipFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the title displayed on the deposit slip.
        /// Defaults to "BORDEREAU DE REMISE GÉNÉRIQUE" (Generic Deposit Slip) if not specified.
        /// </summary>
        [FieldInfo("title", "The title of the deposit slip", isRequired: false, DefaultValue = "BORDEREAU DE REMISE GÉNÉRIQUE")]
        public string Title => GetOptionalStringOrDefault("title", "BORDEREAU DE REMISE GÉNÉRIQUE");

        /// <summary>
        /// Gets the CMC7 line data.
        /// CMC7 is a magnetic ink character recognition format used on French cheques and banking documents
        /// for automated processing. This line typically contains the bank code, branch code, account number,
        /// and cheque number in a machine-readable format.
        /// </summary>
        [FieldInfo("dataread", "The CMC7 line", isRequired: true)]
        public string Dataread => GetRequiredString("dataread");

        /// <summary>
        /// Gets the date when the deposit (remittance) was made or processed.
        /// This date is used for tracking and reconciliation purposes in the banking system.
        /// </summary>
        [FieldInfo("date-remise", "The date of the remittance", isRequired: true)]
        public DateTime DateRemise => (DateTime)GetRequiredField("date-remise").Value;
    }
}
