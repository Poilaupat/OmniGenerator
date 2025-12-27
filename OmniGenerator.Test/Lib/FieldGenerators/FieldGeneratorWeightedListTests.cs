using NUnit.Framework;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmniGenerator.Test.Lib.FieldGenerators
{
    [TestFixture]
    public class FieldGeneratorWeightedListTests
    {
        [Test]
        public void GenerateValue_ReturnsElementFromList()
        {
            var items = new[] { new WeightedValue("A", 1.0), new WeightedValue("B", 1.0), new WeightedValue("C", 1.0) };
            var values = items.Select(i => i.Value).ToArray();
            var generator = new FieldGeneratorWeightedList("TestList", items);

            for (int i = 0; i < 20; i++)
            {
                var value = generator.GenerateNextValue();
                Assert.That(values, Does.Contain(value));
            }
        }

        [Test]
        public void GenerateValue_EmptyList_ReturnsEmptyString()
        {
            var generator = new FieldGeneratorWeightedList("EmptyList", Array.Empty<WeightedValue>());
            var value = generator.GenerateNextValue();
            Assert.That(value, Is.EqualTo(string.Empty));
        }

        [Test]
        public void GenerateValue_ZeroTotalWeight_ReturnsEmptyString()
        {
            var items = new[] { new WeightedValue("A", 0.0), new WeightedValue("B", 0.0), new WeightedValue("C", 0.0) };

            var generator = new FieldGeneratorWeightedList("ZeroWeight", items);
            var value = generator.GenerateNextValue();
            Assert.That(value, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Name_Property_IsSetCorrectly()
        {
            var items = new[] { new WeightedValue("X", 1.0) };
            var generator = new FieldGeneratorWeightedList("MyList", items);
            Assert.That(generator.Name, Is.EqualTo("MyList"));
        }

        [Test]
        public void GenerateValue_DistributionIsUniform()
        {
            var items = new[] { new WeightedValue("A", 1.0), new WeightedValue("B", 1.0), new WeightedValue("C", 1.0) };
            var generator = new FieldGeneratorWeightedList("UniformList", items);
            var counts = items.ToDictionary(x => x.Value, x => 0);
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

        [Test]
        public void GenerateValue_DistributionIsProportionalToWeights()
        {
            var items = new[] { new WeightedValue("A", 1.0), new WeightedValue("B", 3.0) };

            var generator = new FieldGeneratorWeightedList("WeightedList", items);
            var counts = items.ToDictionary(x => x.Value, x => 0);
            int iterations = 8000;

            for (int i = 0; i < iterations; i++)
            {
                var value = generator.GenerateNextValue();
                counts[value]++;
            }

            double totalWeight = items.Sum(x => x.Weight);
            double expectedA = iterations * (items[0].Weight / totalWeight);
            double expectedB = iterations * (items[1].Weight / totalWeight);

            Assert.That(counts["A"], Is.InRange(expectedA * 0.9, expectedA * 1.1));
            Assert.That(counts["B"], Is.InRange(expectedB * 0.9, expectedB * 1.1));
        }
    }
}
