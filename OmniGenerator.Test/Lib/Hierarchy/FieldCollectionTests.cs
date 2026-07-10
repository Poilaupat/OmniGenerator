using NUnit.Framework;
using OmniGenerator.Lib.Hierarchy;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace OmniGenerator.Test.Lib.Hierarchy
{
    [TestFixture]
    public class FieldCollectionTests
    {
        #region Basic Operations Tests

        [Test]
        public void Constructor_CreatesEmptyCollection()
        {
            // Arrange & Act
            var collection = new FieldCollection();

            // Assert
            Assert.That(collection.Count, Is.EqualTo(0));
            Assert.That(collection.Keys, Is.Empty);
            Assert.That(collection.Values, Is.Empty);
        }

        [Test]
        public void Add_SingleField_AddsFieldToCollection()
        {
            // Arrange
            var collection = new FieldCollection();
            var field = new Field("TestField", "TestValue");

            // Act
            collection.Add(field);

            // Assert
            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection.ContainsKey("TestField"), Is.True);
            Assert.That(collection["TestField"].Value, Is.EqualTo("TestValue"));
        }

        [Test]
        public void Add_DuplicateField_PreservesFirstField()
        {
            // Arrange
            var collection = new FieldCollection();
            var field1 = new Field("TestField", "FirstValue");
            var field2 = new Field("TestField", "SecondValue");

            // Act
            collection.Add(field1);
            collection.Add(field2);

            // Assert
            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection["TestField"].Value, Is.EqualTo("FirstValue"));
        }

        [Test]
        public void Indexer_ExistingField_ReturnsField()
        {
            // Arrange
            var collection = new FieldCollection();
            var field = new Field("TestField", 42);
            collection.Add(field);

            // Act
            var result = collection["TestField"];

            // Assert
            Assert.That(result.Name, Is.EqualTo("TestField"));
            Assert.That(result.Value, Is.EqualTo(42));
        }

        [Test]
        public void Indexer_NonExistingField_ThrowsKeyNotFoundException()
        {
            // Arrange
            var collection = new FieldCollection();

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => { var _ = collection["NonExistent"]; });
        }

        [Test]
        public void ContainsKey_ExistingField_ReturnsTrue()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("TestField", "Value"));

            // Act
            var result = collection.ContainsKey("TestField");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ContainsKey_NonExistingField_ReturnsFalse()
        {
            // Arrange
            var collection = new FieldCollection();

            // Act
            var result = collection.ContainsKey("NonExistent");

            // Assert
            Assert.That(result, Is.False);
        }

        #endregion

        #region TryGetValue Tests

        [Test]
        public void TryGetValue_ExistingField_ReturnsTrueAndField()
        {
            // Arrange
            var collection = new FieldCollection();
            var field = new Field("TestField", "TestValue");
            collection.Add(field);

            // Act
            var result = collection.TryGetValue("TestField", out var retrievedField);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(retrievedField, Is.Not.Null);
            Assert.That(retrievedField!.Name, Is.EqualTo("TestField"));
            Assert.That(retrievedField.Value, Is.EqualTo("TestValue"));
        }

        [Test]
        public void TryGetValue_NonExistingField_ReturnsFalse()
        {
            // Arrange
            var collection = new FieldCollection();

            // Act
            var result = collection.TryGetValue("NonExistent", out var retrievedField);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(retrievedField, Is.Null);
        }

        #endregion

        #region TryGetStringValue Tests

        [Test]
        public void TryGetStringValue_ExistingFieldWithStringValue_ReturnsTrueAndStringValue()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("TestField", "TestValue"));

            // Act
            var result = collection.TryGetStringValue("TestField", out var stringValue);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(stringValue, Is.EqualTo("TestValue"));
        }

        [Test]
        public void TryGetStringValue_ExistingFieldWithNumericValue_ReturnsTrueAndConvertedString()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("NumericField", 42));

            // Act
            var result = collection.TryGetStringValue("NumericField", out var stringValue);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(stringValue, Is.EqualTo("42"));
        }

        [Test]
        public void TryGetStringValue_ExistingFieldWithNullValue_ReturnsTrueAndEmptyString()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("NullField", null));

            // Act
            var result = collection.TryGetStringValue("NullField", out var stringValue);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(stringValue, Is.EqualTo(string.Empty));
        }

        [Test]
        public void TryGetStringValue_NonExistingField_ReturnsFalseAndNull()
        {
            // Arrange
            var collection = new FieldCollection();

            // Act
            var result = collection.TryGetStringValue("NonExistent", out var stringValue);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(stringValue, Is.Null);
        }

        #endregion

        #region GetValue and GetStringValue Tests

        [Test]
        public void GetValue_ExistingField_ReturnsField()
        {
            // Arrange
            var collection = new FieldCollection();
            var field = new Field("TestField", "TestValue");
            collection.Add(field);

            // Act
            var result = collection.GetValue("TestField");

            // Assert
            Assert.That(result.Name, Is.EqualTo("TestField"));
            Assert.That(result.Value, Is.EqualTo("TestValue"));
        }

        [Test]
        public void GetValue_NonExistingField_ThrowsFieldNotFoundException()
        {
            // Arrange
            var collection = new FieldCollection();

            // Act & Assert
            var ex = Assert.Throws<OmniGenerator.Lib.Exceptions.FieldNotFoundException>(() =>
                collection.GetValue("NonExistent"));
            Assert.That(ex.Message, Does.Contain("NonExistent"));
        }

        [Test]
        public void GetStringValue_ExistingField_ReturnsStringValue()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("TestField", "TestValue"));

            // Act
            var result = collection.GetStringValue("TestField");

            // Assert
            Assert.That(result, Is.EqualTo("TestValue"));
        }

        [Test]
        public void GetStringValue_ExistingFieldWithNumericValue_ReturnsConvertedString()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("NumericField", 42));

            // Act
            var result = collection.GetStringValue("NumericField");

            // Assert
            Assert.That(result, Is.EqualTo("42"));
        }

        [Test]
        public void GetStringValue_NonExistingField_ThrowsFieldNotFoundException()
        {
            // Arrange
            var collection = new FieldCollection();

            // Act & Assert
            var ex = Assert.Throws<OmniGenerator.Lib.Exceptions.FieldNotFoundException>(() =>
                collection.GetStringValue("NonExistent"));
            Assert.That(ex.Message, Does.Contain("NonExistent"));
        }

        [Test]
        public void GetStringValue_FieldWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("NullField", null));

            // Act
            var result = collection.GetStringValue("NullField");

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        #endregion

        #region GetStringValueOrDefault Tests

        [Test]
        public void GetStringValueOrDefault_ExistingField_ReturnsStringValue()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("TestField", "TestValue"));

            // Act
            var result = collection.GetStringValueOrDefault("TestField", "DefaultValue");

            // Assert
            Assert.That(result, Is.EqualTo("TestValue"));
        }

        [Test]
        public void GetStringValueOrDefault_NonExistingField_ReturnsDefaultValue()
        {
            // Arrange
            var collection = new FieldCollection();

            // Act
            var result = collection.GetStringValueOrDefault("NonExistent", "DefaultValue");

            // Assert
            Assert.That(result, Is.EqualTo("DefaultValue"));
        }

        [Test]
        public void GetStringValueOrDefault_FieldWithNullValue_ReturnsDefaultValue()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("NullField", null));

            // Act
            var result = collection.GetStringValueOrDefault("NullField", "DefaultValue");

            // Assert
            // Field.StringValue returns string.Empty for null values, not null
            Assert.That(result, Is.EqualTo("DefaultValue"));
        }

        [Test]
        public void GetStringValueOrDefault_NumericField_ReturnsConvertedString()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("NumericField", 123));

            // Act
            var result = collection.GetStringValueOrDefault("NumericField", "DefaultValue");

            // Assert
            Assert.That(result, Is.EqualTo("123"));
        }

        #endregion

        #region AddRange Tests

        [Test]
        public void AddRange_FieldCollection_AddsAllFields()
        {
            // Arrange
            var collection1 = new FieldCollection();
            collection1.Add(new Field("Field1", "Value1"));
            collection1.Add(new Field("Field2", "Value2"));

            var collection2 = new FieldCollection();
            collection2.Add(new Field("Field3", "Value3"));

            // Act
            collection2.AddRange(collection1);

            // Assert
            Assert.That(collection2.Count, Is.EqualTo(3));
            Assert.That(collection2.ContainsKey("Field1"), Is.True);
            Assert.That(collection2.ContainsKey("Field2"), Is.True);
            Assert.That(collection2.ContainsKey("Field3"), Is.True);
        }

        [Test]
        public void AddRange_FieldCollection_PreservesExistingFields()
        {
            // Arrange
            var collection1 = new FieldCollection();
            collection1.Add(new Field("Field1", "OriginalValue"));

            var collection2 = new FieldCollection();
            collection2.Add(new Field("Field1", "NewValue"));
            collection2.Add(new Field("Field2", "Value2"));

            // Act
            collection1.AddRange(collection2);

            // Assert
            Assert.That(collection1.Count, Is.EqualTo(2));
            Assert.That(collection1["Field1"].Value, Is.EqualTo("OriginalValue"));
            Assert.That(collection1["Field2"].Value, Is.EqualTo("Value2"));
        }

        [Test]
        public void AddRange_IEnumerable_AddsAllFields()
        {
            // Arrange
            var collection = new FieldCollection();
            var fields = new List<Field>
            {
                new Field("Field1", "Value1"),
                new Field("Field2", "Value2"),
                new Field("Field3", "Value3")
            };

            // Act
            collection.AddRange(fields);

            // Assert
            Assert.That(collection.Count, Is.EqualTo(3));
            Assert.That(collection.ContainsKey("Field1"), Is.True);
            Assert.That(collection.ContainsKey("Field2"), Is.True);
            Assert.That(collection.ContainsKey("Field3"), Is.True);
        }

        [Test]
        public void AddRange_IDictionary_AddsAllFields()
        {
            // Arrange
            var collection = new FieldCollection();
            var dictionary = new Dictionary<string, Field>
            {
                { "Field1", new Field("Field1", "Value1") },
                { "Field2", new Field("Field2", "Value2") }
            };

            // Act
            collection.AddRange(dictionary);

            // Assert
            Assert.That(collection.Count, Is.EqualTo(2));
            Assert.That(collection.ContainsKey("Field1"), Is.True);
            Assert.That(collection.ContainsKey("Field2"), Is.True);
        }

        #endregion

        #region Enumeration Tests

        [Test]
        public void GetEnumerator_CanEnumerateFields()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("Field1", "Value1"));
            collection.Add(new Field("Field2", "Value2"));
            collection.Add(new Field("Field3", "Value3"));

            // Act
            var fieldNames = new List<string>();
            foreach (var kvp in collection)
            {
                fieldNames.Add(kvp.Key);
            }

            // Assert
            Assert.That(fieldNames.Count, Is.EqualTo(3));
            Assert.That(fieldNames, Contains.Item("Field1"));
            Assert.That(fieldNames, Contains.Item("Field2"));
            Assert.That(fieldNames, Contains.Item("Field3"));
        }

        [Test]
        public void Keys_ReturnsAllFieldNames()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("Field1", "Value1"));
            collection.Add(new Field("Field2", "Value2"));
            collection.Add(new Field("Field3", "Value3"));

            // Act
            var keys = collection.Keys.ToList();

            // Assert
            Assert.That(keys.Count, Is.EqualTo(3));
            Assert.That(keys, Contains.Item("Field1"));
            Assert.That(keys, Contains.Item("Field2"));
            Assert.That(keys, Contains.Item("Field3"));
        }

        [Test]
        public void Values_ReturnsAllFields()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("Field1", "Value1"));
            collection.Add(new Field("Field2", 42));

            // Act
            var values = collection.Values.ToList();

            // Assert
            Assert.That(values.Count, Is.EqualTo(2));
            Assert.That(values.Any(f => f.Name == "Field1" && (string)f.Value == "Value1"), Is.True);
            Assert.That(values.Any(f => f.Name == "Field2" && (int)f.Value == 42), Is.True);
        }

        #endregion

        #region Conversion Tests

        [Test]
        public void ToDictionary_CreatesIndependentCopy()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("Field1", "Value1"));
            collection.Add(new Field("Field2", "Value2"));

            // Act
            var dictionary = collection.ToDictionary();

            // Assert
            Assert.That(dictionary.Count, Is.EqualTo(2));
            Assert.That(dictionary.ContainsKey("Field1"), Is.True);
            Assert.That(dictionary.ContainsKey("Field2"), Is.True);
            Assert.That(dictionary["Field1"].Value, Is.EqualTo("Value1"));
            Assert.That(dictionary["Field2"].Value, Is.EqualTo("Value2"));
        }

        [Test]
        public void ToDictionary_ModificationDoesNotAffectOriginal()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("Field1", "Value1"));

            // Act
            var dictionary = collection.ToDictionary();
            dictionary["Field2"] = new Field("Field2", "Value2");

            // Assert
            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(dictionary.Count, Is.EqualTo(2));
        }

        [Test]
        public void ToDynamic_CreatesExpandoObject()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("Field1", "Value1"));
            collection.Add(new Field("Field2", 42));
            collection.Add(new Field("Field3", true));

            // Act
            dynamic expando = collection.ToDynamic();

            // Assert
            Assert.That(expando, Is.InstanceOf<ExpandoObject>());
            Assert.That(expando.Field1, Is.EqualTo("Value1"));
            Assert.That(expando.Field2, Is.EqualTo("42"));
            Assert.That(expando.Field3, Is.EqualTo("True"));
        }

        [Test]
        public void ToDynamic_FieldWithNullValue_AddsEmptyString()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("NullField", null));

            // Act
            dynamic expando = collection.ToDynamic();

            // Assert
            Assert.That(expando.NullField, Is.EqualTo(string.Empty));
        }

        [Test]
        public void ImplicitConversion_ToDictionary_Works()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("Field1", "Value1"));

            // Act
            Dictionary<string, Field> dictionary = collection;

            // Assert
            Assert.That(dictionary, Is.Not.Null);
            Assert.That(dictionary.Count, Is.EqualTo(1));
            Assert.That(dictionary.ContainsKey("Field1"), Is.True);
        }

        #endregion

        #region Edge Cases and Integration Tests

        [Test]
        public void MultipleOperations_WorkCorrectly()
        {
            // Arrange
            var collection = new FieldCollection();

            // Act & Assert - Add multiple fields
            collection.Add(new Field("Field1", "Value1"));
            collection.Add(new Field("Field2", 42));
            collection.Add(new Field("Field3", true));
            Assert.That(collection.Count, Is.EqualTo(3));

            // Try to add duplicate
            collection.Add(new Field("Field1", "NewValue"));
            Assert.That(collection.Count, Is.EqualTo(3));
            Assert.That(collection["Field1"].Value, Is.EqualTo("Value1"));

            // Add range
            var otherCollection = new FieldCollection();
            otherCollection.Add(new Field("Field4", "Value4"));
            collection.AddRange(otherCollection);
            Assert.That(collection.Count, Is.EqualTo(4));

            // Get string values
            Assert.That(collection.GetStringValueOrDefault("Field1", ""), Is.EqualTo("Value1"));
            Assert.That(collection.GetStringValueOrDefault("Field2", ""), Is.EqualTo("42"));
            Assert.That(collection.GetStringValueOrDefault("NonExistent", "Default"), Is.EqualTo("Default"));

            // Convert to dictionary
            var dict = collection.ToDictionary();
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        [Test]
        public void FieldCollection_WithVariousValueTypes_WorksCorrectly()
        {
            // Arrange
            var collection = new FieldCollection();
            collection.Add(new Field("String", "text"));
            collection.Add(new Field("Integer", 123));
            collection.Add(new Field("Double", 45.67));
            collection.Add(new Field("Boolean", true));
            collection.Add(new Field("DateTime", new DateTime(2024, 1, 15)));

            // Act & Assert
            Assert.That(collection.GetStringValueOrDefault("String", ""), Is.EqualTo("text"));
            Assert.That(collection.GetStringValueOrDefault("Integer", ""), Is.EqualTo("123"));
            // Note: Double formatting depends on culture, so we check that it's not empty
            Assert.That(collection.GetStringValueOrDefault("Double", ""), Is.Not.Empty);
            Assert.That(collection.GetStringValueOrDefault("Boolean", ""), Is.EqualTo("True"));
            Assert.That(collection.GetStringValueOrDefault("DateTime", ""), Does.Contain("2024"));
        }

        [Test]
        public void EmptyCollection_AllOperations_BehaveSafely()
        {
            // Arrange
            var collection = new FieldCollection();

            // Act & Assert
            Assert.That(collection.Count, Is.EqualTo(0));
            Assert.That(collection.ContainsKey("Any"), Is.False);
            Assert.That(collection.TryGetValue("Any", out _), Is.False);
            Assert.That(collection.TryGetStringValue("Any", out _), Is.False);
            Assert.That(collection.GetStringValueOrDefault("Any", "Default"), Is.EqualTo("Default"));
            Assert.That(collection.Keys, Is.Empty);
            Assert.That(collection.Values, Is.Empty);
            Assert.That(collection.ToDictionary().Count, Is.EqualTo(0));

            dynamic expando = collection.ToDynamic();
            var expandoDict = (IDictionary<string, object>)expando;
            Assert.That(expandoDict.Count, Is.EqualTo(0));
        }

        #endregion
    }
}
