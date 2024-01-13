using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
