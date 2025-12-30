namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>
    /// Attribute used to specify the entity type for a field (for packagers only).
    /// Must be used in conjunction with <see cref="PluginFieldAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class PluginFieldEntityAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the entity that owns the field.
        /// </summary>
        public EPluginFieldEntityType EntityType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginFieldEntityAttribute"/> class.
        /// </summary>
        /// <param name="entityType">The type of the entity that owns the field.</param>
        public PluginFieldEntityAttribute(EPluginFieldEntityType entityType)
        {
            EntityType = entityType;
        }
    }
}
