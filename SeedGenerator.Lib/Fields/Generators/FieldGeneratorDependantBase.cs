using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Fields.Generators
{
    public abstract class FieldGeneratorDependantBase : FieldGeneratorBase
    {
        public string DependantUpon { get; }

        [JsonIgnore]
        public string? DependantValue { get; set; }


        public FieldGeneratorDependantBase(string name, string dependantUpon)
            : base(name)
        {
            DependantUpon = dependantUpon;
        }
    }
}
