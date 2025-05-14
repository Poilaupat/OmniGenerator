using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using System.Composition;

namespace OmniGenerator.Lib.Infrastructure
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DrawerPluginMetadataAttribute : ExportAttribute, IPluginMetadata
    {
        public string Name { get; }

        public string Description { get; }

        public DrawerPluginMetadataAttribute(string name, string description)
            : base(typeof(IDocumentDrawer))
        {
            Name = name;
            Description = description;
        }
    }
}