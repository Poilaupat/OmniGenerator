using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Composition;
using System.Composition.Hosting;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using System.Reflection;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Configuration;

namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>
    /// Provides plugin discovery and access for the application using the Managed Extensibility Framework (MEF).
    /// This service locates, loads, and exposes plugins such as packagers and document drawers from the Plugins directory.
    /// </summary>
    internal sealed class PluginService : IPluginService
    {
        private CompositionHost _host;

        [ImportMany]
        private IEnumerable<ExportFactory<IDocumentDrawer, PluginMetadataView>>? Drawers { get; set; }

        [ImportMany]
        private IEnumerable<ExportFactory<IPackager, PluginMetadataView>>? Packagers { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="PluginService"/> class.
        /// Scans the Plugins directory for available plugins and composes them using MEF.
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">
        /// Thrown if the Plugins directory does not exist.
        /// </exception>
        public PluginService()
        {
            var pluginpath = Path.Combine(
                Path.GetDirectoryName(typeof(PluginService).Assembly.Location)!,
                "Plugins");

            if (!Directory.Exists(pluginpath))
                throw new DirectoryNotFoundException(pluginpath);

            var loader = new PluginLoader();
            _host = loader.LoadPlugins(pluginpath);
            _host.SatisfyImports(this);
        }

        /// <summary>
        /// Retrieves an <see cref="IPackager"/> plugin by its name.
        /// </summary>
        /// <param name="pluginname">The name of the packager plugin to retrieve.</param>
        /// <returns>
        /// An instance of <see cref="IPackager"/> if a matching plugin is found; otherwise, <c>null</c>.
        /// </returns>
        public IPackager? GetPackager(string? pluginname)
        {
            if (!string.IsNullOrWhiteSpace(pluginname))
            {
                //return GetPlugin<IPackager>(pluginname);
                return Packagers
                    ?.SingleOrDefault(e => e.Metadata.Name?.Equals(pluginname) ?? false)
                    ?.CreateExport()
                    .Value;
            }

            return null;
        }

        /// <summary>
        /// Retrieves an <see cref="IDocumentDrawer"/> plugin by its name.
        /// </summary>
        /// <param name="pluginname">The name of the document drawer plugin to retrieve.</param>
        /// <returns>
        /// An instance of <see cref="IDocumentDrawer"/> if a matching plugin is found; otherwise, <c>null</c>.
        /// </returns>
        public IDocumentDrawer? GetDocumentDrawer(string? pluginname)
        {
            if (!string.IsNullOrWhiteSpace(pluginname))
            {
                //return GetPlugin<IDocumentDrawer>(pluginname);
                return Drawers
                    ?.SingleOrDefault(e => e.Metadata.Name?.Equals(pluginname) ?? false)
                    ?.CreateExport()
                    .Value;
            }

            return null;
        }

        /// <summary>
        /// Gets metadata information about all available plugins of the specified type.
        /// </summary>
        /// <typeparam name="TPlugin">The type of plugin to search for (e.g., <see cref="IPackager"/>, <see cref="IDocumentDrawer"/>).</typeparam>
        /// <returns>
        /// An enumerable collection of <see cref="PluginInfo"/> objects describing each discovered plugin.
        /// </returns>
        public IEnumerable<PluginInfo> GetPlugins<TPlugin>()
            where TPlugin : class
        {
            var exports = _host.GetExports<TPlugin>();

            return exports.Select(e =>
            {
                var assembly = e.GetType().Assembly;
                return new PluginInfo
                {
                    PluginType = typeof(TPlugin).Name,
                    //Name = e.Metadata.Name,
                    //Description = e.Metadata.Description,
                    Location = assembly.Location,
                    AssemblyVersion = assembly.GetName().Version?.ToString() ?? "Unknown"
                };
            });
        }
    }
}
