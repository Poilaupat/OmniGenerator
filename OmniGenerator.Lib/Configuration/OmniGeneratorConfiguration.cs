using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Configuration
{
    /// <summary>
    /// Represents the root configuration for the OmniGenerator system.
    /// Contains settings for the packager, rendering resolution, and document hierarchy.
    /// </summary>
    public class OmniGeneratorConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the packager to use for output.
        /// This value is required and should match the identifier of a registered <c>IPackager</c> implementation.
        /// </summary>
        [JsonPropertyName("packager")]
        public required string PackagerName { get; set; }

        /// <summary>
        /// Gets or sets the rendering resolution in DPI (dots per inch) for image generation.
        /// Defaults to 240 DPI if not specified.
        /// </summary>
        [JsonPropertyName("render-resolution")]
        public int RenderResolutionDPI { get; set; } = 240;
 
        /// <summary>
        /// Gets or sets the hierarchy configuration, which defines the structure of the documents to be generated.
        /// This value is required and must be provided in the configuration.
        /// </summary>
        [JsonPropertyName("hierarchy")]
        public required HierarchyConfiguration Hierarchy { get; set; }
    }
}
