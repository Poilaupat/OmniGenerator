using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>
    /// Describes a field used by a plugin.
    /// </summary>
    public sealed class FieldInfo
    {
        /// <summary>
        /// Gets the name of the field.
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
        public string? DefaultValue { get; }

        /// <summary>
        /// Gets the name of the entity that owns the field (for packagers only).
        /// Null for renderers.
        /// </summary>
        public string? EntityName { get; }

        /// <summary>
        /// Gets the type of the entity that owns the field (for packagers only).
        /// Null for renderers.
        /// </summary>
        public EPluginFieldEntityType? EntityType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldInfo"/> class.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="description">The description of the field.</param>
        /// <param name="isRequired">Whether the field is required.</param>
        /// <param name="defaultValue">The default value for the field.</param>
        /// <param name="entityName">The name of the entity that owns the field.</param>
        /// <param name="entityType">The type of the entity that owns the field.</param>
        public FieldInfo(
            string fieldName,
            string description,
            bool isRequired,
            string? defaultValue = null,
            string? entityName = null,
            EPluginFieldEntityType? entityType = null)
        {
            FieldName = fieldName;
            Description = description;
            IsRequired = isRequired;
            DefaultValue = defaultValue;
            EntityName = entityName;
            EntityType = entityType;
        }
    }

    /// <summary>
    /// Represents the type of entity that owns a field in a packager plugin.
    /// </summary>
    public enum EPluginFieldEntityType
    {
        /// <summary>
        /// The field belongs to the root entity.
        /// </summary>
        Root,

        /// <summary>
        /// The field belongs to a document entity.
        /// </summary>
        Document,

        /// <summary>
        /// The field belongs to a group entity.
        /// </summary>
        Group
    }
}
