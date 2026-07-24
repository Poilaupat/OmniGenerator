using OmniGenerator.Lib.Configuration.ErrorSimulation;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.ErrorSimulation.Mutators
{
    /// <summary>
    /// Simulates a scanner confusion by substituting a single character using a confusion table
    /// (for example 0 to 8, 1 to 7). Characters absent from the table are left unchanged.
    /// Mutates the data channel only; the image is untouched.
    /// </summary>
    public sealed class SubstitutionMutator : IFieldErrorMutator
    {
        /// <summary>
        /// Default confusion table mapping a character to its visually similar counterpart.
        /// </summary>
        private static readonly IReadOnlyDictionary<char, char> ConfusionTable = new Dictionary<char, char>
        {
            ['0'] = '8',
            ['8'] = '0',
            ['1'] = '7',
            ['7'] = '1',
            ['5'] = '6',
            ['6'] = '5',
            ['2'] = 'Z',
            ['Z'] = '2',
            ['B'] = '8',
            ['O'] = '0',
        };

        /// <inheritdoc />
        public EErrorSimulationType Type => EErrorSimulationType.Substitution;

        /// <inheritdoc />
        public void Mutate(Field field)
        {
            var current = field.DataStringValue;
            if (string.IsNullOrEmpty(current))
                return;

            var candidatePositions = new List<int>();
            for (var i = 0; i < current.Length; i++)
            {
                if (ConfusionTable.ContainsKey(current[i]))
                    candidatePositions.Add(i);
            }

            if (candidatePositions.Count == 0)
                return;

            var position = candidatePositions[Random.Shared.Next(candidatePositions.Count)];
            var characters = current.ToCharArray();
            characters[position] = ConfusionTable[current[position]];

            var mutated = new string(characters);
            field.DataValue = field.DataValue.CoerceFromString(mutated);
        }
    }
}
