namespace SeedGenerator.Lib.Data
{
    /// <summary>
    /// Modelize the base class for <see cref="Document" and <see cref="Group"/>/>
    /// </summary>
    public class Element
    {
        /// <summary>
        /// The element type. Can be document or group
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// The element id. 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The element name. Can be seen as a "subtype"
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parent element. Can be null if the element is the top element in the tree
        /// </summary>
        public Group? Parent { get; set; }

        /// <summary>
        /// The fields of the element
        /// </summary>
        public FieldCollection Fields { get; set; } = new FieldCollection();

        /// <summary>
        /// Creates a new <see cref="Element"/>
        /// </summary>
        /// <param name="type">The type (document or group)</param>
        /// <param name="name">The name (subtype)</param>
        /// <param name="id">The id of the element</param>
        public Element(string type, string name, long id) 
        {
            Type = type;
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} ({Name}, {Id})";
        }
    }
}