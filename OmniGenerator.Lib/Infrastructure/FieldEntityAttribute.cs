namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>
    /// Attribute used to specify the entity type for a field (for packagers only).
    /// Must be used in conjunction with <see cref="FieldInfoAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class FieldEntityAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the entity that owns the field.
        /// </summary>
        public EPluginFieldEntityType EntityType { get; }

        /// <summary>
        /// Gets the name of the entity that owns the field.
        /// </summary>
        public string? EntityName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldEntityAttribute"/> class.
        /// </summary>
        /// <param name="entityType">The type of the entity that owns the field.</param>
        /// <param name="entityName">The name of the entity that owns the field.</param>
        public FieldEntityAttribute(EPluginFieldEntityType entityType, string? entityName = null)
        {
            EntityType = entityType;
            EntityName = entityName;
        }
    }
}
