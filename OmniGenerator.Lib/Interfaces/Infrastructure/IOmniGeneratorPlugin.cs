using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Interfaces.Infrastructure
{
    /// <summary>
    /// Defines the contract for OmniGenerator plugins.
    /// Plugins implementing this interface must provide metadata such as name and description.
    /// </summary>
    public interface IOmniGeneratorPlugin
    {
        /// <summary>
        /// Gets the name of the plugin.
        /// This name is used to identify the plugin within the application.
        /// </summary>
        string PluginName { get; }

        /// <summary>
        /// Gets the description of the plugin.
        /// This description provides additional information about the plugin's functionality.
        /// </summary>
        string PluginDescription { get; }
    }
}
