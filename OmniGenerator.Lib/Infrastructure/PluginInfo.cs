using OmniGenerator.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>  
    /// The parent type of the plugin, indicating whether it is a packager or a drawer.
    /// </summary>  
    public enum EPluginParentType
    {
        /// <summary>  
        /// Indicates that the plugin is a packager.  
        /// </summary>  
        Packager,

        /// <summary>  
        /// Indicates that the plugin is a document drawer.  
        /// </summary>  
        Drawer
    }

    /// <summary>  
    /// Encapsulates detailed information about a plugin, such as its name, description, type, location, and parent type.  
    /// </summary>  
    public sealed class PluginInfo
    {
        /// <summary>  
        /// Gets the name of the plugin.  
        /// </summary>  
        public string PluginName { get; }

        /// <summary>  
        /// Gets the description of the plugin.  
        /// </summary>  
        public string PluginDescription { get; }

        /// <summary>  
        /// Gets the <see cref="Type"/> of the plugin.  
        /// </summary>  
        public Type PluginType { get; }

        /// <summary>  
        /// Gets the parent type of the plugin, indicating whether it is a packager or a drawer.  
        /// </summary>  
        /// <exception cref="NotImplementedException">  
        /// Thrown if the plugin type does not match any known parent type.  
        /// </exception>  
        public EPluginParentType ParentType => PluginType switch
        {
            _ when typeof(IDocumentDrawer).IsAssignableFrom(PluginType) => EPluginParentType.Drawer,
            _ when typeof(IPackager).IsAssignableFrom(PluginType) => EPluginParentType.Packager,
            _ => throw new NotImplementedException()
        };

        /// <summary>  
        /// Gets the file system location of the plugin assembly.  
        /// </summary>  
        public string Location { get; }

        /// <summary>  
        /// Gets the version of the plugin assembly.  
        /// </summary>  
        public string AssemblyVersion { get; }

        /// <summary>  
        /// Initializes a new instance of the <see cref="PluginInfo"/> class.  
        /// </summary>  
        /// <param name="pluginName">The name of the plugin.</param>  
        /// <param name="pluginDescription">The description of the plugin.</param>  
        /// <param name="pluginType">The <see cref="Type"/> of the plugin.</param>  
        /// <param name="location">The file system location of the plugin assembly.</param>  
        /// <param name="assemblyVersion">The version of the plugin assembly.</param>  
        public PluginInfo(
            string pluginName,
            string pluginDescription,
            Type pluginType,
            string location,
            Version? assemblyVersion)
        {
            PluginName = pluginName;
            PluginDescription = pluginDescription;
            PluginType = pluginType;
            Location = location;
            AssemblyVersion = assemblyVersion?.ToString() ?? "Unknown";
        }
    }
}
