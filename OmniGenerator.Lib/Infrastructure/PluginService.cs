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
    public class PluginService : IPluginService
    {
        private CompositionContainer _container;

        public PluginService()
        {
            //var catalog = new AssemblyCatalog(typeof(PluginService).Assembly);
            var pluginpath = Path.Combine(
                Path.GetDirectoryName(typeof(PluginService).Assembly.Location),
                "Plugins");

            var catalog = new DirectoryCatalog(pluginpath);
            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);
        }

        public IPackager? GetPackager(string? pluginname) 
        {
            if (pluginname is not null)
            {
                return GetPlugin<IPackager>(pluginname);
            }

            return null;
        }

        public IDocumentDrawer? GetImageComposer(string? pluginname)
        {
            if (pluginname is not null)
            {
                return GetPlugin<IDocumentDrawer>(pluginname);
            }

            return null;
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
