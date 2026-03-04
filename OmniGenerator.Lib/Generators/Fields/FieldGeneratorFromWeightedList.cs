using System;
using System.Collections.Generic;
using System.Linq;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The <see cref="FieldGeneratorFromWeightedList"/> class picks a random value from a collection,
    /// where each item has a probability of being picked proportional to its weight (density).
    /// </summary>
    internal class FieldGeneratorFromWeightedList : AbstractFieldGeneratorFromListBase<string, WeightedValue>
    {
        private readonly Random _random;
        private double? _totalWeight;

        /// <summary>
        /// Gets the total weight of all items in the list.
        /// Calculated lazily and cached until the list changes
        /// </summary>
        private double TotalWeight
        {
            get
            {
                if (_totalWeight is null)
                {
                    _totalWeight = List?.Sum(item => item.Weight) ?? 0.0;
                }
                return _totalWeight.Value;
            }
        }



        /// <summary>
        /// Gets or sets the list of weighted values.
        /// Setting this property invalidates the cached total weight.
        /// </summary>
        public override IEnumerable<WeightedValue> List
        {
            get => base.List;
            set
            {
                base.List = value;
                _totalWeight = null; // Invalidate cache when list changes
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGeneratorFromWeightedList"/> class.
        /// </summary>
        /// <param name="name">The name of the generator.</param>
        /// <param name="list">The collection of value-weight pairs to pick from.</param>
        public FieldGeneratorFromWeightedList(string name, IEnumerable<WeightedValue> list)
            : base(name, list) => _random = new Random();

        /// <summary>
        /// Picks a random value from the list, with probability proportional to its weight.
        /// </summary>
        /// <returns>
        /// A randomly selected value from the list, or an empty string if the list is empty.
        /// </returns>
        protected override string GenerateValue()
        {
            if (!List.Any() || TotalWeight <= 0)
                return string.Empty;

            double r = _random.NextDouble() * TotalWeight;
            double cumulative = 0.0;
            foreach (var weightedValue in List)
            {
                cumulative += weightedValue.Weight;
                if (r < cumulative)
                    return weightedValue.Value;
            }
            // Fallback in case of floating point error
            return List.Last().Value;
        }
    }
}
