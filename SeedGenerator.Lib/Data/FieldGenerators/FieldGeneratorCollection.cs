using System.Data;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorCollection
    {
        private IList<AbstractFieldGenerator> _generators = new List<AbstractFieldGenerator>();

        public IList<AbstractFieldGenerator> FieldGenerators => _generators;

        public IEnumerable<AbstractFieldGenerator> DeterministicFieldGenerators => _generators
            .Where(x => x.GetType().BaseType?.Equals(typeof(AbstractFieldGenerator)) ?? false);

        public IEnumerable<AbstractFieldGeneratorDependant> DependentFieldGenerators => _generators
            .Where(x => x.GetType().IsSubclassOf(typeof(AbstractFieldGeneratorDependant)) &&
                x.GetType() != typeof(FieldGeneratorAggregate))
            .Cast<AbstractFieldGeneratorDependant>()
            .OrderBy(x => x, new FieldGeneratorComparer());

        public IEnumerable<FieldGeneratorAggregate> AggregateFieldGenerators => _generators
            .Where(x => x.GetType() == typeof(FieldGeneratorAggregate))
            .Cast<FieldGeneratorAggregate>();



        public FieldGeneratorCollection(IEnumerable<AbstractFieldGenerator> fieldGenerators)
        {
            AddRange(fieldGenerators);
        }

        public bool Contains(string name) => _generators.Any(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        public void Add(AbstractFieldGenerator item)
        {
            if (Contains(item.Name))
            {
                throw new ArgumentException($"A metadata generator with name '{item.Name}' was already added");
            }

            _generators.Add(item);
        }

        public void AddRange(IEnumerable<AbstractFieldGenerator> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public FieldCollection GenerateFields()
        {
            FieldCollection fields = new FieldCollection();

            foreach (var fieldGenerator in DeterministicFieldGenerators)
            {
                fields.Add(fieldGenerator.Name, new Field(fieldGenerator.Name, fieldGenerator.NextValue()));
            }

            foreach (var fieldGenerator in DependentFieldGenerators)
            {
                foreach (var dependance in fieldGenerator.Dependances.Keys)
                {
                    var dependanceTarget = fields[dependance];
                    fieldGenerator.Dependances[dependance] = dependanceTarget.Value;
                }
                fields.Add(fieldGenerator.Name, new Field(fieldGenerator.Name, fieldGenerator.NextValue()));
            }

            return fields;
        }

        public override string ToString()
        {
            return $"Field count = {_generators.Count}";
        }
    }
}
