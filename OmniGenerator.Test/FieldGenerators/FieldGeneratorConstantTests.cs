using NUnit.Framework;
using OmniGenerator.Lib.Generators.Fields;

namespace OmniGenerator.Test.FieldGenerators
{
    [TestFixture]
    public class FieldGeneratorConstantTests
    {
        [Test]
        public void GenerateValue_ReturnsConstantValue()
        {
            var generator = new FieldGeneratorConstant("TestField", "ABC123");
            var value = generator.GenerateNextValue();
            Assert.That(value, Is.EqualTo("ABC123"));
        }

        [Test]
        public void Name_Property_IsSetCorrectly()
        {
            var generator = new FieldGeneratorConstant("MyField", "Value");
            Assert.That(generator.Name, Is.EqualTo("MyField"));
        }
    }
}
