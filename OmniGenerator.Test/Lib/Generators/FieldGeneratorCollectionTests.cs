using NUnit.Framework;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Generators.Interfaces;
using OmniGenerator.Lib.Hierarchy;
using System.Collections.Generic;
using System.Linq;

namespace OmniGenerator.Test.Lib.Generators
{
    [TestFixture]
    public class FieldGeneratorCollectionTests
    {
        #region Constructor Tests

        [Test]
        public void Constructor_WithName_SetsNameProperty()
        {
            var generators = new List<IFieldGenerator>();
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            Assert.That(collection.Name, Is.EqualTo("TestCollection"));
        }

        [Test]
        public void Constructor_WithEmptyList_InitializesCorrectly()
        {
            var generators = new List<IFieldGenerator>();
            var collection = new FieldGeneratorCollection("Empty", generators);

            // Collection initializes without error
            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Name, Is.EqualTo("Empty"));
        }

        [Test]
        public void Constructor_WithRegularGenerators_InitializesCorrectly()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("Field1", "Value1"),
                new FieldGeneratorNumeric("Field2", 1, 10)
            };

            var collection = new FieldGeneratorCollection("TestCollection", generators);

            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Name, Is.EqualTo("TestCollection"));
        }

        [Test]
        public void Constructor_WithAggregateGenerators_InitializesCorrectly()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("Field1", "Value1"),
                new FieldGeneratorAggregate("AggField", "Field1", EFFieldAggregateType.Sum, EScope.DirectChildren, "Target")
            };

            var collection = new FieldGeneratorCollection("TestCollection", generators);

            Assert.That(collection, Is.Not.Null);
        }

        [Test]
        public void Constructor_WithDependentGenerators_InitializesCorrectly()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("Field1", "Value1"),
                new FieldGeneratorKeyCalculator("KeyField", "Field1", EKeyType.Dummy)
            };

            var collection = new FieldGeneratorCollection("TestCollection", generators);

            Assert.That(collection, Is.Not.Null);
        }

        [Test]
        public void Constructor_WithMixedGenerators_InitializesCorrectly()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("Field1", "Value1"),
                new FieldGeneratorNumeric("Field2", 1, 10),
                new FieldGeneratorKeyCalculator("KeyField", "Field1", EKeyType.Dummy),
                new FieldGeneratorAggregate("AggField", "Field1", EFFieldAggregateType.Sum, EScope.DirectChildren, "Target")
            };

            var collection = new FieldGeneratorCollection("TestCollection", generators);

            Assert.That(collection, Is.Not.Null);
        }

        #endregion

        #region GenerateFields Tests

        [Test]
        public void GenerateFields_WithNoGenerators_ReturnsEmptyDictionary()
        {
            var generators = new List<IFieldGenerator>();
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateFields();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GenerateFields_WithConstantGenerator_ReturnsCorrectField()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("TestField", "TestValue")
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateFields();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.ContainsKey("TestField"), Is.True);
            Assert.That(result["TestField"].Name, Is.EqualTo("TestField"));
            Assert.That(result["TestField"].StringValue, Is.EqualTo("TestValue"));
        }

        [Test]
        public void GenerateFields_WithMultipleGenerators_ReturnsAllFields()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("Field1", "Value1"),
                new FieldGeneratorConstant("Field2", "Value2"),
                new FieldGeneratorConstant("Field3", "Value3")
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateFields();

            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result["Field1"].StringValue, Is.EqualTo("Value1"));
            Assert.That(result["Field2"].StringValue, Is.EqualTo("Value2"));
            Assert.That(result["Field3"].StringValue, Is.EqualTo("Value3"));
        }

        [Test]
        public void GenerateFields_WithNumericGenerator_GeneratesInRange()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorNumeric("NumField", 10, 20)
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateFields();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result["NumField"].Value.ValueType, Is.EqualTo(typeof(int)));
            var value = result["NumField"].Value.Convert<int>();
            Assert.That(value, Is.GreaterThanOrEqualTo(10));
            Assert.That(value, Is.LessThan(20));
        }

        [Test]
        public void GenerateFields_DoesNotIncludeAggregateGenerators()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("RegularField", "Value"),
                new FieldGeneratorAggregate("AggField", "RegularField", EFFieldAggregateType.Sum, EScope.DirectChildren, "Target")
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateFields();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.ContainsKey("RegularField"), Is.True);
            Assert.That(result.ContainsKey("AggField"), Is.False);
        }

        [Test]
        public void GenerateFields_WithDependentGenerators_ResolvesCorrectly()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("SourceField", "TestValue"),
                new FieldGeneratorKeyCalculator("KeyField", "SourceField", EKeyType.Dummy)
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateFields();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.ContainsKey("SourceField"), Is.True);
            Assert.That(result.ContainsKey("KeyField"), Is.True);
            Assert.That(result["KeyField"].Value.RawValue, Is.Not.Null);
        }

        [Test]
        public void GenerateFields_CalledMultipleTimes_GeneratesNewValues()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorIncrement("IncrementField", 1, 1)
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result1 = collection.GenerateFields();
            var result2 = collection.GenerateFields();
            var result3 = collection.GenerateFields();

            Assert.That(result1["IncrementField"].Value.Convert<int>(), Is.EqualTo(1));
            Assert.That(result2["IncrementField"].Value.Convert<int>(), Is.EqualTo(2));
            Assert.That(result3["IncrementField"].Value.Convert<int>(), Is.EqualTo(3));
        }

        [Test]
        public void GenerateFields_WithCompositeGenerator_GeneratesCorrectComposite()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("First", "John"),
                new FieldGeneratorConstant("Last", "Doe"),
                new FieldGeneratorComposite("FullName", "First,Last", "{{First}} {{Last}}")
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateFields();

            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result["FullName"].StringValue, Is.EqualTo("John Doe"));
        }

        [Test]
        public void GenerateFields_WithTopologicalDependencies_ResolvesInCorrectOrder()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("A", "ValueA"),
                new FieldGeneratorKeyCalculator("B", "A", EKeyType.Dummy),
                new FieldGeneratorComposite("C", "A,B", "{{A}}+{{B}}")
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateFields();

            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result["A"].StringValue, Is.EqualTo("ValueA"));
            Assert.That(result["B"].Value.RawValue, Is.Not.Null);
            Assert.That(result["C"].StringValue, Is.EqualTo("ValueA+ValueA"));
        }

        #endregion

        #region GenerateAggregateFields Tests

        [Test]
        public void GenerateAggregateFields_WithNoAggregateGenerators_ReturnsEmptyDictionary()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("RegularField", "Value")
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);
            var group = CreateTestGroup();

            var result = collection.GenerateAggregateFields(group);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GenerateAggregateFields_WithAggregateGenerator_ReturnsAggregateField()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("RegularField", "Value"),
                new FieldGeneratorAggregate("AggField", "", EFFieldAggregateType.Count, EScope.DirectChildren, "TestDoc")
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);
            var group = CreateTestGroupWithDocuments();

            var result = collection.GenerateAggregateFields(group);

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.ContainsKey("AggField"), Is.True);
        }

        [Test]
        public void GenerateAggregateFields_OnlyIncludesAggregateGenerators()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("RegularField", "Value"),
                new FieldGeneratorAggregate("AggField", "", EFFieldAggregateType.Count, EScope.DirectChildren, "TestDoc")
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);
            var group = CreateTestGroupWithDocuments();

            var result = collection.GenerateAggregateFields(group);

            Assert.That(result.ContainsKey("RegularField"), Is.False);
            Assert.That(result.ContainsKey("AggField"), Is.True);
        }

        #endregion

        #region Thread Safety Tests

        [Test]
        public void GenerateFields_CalledConcurrently_IsThreadSafe()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorIncrement("Counter", 1, 1)
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var tasks = Enumerable.Range(0, 100)
                .Select(_ => System.Threading.Tasks.Task.Run(() => collection.GenerateFields()))
                .ToArray();

            System.Threading.Tasks.Task.WaitAll(tasks);

            // Should generate 100 unique increments without crashes
            var finalResult = collection.GenerateFields();
            var counterValue = finalResult["Counter"].Value.Convert<int>();
            Assert.That(counterValue, Is.EqualTo(101));
        }

        #endregion

        #region Helper Methods

        private Group CreateTestGroup()
        {
            return new Group(
                "TestGroup",
                System.Array.Empty<Group>(),
                System.Array.Empty<Document>());
        }

        private Group CreateTestGroupWithDocuments()
        {
            var doc1 = new Document("TestDoc", null);
            var doc2 = new Document("TestDoc", null);
            return new Group(
                "TestGroup",
                System.Array.Empty<Group>(),
                new[] { doc1, doc2 });
        }

        #endregion
    }
}
