using OmniGenerator.Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Interfaces.Infrastructure
{
    public interface IPluginService
    {
        IPackager? GetPackager(string? pluginname);

        IDocumentDrawer? GetDocumentDrawer(string? pluginname);

        IEnumerable<PluginInfo> GetPlugins<TPlugin>() where TPlugin : class;
    }
}
