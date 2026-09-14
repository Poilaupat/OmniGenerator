using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Tessi.Packagers.IoT.Postman
{
    [FieldEntity(EPluginFieldEntityType.Root, "batch")]
    internal class BatchFields : FieldExtractorBase
    {
        public BatchFields(FieldCollection fields) : base(fields)
        {
        }

        public string CodeOrganisation => GetRequiredString("code-org");
        public string CodeOrganizationUnit => GetRequiredString("code-unit-org");
        public string CodeScanner => GetRequiredString("code-cs");
    }
}
