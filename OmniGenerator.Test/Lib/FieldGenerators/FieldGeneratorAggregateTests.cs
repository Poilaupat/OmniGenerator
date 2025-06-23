using NUnit.Framework;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmniGenerator.Test.Lib.FieldGenerators
{
    [TestFixture]
    public class FieldGeneratorAggregateTests
    {
        Group _root = new Group(
            "root",
            [
                new Group(
                    "subgroup",
                    Array.Empty<Group>(),
                    [
                        new Document("Doc", null, new Dictionary<string, Field> { { "Amount", new Field("Amount", 10) } }),
                        new Document("Doc", null, new Dictionary<string, Field> { { "Amount", new Field("Amount", 20) } }),
                        new Document("Other", null, new Dictionary<string, Field> { { "Amount", new Field("Amount", 99) } }),
                    ])
            ],
            [
                new Document("Doc", null, new Dictionary<string, Field> { { "Amount", new Field("Amount", 30) } }),
                new Document("Doc", null, new Dictionary<string, Field> { { "Amount", new Field("Amount", 40) } }),
                new Document("Other", null, new Dictionary<string, Field> { { "Amount", new Field("Amount", 999) } }),
            ]);


        [Test]
        public void GenerateValue_CountAggregate_DirectChildren_ReturnsCorrectCount()
        {
            var generator = new FieldGeneratorAggregate("CountField", "", EFFieldAggregateType.Count, EScope.DirectChildren, "Doc")
            {
                Group = _root
            };

            var result = generator.GenerateNextValue();
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void GenerateValue_CountAggregate_Overall_ReturnsCorrectCount()
        {
            var generator = new FieldGeneratorAggregate("CountField", "", EFFieldAggregateType.Count, EScope.Overall, "Doc")
            {
                Group = _root
            };

            var result = generator.GenerateNextValue();
            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        public void GenerateValue_SumAggregate_DirectChildren_ReturnsCorrectSum()
        {
            var gen = new FieldGeneratorAggregate("SumField", "Amount", EFFieldAggregateType.Sum, EScope.DirectChildren, "Doc")
            {
                Group = _root
            };

            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo(70));
        }

        [Test]
        public void GenerateValue_SumAggregate_Overall_ReturnsCorrectSum()
        {
            var gen = new FieldGeneratorAggregate("SumField", "Amount", EFFieldAggregateType.Sum, EScope.Overall, "Doc")
            {
                Group = _root
            };

            var result = gen.GenerateNextValue();
            Assert.That(result, Is.EqualTo(100));
        }

        [Test]
        public void GenerateValue_ThrowsIfGroupNotSet()
        {
            var gen = new FieldGeneratorAggregate("Field", "", EFFieldAggregateType.Count, EScope.DirectChildren, "Doc");
            Assert.Throws<NullReferenceException>(() => gen.GenerateNextValue());
        }
    }
}
