using OmniGenerator.Lib.Hierarchy;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>
    /// Base class for strongly-typed plugin field accessors.
    /// Derived classes define properties decorated with <see cref="FieldInfoAttribute"/> to provide typed access to fields.
    /// </summary>
    public abstract class FieldExtractorBase
    {
        /// <summary>
        /// The underlying field collection from which values are read.
        /// </summary>
        protected readonly FieldCollection _fields;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldExtractorBase"/> class.
        /// </summary>
        /// <param name="fields">The field collection to wrap.</param>
        protected FieldExtractorBase(FieldCollection fields)
        {
            _fields = fields;
        }

        /// <summary>
        /// Extracts field documentation from all properties decorated with <see cref="FieldInfoAttribute"/>.
        /// </summary>
        /// <typeparam name="TFields">The concrete fields class type.</typeparam>
        /// <returns>An enumerable collection of <see cref="FieldInfo"/> describing the fields.</returns>
        public static IEnumerable<FieldInfo> ExtractFieldsInfos<TFields>()
            where TFields : FieldExtractorBase
        {
            // Check for parent entity attribute
            var entityAttribute = typeof(TFields).GetCustomAttribute<FieldEntityAttribute>();

            var properties = typeof(TFields).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<FieldInfoAttribute>();
                if (attribute != null)
                {
                    yield return new FieldInfo(
                        attribute.FieldName,
                        attribute.Description,
                        attribute.IsRequired,
                        attribute.DefaultValue,
                        entityAttribute?.EntityName,
                        entityAttribute?.EntityType
                    );
                }
            }
        }

        /// <summary>
        /// Gets a required string value from the underlying field collection.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The string value of the field.</returns>
        /// <exception cref="Exceptions.FieldNotFoundException">Thrown when the field is not found.</exception>
        protected string GetRequiredString(string fieldName)
        {
            return _fields.GetStringValue(fieldName);
        }

        /// <summary>
        /// Gets an optional string value from the underlying field collection with a default value.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="defaultValue">The default value to return if the field is not found.</param>
        /// <returns>The string value of the field or the default value.</returns>
        protected string GetOptionalString(string fieldName, string defaultValue)
        {
            return _fields.GetStringValueOrDefault(fieldName, defaultValue);
        }

        /// <summary>
        /// Gets a required value from the underlying field collection.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The field object.</returns>
        /// <exception cref="Exceptions.FieldNotFoundException">Thrown when the field is not found.</exception>
        protected Field GetRequired(string fieldName)
        {
            return _fields.GetValue(fieldName);
        }

        /// <summary>
        /// Tries to get a value from the underlying field collection.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="field">The field object if found.</param>
        /// <returns>True if the field was found; otherwise, false.</returns>
        protected bool TryGet(string fieldName, out Field field)
        {
            return _fields.TryGetValue(fieldName, out field);
        }
    }
}
