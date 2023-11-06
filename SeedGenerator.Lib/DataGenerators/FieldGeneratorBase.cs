namespace SeedGenerator.Lib.Builders
{
    internal abstract class FieldGeneratorBase
    {
        public string Name { get; }

        public FieldGeneratorBase(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException($"The property {nameof(name)} must be specified");

            Name = name;
        }

        public abstract string NextValue();
    }
}
