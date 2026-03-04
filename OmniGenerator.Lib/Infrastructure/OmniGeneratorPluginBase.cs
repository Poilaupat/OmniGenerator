using OmniGenerator.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>
    /// Represents the base class for all OmniGenerator plugins.
    /// Provides common functionality for retrieving plugin metadata such as name and description.
    /// </summary>
    public abstract class OmniGeneratorPluginBase : IOmniGeneratorPlugin
    {
        /// <summary>
        /// Gets the name of the plugin.
        /// The name is retrieved from the <see cref="OmniGeneratorPluginMetadataAttribute"/> applied to the plugin class.
        /// </summary>
        public string PluginName =>
            GetType()
            .GetCustomAttributes(typeof(OmniGeneratorPluginMetadataAttribute), false)
            .FirstOrDefault() is OmniGeneratorPluginMetadataAttribute attribute ? attribute.PluginName : string.Empty;

        /// <summary>
        /// Gets the description of the plugin.
        /// The description is retrieved from the <see cref="OmniGeneratorPluginMetadataAttribute"/> applied to the plugin class.
        /// </summary>
        public string PluginDescription =>
            GetType()
            .GetCustomAttributes(typeof(OmniGeneratorPluginMetadataAttribute), false)
            .FirstOrDefault() is OmniGeneratorPluginMetadataAttribute attribute ? attribute.PluginDescription : string.Empty;

        /// <summary>
        /// Gets the collection of fields used by this plugin.
        /// By default, automatically extracts documentation from A class named "{PluginClassName}Fields" in the same namespace
        /// Override this method to provide custom documentation or specify a different fields class.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="FieldInfo"/> describing the fields.</returns>
        public virtual IEnumerable<FieldInfo> GetFieldsDocumentation()
        {
            var pluginType = GetType();

            // Search in the same assembly + namespace for classes deriving from FieldExtractorBase
            var fieldsTypes = pluginType
                .Assembly
                .GetTypes()
                .Where(t =>
                    t.Namespace == pluginType.Namespace &&
                    typeof(FieldExtractorBase).IsAssignableFrom(t) &&
                    !t.IsAbstract);

            if (!fieldsTypes.Any())
            {
                return Enumerable.Empty<FieldInfo>();
            }

            List<FieldInfo> result = new();
            foreach (var fieldsType in fieldsTypes)
            {
                var method = typeof(FieldExtractorBase)
                .GetMethod(nameof(FieldExtractorBase.ExtractFieldsInfos))
                ?.MakeGenericMethod(fieldsType);

                if (method is not null)
                {
                    var fields = (IEnumerable<FieldInfo>)method.Invoke(null, null)!;
                    result.AddRange(fields);
                }
            }

            return result;
        }
    }
}
