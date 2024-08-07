using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Tools
{
    internal class DiscreteDistributionRandomGenerator<T> where T : notnull
    {
        private readonly Dictionary<T, int> _probabilityDensityFunction;
        private readonly Dictionary<T, int> _cumulativeDensityFunctionInverse = new Dictionary<T, int>();
        private readonly Random _random;

        public int Sum { get; }

        public DiscreteDistributionRandomGenerator(Dictionary<T, int> probabilityDensityFunction, Random? random = null)
        {
            ArgumentNullException.ThrowIfNull(probabilityDensityFunction);

            _random = random  ?? new Random();
            _probabilityDensityFunction = probabilityDensityFunction;

            int cumulprob = 0;
            foreach (var kvp in _probabilityDensityFunction.OrderByDescending(p => p.Value))
            {
                cumulprob += kvp.Value;
                _cumulativeDensityFunctionInverse.Add(kvp.Key, cumulprob);
            }

            Sum = _cumulativeDensityFunctionInverse.Last().Value;
        }

        public T NextValue()
        {
            int r = (int)Math.Ceiling((decimal)(_random.NextDouble() * Sum));
            return _cumulativeDensityFunctionInverse.First(x => x.Value >= r).Key;
        }
    }
}
