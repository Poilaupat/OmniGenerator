using System.Collections;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

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
        private readonly Dictionary<string, Field> _fields = new();

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
            if (!_fields.ContainsKey(field.Name))
            {
                _fields.Add(field.Name, field);
            }
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
        public Dictionary<string, Field> ToDictionary() => new Dictionary<string, Field>(_fields);

        /// <summary>
        /// Implicitly converts a <see cref="FieldCollection"/> to a <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="collection">The collection to convert.</param>
        /// <returns>A new dictionary containing a copy of the field data.</returns>
        public static implicit operator Dictionary<string, Field>(FieldCollection collection) => collection.ToDictionary();

        /// <summary>
        /// Creates an <see cref="ExpandoObject"/> with properties corresponding to field names and values.
        /// </summary>
        /// <returns>A dynamic object exposing each field name as a property with its raw <see cref="Field.Value"/>.</returns>
        /// <remarks>
        /// Useful for serialization scenarios or dynamic binding where field names are not known at compile time.
        /// </remarks>
        /// <example>
        /// <code>
        /// dynamic expando = fieldCollection.ToExpando();
        /// Console.WriteLine(expando.AccountNumber);
        /// </code>
        /// </example>
        public dynamic ToExpando()
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var kv in _fields)
            {
                expando.Add(kv.Key, kv.Value.Value);
            }
            return expando;
        }
    }
}
