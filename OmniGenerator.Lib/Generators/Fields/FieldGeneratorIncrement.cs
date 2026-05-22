using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// Provides a field generator that produces a sequence of incrementing <see cref="long"/> values.
    /// The sequence starts from a specified value and increases by a fixed increment on each generation.
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
        /// Generates the next value in the incrementing sequence.
        /// The method is thread-safe and returns the current value before incrementation.
        /// </summary>
        /// <returns>
        /// The current <see cref="long"/> value before incrementing.
        /// </returns>
        protected override long GenerateValue()
        {
            // Atomically adds the increment to the value and returns the value before incrementation.
            return Interlocked.Add(ref _value, _increment) - _increment;
        }
    }
}
