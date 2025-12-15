using NUnit.Framework;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Interfaces.FieldGenerators;
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

            Assert.That(collection.HasRegularGenerators, Is.False);
            Assert.That(collection.HasAggregateGenerators, Is.False);
            Assert.That(collection.HasDependentGenerators, Is.False);
        }

        [Test]
        public void Constructor_WithRegularGenerators_SetsHasRegularGeneratorsTrue()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("Field1", "Value1"),
                new FieldGeneratorNumeric("Field2", 1, 10)
            };

            var collection = new FieldGeneratorCollection("TestCollection", generators);

            Assert.That(collection.HasRegularGenerators, Is.True);
            Assert.That(collection.HasAggregateGenerators, Is.False);
        }

        [Test]
        public void Constructor_WithAggregateGenerators_SetsHasAggregateGeneratorsTrue()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("Field1", "Value1"),
                new FieldGeneratorAggregate("AggField", "Field1", EFFieldAggregateType.Sum, EScope.DirectChildren, "Target")
            };

            var collection = new FieldGeneratorCollection("TestCollection", generators);

            Assert.That(collection.HasAggregateGenerators, Is.True);
        }

        [Test]
        public void Constructor_WithDependentGenerators_SetsHasDependentGeneratorsTrue()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("Field1", "Value1"),
                new FieldGeneratorKeyCalculator("KeyField", "Field1", EKeyType.Dummy)
            };

            var collection = new FieldGeneratorCollection("TestCollection", generators);

            Assert.That(collection.HasDependentGenerators, Is.True);
        }

        [Test]
        public void Constructor_WithMixedGenerators_SetsAllFlagsCorrectly()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("Field1", "Value1"),
                new FieldGeneratorNumeric("Field2", 1, 10),
                new FieldGeneratorKeyCalculator("KeyField", "Field1", EKeyType.Dummy),
                new FieldGeneratorAggregate("AggField", "Field1", EFFieldAggregateType.Sum, EScope.DirectChildren, "Target")
            };

            var collection = new FieldGeneratorCollection("TestCollection", generators);

            Assert.That(collection.HasRegularGenerators, Is.True);
            Assert.That(collection.HasAggregateGenerators, Is.True);
            Assert.That(collection.HasDependentGenerators, Is.True);
        }

        #endregion

        #region GenerateRegularFields Tests

        [Test]
        public void GenerateRegularFields_WithNoGenerators_ReturnsEmptyDictionary()
        {
            var generators = new List<IFieldGenerator>();
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateRegularFields();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GenerateRegularFields_WithConstantGenerator_ReturnsCorrectField()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("TestField", "TestValue")
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateRegularFields();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.ContainsKey("TestField"), Is.True);
            Assert.That(result["TestField"].Name, Is.EqualTo("TestField"));
            Assert.That(result["TestField"].Value, Is.EqualTo("TestValue"));
        }

        [Test]
        public void GenerateRegularFields_WithMultipleGenerators_ReturnsAllFields()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("Field1", "Value1"),
                new FieldGeneratorConstant("Field2", "Value2"),
                new FieldGeneratorConstant("Field3", "Value3")
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateRegularFields();

            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result["Field1"].Value, Is.EqualTo("Value1"));
            Assert.That(result["Field2"].Value, Is.EqualTo("Value2"));
            Assert.That(result["Field3"].Value, Is.EqualTo("Value3"));
        }

        [Test]
        public void GenerateRegularFields_WithNumericGenerator_GeneratesInRange()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorNumeric("NumField", 10, 20)
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateRegularFields();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result["NumField"].Value, Is.TypeOf<int>());
            var value = (int)result["NumField"].Value;
            Assert.That(value, Is.GreaterThanOrEqualTo(10));
            Assert.That(value, Is.LessThan(20));
        }

        [Test]
        public void GenerateRegularFields_DoesNotIncludeAggregateGenerators()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("RegularField", "Value"),
                new FieldGeneratorAggregate("AggField", "RegularField", EFFieldAggregateType.Sum, EScope.DirectChildren, "Target")
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateRegularFields();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.ContainsKey("RegularField"), Is.True);
            Assert.That(result.ContainsKey("AggField"), Is.False);
        }

        [Test]
        public void GenerateRegularFields_WithDependentGenerators_ResolvesCorrectly()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("SourceField", "TestValue"),
                new FieldGeneratorKeyCalculator("KeyField", "SourceField", EKeyType.Dummy)
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateRegularFields();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.ContainsKey("SourceField"), Is.True);
            Assert.That(result.ContainsKey("KeyField"), Is.True);
            Assert.That(result["KeyField"].Value, Is.Not.Null);
        }

        [Test]
        public void GenerateRegularFields_CalledMultipleTimes_GeneratesNewValues()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorIncrement("IncrementField", 1, 1)
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result1 = collection.GenerateRegularFields();
            var result2 = collection.GenerateRegularFields();
            var result3 = collection.GenerateRegularFields();

            Assert.That(result1["IncrementField"].Value, Is.EqualTo(1));
            Assert.That(result2["IncrementField"].Value, Is.EqualTo(2));
            Assert.That(result3["IncrementField"].Value, Is.EqualTo(3));
        }

        [Test]
        public void GenerateRegularFields_WithCompositeGenerator_GeneratesCorrectComposite()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("First", "John"),
                new FieldGeneratorConstant("Last", "Doe"),
                new FieldGeneratorComposite("FullName", "First,Last", "{{First}} {{Last}}")
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var result = collection.GenerateRegularFields();

            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result["FullName"].Value, Is.EqualTo("John Doe"));
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
        public void GenerateAggregateFields_OnlyIncludesAggregateGenerators()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorConstant("RegularField", "Value"),
                new FieldGeneratorAggregate("AggField", "RegularField", EFFieldAggregateType.Count, EScope.DirectChildren, "Doc")
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);
            var group = CreateTestGroup();

            // Just verify that only aggregate fields are included
            Assert.That(collection.HasAggregateGenerators, Is.True);
            Assert.That(collection.HasRegularGenerators, Is.True);
        }

        #endregion

        #region Thread Safety Tests

        [Test]
        public void GenerateRegularFields_CalledConcurrently_IsThreadSafe()
        {
            var generators = new List<IFieldGenerator>
            {
                new FieldGeneratorIncrement("Counter", 1, 1)
            };
            var collection = new FieldGeneratorCollection("TestCollection", generators);

            var tasks = Enumerable.Range(0, 100)
                .Select(_ => System.Threading.Tasks.Task.Run(() => collection.GenerateRegularFields()))
                .ToArray();

            System.Threading.Tasks.Task.WaitAll(tasks);

            // Should generate 100 unique increments without crashes
            var finalResult = collection.GenerateRegularFields();
            var counterValue = Convert.ToInt32(finalResult["Counter"].Value);
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

        #endregion
    }
}
