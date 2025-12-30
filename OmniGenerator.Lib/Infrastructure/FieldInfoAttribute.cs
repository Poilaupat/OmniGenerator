namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>
    /// Attribute used to document a field used by a plugin.
    /// When applied to a property in a plugin fields class, it provides metadata about the field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class FieldInfoAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the field as it appears in the data source.
        /// If not specified, the property name will be used (converted to kebab-case).
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Gets the description of the field.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets a value indicating whether the field is required.
        /// </summary>
        public bool IsRequired { get; }

        /// <summary>
        /// Gets the default value for the field, if any.
        /// </summary>
        public string? DefaultValue { get; init; }

        /// <summary>
        /// Gets the name of the entity that owns the field (for packagers only).
        /// Null for renderers.
        /// </summary>
        public string? EntityName { get; init; }

        /// <summary>
        /// Gets the type of the entity that owns the field (for packagers only).
        /// This property is set via reflection and should not be set directly in the attribute.
        /// </summary>
        public EPluginFieldEntityType? EntityType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldInfoAttribute"/> class with automatic field name derivation.
        /// </summary>
        /// <param name="description">The description of the field.</param>
        /// <param name="isRequired">Whether the field is required.</param>
        public FieldInfoAttribute(string description, bool isRequired = true)
        {
            Description = description;
            IsRequired = isRequired;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldInfoAttribute"/> class with an explicit field name.
        /// </summary>
        /// <param name="fieldName">The name of the field as it appears in the data source.</param>
        /// <param name="description">The description of the field.</param>
        /// <param name="isRequired">Whether the field is required.</param>
        public FieldInfoAttribute(string fieldName, string description, bool isRequired = true)
        {
            FieldName = fieldName;
            Description = description;
            IsRequired = isRequired;
        }
    }
}
