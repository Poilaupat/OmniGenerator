using NUnit.Framework;
using OmniGenerator.Lib.Generators.Fields;
using System;
using System.Linq;
using System.Collections.Generic;

namespace OmniGenerator.Test.Lib.FieldGenerators
{
    [TestFixture]
    public class FieldGeneratorNumericTests
    {
        [Test]
        public void GenerateValue_ReturnsValueWithinRange()
        {
            var min = 5;
            var max = 10;
            var generator = new FieldGeneratorNumeric("Num", min, max);

            for (int i = 0; i < 100; i++)
            {
                var value = generator.GenerateNextValue();
                Assert.That(value, Is.GreaterThanOrEqualTo(min));
                Assert.That(value, Is.LessThan(max));
            }
        }

        [Test]
        public void GenerateValue_DistributionIsRoughlyUniform()
        {
            var min = 1;
            var max = 4;
            var generator = new FieldGeneratorNumeric("Num", min, max);
            var counts = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 } };
            int iterations = 6000;

            for (int i = 0; i < iterations; i++)
            {
                var value = generator.GenerateNextValue();
                counts[value]++;
            }

            double expected = iterations / (double)counts.Count;
            foreach (var count in counts.Values)
            {
                Assert.That(count, Is.InRange(expected * 0.9, expected * 1.1));
            }
        }

        [Test]
        public void Name_Property_IsSetCorrectly()
        {
            var generator = new FieldGeneratorNumeric("MyNum", 0, 1);
            Assert.That(generator.Name, Is.EqualTo("MyNum"));
        }

        [Test]
        public void Min_And_Max_Properties_AreSetCorrectly()
        {
            var generator = new FieldGeneratorNumeric("Num", 2, 7);
            Assert.That(generator.Min, Is.EqualTo(2));
            Assert.That(generator.Max, Is.EqualTo(7));
        }
    }
}
