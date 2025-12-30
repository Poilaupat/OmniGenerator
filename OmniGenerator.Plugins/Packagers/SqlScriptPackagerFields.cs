using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Packagers
{
    /// <summary>
    /// Strongly-typed field accessor for <see cref="SqlScriptPackager"/>.
    /// Provides type-safe access to root-level fields.
    /// </summary>
    public class SqlScriptPackagerFields : FieldExtractorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlScriptPackagerFields"/> class.
        /// </summary>
        /// <param name="fields">The field collection from the root.</param>
        public SqlScriptPackagerFields(FieldCollection fields) : base(fields)
        {
        }

        /// <summary>
        /// Gets whether to truncate tables before inserting.
        /// </summary>
        [FieldInfo("truncate-before-insert", "Whether to truncate tables before inserting", isRequired: true)]
        [FieldEntity(EPluginFieldEntityType.Root)]
        public Field TruncateBeforeInsert => GetRequired("truncate-before-insert");
    }
}
