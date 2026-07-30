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
    /// Strongly-typed field accessor for cheque document fields in generic remittance rendering.
    /// Provides type-safe access to cheque-specific data such as payer information and amount.
    /// </summary>
    [FieldEntity(EPluginFieldEntityType.Document, "cheque")]
    internal class ChequeFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChequeFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the cheque document.</param>
        public ChequeFields(FieldCollection fields) : base(fields, FieldChannel.Data) // Reading from the data channel here (even if it's in an image rendering context) is intended for cheque fields, to allow inconsistency simulation
        {
        }

        /// <summary>
        /// Gets the name of the cheque payer (the person or entity who wrote the cheque).
        /// Returns null if the payer name is not specified.
        /// </summary>
        [FieldInfo("payor-name", "The name of the cheque emitter", isRequired: false)]
        public string? PayorName => GetOptionalString("payor-name");

        /// <summary>
        /// Gets the data read from the cheque.
        /// </summary>
        [FieldInfo("dataread", "The data read from the cheque", isRequired: true)]
        public string Dataread => GetRequiredString("dataread");

        /// <summary>
        /// Gets the amount of the cheque in cents.
        /// For example, a value of 12345 represents 123.45 in the local currency.
        /// </summary>
        [FieldInfo("amount", "The amount of the cheque in cents", isRequired: true)]
        public int Amount => GetRequiredValue("amount").Convert<int>();
    }
}
