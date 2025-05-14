using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// Generates a sequence of incrementing long values, starting from a specified value and increasing by a fixed increment on each call.
    /// This generator is thread-safe.
    /// </summary>
    internal class FieldGeneratorIncrement : AbstractFieldGenerator<long>
    {
        private readonly Random _random;
        private long _value;
        private long _increment;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGeneratorIncrement"/> class.
        /// </summary>
        /// <param name="name">The unique name of the field generator.</param>
        /// <param name="start">The initial value to start incrementing from.</param>
        /// <param name="increment">The value to add to the current value on each generation.</param>
        public FieldGeneratorIncrement(string name, long start, long increment) : base(name)
        {
            _value = start;
            _increment = increment;
            _random = new Random();
        }

        /// <summary>
        /// Generates the next value in the sequence, incrementing the internal counter by the specified increment.
        /// </summary>
        /// <returns>
        /// The current value before incrementing.
        /// </returns>
        protected override long GenerateValue()
        {
            var value = Interlocked.Read(ref _value);
            Interlocked.Add(ref _value, _increment);
            return value;
        }
    }
}
