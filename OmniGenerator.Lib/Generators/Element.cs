namespace OmniGenerator.Lib.Generators
{
    /// <summary>
    /// Modelize the base class for <see cref="Document" and <see cref="Group"/>/>
    /// </summary>
    public class Element
    {
        private Group? _parent;

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
        /// The parent element.
        /// </summary>
        public Group Parent 
        {
            get
            {
                if(_parent is null)
                    throw new NullReferenceException(nameof(Parent));

                return _parent;
            }

            set
            {
                _parent = value;
            } 
        }

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