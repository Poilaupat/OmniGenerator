using SeedGenerator.Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.MetaData
{
    public class MetaDataGeneratorKeyRlmc : MetaDataGeneratorDependantBase
    {
        public MetaDataGeneratorKeyRlmc(string name, string dependantUpon) 
            : base(name, dependantUpon)
        {
        }

        public override string NextValue()
        {
            return KeyTools.ComputeRlmcKey(DependantValue ?? "0");
        }

    }
}
