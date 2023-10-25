using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.MetaData
{
    public class MetaDataGeneratorCollection
    {
        public List<MetaDataGeneratorBase> Generators { get; } = new List<MetaDataGeneratorBase>();

        public bool Contains(string name) => Generators.Any(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        public void Add(MetaDataGeneratorBase item)
        {
            if(this.Contains(item.Name))
            {
                throw new ArgumentException($"A metadata generator with name '{item.Name}' was already added");
            }

            Generators.Add(item);
        }

        public void AddRange(IEnumerable<MetaDataGeneratorBase> items) 
        {
            foreach(var item in items)
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
