using NUnit.Framework;
using OmniGenerator.Lib.Generators.Fields;
using System;

namespace OmniGenerator.Test.Lib.FieldGenerators
{
    [TestFixture]
    public class FieldGeneratorDateTests
    {
        [Test]
        public void GenerateValue_ReturnsDateWithinRange_PositiveBounds()
        {
            var min = 1;
            var max = 5;
            var generator = new FieldGeneratorDate("TestDate", min, max);
            var today = DateTime.Today;

            for (int i = 0; i < 20; i++)
            {
                var value = generator.GenerateNextValue();
                var diff = (value - today).Days;
                Assert.That(diff, Is.GreaterThanOrEqualTo(min));
                Assert.That(diff, Is.LessThanOrEqualTo(max));
            }
        }

        [Test]
        public void GenerateValue_ReturnsDateWithinRange_NegativeBounds()
        {
            var min = -4;
            var max = -2;
            var generator = new FieldGeneratorDate("TestDate", min, max);
            var today = DateTime.Today;

            for (int i = 0; i < 20; i++)
            {
                var value = generator.GenerateNextValue();
                var diff = (value - today).Days;
                Assert.That(diff, Is.GreaterThanOrEqualTo(min));
                Assert.That(diff, Is.LessThanOrEqualTo(max));
            }
        }

        [Test]
        public void Constructor_SwapsBoundsIfMinGreaterThanMax()
        {
            var generator = new FieldGeneratorDate("TestDate", 10, 2);
            Assert.That(generator.DayDiffMin, Is.EqualTo(2));
            Assert.That(generator.DayDiffMax, Is.EqualTo(11));
        }

        [Test]
        public void Name_Property_IsSetCorrectly()
        {
            var generator = new FieldGeneratorDate("MyDate", 0, 1);
            Assert.That(generator.Name, Is.EqualTo("MyDate"));
        }
    }
}
