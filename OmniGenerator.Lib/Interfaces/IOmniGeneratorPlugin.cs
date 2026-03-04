using OmniGenerator.Lib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Interfaces
{
    /// <summary>
    /// Defines the contract for OmniGenerator plugins.
    /// Plugins implementing this interface must provide metadata such as name, description, and field documentation.
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

        /// <summary>
        /// Gets the collection of fields used by this plugin.
        /// All plugins must document the fields they use.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="FieldInfo"/> describing the fields.</returns>
        IEnumerable<FieldInfo> GetFieldsDocumentation();
    }
}
