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
            var catalog = new AssemblyCatalog(typeof(PluginService).Assembly);
            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);
        }

        public IPackager? GetPackager(string pluginname) 
        {
            return GetPlugin<IPackager>(pluginname);
        }

        public IDocumentDrawer? GetImageComposer(string pluginname)
        {
            return GetPlugin<IDocumentDrawer>(pluginname);
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
