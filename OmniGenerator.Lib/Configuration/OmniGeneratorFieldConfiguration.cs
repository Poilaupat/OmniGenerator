using OmniGenerator.Lib.Configuration.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Configuration
{
    /// <summary>
    /// Represents a configuration container for field definitions that can be loaded from external files.
    /// This class is used to deserialize standalone field configuration files (e.g., fields-*.json)
    /// that contain only an array of field definitions without the full hierarchy structure.
    /// </summary>
    /// <remarks>
    /// This configuration type is typically used for satellite field files referenced by
    /// <see cref="ElementConfiguration.FieldConfigurationFile"/> or
    /// <see cref="HierarchyConfiguration.FieldConfigurationFile"/> properties.
    /// The fields defined in these external files are merged into the parent element's field collection
    /// during configuration loading by <see cref="ConfigurationReader"/>
    /// </remarks>
    public class OmniGeneratorFieldConfiguration
    {
        /// <summary>
        /// Gets or sets the collection of field configurations.
        /// </summary>
        /// <value>
        /// A list of field configuration objects that define how data should be generated for each field.
        /// Each field configuration is a subclass of <see cref="AbstractFieldConfigurationBase"/>.
        /// </value>
        [JsonPropertyName("fields")]
        public List<AbstractFieldConfigurationBase> Fields { get; set; } = new List<AbstractFieldConfigurationBase>();
    }
}
