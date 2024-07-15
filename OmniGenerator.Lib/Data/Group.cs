namespace SeedGenerator.Lib.Data
{
    /// <summary>
    /// Modelize a <see cref="Group"/> with its inner <see cref="Document"/> or <see cref="Group"/> as <see cref="Element"/>.
    /// </summary>
    public class Group : Element
    {
        private IList<Element> _elements = new List<Element>();

        /// <summary>
        /// Creates a new <see cref="Group"/>.
        /// </summary>
        /// <param name="id">^The id of the group.</param>
        /// <param name="name">The name of the group. Can be seen as a group type.</param>
        public Group(long id, string name)
            : base("group", name, id)
        {

        }

        /// <summary>
        /// Adds the provided element to the children of the group.
        /// </summary>
        /// <param name="element">The child element.</param>
        public void Add(Element element)
        {
            element.Parent = this;
            _elements.Add(element);
        }

        /// <summary>
        /// Adds the provided elements to the children of the group.
        /// </summary>
        /// <param name="element">The list of child element.</param>
        public void AddRange(IEnumerable<Element> elements)
        {
            foreach (var element in elements)
            {
                Add(element);
            }
        }

        /// <summary>
        /// Gets the child elements of this group.
        /// The search can be optionally filtered by the name of the element
        /// If the recursive flag is set to true, the result will be a flattened list of all the matching elements in the hierarchy
        /// </summary>
        /// <param name="name">Optionnal. If set, the search will return child elements by name</param>
        /// <param name="recursive">Indicates if the search is limited to the direct child or must scope to the sub-groups</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets all the child groups
        /// </summary>
        /// <param name="recursive">Indicates if the search is limited to the direct child or must scope to the sub-groups</param>
        /// <returns>The list of groups</returns>
        public IEnumerable<Group> GetGroups(bool recursive = false)
        {
            return GetElements(null, recursive)
                .Where(x => x is Group)
                .Cast<Group>();
        }

        /// <summary>
        /// Gets all the child groups whose name match the provided name
        /// </summary>
        /// <param name="recursive">Indicates if the search is limited to the direct child or must scope to the sub-groups</param>
        /// <returns>The list of groups</returns>
        public IEnumerable<Group> GetGroups(string name, bool recursive = false)
        {
            return GetElements(name, recursive)
                .Where(x => x is Group)
                .Cast<Group>();
        }

        /// <summary>
        /// Gets all the child documents
        /// </summary>
        /// <param name="recursive">Indicates if the search is limited to the direct child or must scope to the sub-groups</param>
        /// <returns>The list of groups</returns>
        public IEnumerable<Document> GetDocuments(bool recursive = false)
        {
            return GetElements(null, recursive)
                .Where(x => x is Document)
                .Cast<Document>();
        }

        /// <summary>
        /// Gets all the child documents whose name match the provided name
        /// </summary>
        /// <param name="recursive">Indicates if the search is limited to the direct child or must scope to the sub-groups</param>
        /// <returns>The list of groups</returns>
        public IEnumerable<Document> GetDocuments(string name, bool recursive = false)
        {
            return GetElements(name, recursive)
                .Where(x => x is Document)
                .Cast<Document>();
        }
    }
}
