using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces.Infrastructure;

namespace OmniGenerator.Lib.Services
{
    /// <summary>
    /// Service for analyzing plugins to extract field usage information.
    /// </summary>
    public class PluginFieldAnalyzer
    {
        private readonly IPluginService _pluginService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginFieldAnalyzer"/> class.
        /// </summary>
        /// <param name="pluginService">The plugin service to use for retrieving plugin information.</param>
        public PluginFieldAnalyzer(IPluginService pluginService)
        {
            _pluginService = pluginService;
        }

        /// <summary>
        /// Gets field information for a specific plugin.
        /// </summary>
        /// <param name="pluginName">The name of the plugin.</param>
        /// <returns>An enumerable collection of field information, or null if the plugin is not found or doesn't document its fields.</returns>
        public IEnumerable<FieldInfo>? GetFieldsForPlugin(string pluginName)
        {
            var plugin = _pluginService.GetAllPluginsInfo()
                .FirstOrDefault(p => p.PluginName.Equals(pluginName, StringComparison.InvariantCultureIgnoreCase));

            if (plugin == null)
                return null;

            return GetFieldsForPluginType(plugin.PluginType);
        }

        /// <summary>
        /// Extracts field information from a plugin type.
        /// </summary>
        /// <param name="pluginType">The type of the plugin to analyze.</param>
        /// <returns>An enumerable collection of field information.</returns>
        private IEnumerable<FieldInfo> GetFieldsForPluginType(Type pluginType)
        {
            var instance = Activator.CreateInstance(pluginType);

            // All plugins must implement GetFieldsDocumentation through IOmniGeneratorPlugin
            if (instance is IOmniGeneratorPlugin plugin)
            {
                return plugin.GetFieldsDocumentation();
            }

            // This should never happen as all plugins inherit from IOmniGeneratorPlugin
            return Enumerable.Empty<FieldInfo>();
        }
    }
}
