using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Tools
{
    /// <summary>
    /// Provides random sampling from a discrete probability distribution defined by a set of weighted values.
    /// </summary>
    /// <typeparam name="T">The type of the values to sample. Must be non-nullable.</typeparam>
    internal class DiscreteDistributionRandomGenerator<T>
        where T : notnull
    {
        private readonly Dictionary<T, int> _probabilityDensityFunction;
        private readonly Dictionary<T, int> _cumulativeDensityFunctionInverse = new Dictionary<T, int>();
        private readonly Random _random;

        /// <summary>
        /// Gets the sum of all weights in the probability distribution.
        /// </summary>
        public int Sum { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscreteDistributionRandomGenerator{T}"/> class
        /// with the specified probability density function and optional random number generator.
        /// </summary>
        /// <param name="probabilityDensityFunction">
        /// A dictionary mapping each value of type <typeparamref name="T"/> to its associated weight (probability).
        /// All weights should be positive integers.
        /// </param>
        /// <param name="random">
        /// An optional <see cref="Random"/> instance to use for sampling. If null, a new instance is created.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="probabilityDensityFunction"/> is null.
        /// </exception>
        public DiscreteDistributionRandomGenerator(Dictionary<T, int> probabilityDensityFunction, Random? random = null)
        {
            ArgumentNullException.ThrowIfNull(probabilityDensityFunction);

            _random = random ?? new Random();
            _probabilityDensityFunction = probabilityDensityFunction;

            int cumulprob = 0;
            foreach (var kvp in _probabilityDensityFunction.OrderByDescending(p => p.Value))
            {
                cumulprob += kvp.Value;
                _cumulativeDensityFunctionInverse.Add(kvp.Key, cumulprob);
            }

            Sum = _cumulativeDensityFunctionInverse.Last().Value;
        }

        /// <summary>
        /// Returns a randomly selected value of type <typeparamref name="T"/> according to the defined probability distribution.
        /// </summary>
        /// <returns>
        /// A value of type <typeparamref name="T"/> sampled based on the weights provided in the constructor.
        /// </returns>
        public T NextValue()
        {
            int r = (int)Math.Ceiling((decimal)(_random.NextDouble() * Sum));
            return _cumulativeDensityFunctionInverse.First(x => x.Value >= r).Key;
        }
    }
}
