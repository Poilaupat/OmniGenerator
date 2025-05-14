using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using System.Composition;

namespace OmniGenerator.Lib.Infrastructure
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PackagerPluginMetadataAttribute : ExportAttribute, IPluginMetadata
    {
        public string Name { get; }

        public string Description { get; }

        public PackagerPluginMetadataAttribute(string name, string description)
            : base(typeof(IPackager))
        {
            Name = name;
            Description = description;
        }
    }
}