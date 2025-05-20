using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Infrastructure
{
    public class OmniGeneratorPluginMetadataAttribute : Attribute
    {
        public string PluginName { get; }
        public string PluginDescription { get; }

        public OmniGeneratorPluginMetadataAttribute(string pluginName, string pluginDescription)
        {
            PluginName = pluginName;
            PluginDescription = pluginDescription;
        }
    }
}
