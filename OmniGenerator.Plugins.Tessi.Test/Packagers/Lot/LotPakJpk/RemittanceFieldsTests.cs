using System;

using Moq;
using NUnit.Framework;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk.UnitTests
{
    /// <summary>
    /// Unit tests for <see cref="RemittanceFields"/> class.
    /// </summary>
    public class RemittanceFieldsTests
    {
        /// <summary>
        /// Tests that RemittanceId returns the field value when the field exists with a non-null value.
        /// </summary>
        /// <param name="fieldValue">The value to set for the remittance-id field.</param>
        [TestCase("REM123")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  \t\n  ")]
        [TestCase("RemittanceWithSpecialChars!@#$%^&*()")]
        [TestCase("Very long remittance identifier that exceeds typical length expectations to test boundary conditions and ensure proper string handling without truncation or errors")]
        [TestCase("Remittance-With-Dashes")]
        [TestCase("Remittance_With_Underscores")]
        [TestCase("RemittanceWith123Numbers")]
        public void RemittanceId_WhenFieldExistsWithValue_ReturnsFieldValue(string fieldValue)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("remittance-id", fieldValue));
            var remittanceFields = new RemittanceFields(fieldCollection);

            // Act
            var result = remittanceFields.RemittanceId;

            // Assert
            Assert.That(result, Is.EqualTo(fieldValue));
        }

        /// <summary>
        /// Tests that RemittanceId returns empty string when the field does not exist in the collection.
        /// </summary>
        [Test]
        public void RemittanceId_WhenFieldDoesNotExist_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var remittanceFields = new RemittanceFields(fieldCollection);

            // Act
            var result = remittanceFields.RemittanceId;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that RemittanceId returns empty string when the field exists but its value is null.
        /// </summary>
        [Test]
        public void RemittanceId_WhenFieldValueIsNull_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("remittance-id", null));
            var remittanceFields = new RemittanceFields(fieldCollection);

            // Act
            var result = remittanceFields.RemittanceId;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that RemittanceId returns empty string when the field collection contains other fields but not remittance-id.
        /// </summary>
        [Test]
        public void RemittanceId_WhenOtherFieldsExistButNotRemittanceId_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("other-field", "value"));
            fieldCollection.Add(new Field("another-field", "another-value"));
            var remittanceFields = new RemittanceFields(fieldCollection);

            // Act
            var result = remittanceFields.RemittanceId;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that RemittanceId returns the correct value when multiple fields exist including remittance-id.
        /// </summary>
        [Test]
        public void RemittanceId_WhenMultipleFieldsIncludingRemittanceId_ReturnsRemittanceIdValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("other-field", "other-value"));
            fieldCollection.Add(new Field("remittance-id", "REM456"));
            fieldCollection.Add(new Field("another-field", "another-value"));
            var remittanceFields = new RemittanceFields(fieldCollection);

            // Act
            var result = remittanceFields.RemittanceId;

            // Assert
            Assert.That(result, Is.EqualTo("REM456"));
        }

        /// <summary>
        /// Tests that RemittanceId handles numeric values correctly by converting them to string.
        /// </summary>
        [TestCase(123)]
        [TestCase(0)]
        [TestCase(-456)]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void RemittanceId_WhenFieldContainsNumericValue_ReturnsStringRepresentation(int numericValue)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("remittance-id", numericValue));
            var remittanceFields = new RemittanceFields(fieldCollection);

            // Act
            var result = remittanceFields.RemittanceId;

            // Assert
            Assert.That(result, Is.EqualTo(numericValue.ToString()));
        }

        /// <summary>
        /// Tests that RemittanceId handles various object types correctly by converting them to string.
        /// </summary>
        [Test]
        public void RemittanceId_WhenFieldContainsObjectValue_ReturnsStringRepresentation()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var testObject = new { Id = 123, Name = "Test" };
            fieldCollection.Add(new Field("remittance-id", testObject));
            var remittanceFields = new RemittanceFields(fieldCollection);

            // Act
            var result = remittanceFields.RemittanceId;

            // Assert
            Assert.That(result, Is.EqualTo(testObject.ToString()));
        }

        /// <summary>
        /// Tests that RemittanceId is case-sensitive regarding field name.
        /// </summary>
        [Test]
        public void RemittanceId_WhenFieldNameHasDifferentCase_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("Remittance-Id", "REM789"));
            fieldCollection.Add(new Field("REMITTANCE-ID", "REM999"));
            var remittanceFields = new RemittanceFields(fieldCollection);

            // Act
            var result = remittanceFields.RemittanceId;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the constructor successfully creates a RemittanceFields instance
        /// when provided with a valid FieldCollection.
        /// Expected: Instance is created without throwing an exception.
        /// </summary>
        [Test]
        public void Constructor_WithValidFieldCollection_ShouldConstructSuccessfully()
        {
            // Arrange
            var fieldCollection = new FieldCollection();

            // Act
            var remittanceFields = new RemittanceFields(fieldCollection);

            // Assert
            Assert.That(remittanceFields, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws ArgumentNullException
        /// when provided with a null FieldCollection.
        /// Expected: ArgumentNullException is thrown.
        /// </summary>
        [Test]
        public void Constructor_WithNullFieldCollection_ShouldThrowArgumentNullException()
        {
            // Arrange
            FieldCollection? nullFieldCollection = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new RemittanceFields(nullFieldCollection!));
        }
    }
}