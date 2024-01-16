namespace SeedGenerator.Lib.Data
{
    public class Group : Element
    {
        private IList<Element> _elements = new List<Element>();

        public Group(long id, string name)
            : base("group", name, id)
        {

        }

        public void Add(Element element)
        {
            element.Parent = this;
            _elements.Add(element);
        }

        public void AddRange(IEnumerable<Element> elements)
        {
            foreach (var element in elements)
            {
                Add(element);
            }
        }

        public IEnumerable<Element> GetElements(string? name, bool recursive = false)
        {
            foreach(var element in _elements)
            {
                if(name is null || element.Name.Equals(name))
                {
                    yield return element;
                }

                if(recursive && element is Group group)
                {
                    foreach(var subElement in group.GetElements(name, recursive))
                    {
                        yield return subElement;
                    }
                }
            }
        }

        public IEnumerable<Group> GetGroups(bool recursive = false)
        {
            return GetElements(null, recursive)
                .Where(x => x is Group)
                .Cast<Group>();
        }

        public IEnumerable<Group> GetGroups(string name, bool recursive = false)
        {
            return GetElements(name, recursive)
                .Where(x => x is Group)
                .Cast<Group>();
        }

        public IEnumerable<Document> GetDocuments(bool recursive = false)
        {
            return GetElements(null, recursive)
                .Where(x => x is Document)
                .Cast<Document>();
        }

        public IEnumerable<Document> GetDocuments(string name, bool recursive = false)
        {
            return GetElements(name, recursive)
                .Where(x => x is Document)
                .Cast<Document>();
        }
    }
}
