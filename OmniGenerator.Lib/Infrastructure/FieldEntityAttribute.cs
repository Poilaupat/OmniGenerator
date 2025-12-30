namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>
    /// Attribute used to specify the entity type and name for a fields class.
    /// Applied at the class level to indicate which entity the fields belong to.
    /// Used for packagers to distinguish between Root, Document, and Group entities.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class FieldEntityAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the entity that owns the field.
        /// </summary>
        public EPluginFieldEntityType EntityType { get; }

        /// <summary>
        /// Gets the name of the entity that owns the field. 
        /// Null for Root entities, required for Document and Group entities.
        /// </summary>
        public string? EntityName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldEntityAttribute"/> class.
        /// </summary>
        /// <param name="entityType">The type of the entity that owns the field.</param>
        /// <param name="entityName">The name of the entity that owns the field. Should be null for Root, required for Document and Group.</param>
        public FieldEntityAttribute(EPluginFieldEntityType entityType, string? entityName = null)
        {
            EntityType = entityType;
            EntityName = entityName;
        }
    }
}
