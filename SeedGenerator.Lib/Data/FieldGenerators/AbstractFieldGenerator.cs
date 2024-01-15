namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal abstract class AbstractFieldGenerator
    {
        public string Name { get; }

        protected AbstractFieldGenerator(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException($"The property {nameof(name)} must be specified");


            Name = name;
        }

        public abstract string NextValue();
    }
}
