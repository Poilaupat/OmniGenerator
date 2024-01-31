using SeedGenerator.Lib.Data.FieldGenerators;

namespace SeedGenerator.Test
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class FieldGeneratorComparerTests
    {
        [Test]
        public void OnlyBaseFieldGenerators()
        {
            var fields = new[]
            {
                new FieldGeneratorRegex("CField", ".*"),
                new FieldGeneratorRegex("BField", ".*"),
                new FieldGeneratorRegex("AField", ".*"),
            };

            var ordered = fields.OrderBy(x => x, new FieldGeneratorComparer());

            Assert.That(ordered.ElementAt(0).Name, Is.EqualTo("AField"));
            Assert.That(ordered.ElementAt(1).Name, Is.EqualTo("BField"));
            Assert.That(ordered.ElementAt(2).Name, Is.EqualTo("CField"));
        }

        [Test]
        public void DependantFieldGeneratorsWithoutMutualDependance()
        {
            var fields = new AbstractFieldGenerator[]
            {
                new FieldGeneratorComposite("BField", "AField", "{{AField}}"),
                new FieldGeneratorComposite("AField", "AField", "{{AField}}"),
                new FieldGeneratorRegex("CField", ".*"),
            };

            var ordered = fields.OrderBy(x => x, new FieldGeneratorComparer());

            Assert.That(ordered.ElementAt(0).Name, Is.EqualTo("CField"));
            Assert.That(ordered.ElementAt(1).Name, Is.EqualTo("AField"));
            Assert.That(ordered.ElementAt(2).Name, Is.EqualTo("BField"));
        }

        [Test]
        public void DependantFieldGeneratorsWithMutualDependance()
        {
            var fields = new AbstractFieldGenerator[]
            {
                new FieldGeneratorComposite("AField", "BField", "{{BField}}"),
                new FieldGeneratorComposite("BField", "CField", "{{CField}}"),
                new FieldGeneratorComposite("CField", "DField", "{{DField}}"),
                new FieldGeneratorRegex("DField", ".*"),
                new FieldGeneratorKeyCalculator("AKField", "AField", EKeyType.Rlmc),
            };

            var ordered = fields.OrderBy(x => x, new FieldGeneratorComparer()).ToArray();

            Assert.That(ordered.ElementAt(0).Name, Is.EqualTo("DField"));
            Assert.That(ordered.ElementAt(1).Name, Is.EqualTo("CField"));
            Assert.That(ordered.ElementAt(2).Name, Is.EqualTo("BField"));
            Assert.That(ordered.ElementAt(3).Name, Is.EqualTo("AField"));
            Assert.That(ordered.ElementAt(4).Name, Is.EqualTo("AKField"));
        }
    }
}
