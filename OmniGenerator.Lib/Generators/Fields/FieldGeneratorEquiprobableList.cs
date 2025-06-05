using OmniGenerator.Lib.Tools;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The <see cref="FieldGeneratorEquiprobableList"/> picks a random value from a collection. 
    /// Each item of the collection has an equiprobable chance to be picked.
    /// </summary>
    internal class FieldGeneratorEquiprobableList : AbstractFieldGeneratorFromListBase<string, string>
    {
        private readonly Random _random;
        private readonly int _listcount;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGeneratorEquiprobableList"/> class.
        /// </summary>
        /// <param name="name">The name of the generator.</param>
        /// <param name="list">The collection of possible values to pick from.</param>
        /// <param name="listFilePath">The path to the list file (for reference or loading).</param>
        public FieldGeneratorEquiprobableList(string name, IEnumerable<string>? list, string listFilePath)
            : base(name, list, listFilePath)
        {
            _random = new Random();
            _listcount = List.Count();
        }

        /// <summary>
        /// Picks a random value from the list with equal probability.
        /// </summary>
        /// <returns>
        /// A randomly selected value from the list, or an empty string if the list is empty.
        /// </returns>
        protected override string GenerateValue()
        {
            if (List.Any())
            {
                return List.ElementAt(_random.Next(0, _listcount));
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Parses a line from the list file into a value.
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <returns>The parsed value (the line itself).</returns>
        protected override string ParseLine(string line)
        {
            return line;
        }
    }
}
