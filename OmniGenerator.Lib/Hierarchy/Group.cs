using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Generators;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace OmniGenerator.Lib.Hierarchy
{
    /// <summary>
    /// Modelize a <see cref="Group"/> with its inner <see cref="Document"/> or <see cref="Group"/> as <see cref="Element"/>.
    /// </summary>
    [DebuggerDisplay("Group = {Name}")]
    public class Group : Element
    {
        private Group[] _groups;
        private Document[] _documents;

        /// <summary>
        /// Creates a new <see cref="Group"/>.
        /// </summary>
        /// <param name="name">The name of the group. Can be seen as a group type.</param>
        /// <param name="groups">Inner groups of this grou</param>
        /// <param name="documents">Inner documents of this grou</param>
        public Group(string name, Group[] groups, Document[] documents)
            : base("group", name)
        {
            _groups = groups ?? new Group[0];
            _documents = documents ?? new Document[0];

            foreach (var group in _groups)
            {
                group.Parent = this;
            }

            foreach (var document in _documents)
            {
                document.Parent = this;
            }
        }

        /// <summary>
        /// Gets all the child documents
        /// </summary>
        /// <param name="recursive">Indicates if the search is limited to the direct child or must scope to the sub-groups</param>
        /// <returns>The list of document</returns>
        public IEnumerable<Document> GetDocuments(string? name, bool recursive = false)
        {
            foreach (var document in _documents)
            {
                if (name is null || document.Name.Equals(name))
                    yield return document;
            }

            if (recursive)
            {
                foreach (var group in _groups)
                {
                    foreach (var document in group.GetDocuments(name, recursive))
                        yield return document;
                }
            }
        }

        /// <summary>
        /// Gets all the child groups
        /// </summary>
        /// <param name="recursive">Indicates if the search is limited to the direct child or must scope to the sub-groups</param>
        /// <returns>The list of group</returns>
        public IEnumerable<Group> GetGroups(string? name, bool recursive = false)
        {
            foreach (var group in _groups)
            {
                if (name is null || group.Name.Equals(name))
                {
                    yield return group;
                }

                if (recursive)
                {
                    foreach (var subgroup in group.GetGroups(name, recursive))
                        yield return subgroup;
                }
            }
        }

        public IEnumerable<Element> GetElements(string name, bool recursive = false)
        {
            return GetType(name).Name switch
            {
                nameof(Document) => GetDocuments(name, recursive),
                nameof(Group) => GetGroups(name, recursive),
                _ => Array.Empty<Element>()
            };
        }

        /// <summary>
        /// Generate the fields of this group. 
        /// The regular fields and aggregate fields of scope DirectChildren are generated : It means that this method should be called AFTER all the subDocuments and subGroups have been generated and attached.
        /// </summary>
        /// <param name="generators">A field generator collection</param>
        public override void GenerateFields(FieldGeneratorContainer generators)
        {
            if (generators is null)
                throw new ArgumentNullException(nameof(generators));

            if (generators.ElementHasFields(Name))
            {
                //Regular fields
                Fields.AddRange(generators.GenerateRegularFields(Name));
                //Aggregates fields of scope DirectChildren
                Fields.AddRange(generators.GenerateAggregateFields(Name, this));
            }
        }

        private Type GetType(string name)
        {
            if (GetDocuments(name, true).Any(e => e.Name.Equals(name)))
                return typeof(Document);

            if (GetGroups(name, true).Any(e => e.Name.Equals(name)))
                return typeof(Group);

            throw new ConfigurationException($"{name} is not a group nor a document");
        }
    }
}
