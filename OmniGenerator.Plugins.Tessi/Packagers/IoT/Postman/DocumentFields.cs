using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Tessi.Packagers.IoT.Postman
{
    [FieldEntity(EPluginFieldEntityType.Document, "Any Document")]
    internal class DocumentFields : FieldExtractorBase
    {
        public DocumentFields(FieldCollection fields) : base(fields)
        {
        }

        [FieldInfo("z4", "MICR zone 4", isRequired: true)]
        public string Z4 => GetRequiredString("z4");

        [FieldInfo("z3", "MICR zone 3", isRequired: true)]
        public string Z3 => GetRequiredString("z3");

        [FieldInfo("z2", "MICR zone 2", isRequired: false, DefaultValue = "<Blanks>")]
        public string Z2 => GetOptionalStringOrDefault("z2", string.Empty.PadLeft(32, ' '));

        [FieldInfo("dataread", "MICR data read", isRequired: true)]
        public string Dataread => GetRequiredString("dataread");
    }
}
