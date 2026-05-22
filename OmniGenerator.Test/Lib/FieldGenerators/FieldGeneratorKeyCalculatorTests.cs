using NUnit.Framework;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Tools;
using System;
using System.Collections.Generic;

namespace OmniGenerator.Test.Lib.FieldGenerators
{
    [TestFixture]
    public class FieldGeneratorKeyCalculatorTests
    {
        private FieldGeneratorKeyCalculator CreateWithDependency(EKeyType keyType, string value)
        {
            var generator = new FieldGeneratorKeyCalculator("KeyField", "InputField", keyType);
            var dependency = new FieldGeneratorConstant("InputField", value);
            dependency.GenerateNextValue();
            generator.GeneratorDependencies.Add(dependency);
            return generator;
        }

        [Test]
        public void GenerateValue_DummyKeyType_CallsComputeDummyKey()
        {
            var input = "abc";
            var gen = CreateWithDependency(EKeyType.Dummy, input);
            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo(input));
        }

        [Test]
        public void GenerateValue_RlmcKeyType_WithExpectedValue()
        {
            var input = "2459163 075005135908 204090215504";
            var gen = CreateWithDependency(EKeyType.Rlmc, input);
            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo("11"));
        }

        [Test]
        public void GenerateValue_RibKeyType_WithExpectedValue()
        {
            var input = "123456X7890";
            var gen = CreateWithDependency(EKeyType.Rib, input);
            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo("60"));
        }

        [Test]
        public void GenerateValue_TipKeyType_WithExpectedValue()
        {
            var input = "0630000004";
            var gen = CreateWithDependency(EKeyType.Tip, input);
            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo("82"));
        }

        [Test]
        public void GenerateValue_TipGroup6KeyType_WithExpectedValue()
        {
            var input = "123456789";
            var gen = CreateWithDependency(EKeyType.TipGroup6, input);
            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo("6"));
        }

        [Test]
        public void GenerateValue_IcsKeyType_WithExpectedValue()
        {
            var input = "BCD007008";
            var gen = CreateWithDependency(EKeyType.Ics, input);
            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo("54"));
        }

        [Test]
        public void GenerateValue_IbanKeyType_WithExpectedValue()
        {
            var input = "30003732513751873529738FR";
            var gen = CreateWithDependency(EKeyType.Iban, input);
            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo("76"));
        }

        [Test]
        public void GenerateValue_UnsupportedKeyType_ThrowsException()
        {
            var input = "unsupported";
            var gen = CreateWithDependency((EKeyType)999, input);
            Assert.Throws<Exception>(() => gen.GenerateNextValue());
        }

        [Test]
        public void Name_And_KeyType_AreSetCorrectly()
        {
            var gen = new FieldGeneratorKeyCalculator("MyKey", "Dep", EKeyType.Dummy);
            Assert.That(gen.Name, Is.EqualTo("MyKey"));
            Assert.That(gen.KeyType, Is.EqualTo(EKeyType.Dummy));
        }
    }
}
