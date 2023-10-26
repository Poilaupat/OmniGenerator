namespace SeedGenerator.Lib.MetaData.Generators
{
    public abstract class MetaDataGeneratorBase
    {
        public string Name { get; }

        public MetaDataGeneratorBase(string name)
        {
            Name = name;
        }

        public abstract string NextValue();
    }
}
