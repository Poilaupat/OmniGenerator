namespace SeedGenerator.Lib.Data.Fields
{
    internal class FieldCollection
    {
        private Dictionary<string, Field> _items = new Dictionary<string, Field>();

        public void Add(string key, Field item)
        {
            _items.Add(key, item);
        }

        public Field this[string key] => _items[key];
    }
}
