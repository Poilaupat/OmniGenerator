namespace SeedGenerator.Lib.Data
{
    public class FieldCollection
    {
        private Dictionary<string, Field> _fields { get; } = new Dictionary<string, Field>();

        public void Add(string key, Field field)
        {
            if (_fields.ContainsKey(key))
                _fields[key] = field;

            _fields.Add(key, field);
        }

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

        public Field this[string key] => _fields[key.Replace("-", "_")];

        public override string ToString()
        {
            return $"Field count = {_fields.Count}";
        }
    }
}
