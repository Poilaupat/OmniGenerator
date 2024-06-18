using SeedGenerator.Lib.Tools;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    /// <summary>
    /// The <see cref="FieldGeneratorList"/> picks a random value within a list
    /// </summary>
    internal class FieldGeneratorList : AbstractFieldGenerator<string>
    {
        /// <summary>
        /// The path to the list
        /// The list must be a file with one value by line
        /// </summary>
        public string ListPath { get; }

        /// <summary>
        /// Creates a new <see cref="FieldGeneratorList"
        /// </summary>
        /// <param name="name">The name of the generator</param>
        /// <param name="listpath">The path to the list file</param>
        public FieldGeneratorList(string name, string listpath)
            : base(name)
        {
            ListPath = listpath;
        }

        protected override string GenerateValue()
        {
            var list = ListCache.GetList(ListPath);

            if (list.Length > 0)
            {
                return list[new Random().Next(0, list.Length)];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
