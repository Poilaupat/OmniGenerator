namespace SeedGenerator.Lib.MetaData
{
    public class MetaDataCollection
    {
        private Dictionary<string, MetaDataItem> _items = new Dictionary<string, MetaDataItem>();

        public void Add(string key, MetaDataItem item)
        {
            _items.Add(key, item);
        }

        public MetaDataItem this[string key] => _items[key];
    }
}
