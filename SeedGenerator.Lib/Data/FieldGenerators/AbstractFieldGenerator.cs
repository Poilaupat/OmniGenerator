namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal abstract class AbstractFieldGenerator
    {
        private object? _lastValue;

        public object LastValue 
        { 
            get
            {
                if (_lastValue is null)
                    throw new InvalidOperationException($"No value was generated. Call SetNewValue first.");
                return _lastValue;
            }
        }

        public string Name { get; }

        protected AbstractFieldGenerator(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException($"The property {nameof(name)} must be specified");


            Name = name;
        }

        public virtual void SetNewValue()
        {
            _lastValue = NextValue();
        }

        protected abstract object NextValue();
    }
}
