using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration.Fields
{
    public abstract class AbstractFieldConfigurationBase
    {
        private string _name = default!;

        [JsonPropertyName("name"), JsonPropertyOrder(0)]
        public required string Name { get { return _name; } set { _name = value; } }

        public override bool Equals(object? obj)
        {
            if (obj is not null && obj is AbstractFieldConfigurationBase field)
                return this.Name == field.Name;
            
            return false;
        }

        public override int GetHashCode() 
        { 
            return Name.GetHashCode(); 
        }
    }
}
