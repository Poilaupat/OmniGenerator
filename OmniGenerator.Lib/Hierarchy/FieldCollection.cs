using OmniGenerator.Lib.Exceptions;
using System.Collections;
using System.Diagnostics;
using System.Dynamic;

namespace OmniGenerator.Lib.Hierarchy
{
    /// <summary>
    /// Represents a read-only collection of <see cref="Field"/> instances keyed by field name.
    /// Provides controlled access to generated field data while preventing external mutation.
    /// </summary>
    /// <remarks>
    /// This collection wraps an internal dictionary and exposes only read operations publicly.
    /// Internal methods (<see cref="Add(Field)"/>, <see cref="AddRange(FieldCollection)"/>, etc.)
    /// allow the library to populate instances during hierarchy generation.
    /// </remarks>
    [DebuggerDisplay("Count={Count}")]
    public class FieldCollection : IReadOnlyDictionary<string, Field>
    {
        /// <summary>
        /// Internal storage for fields keyed by their name.
        /// </summary>
        private readonly Dictionary<string, Field> _fields = [];

        /// <summary>
        /// Gets the number of fields in the collection.
        /// </summary>
        public int Count => _fields.Count;

        /// <summary>
        /// Gets the field names in the collection.
        /// </summary>
        public IEnumerable<string> Keys => _fields.Keys;

        /// <summary>
        /// Gets the field values in the collection.
        /// </summary>
        public IEnumerable<Field> Values => _fields.Values;

        /// <summary>
        /// Gets the <see cref="Field"/> with the specified name.
        /// </summary>
        /// <param name="key">The field name.</param>
        /// <returns>The <see cref="Field"/> instance.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the field name does not exist.</exception>
        public Field this[string key] => _fields[key];

        /// <summary>
        /// Determines whether the collection contains a field with the specified name.
        /// </summary>
        /// <param name="key">The field name to locate.</param>
        /// <returns><c>true</c> if the field exists; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(string key) => _fields.ContainsKey(key);

        /// <summary>
        /// Attempts to retrieve a field by name.
        /// </summary>
        /// <param name="key">The field name to locate.</param>
        /// <param name="value">When this method returns, contains the field if found; otherwise, the default value.</param>
        /// <returns><c>true</c> if the field was found; otherwise, <c>false</c>.</returns>
        public bool TryGetValue(string key, out Field value) => _fields.TryGetValue(key, out value);

        /// <summary>
        /// Attempts to retrieve the string value of a field by name.
        /// </summary>
        /// <param name="key">The field name to locate.</param>
        /// <param name="stringValue">When this method returns, contains the string value of the field if found; otherwise, <c>null</c>.</param>
        /// <returns><c>true</c> if the field was found; otherwise, <c>false</c>.</returns>
        public bool TryGetStringValue(string key, out string? stringValue)
        {
            if (_fields.TryGetValue(key, out var field))
            {
                stringValue = field.StringValue;
                return true;
            }
            stringValue = null;
            return false;
        }

        /// <summary>
        /// Gets the <see cref="Field"/> with the specified name.
        /// </summary>
        /// <param name="key">The field name to locate.</param>
        /// <returns>The <see cref="Field"/> instance.</returns>
        /// <exception cref="FieldNotFoundException">Thrown when the field name does not exist in the collection.</exception>
        public Field GetValue(string key)
        {
            if (!_fields.TryGetValue(key, out Field value))
                throw new FieldNotFoundException($"The field '{key}' was not found in the collection.");

            return value;
        }

        /// <summary>
        /// Gets the string value of a field by name.
        /// </summary>
        /// <param name="key">The field name to locate.</param>
        /// <returns>The string value of the field.</returns>
        /// <exception cref="FieldNotFoundException">Thrown when the field name does not exist in the collection.</exception>
        public string GetStringValue(string key)
        {
            var value = GetValue(key);

            return value.StringValue;
        }

        /// <summary>
        /// Gets the string value of a field by name, or returns a default value if the field is not found or has a null value.
        /// </summary>
        /// <param name="key">The field name to locate.</param>
        /// <param name="defaultValue">The default value to return if the field is not found or has a null value.</param>
        /// <returns>The string value of the field if found and non-null; otherwise, the specified <paramref name="defaultValue"/>.</returns>
        public string GetStringValueOrDefault(string key, string defaultValue)
        {
            if (_fields.TryGetValue(key, out var field))
            {
                return field.StringValue ?? defaultValue;
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the field collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public IEnumerator<KeyValuePair<string, Field>> GetEnumerator() => _fields.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => _fields.GetEnumerator();

        /// <summary>
        /// Adds a field to the collection if a field with the same name does not already exist.
        /// </summary>
        /// <param name="field">The field to add.</param>
        /// <remarks>
        /// If a field with the same name already exists, it is preserved and the new field is ignored.
        /// </remarks>
        internal void Add(Field field)
        {
            _fields.TryAdd(field.Name, field);
        }

        /// <summary>
        /// Adds all fields from another <see cref="FieldCollection"/>, preserving existing entries.
        /// </summary>
        /// <param name="other">The source field collection.</param>
        internal void AddRange(FieldCollection other)
        {
            foreach (var f in other.Values)
                Add(f);
        }

        /// <summary>
        /// Adds all fields from an enumerable sequence, preserving existing entries.
        /// </summary>
        /// <param name="other">The source sequence of fields.</param>
        internal void AddRange(IEnumerable<Field> other)
        {
            foreach (var f in other)
                Add(f);
        }

        /// <summary>
        /// Adds all fields from a dictionary, preserving existing entries.
        /// </summary>
        /// <param name="other">The source dictionary of fields keyed by field name.</param>
        internal void AddRange(IDictionary<string, Field> other)
        {
            foreach (var kv in other)
                Add(kv.Value);
        }

        /// <summary>
        /// Creates a new <see cref="Dictionary{TKey, TValue}"/> containing a copy of all fields.
        /// </summary>
        /// <returns>A new dictionary with the same field data.</returns>
        /// <remarks>
        /// The returned dictionary is independent; modifications to it do not affect this collection.
        /// </remarks>
        public Dictionary<string, Field> ToDictionary() => new(_fields);

        /// <summary>
        /// Converts the field collection to a dynamic object (ExpandoObject).
        /// Each field's name becomes a property on the dynamic object, with its string value as the property value.
        /// </summary>
        /// <returns>A dynamic <see cref="ExpandoObject"/> containing all fields as properties.</returns>
        /// <remarks>
        /// This is useful for scenarios requiring dynamic property access, such as templating or scripting.
        /// </remarks>
        public dynamic ToDynamic()
        {
            var expando = new ExpandoObject() as IDictionary<string, object>;
            foreach (var field in _fields)
            {
                expando.Add(field.Key, field.Value.StringValue);
            }
            return expando;
        }

        /// <summary>
        /// Implicitly converts a <see cref="FieldCollection"/> to a <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="collection">The collection to convert.</param>
        /// <returns>A new dictionary containing a copy of the field data.</returns>
        public static implicit operator Dictionary<string, Field>(FieldCollection collection) => collection.ToDictionary();
    }
}
