using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Interfaces.FieldGenerators;
using System.CodeDom;

namespace OmniGenerator.Test.Lib
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class FieldGeneratorComparerTests
    {
        [Test]
        public void DependenceRelationProperties()
        {
            var a = new FieldGeneratorRegex("AField", ".*");
            var b = new FieldGeneratorRegex("BField", ".*");
            var c = new FieldGeneratorComposite("CField", "AField,BField", "{{AField}}{{BField}}");
            var d = new FieldGeneratorKeyCalculator("DField", "CField", EKeyType.Rib);

            var fields =
                new IFieldGenerator[] { a, b, c, d }
                .SetCollateralDependencies();

            Assert.That(c.IsDependentUpon(a), Is.True); // Direct relation
            Assert.That(c.IsDependentUpon(b), Is.True); // Direct relation
            Assert.That(d.IsDependentUpon(c), Is.True); // Direct relation

            Assert.That(d.IsDependentUpon(b), Is.True); // Transitivity
            Assert.That(d.IsDependentUpon(a), Is.True); // Transitivity

            Assert.That(c.IsDependentUpon(d), Is.False); // Anti-symetry
        }

        [Test]
        public void OnlyBaseFieldGenerators()
        {
            var c = new FieldGeneratorRegex("CField", ".*");
            var b = new FieldGeneratorRegex("BField", ".*");
            var a = new FieldGeneratorRegex("AField", ".*");

            var fields =
                new[] { c, b, a }
                .SetCollateralDependencies()
                .OrderBy(x => x, new FieldGeneratorComparer());

            Assert.That(fields.ElementAt(0).Name, Is.EqualTo(a.Name));
            Assert.That(fields.ElementAt(1).Name, Is.EqualTo(b.Name));
            Assert.That(fields.ElementAt(2).Name, Is.EqualTo(c.Name));
        }

        [Test]
        public void GeneratorOrderingWithoutCascadingDependance()
        {
            var a = new FieldGeneratorComposite("AField", "CField", "{{CField}}");
            var b = new FieldGeneratorComposite("BField", "CField", "{{CField}}");
            var c = new FieldGeneratorRegex("CField", ".*");

            var fields =
                new IFieldGenerator[] { b, a, c }
                .SetCollateralDependencies()
                .OrderBy(x => x, new FieldGeneratorComparer())
                .ToList();

            Assert.That(fields.ElementAt(0).Name, Is.EqualTo(c.Name));
            Assert.That(fields.ElementAt(1).Name, Is.EqualTo(a.Name));
            Assert.That(fields.ElementAt(2).Name, Is.EqualTo(b.Name));
        }

        [Test]
        public void GeneratorOrderingWithCascadingDependances()
        {
            var a = new FieldGeneratorComposite("AField", "BField", string.Empty);
            var b = new FieldGeneratorComposite("BField", "CField", string.Empty);
            var c = new FieldGeneratorComposite("CField", "DField", string.Empty);
            var d = new FieldGeneratorRegex("DField", ".*");
            var ak = new FieldGeneratorKeyCalculator("AKField", "AField", EKeyType.Rlmc);

            var fields =
                new IFieldGenerator[] { ak, a, b, c, d }
                .SetCollateralDependencies()
                .OrderBy(x => x, new FieldGeneratorComparer()).ToArray()
                .ToList();

            Assert.That(fields.ElementAt(0).Name, Is.EqualTo(d.Name));
            Assert.That(fields.ElementAt(1).Name, Is.EqualTo(c.Name));
            Assert.That(fields.ElementAt(2).Name, Is.EqualTo(b.Name));
            Assert.That(fields.ElementAt(3).Name, Is.EqualTo(a.Name));
            Assert.That(fields.ElementAt(4).Name, Is.EqualTo(ak.Name));

            fields =
                new IFieldGenerator[] { d, c, b, a, ak }
                .SetCollateralDependencies()
                .OrderBy(x => x, new FieldGeneratorComparer()).ToArray()
                .ToList();

            Assert.That(fields.ElementAt(0).Name, Is.EqualTo(d.Name));
            Assert.That(fields.ElementAt(1).Name, Is.EqualTo(c.Name));
            Assert.That(fields.ElementAt(2).Name, Is.EqualTo(b.Name));
            Assert.That(fields.ElementAt(3).Name, Is.EqualTo(a.Name));
            Assert.That(fields.ElementAt(4).Name, Is.EqualTo(ak.Name));
        }
    }
}
