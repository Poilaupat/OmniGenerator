using OmniGenerator.Lib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Interfaces.Infrastructure
{
    /// <summary>  
    /// Defines the contract for a service that manages plugins in the application.  
    /// Provides methods to retrieve plugin instances and their metadata.  
    /// </summary>  
    public interface IPluginService
    {
        /// <summary>  
        /// Retrieves a plugin instance by its name.  
        /// </summary>  
        /// <typeparam name="TPlugin">The type of plugin to retrieve.</typeparam>  
        /// <param name="pluginName">The name of the plugin to retrieve.</param>  
        /// <returns>An instance of the plugin if found; otherwise, <c>null</c>.</returns>  
        TPlugin? GetPlugin<TPlugin>(string pluginName) where TPlugin : IOmniGeneratorPlugin;

        /// <summary>  
        /// Gets metadata information about all available plugins of the specified type.  
        /// </summary>  
        /// <typeparam name="TPlugin">The type of plugin to search for (e.g., <see cref="IPackager"/>, <see cref="IDocumentDrawer"/>).</typeparam>  
        /// <returns>  
        /// An enumerable collection of <see cref="PluginInfo"/> objects describing each discovered plugin.  
        /// </returns>  
        IEnumerable<PluginInfo> GetPluginsInfo<TPlugin>() where TPlugin : IOmniGeneratorPlugin;
    }
}
