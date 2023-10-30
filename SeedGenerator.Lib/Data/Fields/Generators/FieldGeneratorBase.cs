namespace SeedGenerator.Lib.Data.Fields.Generators
{
    internal abstract class FieldGeneratorBase
    {
        public string Name { get; }

        public FieldGeneratorBase(string name)
        {
            Name = name;
        }

        public abstract string NextValue();
    }
}
