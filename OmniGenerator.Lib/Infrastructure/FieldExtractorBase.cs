using OmniGenerator.Lib.Hierarchy;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>
    /// Base class for strongly-typed plugin field accessors.
    /// Derived classes define properties decorated with <see cref="FieldInfoAttribute"/> to provide typed access to fields.
    /// </summary>
    /// <remarks>
    /// Every read is routed through a <see cref="FieldChannel"/> so error simulation can make the
    /// image and the data package diverge transparently. Renderer-side extractors pass
    /// <see cref="FieldChannel.Image"/>; packager-side extractors keep the default
    /// <see cref="FieldChannel.Data"/>. Plugins never need to know a simulation happened.
    /// </remarks>
    public abstract class FieldExtractorBase
    {
        /// <summary>
        /// The underlying field collection from which values are read.
        /// </summary>
        protected readonly FieldCollection _fields;

        /// <summary>
        /// The channel used to read field values. Defaults to <see cref="FieldChannel.Data"/>.
        /// </summary>
        protected readonly FieldChannel _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldExtractorBase"/> class.
        /// </summary>
        /// <param name="fields">The field collection to wrap.</param>
        /// <param name="channel">The output channel used to read field values. Defaults to <see cref="FieldChannel.Data"/>.</param>
        protected FieldExtractorBase(FieldCollection fields, FieldChannel channel = FieldChannel.Data)
        {
            _fields = fields ?? throw new ArgumentNullException(nameof(fields));
            _channel = channel;
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
        /// Gets a required string value from the underlying field collection, read from the active channel.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The string value of the field.</returns>
        /// <exception cref="Exceptions.FieldNotFoundException">Thrown when the field is not found.</exception>
        protected string GetRequiredString(string fieldName)
        {
            return _fields.GetStringValue(fieldName, _channel);
        }

        /// <summary>
        /// Gets an optional string value from the underlying field collection, read from the active channel.
        /// Returns null if the field is not found.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The string value of the field, or null if the field is not found.</returns>
        protected string? GetOptionalString(string fieldName)
        {
            _ = _fields.TryGetStringValue(fieldName, out var value, _channel);
            return value;
        }

        /// <summary>
        /// Gets an optional string value from the underlying field collection with a default value, read from the active channel.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="defaultValue">The default value to return if the field is not found or the channel value is null.</param>
        /// <returns>The string value of the field or the default value.</returns>
        protected string GetOptionalStringOrDefault(string fieldName, string defaultValue)
        {
            return _fields.GetStringValueOrDefault(fieldName, defaultValue, _channel);
        }

        /// <summary>
        /// Gets a required value from the underlying field collection.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The field object.</returns>
        /// <exception cref="Exceptions.FieldNotFoundException">Thrown when the field is not found.</exception>
        protected Field GetRequiredField(string fieldName)
        {
            return _fields.GetValue(fieldName);
        }

        /// <summary>
        /// Tries to get a value from the underlying field collection.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The field object if found; otherwise, null.</returns>
        protected Field? GetOptionalField(string fieldName)
        {
            return _fields.TryGetValue(fieldName, out var field) ? field : null;
        }

        /// <summary>
        /// Gets the typed value carried by the active channel for a required field.
        /// Prefer this over <see cref="GetRequiredField"/> when casting a value (for example to <c>int</c> or <c>DateTime</c>),
        /// so that channel-specific error simulations are honored.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The channel value of the field.</returns>
        /// <exception cref="Exceptions.FieldNotFoundException">Thrown when the field is not found.</exception>
        protected FieldValue GetRequiredValue(string fieldName)
        {
            return _fields.GetValue(fieldName).GetValue(_channel);
        }

        /// <summary>
        /// Tries to get the typed value carried by the active channel for an optional field.
        /// Prefer this over <see cref="GetOptionalField"/> when casting a value (for example to <c>int</c> or <c>DateTime</c>),
        /// so that channel-specific error simulations are honored.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The channel value of the field, or null if the field is not found.</returns>
        protected FieldValue? GetOptionalValue(string fieldName)
        {
            return _fields.TryGetValue(fieldName, out var field) ? field.GetValue(_channel) : (FieldValue?)null;
        }
    }
}
