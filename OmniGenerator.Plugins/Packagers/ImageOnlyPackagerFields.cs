using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Packagers
{
    /// <summary>
    /// Strongly-typed field accessor for <see cref="ImageOnlyPackager"/>.
    /// Provides type-safe access to root-level fields.
    /// </summary>
    public class ImageOnlyPackagerFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageOnlyPackagerFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the root.</param>
        public ImageOnlyPackagerFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the lot number used for directory naming.
        /// </summary>
        [FieldInfo("numlot", "Lot number used for directory naming", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Root)]
        public Field Numlot => GetRequired("numlot");
    }
}
