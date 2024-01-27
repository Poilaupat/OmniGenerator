using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    public enum EFFieldAggregateType
    {
        Count,
        Sum,
    }

    public enum EScope
    {
        Overall,
        DirectChildren,
    }

    public enum EAmountFormat
    {
        Euro,
        Cent,
    }

    public enum EKeyType
    {
        Rlmc,
        Rib,
        Tip,
        TipGroup6
    }
}
