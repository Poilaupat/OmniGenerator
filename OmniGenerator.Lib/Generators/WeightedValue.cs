using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Generators
{
    public class WeightedValue(string value, double weight)
    {
        public string Value { get; } = value;
        public double Weight { get; } = weight;
    }
}
