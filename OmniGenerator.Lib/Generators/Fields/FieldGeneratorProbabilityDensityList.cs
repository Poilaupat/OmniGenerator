using OmniGenerator.Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// <see cref="FieldGeneratorProbabilityDensityList"/> picks up items in a collection according to a discrete probability density function
    /// </summary>
    internal class FieldGeneratorProbabilityDensityList : AbstractFieldGeneratorCollectionBase<string, KeyValuePair<string, int>>
    {
        private readonly DiscreteDistributionRandomGenerator<string> _random;

        /// <summary>
        /// Creates a new <see cref="FieldGeneratorProbabilityDensityList"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="list"></param>
        /// <param name="listFilePath"></param>
        public FieldGeneratorProbabilityDensityList(string name, IEnumerable<KeyValuePair<string, int>>? list, string listFilePath)
            :base(name, list, listFilePath) 
        {
            _random = new DiscreteDistributionRandomGenerator<string>(List.ToDictionary(k => k.Key, v => v.Value));
        }

        protected override string GenerateValue()
        {
            if (List.Any())
                return _random.NextValue();
            else
                return string.Empty;
        }

        protected override KeyValuePair<string, int> ParseLine(string line)
        {
            var chunks = line.Split(new char[] { ';', '|' });

            if(chunks.Length > 1 && int.TryParse(chunks[1], out int value))
            {
                return KeyValuePair.Create(chunks[0], value);
            }

            throw new FormatException($"Parsing of the {line} failed");
        }
    }
}
