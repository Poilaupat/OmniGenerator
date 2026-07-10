using OmniGenerator.Lib.Configuration.ErrorSimulation;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.ErrorSimulation.Mutators
{
    /// <summary>
    /// Simulates a human inconsistency: the value visible on the image diverges from the correct metadata
    /// (for example a mistyped handwritten amount). Mutates the image channel only; the metadata is untouched.
    /// The mutation is field-agnostic: it alters a single character, preferring digits when present.
    /// </summary>
    public sealed class InconsistencyMutator : IFieldErrorMutator
    {
        /// <inheritdoc />
        public EErrorSimulationType Type => EErrorSimulationType.Inconsistency;

        /// <inheritdoc />
        public void Mutate(Field field)
        {
            var current = field.ImageStringValue;
            if (string.IsNullOrEmpty(current))
                return;

            var characters = current.ToCharArray();
            var digitPositions = new List<int>();
            for (var i = 0; i < characters.Length; i++)
            {
                if (char.IsDigit(characters[i]))
                    digitPositions.Add(i);
            }

            if (digitPositions.Count > 0)
            {
                var position = digitPositions[Random.Shared.Next(digitPositions.Count)];
                characters[position] = ShiftDigit(characters[position]);
            }
            else
            {
                var position = Random.Shared.Next(characters.Length);
                characters[position] = ShiftLetterOrKeep(characters[position]);
            }

            var mutated = new string(characters);
            field.ImageValue = FieldValueCoercion.Coerce(field.ImageValue, mutated);
        }

        private static char ShiftDigit(char digit)
        {
            var value = digit - '0';
            var delta = Random.Shared.Next(1, 10);
            return (char)('0' + (value + delta) % 10);
        }

        private static char ShiftLetterOrKeep(char character)
        {
            if (char.IsLetter(character))
            {
                var baseChar = char.IsUpper(character) ? 'A' : 'a';
                var offset = character - baseChar;
                var delta = Random.Shared.Next(1, 26);
                return (char)(baseChar + (offset + delta) % 26);
            }

            return character;
        }
    }
}
