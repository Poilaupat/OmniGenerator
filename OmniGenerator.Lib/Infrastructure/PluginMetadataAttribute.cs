using OmniGenerator.Lib.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Infrastructure
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PluginMetadataAttribute : ExportAttribute, IPluginMetadata
    {
        public string Name { get; }

        public string Description { get; }

        public PluginMetadataAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}