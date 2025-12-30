using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Packagers.ImageOnlyPackager
{
    /// <summary>
    /// Strongly-typed field accessor for <see cref="ImageOnlyPackager"/>.
    /// Provides type-safe access to root-level fields.
    /// </summary>
    [FieldEntity(EPluginFieldEntityType.Root)]
    public class RootFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the root.</param>
        public RootFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets the lot number used for directory naming.
        /// </summary>
        [FieldInfo("numlot", "Lot number used for directory naming", isRequired: true)]
        public Field Numlot => GetRequired("numlot");
    }
}
