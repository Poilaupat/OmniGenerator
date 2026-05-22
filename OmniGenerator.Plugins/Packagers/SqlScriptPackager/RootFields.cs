using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Packagers.SqlScriptPackager
{
    /// <summary>
    /// Strongly-typed field accessor for <see cref="SqlScriptPackager"/>.
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
        /// Gets whether to truncate tables before inserting.
        /// </summary>
        [FieldInfo("truncate-before-insert", "Whether to truncate tables before inserting", isRequired: true)]
        public Field TruncateBeforeInsert => GetRequiredField("truncate-before-insert");
    }
}
