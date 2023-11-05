namespace SeedGenerator.Lib.Data
{
    public class FieldCollection
    {
        private Dictionary<string, Field> _items = new Dictionary<string, Field>();

        public void Add(string key, Field item)
        {
            if (_items.ContainsKey(key))
                throw new Exception($"A field with the same key already exists ({key})");

            _items.Add(key, item);
        }

        public Field this[string key] => _items[key];
    }
}
