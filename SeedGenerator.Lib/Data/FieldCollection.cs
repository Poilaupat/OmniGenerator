namespace SeedGenerator.Lib.Data
{
    public class FieldCollection
    {
        private Dictionary<string, Field> _items { get; } = new Dictionary<string, Field>();

        public void Add(string key, Field item)
        {
            if (_items.ContainsKey(key))
                _items[key] = item;

            _items.Add(key, item);
        }

        public void AddRange(FieldCollection collection)
        {
            if (collection is not null)
            {
                foreach (var item in collection._items)
                {
                    Add(item.Key, item.Value);
                }
            }
        }

        public Field this[string key] => _items[key.Replace("-", "_")];
    }
}
