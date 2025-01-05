namespace OmniGenerator.Lib.Generators
{
    /// <summary>
    /// An indexed collection of fields
    /// </summary>
    public class FieldCollection
    {
        private Dictionary<string, Field> _fields { get; } = new Dictionary<string, Field>();
        
        /// <summary>
        /// Returns all the field names of this collection
        /// </summary>
        public IEnumerable<string> FieldNames => _fields.Keys;

        /// <summary>
        /// Returns the number of fields of this collection
        /// </summary>
        public long FieldCount => _fields.Count;

        /// <summary>
        /// Adds a field to the collection.
        /// If the entry with the same key already exists, the field is updated
        /// </summary>
        /// <param name="key">The field name as key</param>
        /// <param name="field">The field value</param>
        public void Add(string key, Field field)
        {
            if (_fields.ContainsKey(key))
                _fields[key] = field;
            else
                _fields.Add(key, field);
        }

        /// <summary>
        /// Adds a field to the collection.
        /// If the entry with the same key already exists, the field is updated 
        /// </summary>
        /// <param name="key">The field nama as key</param>
        /// <param name="value">The field value</param>
        public void Add(string key, object value)
        {
            var field = new Field(key, value);
            _fields.Add(key, field);
        }

        /// <summary>
        /// Merge a collection into the current collection
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(FieldCollection collection)
        {
            if (collection is not null)
            {
                foreach (var field in collection._fields)
                {
                    Add(field.Key, field.Value);
                }
            }
        }

        /// <summary>
        /// Access a field of the collection by its key
        /// </summary>
        /// <param name="key">The searched key</param>
        /// <returns>The field if the key exists. Throws exception if the key is not found</returns>
        public Field this[string key] => _fields[key.Replace("-", "_")];

        public override string ToString()
        {
            return $"Field count = {_fields.Count}";
        }

    }
}
