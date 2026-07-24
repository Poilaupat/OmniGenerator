using OmniGenerator.Lib.Configuration.ErrorSimulation;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.ErrorSimulation.Mutators
{
    /// <summary>
    /// Simulates an OCR misread by replacing a block of 1 to 4 consecutive characters with '?'.
    /// The block position is chosen randomly. Mutates the data channel only; the image is untouched.
    /// </summary>
    public sealed class MisreadMutator : IFieldErrorMutator
    {
        private const int MaxBlockLength = 4;
        private const char MisreadChar = '?';

        /// <inheritdoc />
        public EErrorSimulationType Type => EErrorSimulationType.Misread;

        /// <inheritdoc />
        public void Mutate(Field field)
        {
            var current = field.DataStringValue;
            if (string.IsNullOrEmpty(current))
                return;

            var blockLength = Random.Shared.Next(1, Math.Min(MaxBlockLength, current.Length) + 1);
            var start = Random.Shared.Next(0, current.Length - blockLength + 1);

            var characters = current.ToCharArray();
            for (var i = start; i < start + blockLength; i++)
                characters[i] = MisreadChar;

            var mutated = new string(characters);
            field.DataValue = field.DataValue.CoerceFromString(mutated);
        }
    }
}
