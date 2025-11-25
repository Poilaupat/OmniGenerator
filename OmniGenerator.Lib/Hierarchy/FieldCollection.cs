using System.Collections;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

namespace OmniGenerator.Lib.Hierarchy
{
    [DebuggerDisplay("Count={Count}")]
    public class FieldCollection : IReadOnlyDictionary<string, Field>
    {
        private readonly Dictionary<string, Field> _fields = new();
        public int Count => _fields.Count;
        public IEnumerable<string> Keys => _fields.Keys;
        public IEnumerable<Field> Values => _fields.Values;
        public Field this[string key] => _fields[key];
        public bool ContainsKey(string key) => _fields.ContainsKey(key);
        public bool TryGetValue(string key, out Field value) => _fields.TryGetValue(key, out value);
        public IEnumerator<KeyValuePair<string, Field>> GetEnumerator() => _fields.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _fields.GetEnumerator();

        internal void Add(Field field)
        {
            if (!_fields.ContainsKey(field.Name))
            {
                _fields.Add(field.Name, field);
            }
        }
        internal void AddRange(FieldCollection other)
        {
            foreach (var f in other.Values)
                Add(f);
        }
        internal void AddRange(IEnumerable<Field> other)
        {
            foreach (var f in other)
                Add(f);
        }
        internal void AddRange(IDictionary<string, Field> other)
        {
            foreach (var kv in other)
                Add(kv.Value);
        }

        /// <summary>
        /// Returns a new dictionary copy of the fields.
        /// </summary>
        public Dictionary<string, Field> ToDictionary() => new Dictionary<string, Field>(_fields);

        /// <summary>
        /// Implicit conversion to a Dictionary for interoperability with existing APIs expecting IDictionary.
        /// </summary>
        public static implicit operator Dictionary<string, Field>(FieldCollection collection) => collection.ToDictionary();

        /// <summary>
        /// Builds an ExpandoObject exposing field names and their raw values.
        /// </summary>
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
