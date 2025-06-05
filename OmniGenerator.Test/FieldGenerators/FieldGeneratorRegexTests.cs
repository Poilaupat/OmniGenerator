using NUnit.Framework;
using OmniGenerator.Lib.Generators.Fields;
using System;
using System.Text.RegularExpressions;

namespace OmniGenerator.Test.FieldGenerators
{
    [TestFixture]
    public class FieldGeneratorRegexTests
    {
        [Test]
        public void GenerateValue_MatchesPattern()
        {
            // Arrange
            var pattern = @"[A-Z]{3}\d{2}";
            var generator = new FieldGeneratorRegex("TestField", pattern);

            // Act
            var value = generator.GenerateNextValue() as string;

            // Assert
            Assert.That(value, Is.Not.Null);
            Assert.That(Regex.IsMatch(value, pattern), Is.True);
        }

        [Test]
        public void GenerateValue_IsRandom()
        {
            // Arrange
            var pattern = @"[A-Z]{30}";
            var generator = new FieldGeneratorRegex("TestField", pattern);

            // Act
            var value1 = generator.GenerateNextValue() as string;
            var value2 = generator.GenerateNextValue() as string;

            // Assert
            Assert.That(value1, Is.Not.Null);
            Assert.That(value2, Is.Not.Null);
            Assert.That(value1, Is.Not.EqualTo(value2)); // This test is not totally deterministic, but should generally pass as the probability of generating the same value twice is low.
        }

        [Test]
        public void Constructor_InvalidPattern_Throws()
        {
            // Arrange
            var invalidPattern = @"[A-Z"; // Unclosed character class

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var generator = new FieldGeneratorRegex("TestField", invalidPattern);
                generator.GenerateNextValue();
            });
        }

        [Test]
        public void NameProperty_IsSet()
        {
            // Arrange
            var generator = new FieldGeneratorRegex("MyField", ".*");

            // Act & Assert
            Assert.That(generator.Name, Is.EqualTo("MyField"));
        }
    }
}
