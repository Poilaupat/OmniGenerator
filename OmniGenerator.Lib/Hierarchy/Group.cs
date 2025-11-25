using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Tools;
using System.Diagnostics;

namespace OmniGenerator.Lib.Hierarchy
{
    /// <summary>
    /// Represents a group element in the hierarchy, containing inner <see cref="Document"/> and <see cref="Group"/> elements.
    /// </summary>
    [DebuggerDisplay("Group = {Name}")]
    public class Group : Element
    {
        private readonly ElementCollection<Group> _groups;
        private readonly ElementCollection<Document> _documents;

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class with the specified name, groups, and documents.
        /// </summary>
        /// <param name="name">The name of the group. Can be seen as a group type.</param>
        /// <param name="groups">The inner groups of this group.</param>
        /// <param name="documents">The inner documents of this group.</param>
        public Group(string name, Group[] groups, Document[] documents)
            : base("group", name)
        {
            _groups = new ElementCollection<Group>(this);
            _documents = new ElementCollection<Document>(this);
            _groups.AddRange(groups);
            _documents.AddRange(documents);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class with the specified name, groups, documents, and fields.
        /// </summary>
        /// <param name="name">The name of the group. Can be seen as a group type.</param>
        /// <param name="groups">The inner groups of this group.</param>
        /// <param name="documents">The inner documents of this group.</param>
        /// <param name="fields">The fields to associate with the group.</param>
        public Group(string name, Group[] groups, Document[] documents, IDictionary<string, Field> fields)
            : base("group", name, fields)
        {
            _groups = new ElementCollection<Group>(this);
            _documents = new ElementCollection<Document>(this);
            _groups.AddRange(groups);
            _documents.AddRange(documents);
        }

        /// <summary>
        /// Gets all the child documents of this group, optionally searching recursively in sub-groups.
        /// </summary>
        /// <param name="name">The name of the documents to retrieve, or null to retrieve all documents.</param>
        /// <param name="recursive">Indicates if the search should include sub-groups recursively.</param>
        /// <returns>An enumerable of <see cref="Document"/> objects.</returns>
        public IEnumerable<Document> GetDocuments(string? name, bool recursive = false)
        {
            foreach (var document in _documents.AsEnumerable())
            {
                if (name is null || document.Name.Equals(name))
                    yield return document;
            }

            if (recursive)
            {
                foreach (var group in _groups.AsEnumerable())
                {
                    foreach (var document in group.GetDocuments(name, recursive))
                        yield return document;
                }
            }
        }

        /// <summary>
        /// Gets all the child groups of this group, optionally searching recursively in sub-groups.
        /// </summary>
        /// <param name="name">The name of the groups to retrieve, or null to retrieve all groups.</param>
        /// <param name="recursive">Indicates if the search should include sub-groups recursively.</param>
        /// <returns>An enumerable of <see cref="Group"/> objects.</returns>
        public IEnumerable<Group> GetGroups(string? name, bool recursive = false)
        {
            foreach (var group in _groups.AsEnumerable())
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

        /// <summary>
        /// Gets all the child elements (documents or groups) of this group with the specified name.
        /// </summary>
        /// <param name="name">The name of the elements to retrieve.</param>
        /// <param name="recursive">Indicates if the search should include sub-groups recursively.</param>
        /// <returns>An enumerable of <see cref="Element"/> objects.</returns>
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
        /// Generates the fields of this group, including regular fields and aggregate fields of scope DirectChildren.
        /// This method should be called after all sub-documents and sub-groups have been generated and attached.
        /// </summary>
        /// <param name="generators">A <see cref="FieldGeneratorContainer"/> containing field generators.</param>
        public override void GenerateFields(FieldGeneratorContainer generators)
        {
            base.GenerateFields(generators);

            if (generators.ElementHasFields(Name))
            {
                var aggregates = generators.GenerateAggregateFields(Name, this);
                Fields.AddRange(aggregates); // Use AddRange
            }
        }

        /// <summary>
        /// Determines the type of element (document or group) for the specified name.
        /// </summary>
        /// <param name="name">The name of the element to check.</param>
        /// <returns>The <see cref="Type"/> of the element.</returns>
        /// <exception cref="ConfigurationException">Thrown if the name does not correspond to a group or document.</exception>
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
