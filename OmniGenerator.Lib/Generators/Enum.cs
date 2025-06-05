using OmniGenerator.Lib.Generators.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Generators
{
    /// <summary>
    /// Specifies the aggregate operation type for <see cref="FieldGeneratorAggregate"/> generators.
    /// </summary>
    public enum EFFieldAggregateType
    {
        /// <summary>
        /// Represents a count aggregation.
        /// </summary>
        Count,
        /// <summary>
        /// Represents a sum aggregation.
        /// </summary>
        Sum,
    }

    /// <summary>
    /// Specifies the scope for <see cref="FieldGeneratorAggregate"/> generators.
    /// </summary>
    public enum EScope
    {
        /// <summary>
        /// The aggregation applies to all elements overall.
        /// </summary>
        Overall,
        /// <summary>
        /// The aggregation applies only to direct children.
        /// </summary>
        DirectChildren,
    }

    /// <summary>
    /// Specifies the key type for <see cref="FieldGeneratorKeyCalculator"/> generators.
    /// </summary>
    public enum EKeyType
    {
        /// <summary>
        /// Dummy key type (for unit test).
        /// </summary>
        Dummy,
        /// <summary>
        /// RLMC key type.
        /// </summary>
        Rlmc,
        /// <summary>
        /// RIB key type.
        /// </summary>
        Rib,
        /// <summary>
        /// TIP key type.
        /// </summary>
        Tip,
        /// <summary>
        /// TIP Group 6 key type.
        /// </summary>
        TipGroup6
    }
}
