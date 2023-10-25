using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.MetaData
{
    public class MetaDataGeneratorFixedValue : MetaDataGeneratorBase
    {
        public string FixedValue { get; }

        public MetaDataGeneratorFixedValue(string name, string fixedvalue) 
            : base(name)
        {
            FixedValue = fixedvalue;
        }

        public override string NextValue()
        {
            return FixedValue;
        }
    }
}
