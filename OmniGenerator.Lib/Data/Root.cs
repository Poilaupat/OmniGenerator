namespace SeedGenerator.Lib.Data
{
    /// <summary>
    /// The root of the <see cref="Element"/> generating hierarchy.
    /// The root contains all the data needed to create the seed files
    /// </summary>
    public class Root
    {
        /// <summary>
        /// The top level fields. 
        /// Those fields should only contain information on the very seed such as batch number or capture date.
        /// </summary>
        public FieldCollection Fields { get; set; } = new FieldCollection();

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
        /// <param name="recursive">Indicates if teh search is recursive</param>
        /// <returns>The list of <see cref="Document"/></returns>
        public IEnumerable<Document> GetDocuments(bool recursive = false)
        {
            foreach (var document in Groups.SelectMany(x => x.GetDocuments(true)))
            {
                yield return document;
            }
        }
    }
}
