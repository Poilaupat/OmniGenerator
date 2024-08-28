using OmniGenerator.Lib.Tools;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The <see cref="FieldGeneratorEquiprobableList"/> picks a random values within a collection. Each item of the collection has an equiprobable chance to be picked up. 
    /// </summary>
    internal class FieldGeneratorEquiprobableList : AbstractFieldGeneratorCollectionBase<string, string>
    {
        private readonly Random _random;

        /// <summary>
        /// Creates a new <see cref="FieldGeneratorEquiprobableList"
        /// </summary>
        /// <param name="name">The name of the generator</param>
        /// <param name="listpath">The path to the list file</param>
        public FieldGeneratorEquiprobableList(string name, IEnumerable<string>? list, string listFilePath)
            : base(name, list, listFilePath)
        {
            _random = new Random();
        }

        protected override string GenerateValue()
        {
            if (List.Any())
            {
                return List.ElementAt(_random.Next(0, List.Count()));
            }
            else
            {
                return string.Empty;
            }
        }

        protected override string ParseLine(string line)
        {
            return line;
        }
    }
}
