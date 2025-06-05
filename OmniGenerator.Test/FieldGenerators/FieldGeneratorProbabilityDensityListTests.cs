using NUnit.Framework;
using OmniGenerator.Lib.Generators.Fields;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmniGenerator.Test.FieldGenerators
{
    [TestFixture]
    public class FieldGeneratorProbabilityDensityListTests
    {
        [Test]
        public void GenerateValue_ReturnsElementFromList()
        {
            var items = new List<(string, double)>
            {
                ("A", 1.0),
                ("B", 2.0),
                ("C", 3.0)
            };
            var generator = new FieldGeneratorProbabilityDensityList("TestList", items, "dummy.txt");

            for (int i = 0; i < 20; i++)
            {
                var value = generator.GenerateNextValue();
                Assert.That(items.Select(x => x.Item1), Does.Contain(value));
            }
        }

        [Test]
        public void GenerateValue_EmptyList_ReturnsEmptyString()
        {
            var generator = new FieldGeneratorProbabilityDensityList("EmptyList", Array.Empty<(string, double)>(), "dummy.txt");
            var value = generator.GenerateNextValue();
            Assert.That(value, Is.EqualTo(string.Empty));
        }

        [Test]
        public void GenerateValue_ZeroTotalWeight_ReturnsEmptyString()
        {
            var items = new List<(string, double)>
            {
                ("A", 0.0),
                ("B", 0.0)
            };
            var generator = new FieldGeneratorProbabilityDensityList("ZeroWeight", items, "dummy.txt");
            var value = generator.GenerateNextValue();
            Assert.That(value, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Name_Property_IsSetCorrectly()
        {
            var generator = new FieldGeneratorProbabilityDensityList("MyList", new[] { ("X", 1.0) }, "dummy.txt");
            Assert.That(generator.Name, Is.EqualTo("MyList"));
        }

        [Test]
        public void GenerateValue_DistributionIsProportionalToWeights()
        {
            var items = new List<(string, double)>
            {
                ("A", 1.0),
                ("B", 3.0)
            };
            var generator = new FieldGeneratorProbabilityDensityList("WeightedList", items, "dummy.txt");
            var counts = items.ToDictionary(x => x.Item1, x => 0);
            int iterations = 8000;

            for (int i = 0; i < iterations; i++)
            {
                var value = generator.GenerateNextValue();
                counts[value]++;
            }

            double totalWeight = items.Sum(x => x.Item2);
            double expectedA = iterations * (items[0].Item2 / totalWeight);
            double expectedB = iterations * (items[1].Item2 / totalWeight);

            Assert.That(counts["A"], Is.InRange(expectedA * 0.9, expectedA * 1.1));
            Assert.That(counts["B"], Is.InRange(expectedB * 0.9, expectedB * 1.1));
        }
    }
}
