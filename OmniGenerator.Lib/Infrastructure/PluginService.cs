using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using System.Reflection;
using OmniGenerator.Lib.Interfaces;

namespace OmniGenerator.Lib.Infrastructure
{
    internal sealed class PluginService : IPluginService
    {
        private CompositionContainer _container;

        public PluginService()
        {
            var pluginpath = Path.Combine(
                Path.GetDirectoryName(typeof(PluginService).Assembly.Location)!,
                "Plugins");

            if (!Directory.Exists(pluginpath))
                throw new DirectoryNotFoundException(pluginpath);

            var catalog = new DirectoryCatalog(pluginpath);
            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);
        }

        public IPackager? GetPackager(string? pluginname)
        {
            if (!string.IsNullOrWhiteSpace(pluginname))
            {
                return GetPlugin<IPackager>(pluginname);
            }

            return null;
        }

        public IDocumentDrawer? GetDocumentDrawer(string? pluginname)
        {
            if (!string.IsNullOrWhiteSpace(pluginname))
            {
                return GetPlugin<IDocumentDrawer>(pluginname);
            }

            return null;
        }

        public IEnumerable<PluginInfo> GetPlugins<TPlugin>()
            where TPlugin : class
        {
            var exports = _container.GetExports<TPlugin, IPluginMetadata>();

            return exports.Select(e =>
            {
                var assembly = e.Value.GetType().Assembly;
                return new PluginInfo
                {
                    PluginType = typeof(TPlugin).Name,
                    Name = e.Metadata.Name,
                    Description = e.Metadata.Description,
                    Location = assembly.Location,
                    AssemblyVersion = assembly.GetName().Version?.ToString() ?? "Unknown"
                };
            });
        }

        private TPlugin? GetPlugin<TPlugin>(string pluginname)
            where TPlugin : class
        {
            return _container
                .GetExports<TPlugin, IPluginMetadata>()
                .SingleOrDefault(e => e.Metadata.Name.Equals(pluginname))
                ?.Value;
        }
    }
}
