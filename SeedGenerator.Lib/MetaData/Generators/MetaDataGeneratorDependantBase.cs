using System.Text.Json.Serialization;

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
