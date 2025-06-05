using NUnit.Framework;
using OmniGenerator.Lib.Generators.Fields;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace OmniGenerator.Test.FieldGenerators
{
    [TestFixture]
    public class FieldGeneratorIncrementTests
    {
        [Test]
        public void GenerateValue_Sequentially_IncrementsCorrectly()
        {
            var generator = new FieldGeneratorIncrement("Inc", 10, 2);
            Assert.That(generator.GenerateNextValue(), Is.EqualTo(10));
            Assert.That(generator.GenerateNextValue(), Is.EqualTo(12));
            Assert.That(generator.GenerateNextValue(), Is.EqualTo(14));
        }

        [Test]
        public void GenerateValue_NegativeIncrement_DecrementsCorrectly()
        {
            var generator = new FieldGeneratorIncrement("Dec", 5, -3);
            Assert.That(generator.GenerateNextValue(), Is.EqualTo(5));
            Assert.That(generator.GenerateNextValue(), Is.EqualTo(2));
            Assert.That(generator.GenerateNextValue(), Is.EqualTo(-1));
        }

        [Test]
        public void Name_Property_IsSetCorrectly()
        {
            var generator = new FieldGeneratorIncrement("MyInc", 0, 1);
            Assert.That(generator.Name, Is.EqualTo("MyInc"));
        }
    }
}
