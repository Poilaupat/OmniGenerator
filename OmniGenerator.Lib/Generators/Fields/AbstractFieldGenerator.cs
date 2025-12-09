using OmniGenerator.Lib.Interfaces.FieldGenerators;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// Base type for all field generators.
    /// Provides a framework for generating typed values and managing the last generated value.
    /// </summary>
    /// <typeparam name="T">The type of the values produced by the <see cref="AbstractFieldGenerator{T}"/>.</typeparam>
    internal abstract class AbstractFieldGenerator<T> : IFieldGenerator
        where T : notnull
    {
        private readonly object _lock = new();
        private T? _lastValue;

        /// <summary>
        /// Gets the name of the generator.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the last value produced by the generator, boxed as an object.
        /// If no value was ever generated, throws a <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if no value was generated.</exception>
        object IFieldGenerator.LastValue => LastValue;

        /// <summary>
        /// Generates the next value and returns it, boxed as an object.
        /// </summary>
        /// <returns>The next generated value, boxed as an object.</returns>
        object IFieldGenerator.GenerateNextValue()
        {
            return GenerateNextValue();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractFieldGenerator{T}"/> class.
        /// If the provided name is not valid, throws a <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="name">The name of the generator.</param>
        /// <exception cref="ArgumentException">Thrown if the name is null, empty, or whitespace.</exception>
        protected AbstractFieldGenerator(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"The property {nameof(name)} must be specified");

            Name = name;
        }

        /// <summary>
        /// Gets the last value produced by the generator.
        /// If no value was ever generated, throws a <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if no value was generated.</exception>
        public T LastValue
        {
            get
            {
                if (_lastValue is null)
                    throw new InvalidOperationException($"{this.Name} : No value was generated. Call a value generation method first.");
                return _lastValue;
            }
        }

        /// <summary>
        /// Generates the next value and returns it.
        /// </summary>
        /// <returns>The next generated value.</returns>
        public T GenerateNextValue()
        {
            lock (_lock)
            {
                _lastValue = GenerateValue();
                return _lastValue;
            }
        }

        /// <summary>
        /// Generates a new value.
        /// This method must be implemented by derived classes to define the logic for value generation.
        /// </summary>
        /// <returns>The generated value.</returns>
        protected abstract T GenerateValue();
    }
}
