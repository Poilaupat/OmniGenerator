namespace SeedGenerator.Lib.DataGenerators
{
    internal abstract class AbstractFieldGenerator
    {
        public string Name { get; }

        public AbstractFieldGenerator(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException($"The property {nameof(name)} must be specified");


            Name = name;
        }

        public abstract string NextValue();
    }
}
