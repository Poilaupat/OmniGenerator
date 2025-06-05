using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Tools;
using System.Diagnostics;

namespace OmniGenerator.Lib.Hierarchy
{
    /// <summary>
    /// Represents the base class for <see cref="Document"/> and <see cref="Group"/> elements in the hierarchy.
    /// </summary>
    public abstract class Element
    {
        /// <summary>
        /// The parent <see cref="Group"/> of this <see cref="Element"/>.
        /// </summary>
        private Group? _parent;

        /// <summary>
        /// Gets the type of the <see cref="Element"/>. Can be "document" or "group".
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets or sets the name of the <see cref="Element"/>. Can be seen as a "subtype".
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parent <see cref="Group"/> of this <see cref="Element"/>.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if the parent group is not set when getting.</exception>
        public Group Parent
        {
            get
            {
                if (_parent is null)
                    throw new NullReferenceException(nameof(Parent));

                return _parent;
            }

            set
            {
                _parent = value;
            }
        }

        /// <summary>
        /// Gets or sets the fields of the <see cref="Element"/>.
        /// </summary>
        public IDictionary<string, Field> Fields { get; set; } = new Dictionary<string, Field>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Element"/> class.
        /// </summary>
        /// <param name="type">The type of the element (e.g., "document" or "group").</param>
        /// <param name="name">The name (subtype) of the element.</param>
        public Element(string type, string name)
        {
            Type = type;
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Element"/> class with the specified fields.
        /// </summary>
        /// <param name="type">The type of the element (e.g., "document" or "group").</param>
        /// <param name="name">The name (subtype) of the element.</param>
        /// <param name="fields">The fields to associate with the element.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fields"/> is null.</exception>
        public Element(string type, string name, IDictionary<string, Field> fields)
            : this(type, name)
        {
            if (fields is null)
                throw new ArgumentNullException(nameof(fields));

            Fields = fields;
        }

        /// <summary>
        /// Generates the fields of this element using the provided field generators.
        /// </summary>
        /// <param name="generators">
        /// A <see cref="FieldGeneratorContainer"/> containing field generators.
        /// If the collection contains no generators for this <see cref="Element"/>, no fields are generated.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="generators"/> is null.</exception>
        public virtual void GenerateFields(FieldGeneratorContainer generators)
        {
            if (generators is null)
                throw new ArgumentNullException(nameof(generators));

            if (generators.ElementHasFields(Name))
            {
                Fields.Merge(generators.GenerateRegularFields(Name));
            }
        }
    }
}