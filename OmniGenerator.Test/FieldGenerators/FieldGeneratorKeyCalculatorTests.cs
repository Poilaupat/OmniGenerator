using NUnit.Framework;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Tools;
using System;
using System.Collections.Generic;

namespace OmniGenerator.Test.FieldGenerators
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
            var expected = KeyTools.ComputeDummyKey(input);
            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GenerateValue_RlmcKeyType_CallsComputeRlmcKey()
        {
            var input = "1234567";
            var gen = CreateWithDependency(EKeyType.Rlmc, input);
            var expected = KeyTools.ComputeRlmcKey(input);
            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GenerateValue_RibKeyType_CallsComputeRibKey()
        {
            var input = "1234567";
            var gen = CreateWithDependency(EKeyType.Rib, input);
            var expected = KeyTools.ComputeRibKey(input);
            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GenerateValue_TipKeyType_CallsComputeTipKey()
        {
            var input = "1234567";
            var gen = CreateWithDependency(EKeyType.Tip, input);
            var expected = KeyTools.ComputeTipKey(input);
            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GenerateValue_TipGroup6KeyType_CallsComputeTipGroup6Key()
        {
            var input = "1234567";
            var gen = CreateWithDependency(EKeyType.TipGroup6, input);
            var expected = KeyTools.ComputeTipGroup6Key(input);
            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo(expected));
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
