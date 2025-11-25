using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class OmniGeneratorPluginMetadataAttribute(string pluginName, string pluginDescription) : Attribute
    {
        public string PluginName { get; } = pluginName;
        public string PluginDescription { get; } = pluginDescription;
    }
}
