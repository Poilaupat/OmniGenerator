using OmniGenerator.Lib.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>  
    /// Represents the base class for all OmniGenerator plugins.  
    /// Provides common functionality for retrieving plugin metadata such as name and description.  
    /// </summary>  
    public abstract class OmniGeneratorPluginBase : IOmniGeneratorPlugin
    {
        /// <summary>  
        /// Gets the name of the plugin.  
        /// The name is retrieved from the <see cref="OmniGeneratorPluginMetadataAttribute"/> applied to the plugin class.  
        /// </summary>  
        public string PluginName =>
            GetType()
            .GetCustomAttributes(typeof(OmniGeneratorPluginMetadataAttribute), false)
            .FirstOrDefault() is OmniGeneratorPluginMetadataAttribute attribute ? attribute.PluginName : string.Empty;

        /// <summary>  
        /// Gets the description of the plugin.  
        /// The description is retrieved from the <see cref="OmniGeneratorPluginMetadataAttribute"/> applied to the plugin class.  
        /// </summary>  
        public string PluginDescription =>
            GetType()
            .GetCustomAttributes(typeof(OmniGeneratorPluginMetadataAttribute), false)
            .FirstOrDefault() is OmniGeneratorPluginMetadataAttribute attribute ? attribute.PluginDescription : string.Empty;
    }
}
