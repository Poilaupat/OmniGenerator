using System.Data;

namespace SeedGenerator.Lib.MetaData.Generators
{
    public class MetaDataGeneratorCollection
    {
        public List<MetaDataGeneratorBase> Generators { get; } = new List<MetaDataGeneratorBase>();

        public MetaDataGeneratorCollection(IEnumerable<MetaDataGeneratorBase> generators) 
        { 
            AddRange(generators);
        }

        public bool Contains(string name) => Generators.Any(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        public void Add(MetaDataGeneratorBase item)
        {
            if (Contains(item.Name))
            {
                throw new ArgumentException($"A metadata generator with name '{item.Name}' was already added");
            }

            Generators.Add(item);
        }

        public void AddRange(IEnumerable<MetaDataGeneratorBase> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public MetaDataCollection GenerateMetaData()
        {
            MetaDataCollection metadatas = new MetaDataCollection();

            foreach (var generator in Generators.OrderBy(x => x, new MetaDataGeneratorComparer()))
            {
                if (generator is MetaDataGeneratorDependantBase dependantGenerator)
                {
                    var targetMetadata = metadatas[dependantGenerator.DependantUpon];
                    dependantGenerator.DependantValue = targetMetadata.Value;
                }

                metadatas.Add(generator.Name, new MetaDataItem(generator.Name, generator.NextValue()));
            }

            return metadatas;
        }
    }
}
