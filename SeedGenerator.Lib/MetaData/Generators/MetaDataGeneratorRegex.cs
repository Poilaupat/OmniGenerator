using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fare;

namespace SeedGenerator.Lib.MetaData.Generators
{
    public class MetaDataGeneratorRegex : MetaDataGeneratorBase
    {
        public string Pattern { get; set; }

        public MetaDataGeneratorRegex(string name, string pattern)
            : base(name)
        {
            Pattern = pattern;
        }

        public override string NextValue()
        {
            var xeger = new Xeger(Pattern, new Random());
            return xeger.Generate();
        }
    }
}
