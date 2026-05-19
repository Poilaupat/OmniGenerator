using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Configuration;
using McMaster.NETCore.Plugins;

namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>
    /// Provides plugin discovery and access for the application.
    /// This service locates, loads, and exposes plugins such as packagers and document drawers from the Plugins directory.
    /// </summary>
    internal sealed class PluginService : IPluginService
    {
        private Dictionary<string, PluginInfo> _repository = new Dictionary<string, PluginInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginService"/> class.
        /// Scans the Plugins directory for available plugins.
        /// If the Plugins directory does not exist, the service starts with no plugins loaded.
        /// </summary>
        public PluginService() => LoadPlugins();

        /// <summary>
        /// Loads plugins from the Plugins directory.
        /// Each plugin is expected to be in a folder with the same name as its DLL file.
        /// If the Plugins directory does not exist, no plugins are loaded and the method returns silently.
        /// </summary>
        private void LoadPlugins()
        {
            var pluginsDir = Path.Combine(AppContext.BaseDirectory, "plugins");
            if (!Directory.Exists(pluginsDir))
                return;

            foreach (var directory in Directory.GetDirectories(pluginsDir))
            {
                var dll = Path.Combine(directory, Path.GetFileName(directory) + ".dll"); // Plugin folder MUST have the same name as the dll  
                if (File.Exists(dll))
                {
                    var loader = PluginLoader.CreateFromAssemblyFile(
                            dll,
                            sharedTypes: new[] { typeof(IPackager), typeof(IDocumentRenderer) });

                    foreach (var pluginType in loader
                        .LoadDefaultAssembly()
                        .GetTypes()
                        .Where(t => typeof(OmniGeneratorPluginBase).IsAssignableFrom(t) && !t.IsAbstract))
                    {
                        TryAddPlugin(pluginType);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves a plugin instance by its name.
        /// </summary>
        /// <typeparam name="TPlugin">The type of plugin to retrieve.</typeparam>
        /// <param name="pluginName">The name of the plugin to retrieve.</param>
        /// <returns>An instance of the plugin if found; otherwise, <c>null</c>.</returns>
        public TPlugin? GetPlugin<TPlugin>(string pluginName)
            where TPlugin : IOmniGeneratorPlugin
        {
            if (_repository.TryGetValue(pluginName, out var pluginInfo))
            {
                var instance = Activator.CreateInstance(pluginInfo.PluginType);
                if (instance is not null)
                {
                    return (TPlugin)instance;
                }
            }

            return default;
        }

        /// <summary>
        /// Attempts to add a plugin to the repository.
        /// </summary>
        /// <param name="pluginType">The type of the plugin to add.</param>
        private void TryAddPlugin(Type pluginType)
        {
            var attribute = pluginType.GetCustomAttribute<OmniGeneratorPluginMetadataAttribute>();
            if (attribute is null) return;

            var assembly = pluginType.Assembly;
            var pi = new PluginInfo(
                attribute.PluginName,
                attribute.PluginDescription,
                pluginType,
                assembly.Location,
                assembly.GetName().Version
            );

            if (!_repository.ContainsKey(pi.PluginName))
                _repository.Add(pi.PluginName, pi);
        }

        /// <summary>
        /// Gets metadata information about all available plugins of the specified type.
        /// </summary>
        /// <typeparam name="TPlugin">The type of plugin to search for (e.g., <see cref="IPackager"/>, <see cref="IDocumentRenderer"/>).</typeparam>
        /// <returns>
        /// An enumerable collection of <see cref="PluginInfo"/> objects describing each discovered plugin.
        /// </returns>
        public IEnumerable<PluginInfo> GetPluginsInfo<TPlugin>()
            where TPlugin : IOmniGeneratorPlugin
        {
            return _repository
                .Where(p => p.Value.PluginType.IsAssignableTo(typeof(TPlugin)))
                .Select(p => p.Value);
        }

        /// <summary>
        /// Retrieves information about all available plugins.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="PluginInfo"/> objects representing all plugins. The collection will
        /// be empty if no plugins are available.</returns>
        public IEnumerable<PluginInfo> GetAllPluginsInfo()
        {
            return _repository.Values;
        }
    }
}
