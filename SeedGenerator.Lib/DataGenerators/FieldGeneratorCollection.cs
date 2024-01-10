using System.Data;
using SeedGenerator.Lib.Data;

namespace SeedGenerator.Lib.DataGenerators
{
    internal class FieldGeneratorCollection
    {
        public List<AbstractFieldGenerator> Builders { get; } = new List<AbstractFieldGenerator>();

        public FieldGeneratorCollection(IEnumerable<AbstractFieldGenerator> builders)
        {
            AddRange(builders);
        }

        public bool Contains(string name) => Builders.Any(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        public void Add(AbstractFieldGenerator item)
        {
            if (Contains(item.Name))
            {
                throw new ArgumentException($"A metadata generator with name '{item.Name}' was already added");
            }

            Builders.Add(item);
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

            foreach (var builder in Builders.OrderBy(x => x, new FieldGeneratorComparer()))
            {
                if (builder is AbstractFieldGeneratorDependant dependantBuilder)
                {
                    foreach (var dependance in dependantBuilder.Dependances.Keys)
                    {
                        var dependanceTarget = fields[dependance];
                        dependantBuilder.Dependances[dependance] = dependanceTarget.Value;
                    }
                }
                fields.Add(builder.Name, new Field(builder.Name, builder.NextValue()));
            }

            return fields;
        }
    }
}
