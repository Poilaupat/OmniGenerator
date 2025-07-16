using NUnit.Framework;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Hierarchy;
using System.Collections.Generic;

namespace OmniGenerator.Test.Lib.FieldGenerators
{
    [TestFixture]
    public class FieldGeneratorCompositeTests
    {
        [Test]
        public void GenerateValue_SimpleTemplate_ReturnsExpectedString()
        {
            var generator = new FieldGeneratorComposite("FullName", "First,Last", "{{First}} {{Last}}");
            generator.GeneratorDependencies.Add(new FieldGeneratorConstant("First", "John"));
            generator.GeneratorDependencies.Add(new FieldGeneratorConstant("Last", "Doe"));
            generator.GeneratorDependencies.Add(new FieldGeneratorConstant("WhatEver", "WhatEver"));
            generator.GeneratorDependencies.ForEach(d => d.GenerateNextValue());

            var result = generator.GenerateNextValue();
            Assert.That(result, Is.EqualTo("John Doe"));
        }

        [Test]
        public void GenerateValue_ComplexTemplate_ReturnsExpectedString()
        {
            var generator = new FieldGeneratorComposite("Info", "First,Last,Age", "{{Last}}, {{First}} ({{Age}})");
            generator.GeneratorDependencies.Add(new FieldGeneratorConstant("First", "Jane"));
            generator.GeneratorDependencies.Add(new FieldGeneratorConstant("Last", "Smith"));
            generator.GeneratorDependencies.Add(new FieldGeneratorConstant("Age", "33"));
            generator.GeneratorDependencies.Add(new FieldGeneratorConstant("WhatEver", "WhatEver"));
            generator.GeneratorDependencies.ForEach(d => d.GenerateNextValue());

            var result = generator.GenerateNextValue();
            Assert.That(result, Is.EqualTo("Smith, Jane (33)"));
        }

        [Test]
        public void Properties_AreSetCorrectly()
        {
            var generator = new FieldGeneratorComposite("CompositeField", "A,B", "{{A}}-{{B}}");
            Assert.That(generator.Name, Is.EqualTo("CompositeField"));
            Assert.That(generator.Format, Is.EqualTo("{{A}}_{{B}}"));
        }
    }
}
