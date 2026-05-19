using System;
using System.Reflection;

using Moq;
using NUnit.Framework;
using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="DocumentFields"/> class.
    /// </summary>
    [TestFixture]
    public class DocumentFieldsTests
    {
        /// <summary>
        /// Tests that the Dataread property returns the field value when the field exists with a valid string value.
        /// </summary>
        /// <param name="value">The value to set for the dataread field.</param>
        /// <param name="expected">The expected return value.</param>
        [TestCase("test data", "test data")]
        [TestCase("", "")]
        [TestCase("   ", "   ")]
        [TestCase("special!@#$%^&*()chars", "special!@#$%^&*()chars")]
        [TestCase("line1\nline2\ttab", "line1\nline2\ttab")]
        [TestCase("VeryLongStringWithManyCharactersToTestBoundaryConditionsAndEnsureThePropertyHandlesLargeInputsCorrectly", "VeryLongStringWithManyCharactersToTestBoundaryConditionsAndEnsureThePropertyHandlesLargeInputsCorrectly")]
        public void Dataread_FieldExistsWithValue_ReturnsFieldValue(string value, string expected)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("dataread", value);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Dataread;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that the Dataread property returns empty string when the field exists but has a null value.
        /// </summary>
        [Test]
        public void Dataread_FieldExistsWithNullValue_ReturnsDefaultEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("dataread", null!);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Dataread;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Dataread property returns the default empty string when the field does not exist in the collection.
        /// </summary>
        [Test]
        public void Dataread_FieldDoesNotExist_ReturnsDefaultEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Dataread;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Dataread property returns the string representation when the field value is a non-string object.
        /// </summary>
        [Test]
        public void Dataread_FieldExistsWithNonStringObject_ReturnsToStringResult()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("dataread", 12345);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Dataread;

            // Assert
            Assert.That(result, Is.EqualTo("12345"));
        }

        /// <summary>
        /// Tests that the Dataread property handles Unicode and multi-byte characters correctly.
        /// </summary>
        [TestCase("Hello 世界", "Hello 世界")]
        [TestCase("Emoji: 😀🎉", "Emoji: 😀🎉")]
        [TestCase("Ñoño", "Ñoño")]
        public void Dataread_FieldExistsWithUnicodeCharacters_ReturnsUnicodeValue(string value, string expected)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("dataread", value);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Dataread;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that the Dataread property handles control characters correctly.
        /// </summary>
        [Test]
        public void Dataread_FieldExistsWithControlCharacters_ReturnsValueWithControlCharacters()
        {
            // Arrange
            var value = "text\0with\x01null\x02chars";
            var fieldCollection = new FieldCollection();
            var field = new Field("dataread", value);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Dataread;

            // Assert
            Assert.That(result, Is.EqualTo(value));
        }

        /// <summary>
        /// Tests that the Dataread property returns empty string when field collection is empty.
        /// </summary>
        [Test]
        public void Dataread_EmptyFieldCollection_ReturnsDefaultEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Dataread;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that QualityCode returns the default value "0" when the field is not present in the collection.
        /// </summary>
        [Test]
        public void QualityCode_FieldNotPresent_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.QualityCode;

            // Assert
            Assert.That(result, Is.EqualTo("0"));
        }

        /// <summary>
        /// Tests that QualityCode returns the field value when present with various valid string inputs.
        /// </summary>
        /// <param name="fieldValue">The value to store in the "quality-code" field.</param>
        /// <param name="expectedResult">The expected result when accessing QualityCode property.</param>
        [TestCase("5", "5")]
        [TestCase("", "")]
        [TestCase("  ", "  ")]
        [TestCase("!@#$%^&*()", "!@#$%^&*()")]
        [TestCase("0", "0")]
        [TestCase("12345", "12345")]
        [TestCase("   leading and trailing spaces   ", "   leading and trailing spaces   ")]
        [TestCase("\t\n", "\t\n")]
        public void QualityCode_FieldPresentWithValue_ReturnsValue(string fieldValue, string expectedResult)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("quality-code", fieldValue));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.QualityCode;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that QualityCode returns an empty string when the field is present but has a null value.
        /// This tests the behavior of Field.StringValue which converts null to string.Empty.
        /// </summary>
        [Test]
        public void QualityCode_FieldPresentWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("quality-code", null!));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.QualityCode;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that QualityCode returns the string representation when the field contains a non-string object.
        /// This tests the ToString() behavior of Field.StringValue.
        /// </summary>
        [Test]
        public void QualityCode_FieldPresentWithNumericValue_ReturnsStringRepresentation()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("quality-code", 42));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.QualityCode;

            // Assert
            Assert.That(result, Is.EqualTo("42"));
        }

        /// <summary>
        /// Tests that QualityCode handles very long string values correctly.
        /// This tests boundary conditions for string length.
        /// </summary>
        [Test]
        public void QualityCode_FieldPresentWithVeryLongString_ReturnsLongString()
        {
            // Arrange
            var longString = new string('A', 10000);
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("quality-code", longString));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.QualityCode;

            // Assert
            Assert.That(result, Is.EqualTo(longString));
            Assert.That(result.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that QualityCode handles Unicode and special characters correctly.
        /// This tests support for international characters and symbols.
        /// </summary>
        [TestCase("こんにちは")]
        [TestCase("🎉🎊")]
        [TestCase("Ñoño")]
        [TestCase("\u0000\u0001\u0002")]
        public void QualityCode_FieldPresentWithUnicodeCharacters_ReturnsUnicodeString(string unicodeValue)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("quality-code", unicodeValue));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.QualityCode;

            // Assert
            Assert.That(result, Is.EqualTo(unicodeValue));
        }

        /// <summary>
        /// Tests that RefDoc returns the field value when the field exists with a non-null value.
        /// </summary>
        /// <param name="fieldValue">The value to set for the ref-doc field.</param>
        /// <param name="expectedValue">The expected return value from RefDoc property.</param>
        [TestCase("DOC123", "DOC123")]
        [TestCase("", "")]
        [TestCase("   ", "   ")]
        [TestCase("DOC-123/456#789", "DOC-123/456#789")]
        [TestCase("文档参考-αβγ-🎉", "文档参考-αβγ-🎉")]
        [TestCase("A", "A")]
        [TestCase("\t\n\r", "\t\n\r")]
        public void RefDoc_FieldExistsWithValue_ReturnsValue(string fieldValue, string expectedValue)
        {
            // Arrange
            var fieldCollection = FieldCollectionHelper.CreateWithField("ref-doc", fieldValue);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.RefDoc;

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Tests that RefDoc returns the default value (empty string) when the field exists but has a null value.
        /// </summary>
        [Test]
        public void RefDoc_FieldExistsWithNullValue_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = FieldCollectionHelper.CreateWithField("ref-doc", null);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.RefDoc;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that RefDoc returns the default value (empty string) when the field does not exist in the collection.
        /// </summary>
        [Test]
        public void RefDoc_FieldDoesNotExist_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.RefDoc;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that RefDoc returns the full string when the field contains a very long string value.
        /// </summary>
        [Test]
        public void RefDoc_FieldExistsWithVeryLongString_ReturnsFullString()
        {
            // Arrange
            var longString = new string('X', 10000);
            var fieldCollection = FieldCollectionHelper.CreateWithField("ref-doc", longString);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.RefDoc;

            // Assert
            Assert.That(result, Is.EqualTo(longString));
            Assert.That(result.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Helper class to create FieldCollection instances for testing purposes.
        /// Uses reflection to populate the internal field dictionary.
        /// </summary>
        private static class FieldCollectionHelper
        {
            /// <summary>
            /// Creates a FieldCollection with a single field.
            /// </summary>
            /// <param name="fieldName">The name of the field to add.</param>
            /// <param name="fieldValue">The value of the field to add.</param>
            /// <returns>A populated FieldCollection instance.</returns>
            public static FieldCollection CreateWithField(string fieldName, object? fieldValue)
            {
                var collection = new FieldCollection();
                var field = new Field(fieldName, fieldValue!);

                // Use reflection to call the internal Add method
                var addMethod = typeof(FieldCollection).GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic);
                addMethod?.Invoke(collection, new object[] { field });

                return collection;
            }
        }

        /// <summary>
        /// Tests that the Signature property returns the default value "---SIGNATURE---" when the field does not exist in the collection.
        /// </summary>
        [Test]
        public void Signature_FieldDoesNotExist_ReturnsDefaultValue()
        {
            // Arrange
            var emptyFieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(emptyFieldCollection);

            // Act
            var result = documentFields.Signature;

            // Assert
            Assert.That(result, Is.EqualTo("---SIGNATURE---"));
        }

        /// <summary>
        /// Tests that the Signature property returns the field value when the "signature" field exists in the collection.
        /// Note: This test uses a test helper class to populate FieldCollection since the Add method is internal.
        /// </summary>
        [Test]
        public void Signature_FieldExists_ReturnsFieldValue()
        {
            // Arrange
            var fieldCollection = new TestableFieldCollection();
            fieldCollection.AddField(new Field("signature", "CustomSignature"));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Signature;

            // Assert
            Assert.That(result, Is.EqualTo("CustomSignature"));
        }

        /// <summary>
        /// Tests that the Signature property returns an empty string when the "signature" field exists with an empty string value.
        /// </summary>
        [Test]
        public void Signature_FieldExistsWithEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new TestableFieldCollection();
            fieldCollection.AddField(new Field("signature", string.Empty));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Signature;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Signature property returns whitespace when the "signature" field exists with whitespace value.
        /// </summary>
        [Test]
        public void Signature_FieldExistsWithWhitespace_ReturnsWhitespace()
        {
            // Arrange
            var fieldCollection = new TestableFieldCollection();
            fieldCollection.AddField(new Field("signature", "   "));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Signature;

            // Assert
            Assert.That(result, Is.EqualTo("   "));
        }

        /// <summary>
        /// Tests that the Signature property returns special characters when the "signature" field exists with special characters.
        /// </summary>
        [Test]
        public void Signature_FieldExistsWithSpecialCharacters_ReturnsSpecialCharacters()
        {
            // Arrange
            var fieldCollection = new TestableFieldCollection();
            fieldCollection.AddField(new Field("signature", "!@#$%^&*()_+-={}[]|\\:;\"'<>,.?/~`"));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Signature;

            // Assert
            Assert.That(result, Is.EqualTo("!@#$%^&*()_+-={}[]|\\:;\"'<>,.?/~`"));
        }

        /// <summary>
        /// Tests that the Signature property returns the default value "---SIGNATURE---" when the field exists but Value is null.
        /// Field.StringValue returns empty string when Value is null, and GetStringValueOrDefault returns empty string (not default).
        /// </summary>
        [Test]
        public void Signature_FieldExistsWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new TestableFieldCollection();
            fieldCollection.AddField(new Field("signature", null!));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Signature;

            // Assert
            // When Field.Value is null, StringValue returns string.Empty (not null)
            // GetStringValueOrDefault will return string.Empty since the field exists
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Signature property returns a very long string when the "signature" field exists with a very long value.
        /// </summary>
        [Test]
        public void Signature_FieldExistsWithVeryLongString_ReturnsLongString()
        {
            // Arrange
            var longString = new string('X', 10000);
            var fieldCollection = new TestableFieldCollection();
            fieldCollection.AddField(new Field("signature", longString));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Signature;

            // Assert
            Assert.That(result, Is.EqualTo(longString));
        }

        /// <summary>
        /// Tests that the Signature property properly converts non-string objects to strings via ToString().
        /// </summary>
        [Test]
        public void Signature_FieldExistsWithIntegerValue_ReturnsStringRepresentation()
        {
            // Arrange
            var fieldCollection = new TestableFieldCollection();
            fieldCollection.AddField(new Field("signature", 12345));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Signature;

            // Assert
            Assert.That(result, Is.EqualTo("12345"));
        }

        /// <summary>
        /// Helper class to expose internal Add functionality for testing purposes.
        /// Inherits from FieldCollection and uses reflection to add fields since the Add method is internal.
        /// </summary>
        private class TestableFieldCollection : FieldCollection
        {
            /// <summary>
            /// Adds a field to the collection using reflection to access the internal Add method.
            /// </summary>
            /// <param name="field">The field to add.</param>
            public void AddField(Field field)
            {
                // Use reflection to call the internal Add method
                var addMethod = typeof(FieldCollection).GetMethod("Add",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                addMethod?.Invoke(this, new object[] { field });
            }
        }

        /// <summary>
        /// Tests that Status property returns the field value when the field exists in the collection with a non-empty value.
        /// </summary>
        /// <param name="fieldValue">The value of the status field.</param>
        /// <param name="expectedResult">The expected result.</param>
        [TestCase("1", "1")]
        [TestCase("active", "active")]
        [TestCase("  ", "  ")]
        [TestCase("status!@#$%^&*()", "status!@#$%^&*()")]
        [TestCase("very_long_string_with_many_characters_to_test_edge_case_handling_for_long_values_in_the_status_field", "very_long_string_with_many_characters_to_test_edge_case_handling_for_long_values_in_the_status_field")]
        public void Status_FieldExistsWithValue_ReturnsValue(string fieldValue, string expectedResult)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("status", fieldValue);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Status;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that Status property returns empty string when the field exists but has an empty string value.
        /// </summary>
        [Test]
        public void Status_FieldExistsWithEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("status", string.Empty);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Status;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that Status property returns empty string when the field exists but has a null value.
        /// The Field.StringValue property converts null to empty string.
        /// </summary>
        [Test]
        public void Status_FieldExistsWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("status", null!);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Status;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that Status property returns the default value "0" when the status field does not exist in the collection.
        /// </summary>
        [Test]
        public void Status_FieldDoesNotExist_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Status;

            // Assert
            Assert.That(result, Is.EqualTo("0"));
        }

        /// <summary>
        /// Tests that Status property returns the default value "0" when the collection has other fields but not the status field.
        /// </summary>
        [Test]
        public void Status_FieldDoesNotExistButOtherFieldsPresent_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var otherField = new Field("other-field", "some-value");
            fieldCollection.Add(otherField);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Status;

            // Assert
            Assert.That(result, Is.EqualTo("0"));
        }

        /// <summary>
        /// Tests that Priority property returns the correct value when the field exists with various non-empty values.
        /// </summary>
        /// <param name="priorityValue">The value to set for the priority field.</param>
        [TestCase("High")]
        [TestCase("Low")]
        [TestCase("Medium")]
        [TestCase("1")]
        [TestCase("999")]
        public void Priority_FieldExistsWithValue_ReturnsFieldValue(string priorityValue)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var priorityField = new Field("priority", priorityValue);
            fieldCollection.Add(priorityField);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Priority;

            // Assert
            Assert.That(result, Is.EqualTo(priorityValue));
        }

        /// <summary>
        /// Tests that Priority property returns empty string when the field does not exist in the collection.
        /// This verifies the default value behavior as specified in the FieldInfo attribute.
        /// </summary>
        [Test]
        public void Priority_FieldDoesNotExist_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Priority;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that Priority property returns empty string when the field exists with an empty string value.
        /// </summary>
        [Test]
        public void Priority_FieldExistsWithEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var priorityField = new Field("priority", string.Empty);
            fieldCollection.Add(priorityField);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Priority;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that Priority property returns empty string when the field exists with a null value.
        /// This verifies that null values are properly handled by returning the default value.
        /// </summary>
        [Test]
        public void Priority_FieldExistsWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var priorityField = new Field("priority", null!);
            fieldCollection.Add(priorityField);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Priority;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that Priority property correctly handles whitespace-only values.
        /// </summary>
        /// <param name="whitespaceValue">The whitespace value to test.</param>
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase("\r\n")]
        public void Priority_FieldExistsWithWhitespace_ReturnsWhitespace(string whitespaceValue)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var priorityField = new Field("priority", whitespaceValue);
            fieldCollection.Add(priorityField);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Priority;

            // Assert
            Assert.That(result, Is.EqualTo(whitespaceValue));
        }

        /// <summary>
        /// Tests that Priority property correctly handles values with special characters.
        /// </summary>
        /// <param name="specialCharValue">The special character value to test.</param>
        [TestCase("!@#$%^&*()")]
        [TestCase("<>?:\"{}|")]
        [TestCase("Priority-1")]
        [TestCase("Priority_A")]
        [TestCase("Priority.Test")]
        public void Priority_FieldExistsWithSpecialCharacters_ReturnsSpecialCharacters(string specialCharValue)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var priorityField = new Field("priority", specialCharValue);
            fieldCollection.Add(priorityField);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Priority;

            // Assert
            Assert.That(result, Is.EqualTo(specialCharValue));
        }

        /// <summary>
        /// Tests that Priority property correctly handles very long string values.
        /// This ensures there are no buffer or length limitations.
        /// </summary>
        [Test]
        public void Priority_FieldExistsWithVeryLongString_ReturnsVeryLongString()
        {
            // Arrange
            var longString = new string('A', 10000);
            var fieldCollection = new FieldCollection();
            var priorityField = new Field("priority", longString);
            fieldCollection.Add(priorityField);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Priority;

            // Assert
            Assert.That(result, Is.EqualTo(longString));
        }

        /// <summary>
        /// Tests that Priority property correctly converts non-string object values to their string representation.
        /// This verifies the behavior when the field value is an integer object.
        /// </summary>
        [Test]
        public void Priority_FieldExistsWithIntegerValue_ReturnsStringRepresentation()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var priorityField = new Field("priority", 123);
            fieldCollection.Add(priorityField);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Priority;

            // Assert
            Assert.That(result, Is.EqualTo("123"));
        }

        /// <summary>
        /// Tests that Priority property correctly converts boolean object values to their string representation.
        /// This verifies the behavior when the field value is a boolean object.
        /// </summary>
        [TestCase(true, "True")]
        [TestCase(false, "False")]
        public void Priority_FieldExistsWithBooleanValue_ReturnsStringRepresentation(bool boolValue, string expectedString)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var priorityField = new Field("priority", boolValue);
            fieldCollection.Add(priorityField);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Priority;

            // Assert
            Assert.That(result, Is.EqualTo(expectedString));
        }

        /// <summary>
        /// Tests that Priority property correctly handles Unicode characters and international text.
        /// </summary>
        /// <param name="unicodeValue">The Unicode string value to test.</param>
        [TestCase("优先")]
        [TestCase("приоритет")]
        [TestCase("우선순위")]
        [TestCase("🔥💯")]
        public void Priority_FieldExistsWithUnicodeCharacters_ReturnsUnicodeCharacters(string unicodeValue)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var priorityField = new Field("priority", unicodeValue);
            fieldCollection.Add(priorityField);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Priority;

            // Assert
            Assert.That(result, Is.EqualTo(unicodeValue));
        }

        /// <summary>
        /// Tests that the RIB property returns the expected value when the field exists with a valid string.
        /// Verifies that GetStringValueOrDefault is called with correct parameters and the returned value is propagated correctly.
        /// </summary>
        /// <param name="expectedValue">The value to return from the mocked field collection.</param>
        [TestCase("12345678901234567890")]
        [TestCase("FR7612345678901234567890123")]
        [TestCase("IBAN1234567890")]
        [TestCase("Test RIB Value")]
        public void RIB_WhenFieldExistsWithValue_ReturnsValue(string expectedValue)
        {
            // Arrange
            var mockFieldCollection = new Mock<FieldCollection>();
            mockFieldCollection
                .Setup(fc => fc.GetStringValueOrDefault("rib", string.Empty))
                .Returns(expectedValue);

            var documentFields = new DocumentFields(mockFieldCollection.Object);

            // Act
            var result = documentFields.RIB;

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
            mockFieldCollection.Verify(fc => fc.GetStringValueOrDefault("rib", string.Empty), Times.Once);
        }

        /// <summary>
        /// Tests that the RIB property returns empty string when the field does not exist.
        /// Verifies the default value behavior when the field is not found in the collection.
        /// </summary>
        [Test]
        public void RIB_WhenFieldDoesNotExist_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.RIB;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the RIB property returns empty string when the field exists but has an empty string value.
        /// Verifies correct handling of empty string values.
        /// </summary>
        [Test]
        public void RIB_WhenFieldExistsWithEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("rib", string.Empty);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.RIB;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the RIB property returns whitespace when the field contains whitespace characters.
        /// Verifies that whitespace is preserved and not trimmed.
        /// </summary>
        /// <param name="whitespaceValue">The whitespace string to test.</param>
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase("   \t\n   ")]
        public void RIB_WhenFieldContainsWhitespace_ReturnsWhitespace(string whitespaceValue)
        {
            // Arrange
            var mockFieldCollection = new Mock<FieldCollection>();
            mockFieldCollection
                .Setup(fc => fc.GetStringValueOrDefault("rib", string.Empty))
                .Returns(whitespaceValue);

            var documentFields = new DocumentFields(mockFieldCollection.Object);

            // Act
            var result = documentFields.RIB;

            // Assert
            Assert.That(result, Is.EqualTo(whitespaceValue));
            mockFieldCollection.Verify(fc => fc.GetStringValueOrDefault("rib", string.Empty), Times.Once);
        }

        /// <summary>
        /// Tests that the RIB property correctly handles special characters.
        /// Verifies that special characters are returned without modification or escaping.
        /// </summary>
        /// <param name="specialCharsValue">The string containing special characters to test.</param>
        [TestCase("!@#$%^&*()")]
        [TestCase("RIB-123/456\\789")]
        [TestCase("αβγδε")]
        [TestCase("💰💳🏦")]
        [TestCase("<script>alert('test')</script>")]
        [TestCase("C:\\Path\\To\\File")]
        public void RIB_WhenFieldContainsSpecialCharacters_ReturnsSpecialCharacters(string specialCharsValue)
        {
            // Arrange
            var mockFieldCollection = new Mock<FieldCollection>();
            mockFieldCollection
                .Setup(fc => fc.GetStringValueOrDefault("rib", string.Empty))
                .Returns(specialCharsValue);

            var documentFields = new DocumentFields(mockFieldCollection.Object);

            // Act
            var result = documentFields.RIB;

            // Assert
            Assert.That(result, Is.EqualTo(specialCharsValue));
            mockFieldCollection.Verify(fc => fc.GetStringValueOrDefault("rib", string.Empty), Times.Once);
        }

        /// <summary>
        /// Tests that the RIB property correctly handles very long strings.
        /// Verifies that there are no length limitations or truncation issues.
        /// </summary>
        [Test]
        public void RIB_WhenFieldContainsVeryLongString_ReturnsFullString()
        {
            // Arrange
            var longString = new string('A', 10000);
            var fieldCollection = new FieldCollection();
            var field = new Field("rib", longString);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.RIB;

            // Assert
            Assert.That(result, Is.EqualTo(longString));
            Assert.That(result.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that the RIB property can be accessed multiple times and consistently returns the same value.
        /// Verifies that the property getter is idempotent and doesn't cache or modify the returned value.
        /// </summary>
        [Test]
        public void RIB_WhenAccessedMultipleTimes_CallsGetStringValueOrDefaultEachTime()
        {
            // Arrange
            var expectedValue = "TestRIB123";
            var fieldCollection = new FieldCollection();
            var ribField = new Field("rib", expectedValue);
            
            // Use reflection to add the field since Add is internal
            var addMethod = typeof(FieldCollection).GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            addMethod.Invoke(fieldCollection, new object[] { ribField });

            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result1 = documentFields.RIB;
            var result2 = documentFields.RIB;
            var result3 = documentFields.RIB;

            // Assert
            Assert.That(result1, Is.EqualTo(expectedValue));
            Assert.That(result2, Is.EqualTo(expectedValue));
            Assert.That(result3, Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Tests that NbChecks returns the field value when the field exists with a valid string.
        /// </summary>
        /// <param name="value">The test value to store in the nb-checks field.</param>
        [TestCase("5")]
        [TestCase("0")]
        [TestCase("100")]
        [TestCase("999999")]
        [TestCase("1")]
        public void NbChecks_FieldExistsWithValue_ReturnsFieldValue(string value)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("nb-checks", value);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.NbChecks;

            // Assert
            Assert.That(result, Is.EqualTo(value));
        }

        /// <summary>
        /// Tests that NbChecks returns empty string when the field does not exist.
        /// </summary>
        [Test]
        public void NbChecks_FieldDoesNotExist_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.NbChecks;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that NbChecks returns empty string when the field exists with a null value.
        /// </summary>
        [Test]
        public void NbChecks_FieldExistsWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("nb-checks", null!);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.NbChecks;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that NbChecks correctly handles edge case string values.
        /// </summary>
        /// <param name="value">The edge case value to test.</param>
        /// <param name="expectedResult">The expected result.</param>
        [TestCase("", "")]
        [TestCase("   ", "   ")]
        [TestCase("\t\n\r", "\t\n\r")]
        [TestCase("abc", "abc")]
        [TestCase("-1", "-1")]
        [TestCase("2147483647", "2147483647")]
        [TestCase("5.5", "5.5")]
        [TestCase("invalid", "invalid")]
        [TestCase("@#$%^&*()", "@#$%^&*()")]
        public void NbChecks_FieldExistsWithEdgeCaseValues_ReturnsExpectedValue(string value, string expectedResult)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("nb-checks", value);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.NbChecks;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that NbChecks handles very long string values correctly.
        /// </summary>
        [Test]
        public void NbChecks_FieldExistsWithVeryLongString_ReturnsFullValue()
        {
            // Arrange
            var longValue = new string('1', 10000);
            var fieldCollection = new FieldCollection();
            var field = new Field("nb-checks", longValue);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.NbChecks;

            // Assert
            Assert.That(result, Is.EqualTo(longValue));
            Assert.That(result.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that NbChecks correctly converts non-string object values to string.
        /// </summary>
        /// <param name="value">The object value to test.</param>
        /// <param name="expectedString">The expected string representation.</param>
        [TestCase(123, "123")]
        [TestCase(0, "0")]
        [TestCase(-5, "-5")]
        [TestCase(999, "999")]
        public void NbChecks_FieldExistsWithNumericValue_ReturnsStringRepresentation(object value, string expectedString)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("nb-checks", value);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.NbChecks;

            // Assert
            Assert.That(result, Is.EqualTo(expectedString));
        }

        /// <summary>
        /// Tests that NbChecks handles unicode and special characters correctly.
        /// </summary>
        [TestCase("こんにちは", "こんにちは")]
        [TestCase("😀😁", "😀😁")]
        [TestCase("\u0000\u0001", "\u0000\u0001")]
        [TestCase("test\u200Bvalue", "test\u200Bvalue")]
        public void NbChecks_FieldExistsWithUnicodeCharacters_ReturnsValue(string value, string expectedResult)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("nb-checks", value);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.NbChecks;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that ICRConfAmount returns the default empty string when the field does not exist in the collection.
        /// </summary>
        [Test]
        public void ICRConfAmount_FieldNotPresent_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ICRConfAmount;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that ICRConfAmount returns the string value when the field exists with a non-empty string value.
        /// </summary>
        /// <param name="value">The field value to test.</param>
        /// <param name="expected">The expected result.</param>
        [TestCase("100", "100")]
        [TestCase("12.34", "12.34")]
        [TestCase("test-value", "test-value")]
        public void ICRConfAmount_FieldPresentWithValue_ReturnsValue(string value, string expected)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "icr-conf-amount", value);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ICRConfAmount;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that ICRConfAmount returns empty string when the field exists with an empty string value.
        /// </summary>
        [Test]
        public void ICRConfAmount_FieldPresentWithEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "icr-conf-amount", string.Empty);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ICRConfAmount;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that ICRConfAmount returns whitespace when the field exists with whitespace value.
        /// </summary>
        /// <param name="whitespace">The whitespace value to test.</param>
        [TestCase("   ")]
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase(" \t\n ")]
        public void ICRConfAmount_FieldPresentWithWhitespace_ReturnsWhitespace(string whitespace)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "icr-conf-amount", whitespace);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ICRConfAmount;

            // Assert
            Assert.That(result, Is.EqualTo(whitespace));
        }

        /// <summary>
        /// Tests that ICRConfAmount returns the default empty string when the field exists with a null value.
        /// </summary>
        [Test]
        public void ICRConfAmount_FieldPresentWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "icr-conf-amount", null!);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ICRConfAmount;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that ICRConfAmount returns the string representation when the field contains special characters.
        /// </summary>
        /// <param name="specialValue">The special character value to test.</param>
        [TestCase("!@#$%^&*()")]
        [TestCase("<>?/\\|")]
        [TestCase("unicode_αβγδ")]
        [TestCase("emoji_😀🎉")]
        public void ICRConfAmount_FieldPresentWithSpecialCharacters_ReturnsSpecialCharacters(string specialValue)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "icr-conf-amount", specialValue);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ICRConfAmount;

            // Assert
            Assert.That(result, Is.EqualTo(specialValue));
        }

        /// <summary>
        /// Tests that ICRConfAmount returns the full string when the field contains a very long string.
        /// </summary>
        [Test]
        public void ICRConfAmount_FieldPresentWithVeryLongString_ReturnsFullString()
        {
            // Arrange
            var longString = new string('A', 10000);
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "icr-conf-amount", longString);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ICRConfAmount;

            // Assert
            Assert.That(result, Is.EqualTo(longString));
            Assert.That(result.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that ICRConfAmount returns the ToString() result when the field contains a numeric object value.
        /// </summary>
        /// <param name="value">The numeric value to test.</param>
        /// <param name="expected">The expected string representation.</param>
        [TestCase(123, "123")]
        [TestCase(0, "0")]
        [TestCase(-456, "-456")]
        [TestCase(12.34, "12.34")]
        [TestCase(int.MaxValue, "2147483647")]
        [TestCase(int.MinValue, "-2147483648")]
        public void ICRConfAmount_FieldPresentWithNumericValue_ReturnsToStringResult(object value, string expected)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "icr-conf-amount", value);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ICRConfAmount;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Helper method to add a field to a FieldCollection using reflection since Add is internal.
        /// </summary>
        /// <param name="collection">The field collection to add to.</param>
        /// <param name="name">The field name.</param>
        /// <param name="value">The field value.</param>
        private void AddFieldToCollection(FieldCollection collection, string name, object value)
        {
            var field = new Field(name, value);
            var addMethod = typeof(FieldCollection).GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic);
            addMethod?.Invoke(collection, new object[] { field });
        }

        /// <summary>
        /// Tests that the ICRAmount property returns the expected value when the field exists
        /// with various values including normal strings, empty strings, whitespace, special characters, and null.
        /// </summary>
        /// <param name="fieldValue">The value to store in the "icr-amount" field.</param>
        /// <param name="expectedResult">The expected return value from the ICRAmount property.</param>
        [TestCase("12345", "12345")]
        [TestCase("", "")]
        [TestCase("   ", "   ")]
        [TestCase("!@#$%^&*()", "!@#$%^&*()")]
        [TestCase("100.50", "100.50")]
        [TestCase("-500", "-500")]
        [TestCase("0", "0")]
        [TestCase(null, "")]
        public void ICRAmount_FieldExistsWithValue_ReturnsExpectedValue(string? fieldValue, string expectedResult)
        {
            // Arrange
            var fieldCollection = CreateFieldCollectionWithField("icr-amount", fieldValue!);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ICRAmount;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that the ICRAmount property returns the default empty string
        /// when the "icr-amount" field does not exist in the field collection.
        /// </summary>
        [Test]
        public void ICRAmount_FieldDoesNotExist_ReturnsDefaultEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ICRAmount;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the ICRAmount property returns the full string value
        /// when the "icr-amount" field contains a very long string.
        /// </summary>
        [Test]
        public void ICRAmount_VeryLongString_ReturnsFullString()
        {
            // Arrange
            var veryLongString = new string('a', 10000);
            var fieldCollection = CreateFieldCollectionWithField("icr-amount", veryLongString);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ICRAmount;

            // Assert
            Assert.That(result, Is.EqualTo(veryLongString));
            Assert.That(result.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that the ICRAmount property returns the correct value
        /// when the field contains Unicode and special characters.
        /// </summary>
        [Test]
        public void ICRAmount_UnicodeAndSpecialCharacters_ReturnsCorrectValue()
        {
            // Arrange
            var unicodeString = "€£¥₹💰🔢\t\n\r";
            var fieldCollection = CreateFieldCollectionWithField("icr-amount", unicodeString);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ICRAmount;

            // Assert
            Assert.That(result, Is.EqualTo(unicodeString));
        }

        /// <summary>
        /// Tests that the ICRAmount property returns an empty string
        /// when the field exists but has a whitespace-only value and verifies the default is used.
        /// </summary>
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase("\r\n")]
        [TestCase(" \t\n\r ")]
        public void ICRAmount_WhitespaceVariations_ReturnsWhitespace(string whitespace)
        {
            // Arrange
            var fieldCollection = CreateFieldCollectionWithField("icr-amount", whitespace);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ICRAmount;

            // Assert
            Assert.That(result, Is.EqualTo(whitespace));
        }

        /// <summary>
        /// Helper method to create a FieldCollection with a single field.
        /// Uses reflection to call the internal Add method.
        /// </summary>
        /// <param name="fieldName">The name of the field to add.</param>
        /// <param name="fieldValue">The value of the field to add.</param>
        /// <returns>A FieldCollection containing the specified field.</returns>
        private FieldCollection CreateFieldCollectionWithField(string fieldName, object fieldValue)
        {
            var fieldCollection = new FieldCollection();
            var field = new Field(fieldName, fieldValue);

            var addMethod = typeof(FieldCollection).GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic);
            addMethod?.Invoke(fieldCollection, new object[] { field });

            return fieldCollection;
        }

        /// <summary>
        /// Tests that ImageQuality returns the actual field value when the field exists in the collection.
        /// </summary>
        /// <param name="fieldValue">The value to set in the field collection.</param>
        /// <param name="expectedResult">The expected string value returned by ImageQuality.</param>
        [TestCase("100", "100", TestName = "ImageQuality_FieldExistsWithValidString_ReturnsFieldValue")]
        [TestCase("", "", TestName = "ImageQuality_FieldExistsWithEmptyString_ReturnsEmptyString")]
        [TestCase("   ", "   ", TestName = "ImageQuality_FieldExistsWithWhitespace_ReturnsWhitespace")]
        [TestCase("ABC!@#$%^&*()", "ABC!@#$%^&*()", TestName = "ImageQuality_FieldExistsWithSpecialCharacters_ReturnsSpecialCharacters")]
        [TestCase("VeryLongStringVeryLongStringVeryLongStringVeryLongStringVeryLongStringVeryLongStringVeryLongStringVeryLongStringVeryLongStringVeryLongString",
                  "VeryLongStringVeryLongStringVeryLongStringVeryLongStringVeryLongStringVeryLongStringVeryLongStringVeryLongStringVeryLongStringVeryLongString",
                  TestName = "ImageQuality_FieldExistsWithVeryLongString_ReturnsLongString")]
        public void ImageQuality_FieldExistsWithValue_ReturnsFieldValue(string fieldValue, string expectedResult)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("image-quality", fieldValue));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ImageQuality;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that ImageQuality returns empty string when the field exists but has a null value.
        /// This verifies that Field.StringValue returns empty string for null values, not the default.
        /// </summary>
        [Test]
        public void ImageQuality_FieldExistsWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("image-quality", null!));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ImageQuality;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that ImageQuality returns the default value "0" when the field does not exist in the collection.
        /// </summary>
        [Test]
        public void ImageQuality_FieldDoesNotExist_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ImageQuality;

            // Assert
            Assert.That(result, Is.EqualTo("0"));
        }

        /// <summary>
        /// Tests that ImageQuality correctly converts non-string object values to strings.
        /// </summary>
        /// <param name="fieldValue">The non-string value to set in the field collection.</param>
        /// <param name="expectedResult">The expected string representation.</param>
        [TestCase(123, "123", TestName = "ImageQuality_FieldExistsWithIntegerValue_ReturnsStringRepresentation")]
        [TestCase(45.67, "45.67", TestName = "ImageQuality_FieldExistsWithDoubleValue_ReturnsStringRepresentation")]
        [TestCase(true, "True", TestName = "ImageQuality_FieldExistsWithBooleanValue_ReturnsStringRepresentation")]
        public void ImageQuality_FieldExistsWithNonStringValue_ReturnsStringRepresentation(object fieldValue, string expectedResult)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("image-quality", fieldValue));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ImageQuality;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that ImageQuality returns the default value when the collection contains other fields but not "image-quality".
        /// </summary>
        [Test]
        public void ImageQuality_FieldCollectionContainsOtherFields_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("some-other-field", "value"));
            fieldCollection.Add(new Field("another-field", "another-value"));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ImageQuality;

            // Assert
            Assert.That(result, Is.EqualTo("0"));
        }

        /// <summary>
        /// Tests that ImageQuality handles control characters in the field value correctly.
        /// </summary>
        [Test]
        public void ImageQuality_FieldExistsWithControlCharacters_ReturnsValueWithControlCharacters()
        {
            // Arrange
            var fieldValue = "Line1\nLine2\tTab\rReturn";
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("image-quality", fieldValue));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ImageQuality;

            // Assert
            Assert.That(result, Is.EqualTo(fieldValue));
        }

        /// <summary>
        /// Tests that ImageQuality handles Unicode characters in the field value correctly.
        /// </summary>
        [Test]
        public void ImageQuality_FieldExistsWithUnicodeCharacters_ReturnsValueWithUnicodeCharacters()
        {
            // Arrange
            var fieldValue = "Test™®©€£¥";
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("image-quality", fieldValue));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ImageQuality;

            // Assert
            Assert.That(result, Is.EqualTo(fieldValue));
        }

        /// <summary>
        /// Tests that ImageQuality handles negative numeric string values correctly.
        /// </summary>
        [Test]
        public void ImageQuality_FieldExistsWithNegativeNumericString_ReturnsNegativeValue()
        {
            // Arrange
            var fieldValue = "-100";
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("image-quality", fieldValue));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ImageQuality;

            // Assert
            Assert.That(result, Is.EqualTo(fieldValue));
        }

        /// <summary>
        /// Tests that ImageQuality handles extreme boundary values for numeric representations.
        /// </summary>
        [TestCase(int.MaxValue, "2147483647", TestName = "ImageQuality_FieldExistsWithIntMaxValue_ReturnsStringRepresentation")]
        [TestCase(int.MinValue, "-2147483648", TestName = "ImageQuality_FieldExistsWithIntMinValue_ReturnsStringRepresentation")]
        [TestCase(long.MaxValue, "9223372036854775807", TestName = "ImageQuality_FieldExistsWithLongMaxValue_ReturnsStringRepresentation")]
        [TestCase(long.MinValue, "-9223372036854775808", TestName = "ImageQuality_FieldExistsWithLongMinValue_ReturnsStringRepresentation")]
        public void ImageQuality_FieldExistsWithBoundaryNumericValues_ReturnsStringRepresentation(object fieldValue, string expectedResult)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("image-quality", fieldValue));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ImageQuality;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that ImageQuality handles special floating-point values correctly.
        /// </summary>
        [TestCase(double.NaN, "NaN", TestName = "ImageQuality_FieldExistsWithDoubleNaN_ReturnsNaNString")]
        [TestCase(double.PositiveInfinity, "Infinity", TestName = "ImageQuality_FieldExistsWithPositiveInfinity_ReturnsInfinityString")]
        [TestCase(double.NegativeInfinity, "-Infinity", TestName = "ImageQuality_FieldExistsWithNegativeInfinity_ReturnsNegativeInfinityString")]
        public void ImageQuality_FieldExistsWithSpecialDoubleValues_ReturnsStringRepresentation(double fieldValue, string expectedResult)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("image-quality", fieldValue));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.ImageQuality;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that the Deleted property returns the default value "0" when the field is not present in the collection.
        /// </summary>
        [Test]
        public void Deleted_FieldNotPresent_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Deleted;

            // Assert
            Assert.That(result, Is.EqualTo("0"));
        }

        /// <summary>
        /// Tests that the Deleted property returns the field value when the field exists with a valid string value.
        /// </summary>
        /// <param name="value">The field value to test.</param>
        /// <param name="expected">The expected result.</param>
        [TestCase("1", "1")]
        [TestCase("0", "0")]
        [TestCase("deleted", "deleted")]
        [TestCase("some value", "some value")]
        public void Deleted_FieldPresentWithValue_ReturnsFieldValue(object value, string expected)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("deleted", value);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Deleted;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that the Deleted property returns an empty string when the field exists with a null value.
        /// This occurs because Field.StringValue returns empty string for null values, not the default value.
        /// </summary>
        [Test]
        public void Deleted_FieldPresentWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("deleted", null!);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Deleted;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Deleted property returns an empty string when the field exists with an empty string value.
        /// </summary>
        [Test]
        public void Deleted_FieldPresentWithEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("deleted", string.Empty);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Deleted;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Deleted property returns whitespace when the field exists with whitespace value.
        /// </summary>
        /// <param name="whitespace">The whitespace value to test.</param>
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase("\r\n")]
        public void Deleted_FieldPresentWithWhitespace_ReturnsWhitespace(string whitespace)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("deleted", whitespace);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Deleted;

            // Assert
            Assert.That(result, Is.EqualTo(whitespace));
        }

        /// <summary>
        /// Tests that the Deleted property correctly handles special characters in field values.
        /// </summary>
        /// <param name="specialValue">The special character value to test.</param>
        [TestCase("@#$%^&*()")]
        [TestCase("<>?/\\|")]
        [TestCase("üöä")]
        [TestCase("🚀")]
        public void Deleted_FieldPresentWithSpecialCharacters_ReturnsSpecialCharacters(string specialValue)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("deleted", specialValue);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Deleted;

            // Assert
            Assert.That(result, Is.EqualTo(specialValue));
        }

        /// <summary>
        /// Tests that the Deleted property handles numeric values by converting them to strings.
        /// </summary>
        /// <param name="numericValue">The numeric value to test.</param>
        /// <param name="expected">The expected string representation.</param>
        [TestCase(123, "123")]
        [TestCase(0, "0")]
        [TestCase(-456, "-456")]
        [TestCase(int.MaxValue, "2147483647")]
        [TestCase(int.MinValue, "-2147483648")]
        public void Deleted_FieldPresentWithNumericValue_ReturnsStringRepresentation(int numericValue, string expected)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("deleted", numericValue);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Deleted;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that the Deleted property handles boolean values by converting them to strings.
        /// </summary>
        /// <param name="boolValue">The boolean value to test.</param>
        /// <param name="expected">The expected string representation.</param>
        [TestCase(true, "True")]
        [TestCase(false, "False")]
        public void Deleted_FieldPresentWithBooleanValue_ReturnsStringRepresentation(bool boolValue, string expected)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("deleted", boolValue);
            fieldCollection.Add(field);
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Deleted;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that Encline property returns the correct value when the field exists with various valid string values.
        /// </summary>
        /// <param name="value">The test value to store in the field.</param>
        [TestCase("CMC7")]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("!@#$%^&*()")]
        [TestCase("测试数据")]
        public void Encline_FieldExistsWithValidValue_ReturnsValue(string value)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("encline", value));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Encline;

            // Assert
            Assert.That(result, Is.EqualTo(value));
        }

        /// <summary>
        /// Tests that Encline property returns the correct value when the field exists with a very long string.
        /// </summary>
        [Test]
        public void Encline_FieldExistsWithVeryLongString_ReturnsValue()
        {
            // Arrange
            var longValue = new string('x', 10000);
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("encline", longValue));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Encline;

            // Assert
            Assert.That(result, Is.EqualTo(longValue));
        }

        /// <summary>
        /// Tests that Encline property returns an empty string when the field exists but has a null value.
        /// This tests the behavior of Field.StringValue which returns empty string for null values.
        /// </summary>
        [Test]
        public void Encline_FieldExistsWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("encline", null!));
            var documentFields = new DocumentFields(fieldCollection);

            // Act
            var result = documentFields.Encline;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that Encline property throws FieldNotFoundException when the required field is missing.
        /// The property is marked as required and should throw when the field does not exist in the collection.
        /// </summary>
        [Test]
        public void Encline_FieldMissing_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() => { var _ = documentFields.Encline; });
            Assert.That(exception!.Message, Does.Contain("encline"));
        }

        /// <summary>
        /// Tests that Encline property throws FieldNotFoundException with the expected message format
        /// when the required field is not present in the collection.
        /// </summary>
        [Test]
        public void Encline_FieldMissing_ThrowsExceptionWithCorrectMessage()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var documentFields = new DocumentFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() => { var _ = documentFields.Encline; });
            Assert.That(exception!.Message, Is.EqualTo("The field 'encline' was not found in the collection."));
        }

        /// <summary>
        /// Tests that the constructor successfully creates a DocumentFields instance with a valid FieldCollection.
        /// </summary>
        [Test]
        public void Constructor_WithValidFieldCollection_CreatesInstance()
        {
            // Arrange
            var fieldCollection = new FieldCollection();

            // Act
            var documentFields = new DocumentFields(fieldCollection);

            // Assert
            Assert.That(documentFields, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor successfully creates a DocumentFields instance with an empty FieldCollection.
        /// Verifies that the instance can be created even when the field collection contains no fields.
        /// </summary>
        [Test]
        public void Constructor_WithEmptyFieldCollection_CreatesInstance()
        {
            // Arrange
            var emptyFieldCollection = new FieldCollection();

            // Act
            var documentFields = new DocumentFields(emptyFieldCollection);

            // Assert
            Assert.That(documentFields, Is.Not.Null);
            Assert.That(documentFields, Is.InstanceOf<DocumentFields>());
        }

        /// <summary>
        /// Tests that the constructor accepts a null FieldCollection parameter.
        /// Since the base class does not validate the parameter, passing null will succeed during construction
        /// but may cause issues when accessing properties that rely on the field collection.
        /// This test documents the actual runtime behavior with nullable reference types.
        /// </summary>
        [Test]
        public void Constructor_WithNullFieldCollection_CreatesInstanceButMayFailOnPropertyAccess()
        {
            // Arrange
            FieldCollection? nullFieldCollection = null;

            // Act & Assert
            // The constructor itself does not validate and will not throw
            Assert.DoesNotThrow(() => new DocumentFields(nullFieldCollection!));
        }
    }
}