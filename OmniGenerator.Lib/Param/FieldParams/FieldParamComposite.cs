using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param.FieldParams
{
    public class FieldParamComposite : FieldParamDependantBase
    {
        private string? _format;

        [JsonPropertyName("format")]
        public string? Format 
        { 
            get { return _format; }
            set { _format = value?.Replace("-", "_"); }
        }
    }
}
