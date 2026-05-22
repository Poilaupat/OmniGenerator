using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Renderers.DepositSlipRenderer
{
    /// <summary>
    /// Strongly-typed field accessor for generic remittance (deposit) group-level fields.
    /// Provides type-safe access to aggregate fields that summarize the total count and amount
    /// of cheques within a remittance batch.
    /// </summary>
    [FieldEntity(EPluginFieldEntityType.Group, "Remittance")]
    internal class RemittanceFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemittanceFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the remittance group.</param>
        public RemittanceFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the total count of cheques in the remittance.
        /// Returns null if the field is not present or cannot be converted to an integer.
        /// </summary>
        /// <remarks>
        /// This is typically an aggregate field that counts all cheque documents within the remittance group.
        /// </remarks>
        public int? TotalCheques
        {
            get
            {
                if (GetOptionalField("total-cheque") is Field field && field.Value is int intValue)
                    return intValue;
                return null;
            }
        }

        /// <summary>
        /// Gets the total amount of all cheques in the remittance.
        /// The amount is typically stored in cents (e.g., 12345 for 123.45 EUR).
        /// Returns null if the field is not present or cannot be converted to an integer.
        /// </summary>
        /// <remarks>
        /// This is typically an aggregate field that sums all individual cheque amounts within the remittance group.
        /// </remarks>
        public int? Amount
        {
            get
            {
                if (GetOptionalField("total-amount") is Field field && field.Value is int intValue)
                    return intValue;
                return null;
            }
        }
    }
}