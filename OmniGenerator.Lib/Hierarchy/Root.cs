namespace OmniGenerator.Lib.Hierarchy
{
    /// <summary>
    /// The root of the <see cref="Element"/> generating hierarchy.
    /// The root contains all the data needed to create the files
    /// </summary>
    public class Root
    {
        /// <summary>
        /// The top level fields. 
        /// Those fields should only contain top level information in the hierarchy such as batch number or capture date.
        /// </summary>
        public IDictionary<string, Field> Fields { get; set; } = new Dictionary<string, Field>();

        /// <summary>
        /// The top level groups
        /// </summary>
        public IEnumerable<Group> Groups { get; set; } = new List<Group>();

        /// <summary>
        /// Creates a new <see cref="Root"/>
        /// </summary>
        /// <param name="groups">The root top level groups</param>
        public Root(IEnumerable<Group> groups)
        {
            Groups = groups;
        }

        /// <summary>
        /// Gets the <see cref="Document"/> of the root
        /// </summary>
        /// <returns>The list of <see cref="Document"/></returns>
        public IEnumerable<Document> GetDocuments()
        {
            return Groups
                .SelectMany(x => x.GetDocuments(null, true));
        }

        /// <summary>
        /// Gets the <see cref="Group"/> of the root
        /// </summary>
        /// <param name="recursive"></param>
        /// <returns>The list of <see cref="Group"/></returns>
        public IEnumerable<Group> GetGroups()
        {
            return Groups
                .Union(Groups
                    .SelectMany(g => g.GetGroups(null, true)));
        }
    }
}
