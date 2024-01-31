using SeedGenerator.Lib.Interfaces;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal abstract class AbstractFieldGenerator<T> : IFieldGenerator
        where T : notnull
    {
        private T? _lastValue;

        public T LastValue 
        { 
            get
            {
                if (_lastValue is null)
                    throw new InvalidOperationException($"No value was generated. Call {nameof(RefreshValue)} first.");
                return _lastValue;
            }
        }

        object IFieldGenerator.LastValue => LastValue;

        public string Name { get; }

        protected AbstractFieldGenerator(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException($"The property {nameof(name)} must be specified");

            Name = name;
        }

        public virtual void RefreshValue()
        {
            _lastValue = GenerateValue();
        }

        protected abstract T GenerateValue();
    }
}
