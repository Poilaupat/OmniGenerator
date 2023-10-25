using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.MetaData
{
    public abstract class MetaDataGeneratorBase
    {
        public string Name { get; }
        
        public MetaDataGeneratorBase(string name)
        {
            Name = name;
        }

        public abstract string NextValue();
    }
}
