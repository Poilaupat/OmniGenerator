using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Data.FieldGenerators
{
    /// <summary>
    /// The aggregate type of the <see cref="FieldGeneratorAggregate" generators
    /// </summary>
    public enum EFFieldAggregateType
    {
        Count,
        Sum,
    }

    /// <summary>
    /// The scope of the <see cref="FieldGeneratorAggregate"/> generators
    /// </summary>
    public enum EScope
    {
        Overall,
        DirectChildren,
    }

    /// <summary>
    /// The key type of the <see cref="FieldGeneratorKeyCalculator"/> generators
    /// </summary>
    public enum EKeyType
    {
        Rlmc,
        Rib,
        Tip,
        TipGroup6
    }
}
