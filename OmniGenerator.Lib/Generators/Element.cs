using OmniGenerator.Lib.Generators.Fields;

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
        /// 
        public Element(string type, string name)
        {
            Type = type;
            Name = name;
        }

        /// <summary>
        /// Generates the fields of this element
        /// </summary>
        /// <param name="generators">A field generator collection</param>
        /// <exception cref="ArgumentNullException">The field generator collection must not be null</exception>
        public virtual void GenerateFields(FieldGeneratorContainer generators)
        {
            if(generators is null)
                throw new ArgumentNullException(nameof(generators));

            if(generators.ElementHasFields(this.Name))
            {
                Fields.AddRange(generators.GenerateRegularFields(this.Name));
            }
        }


        public override string ToString()
        {
            return $"{this.GetType().Name} ({Name})";
        }
    }
}