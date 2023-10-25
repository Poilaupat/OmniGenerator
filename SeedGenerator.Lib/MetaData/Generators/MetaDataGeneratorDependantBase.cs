using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.MetaData.Generators
{
    public abstract class MetaDataGeneratorDependantBase : MetaDataGeneratorBase
    {
        public string DependantUpon { get; }

        [JsonIgnore]
        public string? DependantValue { get; set; }


        public MetaDataGeneratorDependantBase(string name, string dependantUpon)
            : base(name)
        {
            DependantUpon = dependantUpon;
        }
    }
}
