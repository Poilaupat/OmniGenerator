using System.Collections;

namespace OmniGenerator.Lib.Hierarchy
{
    /// <summary>
    /// Represents a strongly typed read-only collection of child <see cref="Element"/> instances
    /// belonging to a <see cref="Group"/>.
    /// </summary>
    /// <typeparam name="TElement">The element type. Must inherit from <see cref="Element"/>.</typeparam>
    /// <remarks>
    /// This collection is responsible for enforcing parent assignment when elements are added.
    /// It exposes only read-only enumeration to external callers to prevent uncontrolled mutation.
    /// Mutation is limited to <see cref="Add(TElement)"/> and <see cref="AddRange(IEnumerable{TElement}?)"/>.
    /// </remarks>
    internal sealed class ElementCollection<TElement> : IReadOnlyCollection<TElement>
        where TElement : Element
    {
        /// <summary>
        /// The internal storage for items.
        /// </summary>
        private readonly List<TElement> _items = new();

        /// <summary>
        /// The owning <see cref="Group"/> whose children are represented by this collection.
        /// </summary>
        private readonly Group _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementCollection{TElement}"/> class.
        /// </summary>
        /// <param name="owner">The owning group. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="owner"/> is null.</exception>
        public ElementCollection(Group owner)
        {
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator over <typeparamref name="TElement"/> items.</returns>
        public IEnumerator<TElement> GetEnumerator() => _items.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Adds an element to the collection and sets its <see cref="Element.Parent"/> to the owning <see cref="Group"/>.
        /// </summary>
        /// <param name="element">The element to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="element"/> is null.</exception>
        public void Add(TElement element)
        {
            if (element is null) throw new ArgumentNullException(nameof(element));
            element.Parent = _owner;
            _items.Add(element);
        }

        /// <summary>
        /// Adds a range of elements to the collection (ignores null input) and assigns their parent.
        /// </summary>
        /// <param name="elements">The elements to add. If null, nothing is done.</param>
        public void AddRange(IEnumerable<TElement>? elements)
        {
            if (elements is null) return;
            foreach (var e in elements)
                Add(e);
        }

        /// <summary>
        /// Returns a snapshot enumeration of the underlying items.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{TElement}"/> sequence of items.</returns>
        public IEnumerable<TElement> AsEnumerable() => _items.AsEnumerable();
    }
}
