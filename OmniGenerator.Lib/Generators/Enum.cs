using OmniGenerator.Lib.Generators.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Generators
{
    /// <summary>
    /// Enumerates the types of aggregate operations available for <see cref="FieldGeneratorAggregate"/> generators.
    /// </summary>
    public enum EFFieldAggregateType
    {
        /// <summary>
        /// Aggregates by counting the number of elements.
        /// </summary>
        Count,
        /// <summary>
        /// Aggregates by summing the values of elements.
        /// </summary>
        Sum,
    }

    /// <summary>
    /// Enumerates the possible scopes for aggregation in <see cref="FieldGeneratorAggregate"/> generators.
    /// </summary>
    public enum EScope
    {
        /// <summary>
        /// The aggregation is performed over all children elements, regardless of hierarchy.
        /// </summary>
        Overall,
        /// <summary>
        /// The aggregation is performed only on direct child elements.
        /// </summary>
        DirectChildren,
    }

    /// <summary>
    /// Enumerates the supported key types for <see cref="FieldGeneratorKeyCalculator"/> generators.
    /// </summary>
    public enum EKeyType
    {
        /// <summary>
        /// Dummy key type, typically used for unit testing.
        /// </summary>
        Dummy,
        /// <summary>
        /// RLMC (Recomposition Ligne Magnétique Chèque) key type.
        /// </summary>
        Rlmc,
        /// <summary>
        /// RIB (Relevé d'Identité Bancaire) key type.
        /// </summary>
        Rib,
        /// <summary>
        /// TIP (Titre Interbancaire de Paiement) key type.
        /// </summary>
        Tip,
        /// <summary>
        /// TIP Group 6 key type, used only in the group 6 of a TIP.
        /// </summary>
        TipGroup6
    }
}
