using OmniGenerator.Lib.Interfaces;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// Base type for all field generators
    /// </summary>
    /// <typeparam name="T">The type of the values produced by the <see cref="AbstractFieldGenerator"/></typeparam>
    internal abstract class AbstractFieldGenerator<T> : IFieldGenerator
        where T : notnull
    {
        private T? _lastValue;

        /// <summary>
        /// The typed last value produced by the <see cref="AbstractFieldGenerator{T}"/>
        /// If no value was was ever generated, throw a <see cref="InvalidOperationException"/>
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        public T LastValue 
        { 
            get
            {
                if (_lastValue is null)
                    throw new InvalidOperationException($"{ this.Name } : No value was generated. Call {nameof(RefreshValue)} first.");
                return _lastValue;
            }
        }

        /// <summary>
        /// The boxed last value produced by the <see cref="AbstractFieldGenerator{T}"/>
        /// If no value was was ever generated, throw a <see cref="InvalidOperationException"/>
        /// </summary>
        object IFieldGenerator.LastValue => LastValue;

        /// <summary>
        /// The <see cref="AbstractFieldGenerator{T}" name/>
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a new <see cref="AbstractFieldGenerator{T}"/>
        /// If the provided name is not valid, throws a <see cref="ArgumentException"/>
        /// </summary>
        /// <param name="name">The name of the generator</param>
        /// <exception cref="ArgumentException"></exception>
        protected AbstractFieldGenerator(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"The property {nameof(name)} must be specified");

            Name = name;
        }

        /// <summary>
        /// Affects a new value to LastValue 
        /// </summary>
        public virtual void RefreshValue()
        {
            _lastValue = GenerateValue();
        }

        /// <summary>
        /// Generates a new value;
        /// </summary>
        /// <returns></returns>
        protected abstract T GenerateValue();
    }
}
