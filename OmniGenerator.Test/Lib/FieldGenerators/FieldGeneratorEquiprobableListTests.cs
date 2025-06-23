using NUnit.Framework;
using OmniGenerator.Lib.Generators.Fields;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmniGenerator.Test.Lib.FieldGenerators
{
    [TestFixture]
    public class FieldGeneratorEquiprobableListTests
    {
        [Test]
        public void GenerateValue_ReturnsElementFromList()
        {
            var items = new[] { "A", "B", "C" };
            var generator = new FieldGeneratorEquiprobableList("TestList", items, "dummy.txt");

            for (int i = 0; i < 20; i++)
            {
                var value = generator.GenerateNextValue();
                Assert.That(items, Does.Contain(value));
            }
        }

        [Test]
        public void GenerateValue_EmptyList_ReturnsEmptyString()
        {
            var generator = new FieldGeneratorEquiprobableList("EmptyList", Array.Empty<string>(), "dummy.txt");
            var value = generator.GenerateNextValue();
            Assert.That(value, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Name_Property_IsSetCorrectly()
        {
            var generator = new FieldGeneratorEquiprobableList("MyList", new[] { "X" }, "dummy.txt");
            Assert.That(generator.Name, Is.EqualTo("MyList"));
        }

        [Test]
        public void GenerateValue_DistributionIsUniform()
        {
            var items = new[] { "A", "B", "C" };
            var generator = new FieldGeneratorEquiprobableList("UniformList", items, "dummy.txt");
            var counts = items.ToDictionary(x => x, x => 0);
            int iterations = 10000;

            for (int i = 0; i < iterations; i++)
            {
                var value = generator.GenerateNextValue();
                counts[value]++;
            }

            double expected = iterations / (double)items.Length;
            foreach (var count in counts.Values)
            {
                // Allow a 5% margin
                Assert.That(count, Is.InRange(expected * 0.95, expected * 1.05));
            }
        }
    }
}
