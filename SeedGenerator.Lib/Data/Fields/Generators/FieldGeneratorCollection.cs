using System.Data;

namespace SeedGenerator.Lib.Data.Fields.Generators
{
    internal class FieldGeneratorCollection
    {
        public List<FieldGeneratorBase> Generators { get; } = new List<FieldGeneratorBase>();

        public FieldGeneratorCollection(IEnumerable<FieldGeneratorBase> generators)
        {
            AddRange(generators);
        }

        public bool Contains(string name) => Generators.Any(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        public void Add(FieldGeneratorBase item)
        {
            if (Contains(item.Name))
            {
                throw new ArgumentException($"A metadata generator with name '{item.Name}' was already added");
            }

            Generators.Add(item);
        }

        public void AddRange(IEnumerable<FieldGeneratorBase> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public FieldCollection GenerateFields()
        {
            FieldCollection fields = new FieldCollection();

            foreach (var generator in Generators.OrderBy(x => x, new FieldGeneratorComparer()))
            {
                if (generator is FieldGeneratorDependantBase dependantGenerator)
                {
                    foreach (var dependance in dependantGenerator.Dependances.Keys)
                    {
                        var dependanceTarget = fields[dependance];
                        dependantGenerator.Dependances[dependance] = dependanceTarget.Value;
                    }
                }
                fields.Add(generator.Name, new Field(generator.Name, generator.NextValue()));
            }

            return fields;
        }
    }
}
