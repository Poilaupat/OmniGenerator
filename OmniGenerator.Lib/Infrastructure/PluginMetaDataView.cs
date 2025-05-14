using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Infrastructure
{
    public class PluginMetadataView
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public PluginMetadataView() { }

        public PluginMetadataView(IDictionary<string, object> metadata)
        {
            if (metadata.TryGetValue(nameof(Name), out var name))
                Name = name as string ?? string.Empty;
            if (metadata.TryGetValue(nameof(Description), out var desc))
                Description = desc as string ?? string.Empty;
        }
    }
}
