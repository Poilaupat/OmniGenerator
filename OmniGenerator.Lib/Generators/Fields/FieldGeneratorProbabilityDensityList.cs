using OmniGenerator.Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The <see cref="FieldGeneratorProbabilityDensityList"/> class picks a random value from a collection,
    /// where each item has a probability of being picked proportional to its weight (density).
    /// </summary>
    internal class FieldGeneratorProbabilityDensityList : AbstractFieldGeneratorFromListBase<string, (string, double)>
    {
        private readonly Random _random;
        private readonly List<(string Value, double Weight)> _weightedList;
        private readonly double _totalWeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGeneratorProbabilityDensityList"/> class.
        /// </summary>
        /// <param name="name">The name of the generator.</param>
        /// <param name="list">The collection of value-weight pairs to pick from.</param>
        /// <param name="listFilePath">The path to the list file (for reference or loading).</param>
        public FieldGeneratorProbabilityDensityList(string name, IEnumerable<(string, double)>? list, string listFilePath)
            : base(name, list, listFilePath)
        {
            _random = new Random();
            _weightedList = List.ToList();
            _totalWeight = _weightedList.Sum(item => item.Weight);
        }

        /// <summary>
        /// Picks a random value from the list, with probability proportional to its weight.
        /// </summary>
        /// <returns>
        /// A randomly selected value from the list, or an empty string if the list is empty.
        /// </returns>
        protected override string GenerateValue()
        {
            if (!_weightedList.Any() || _totalWeight <= 0)
                return string.Empty;

            double r = _random.NextDouble() * _totalWeight;
            double cumulative = 0.0;
            foreach (var (value, weight) in _weightedList)
            {
                cumulative += weight;
                if (r < cumulative)
                    return value;
            }
            // Fallback in case of floating point error
            return _weightedList.Last().Value;
        }

        /// <summary>
        /// Parses a line from the list file into a value-weight pair.
        /// </summary>
        /// <param name="line">The line to parse, expected in the format "value,weight".</param>
        /// <returns>The parsed value-weight pair.</returns>
        protected override (string, double) ParseLine(string line)
        {
            var parts = line.Split(',');
            if (parts.Length != 2 || !double.TryParse(parts[1], out double weight))
                throw new FormatException("Each line must be in the format 'value,weight'.");
            return (parts[0], weight);
        }
    }
}
