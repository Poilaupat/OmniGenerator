using System;
using System.Linq;
using System.Reflection;

using NUnit.Framework;
using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Plugins.Tessi.Packagers.Compliance;

namespace OmniGenerator.Plugins.Tessi.Packagers.Compliance.UnitTests
{
    /// <summary>
    /// Unit tests for <see cref="ChequeFields"/> class.
    /// </summary>
    [TestFixture]
    public class ChequeFieldsTests
    {
        /// <summary>
        /// Tests that the constructor successfully initializes with a valid non-empty FieldCollection.
        /// Verifies that the constructor passes the FieldCollection to the base class without throwing an exception.
        /// </summary>
        [Test]
        public void Constructor_WithValidFieldCollection_CreatesInstance()
        {
            // Arrange
            var fieldCollection = new FieldCollection();

            // Act & Assert
            Assert.DoesNotThrow(() => new ChequeFields(fieldCollection));
        }

        /// <summary>
        /// Tests that the constructor successfully initializes with an empty FieldCollection.
        /// Verifies that the constructor accepts an empty FieldCollection without throwing an exception.
        /// </summary>
        [Test]
        public void Constructor_WithEmptyFieldCollection_CreatesInstance()
        {
            // Arrange
            var emptyFieldCollection = new FieldCollection();

            // Act
            var result = new ChequeFields(emptyFieldCollection);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor behavior when passed a null FieldCollection.
        /// Since the parameter is non-nullable and the base class does not validate,
        /// this documents the runtime behavior when nullability constraints are bypassed.
        /// </summary>
        [Test]
        public void Constructor_WithNullFieldCollection_DoesNotThrowAtConstruction()
        {
            // Arrange
            FieldCollection? nullFieldCollection = null;

            // Act & Assert
            // The constructor itself doesn't validate, so it doesn't throw at construction time
            Assert.DoesNotThrow(() => new ChequeFields(nullFieldCollection!));
        }

        /// <summary>
        /// Tests that RemittingBranchCode returns the correct value when the field exists with a valid string.
        /// </summary>
        [Test]
        public void RemittingBranchCode_FieldExists_ReturnsValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var expectedValue = "BR12345";
            var field = new Field("remittingBranchCode", expectedValue);
            fieldCollection.Add(field);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.RemittingBranchCode;

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Tests that RemittingBranchCode returns empty string when the field exists with an empty string value.
        /// </summary>
        [Test]
        public void RemittingBranchCode_FieldExistsWithEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("remittingBranchCode", string.Empty);
            fieldCollection.Add(field);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.RemittingBranchCode;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that RemittingBranchCode returns empty string when the field exists with a null value.
        /// Field.StringValue converts null to empty string.
        /// </summary>
        [Test]
        public void RemittingBranchCode_FieldExistsWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("remittingBranchCode", null);
            fieldCollection.Add(field);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.RemittingBranchCode;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that RemittingBranchCode returns the correct value when the field contains whitespace.
        /// </summary>
        [Test]
        public void RemittingBranchCode_FieldExistsWithWhitespace_ReturnsWhitespace()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var whitespaceValue = "   ";
            var field = new Field("remittingBranchCode", whitespaceValue);
            fieldCollection.Add(field);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.RemittingBranchCode;

            // Assert
            Assert.That(result, Is.EqualTo(whitespaceValue));
        }

        /// <summary>
        /// Tests that RemittingBranchCode returns the correct value when the field contains special characters.
        /// </summary>
        [Test]
        public void RemittingBranchCode_FieldExistsWithSpecialCharacters_ReturnsValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var specialCharsValue = "!@#$%^&*()_+-=[]{}|;':\",./<>?";
            var field = new Field("remittingBranchCode", specialCharsValue);
            fieldCollection.Add(field);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.RemittingBranchCode;

            // Assert
            Assert.That(result, Is.EqualTo(specialCharsValue));
        }

        /// <summary>
        /// Tests that RemittingBranchCode returns the correct value when the field contains a very long string.
        /// </summary>
        [Test]
        public void RemittingBranchCode_FieldExistsWithVeryLongString_ReturnsValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var longValue = new string('A', 10000);
            var field = new Field("remittingBranchCode", longValue);
            fieldCollection.Add(field);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.RemittingBranchCode;

            // Assert
            Assert.That(result, Is.EqualTo(longValue));
        }

        /// <summary>
        /// Tests that RemittingBranchCode returns the correct string representation when the field contains a non-string object.
        /// Field.StringValue calls ToString() on the value.
        /// </summary>
        [Test]
        public void RemittingBranchCode_FieldExistsWithNonStringValue_ReturnsStringRepresentation()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var numericValue = 12345;
            var field = new Field("remittingBranchCode", numericValue);
            fieldCollection.Add(field);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.RemittingBranchCode;

            // Assert
            Assert.That(result, Is.EqualTo("12345"));
        }

        /// <summary>
        /// Tests that RemittingBranchCode throws FieldNotFoundException when the field does not exist in the collection.
        /// </summary>
        [Test]
        public void RemittingBranchCode_FieldDoesNotExist_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var chequeFields = new ChequeFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() =>
            {
                var _ = chequeFields.RemittingBranchCode;
            });

            Assert.That(exception.Message, Does.Contain("remittingBranchCode"));
        }

        /// <summary>
        /// Tests that RemittingBranchCode throws FieldNotFoundException when the collection contains other fields but not remittingBranchCode.
        /// </summary>
        [Test]
        public void RemittingBranchCode_CollectionHasOtherFieldsButNotTarget_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var otherField = new Field("someOtherField", "value");
            fieldCollection.Add(otherField);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() =>
            {
                var _ = chequeFields.RemittingBranchCode;
            });

            Assert.That(exception.Message, Does.Contain("remittingBranchCode"));
        }

        /// <summary>
        /// Tests that ProviderId returns the correct value when the field exists.
        /// Input: FieldCollection with "providerId" field containing "PROVIDER123".
        /// Expected: Returns "PROVIDER123".
        /// </summary>
        [Test]
        public void ProviderId_WhenFieldExists_ReturnsValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "providerId", "PROVIDER123");
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.ProviderId;

            // Assert
            Assert.That(result, Is.EqualTo("PROVIDER123"));
        }

        /// <summary>
        /// Tests that ProviderId throws FieldNotFoundException when the field is missing.
        /// Input: FieldCollection without "providerId" field.
        /// Expected: Throws FieldNotFoundException.
        /// </summary>
        [Test]
        public void ProviderId_WhenFieldMissing_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var chequeFields = new ChequeFields(fieldCollection);

            // Act & Assert
            var ex = Assert.Throws<FieldNotFoundException>(() => { var _ = chequeFields.ProviderId; });
            Assert.That(ex.Message, Does.Contain("providerId"));
        }

        /// <summary>
        /// Tests that ProviderId returns empty string when the field value is null.
        /// Input: FieldCollection with "providerId" field containing null value.
        /// Expected: Returns empty string (based on Field.StringValue implementation).
        /// </summary>
        [Test]
        public void ProviderId_WhenFieldValueIsNull_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "providerId", null);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.ProviderId;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that ProviderId returns empty string when the field value is an empty string.
        /// Input: FieldCollection with "providerId" field containing empty string.
        /// Expected: Returns empty string.
        /// </summary>
        [Test]
        public void ProviderId_WhenFieldValueIsEmpty_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "providerId", string.Empty);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.ProviderId;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that ProviderId returns whitespace when the field value contains only whitespace.
        /// Input: FieldCollection with "providerId" field containing "   ".
        /// Expected: Returns "   ".
        /// </summary>
        [Test]
        public void ProviderId_WhenFieldValueIsWhitespace_ReturnsWhitespace()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "providerId", "   ");
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.ProviderId;

            // Assert
            Assert.That(result, Is.EqualTo("   "));
        }

        /// <summary>
        /// Tests that ProviderId handles special characters correctly.
        /// Input: FieldCollection with "providerId" field containing special characters.
        /// Expected: Returns the value with special characters preserved.
        /// </summary>
        [Test]
        public void ProviderId_WhenFieldContainsSpecialCharacters_ReturnsValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "providerId", "PROV!@#$%^&*()123");
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.ProviderId;

            // Assert
            Assert.That(result, Is.EqualTo("PROV!@#$%^&*()123"));
        }

        /// <summary>
        /// Tests that ProviderId handles very long strings correctly.
        /// Input: FieldCollection with "providerId" field containing a very long string.
        /// Expected: Returns the full long string value.
        /// </summary>
        [Test]
        public void ProviderId_WhenFieldValueIsVeryLong_ReturnsValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var longValue = new string('A', 10000);
            AddField(fieldCollection, "providerId", longValue);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.ProviderId;

            // Assert
            Assert.That(result, Is.EqualTo(longValue));
        }

        /// <summary>
        /// Tests that ProviderId converts non-string field values to string via ToString().
        /// Input: FieldCollection with "providerId" field containing an integer value.
        /// Expected: Returns the string representation of the integer.
        /// </summary>
        [Test]
        public void ProviderId_WhenFieldValueIsNonString_ReturnsToStringRepresentation()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "providerId", 12345);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.ProviderId;

            // Assert
            Assert.That(result, Is.EqualTo("12345"));
        }

        /// <summary>
        /// Helper method to add a field to a FieldCollection using reflection.
        /// Required because FieldCollection.Add is internal and cannot be directly accessed.
        /// </summary>
        /// <param name="fieldCollection">The FieldCollection to add the field to.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        private static void AddField(FieldCollection fieldCollection, string name, object? value)
        {
            var field = new Field(name, value!);
            var addMethod = typeof(FieldCollection).GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic);
            addMethod?.Invoke(fieldCollection, new object[] { field });
        }

        /// <summary>
        /// Tests that Z4 property returns the correct value when the field exists with a valid string.
        /// </summary>
        [Test]
        public void Z4_WhenFieldExistsWithValidString_ReturnsCorrectValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "z4", "validZ4Value");
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z4;

            // Assert
            Assert.That(result, Is.EqualTo("validZ4Value"));
        }

        /// <summary>
        /// Tests that Z4 property throws FieldNotFoundException when the field does not exist.
        /// </summary>
        [Test]
        public void Z4_WhenFieldDoesNotExist_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var chequeFields = new ChequeFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() => { var _ = chequeFields.Z4; });
            Assert.That(exception.Message, Does.Contain("z4"));
        }

        /// <summary>
        /// Tests that Z4 property returns empty string when the field exists with null value.
        /// </summary>
        [Test]
        public void Z4_WhenFieldExistsWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "z4", null);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z4;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that Z4 property returns empty string when the field exists with empty string value.
        /// </summary>
        [Test]
        public void Z4_WhenFieldExistsWithEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "z4", string.Empty);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z4;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that Z4 property returns whitespace string when the field exists with whitespace-only value.
        /// </summary>
        [Test]
        public void Z4_WhenFieldExistsWithWhitespace_ReturnsWhitespace()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "z4", "   ");
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z4;

            // Assert
            Assert.That(result, Is.EqualTo("   "));
        }

        /// <summary>
        /// Tests that Z4 property returns correct value for very long strings.
        /// </summary>
        [Test]
        public void Z4_WhenFieldExistsWithVeryLongString_ReturnsCorrectValue()
        {
            // Arrange
            var longString = new string('A', 10000);
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "z4", longString);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z4;

            // Assert
            Assert.That(result, Is.EqualTo(longString));
        }

        /// <summary>
        /// Tests that Z4 property returns correct value when the field contains special characters.
        /// </summary>
        [TestCase("!@#$%^&*()")]
        [TestCase("<>?:\"{}|")]
        [TestCase("Line1\nLine2\rLine3")]
        [TestCase("\t\t\tTabs")]
        public void Z4_WhenFieldExistsWithSpecialCharacters_ReturnsCorrectValue(string specialValue)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "z4", specialValue);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z4;

            // Assert
            Assert.That(result, Is.EqualTo(specialValue));
        }

        /// <summary>
        /// Tests that Z4 property returns correct value when the field contains Unicode characters.
        /// </summary>
        [Test]
        public void Z4_WhenFieldExistsWithUnicodeCharacters_ReturnsCorrectValue()
        {
            // Arrange
            var unicodeString = "Héllo Wörld 你好 🎉";
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "z4", unicodeString);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z4;

            // Assert
            Assert.That(result, Is.EqualTo(unicodeString));
        }

        /// <summary>
        /// Tests that Z4 property returns correct string representation when the field contains numeric value.
        /// </summary>
        [Test]
        public void Z4_WhenFieldExistsWithNumericValue_ReturnsStringRepresentation()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "z4", 12345);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z4;

            // Assert
            Assert.That(result, Is.EqualTo("12345"));
        }

        /// <summary>
        /// Tests that Z4 property returns correct string representation when the field contains boolean value.
        /// </summary>
        [TestCase(true, "True")]
        [TestCase(false, "False")]
        public void Z4_WhenFieldExistsWithBooleanValue_ReturnsStringRepresentation(bool value, string expected)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddField(fieldCollection, "z4", value);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z4;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that the ScanType property returns the expected value when the field exists with various valid string values.
        /// </summary>
        /// <param name="fieldValue">The value to set in the scanType field.</param>
        [TestCase("recto")]
        [TestCase("verso")]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("scan-type-with-special-chars-!@#$%^&*()")]
        [TestCase("AVeryLongStringValueThatExceedsTypicalLengthsToTestBoundaryConditionsAndEnsureThePropertyHandlesLargeInputsCorrectlyWithoutAnyIssuesOrUnexpectedBehavior")]
        public void ScanType_WithVariousValidValues_ReturnsExpectedValue(string fieldValue)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("scanType", fieldValue));
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.ScanType;

            // Assert
            Assert.That(result, Is.EqualTo(fieldValue));
        }

        /// <summary>
        /// Tests that the ScanType property returns an empty string when the field exists but has a null Value.
        /// This tests the StringValue behavior which returns string.Empty for null values.
        /// </summary>
        [Test]
        public void ScanType_WhenFieldHasNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("scanType", null!));
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.ScanType;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the ScanType property throws <see cref="FieldNotFoundException"/> when the scanType field does not exist in the collection.
        /// This validates the required field behavior.
        /// </summary>
        [Test]
        public void ScanType_WhenFieldNotFound_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var chequeFields = new ChequeFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() =>
            {
                var _ = chequeFields.ScanType;
            });
            Assert.That(exception.Message, Does.Contain("scanType"));
        }

        /// <summary>
        /// Tests that the Z3 property returns the expected string value when the field exists in the collection.
        /// </summary>
        /// <param name="fieldValue">The value to store in the field.</param>
        /// <param name="expectedResult">The expected string value returned by the property.</param>
        [TestCase("123456", "123456", TestName = "Z3_ValidStringValue_ReturnsStringValue")]
        [TestCase("", "", TestName = "Z3_EmptyString_ReturnsEmptyString")]
        [TestCase("   ", "   ", TestName = "Z3_WhitespaceString_ReturnsWhitespace")]
        [TestCase("ABC!@#$%^&*()_+-=[]{}|;':\",./<>?`~", "ABC!@#$%^&*()_+-=[]{}|;':\",./<>?`~", TestName = "Z3_SpecialCharacters_ReturnsSpecialCharacters")]
        [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789", TestName = "Z3_VeryLongString_ReturnsLongString")]
        public void Z3_FieldExistsWithValue_ReturnsExpectedValue(string fieldValue, string expectedResult)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("z3", fieldValue);
            fieldCollection.Add(field);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z3;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that the Z3 property returns an empty string when the field exists with a null value.
        /// This validates that the StringValue property converts null to empty string.
        /// </summary>
        [Test]
        public void Z3_FieldExistsWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("z3", null!);
            fieldCollection.Add(field);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z3;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Z3 property returns the string representation when the field contains a non-string object.
        /// This validates that the StringValue property calls ToString() on the value.
        /// </summary>
        [Test]
        public void Z3_FieldExistsWithNonStringObject_ReturnsToStringValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("z3", 12345);
            fieldCollection.Add(field);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z3;

            // Assert
            Assert.That(result, Is.EqualTo("12345"));
        }

        /// <summary>
        /// Tests that the Z3 property throws a FieldNotFoundException when the field does not exist in the collection.
        /// This validates the required field behavior.
        /// </summary>
        [Test]
        public void Z3_FieldDoesNotExist_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var chequeFields = new ChequeFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() => { var _ = chequeFields.Z3; });
            Assert.That(exception?.Message, Does.Contain("z3"));
        }

        /// <summary>
        /// Tests that the Z3 property throws a FieldNotFoundException with the expected message
        /// when the field does not exist in the collection.
        /// </summary>
        [Test]
        public void Z3_FieldDoesNotExist_ThrowsFieldNotFoundExceptionWithExpectedMessage()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var chequeFields = new ChequeFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() => { var _ = chequeFields.Z3; });
            Assert.That(exception?.Message, Is.EqualTo("The field 'z3' was not found in the collection."));
        }

        /// <summary>
        /// Tests that the Z3 property returns the correct value when accessed multiple times.
        /// This validates that the property getter is idempotent.
        /// </summary>
        [Test]
        public void Z3_AccessedMultipleTimes_ReturnsConsistentValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("z3", "TestValue");
            fieldCollection.Add(field);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result1 = chequeFields.Z3;
            var result2 = chequeFields.Z3;
            var result3 = chequeFields.Z3;

            // Assert
            Assert.That(result1, Is.EqualTo("TestValue"));
            Assert.That(result2, Is.EqualTo("TestValue"));
            Assert.That(result3, Is.EqualTo("TestValue"));
        }

        /// <summary>
        /// Tests that the Z3 property handles unicode characters correctly.
        /// </summary>
        [Test]
        public void Z3_FieldExistsWithUnicodeCharacters_ReturnsUnicodeString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var unicodeValue = "你好世界🌍éàü";
            var field = new Field("z3", unicodeValue);
            fieldCollection.Add(field);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z3;

            // Assert
            Assert.That(result, Is.EqualTo(unicodeValue));
        }

        /// <summary>
        /// Tests that the Z3 property handles control characters correctly.
        /// </summary>
        [Test]
        public void Z3_FieldExistsWithControlCharacters_ReturnsStringWithControlCharacters()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var controlCharsValue = "Line1\nLine2\r\nLine3\tTabbed";
            var field = new Field("z3", controlCharsValue);
            fieldCollection.Add(field);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z3;

            // Assert
            Assert.That(result, Is.EqualTo(controlCharsValue));
        }

        /// <summary>
        /// Tests that Z2 property returns the correct string value when the field exists with various valid string values.
        /// </summary>
        /// <param name="fieldValue">The value to set for the z2 field.</param>
        /// <param name="expectedResult">The expected return value from the Z2 property.</param>
        [TestCase("12345", "12345", TestName = "Z2_ValidStringValue_ReturnsValue")]
        [TestCase("", "", TestName = "Z2_EmptyString_ReturnsEmptyString")]
        [TestCase("   ", "   ", TestName = "Z2_WhitespaceString_ReturnsWhitespace")]
        [TestCase("!@#$%^&*()_+-={}[]|\\:;\"'<>,.?/~`", "!@#$%^&*()_+-={}[]|\\:;\"'<>,.?/~`", TestName = "Z2_SpecialCharacters_ReturnsValue")]
        [TestCase("A very long string that exceeds normal length expectations for testing purposes and ensures proper handling of large data", "A very long string that exceeds normal length expectations for testing purposes and ensures proper handling of large data", TestName = "Z2_VeryLongString_ReturnsValue")]
        [TestCase(null, "", TestName = "Z2_NullValue_ReturnsEmptyString")]
        public void Z2_WithVariousStringValues_ReturnsExpectedValue(object? fieldValue, string expectedResult)
        {
            // Arrange
            var fieldCollection = CreateFieldCollection("z2", fieldValue);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z2;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that Z2 property throws FieldNotFoundException when the z2 field does not exist in the collection.
        /// </summary>
        [Test]
        public void Z2_FieldNotFound_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection(); // Empty collection without z2 field
            var chequeFields = new ChequeFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() => { var _ = chequeFields.Z2; });
            Assert.That(exception?.Message, Does.Contain("z2"));
        }

        /// <summary>
        /// Tests that Z2 property handles numeric values by converting them to strings.
        /// </summary>
        [TestCase(123, "123", TestName = "Z2_IntegerValue_ReturnsStringRepresentation")]
        [TestCase(123.456, "123.456", TestName = "Z2_DoubleValue_ReturnsStringRepresentation")]
        [TestCase(true, "True", TestName = "Z2_BooleanValue_ReturnsStringRepresentation")]
        public void Z2_WithNonStringValues_ReturnsStringRepresentation(object fieldValue, string expectedResult)
        {
            // Arrange
            var fieldCollection = CreateFieldCollection("z2", fieldValue);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Z2;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Helper method to create a FieldCollection with a single field.
        /// </summary>
        /// <param name="fieldName">The name of the field to add.</param>
        /// <param name="fieldValue">The value of the field to add.</param>
        /// <returns>A FieldCollection containing the specified field.</returns>
        private static FieldCollection CreateFieldCollection(string fieldName, object? fieldValue)
        {
            var collection = new FieldCollection();
            var field = new Field(fieldName, fieldValue!);
            collection.Add(field);
            return collection;
        }

        /// <summary>
        /// Helper method to create a FieldCollection with specified fields.
        /// Requires InternalsVisibleTo access from OmniGenerator.Lib to the test assembly.
        /// </summary>
        /// <param name="fields">Array of tuples containing field name and value pairs.</param>
        /// <returns>A populated FieldCollection instance.</returns>
        private static FieldCollection CreateFieldCollection(params (string name, object? value)[] fields)
        {
            var collection = new FieldCollection();
            foreach (var (name, value) in fields)
            {
                collection.Add(new Field(name, value!));
            }
            return collection;
        }

        /// <summary>
        /// Tests that the Amount property returns the correct Field when the "amount" field exists in the collection.
        /// </summary>
        [Test]
        public void Amount_WhenFieldExists_ReturnsField()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var expectedField = new Field("amount", 12345.67m);
            AddFieldToCollection(fieldCollection, expectedField);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Amount;

            // Assert
            Assert.That(result.Name, Is.EqualTo("amount"));
            Assert.That(result.Value, Is.EqualTo(12345.67m));
        }

        /// <summary>
        /// Tests that the Amount property returns the correct Field when the "amount" field has a null value.
        /// </summary>
        [Test]
        public void Amount_WhenFieldExistsWithNullValue_ReturnsFieldWithNullValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var expectedField = new Field("amount", null!);
            AddFieldToCollection(fieldCollection, expectedField);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Amount;

            // Assert
            Assert.That(result.Name, Is.EqualTo("amount"));
            Assert.That(result.Value, Is.Null);
        }

        /// <summary>
        /// Tests that the Amount property returns the correct Field when the "amount" field has various numeric types.
        /// Verifies that different numeric value types (int, double, decimal) are correctly retrieved.
        /// </summary>
        [TestCase(0)]
        [TestCase(100)]
        [TestCase(-50)]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void Amount_WhenFieldExistsWithVariousNumericValues_ReturnsField(int value)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var expectedField = new Field("amount", value);
            AddFieldToCollection(fieldCollection, expectedField);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Amount;

            // Assert
            Assert.That(result.Name, Is.EqualTo("amount"));
            Assert.That(result.Value, Is.EqualTo(value));
        }

        /// <summary>
        /// Tests that the Amount property returns the correct Field when the "amount" field contains a string value.
        /// </summary>
        [Test]
        public void Amount_WhenFieldExistsWithStringValue_ReturnsField()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var expectedField = new Field("amount", "1234.56");
            AddFieldToCollection(fieldCollection, expectedField);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Amount;

            // Assert
            Assert.That(result.Name, Is.EqualTo("amount"));
            Assert.That(result.Value, Is.EqualTo("1234.56"));
        }

        /// <summary>
        /// Tests that the Amount property throws FieldNotFoundException when the "amount" field does not exist in the collection.
        /// Verifies that accessing a required field that is missing throws the correct exception with appropriate message.
        /// </summary>
        [Test]
        public void Amount_WhenFieldDoesNotExist_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var chequeFields = new ChequeFields(fieldCollection);

            // Act & Assert
            var ex = Assert.Throws<FieldNotFoundException>(() => { var _ = chequeFields.Amount; });
            Assert.That(ex?.Message, Does.Contain("amount"));
            Assert.That(ex?.Message, Does.Contain("not found"));
        }

        /// <summary>
        /// Tests that the Amount property throws FieldNotFoundException when the field collection contains other fields but not "amount".
        /// Verifies that the property specifically looks for the "amount" field by name.
        /// </summary>
        [Test]
        public void Amount_WhenCollectionContainsOtherFieldsButNotAmount_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, new Field("scanner", "SCANNER001"));
            AddFieldToCollection(fieldCollection, new Field("chain", "CHAIN001"));
            var chequeFields = new ChequeFields(fieldCollection);

            // Act & Assert
            var ex = Assert.Throws<FieldNotFoundException>(() => { var _ = chequeFields.Amount; });
            Assert.That(ex?.Message, Does.Contain("amount"));
        }

        /// <summary>
        /// Tests that the Amount property returns the correct Field when the "amount" field contains special floating-point values.
        /// Verifies handling of edge cases like NaN, PositiveInfinity, and NegativeInfinity.
        /// </summary>
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(0.0)]
        [TestCase(-0.0)]
        [TestCase(double.Epsilon)]
        [TestCase(double.MaxValue)]
        [TestCase(double.MinValue)]
        public void Amount_WhenFieldExistsWithSpecialDoubleValues_ReturnsField(double value)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var expectedField = new Field("amount", value);
            AddFieldToCollection(fieldCollection, expectedField);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Amount;

            // Assert
            Assert.That(result.Name, Is.EqualTo("amount"));
            Assert.That(result.Value, Is.EqualTo(value));
        }

        /// <summary>
        /// Helper method to add a field to a FieldCollection using reflection.
        /// This is necessary because the Add method is internal and not accessible from test code.
        /// </summary>
        /// <param name="collection">The FieldCollection to add the field to.</param>
        /// <param name="field">The Field to add.</param>
        private static void AddFieldToCollection(FieldCollection collection, Field field)
        {
            var addMethod = typeof(FieldCollection).GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic);
            if (addMethod == null)
            {
                throw new InvalidOperationException("Unable to find the internal Add method on FieldCollection.");
            }
            addMethod.Invoke(collection, new object[] { field });
        }

        /// <summary>
        /// Tests that the Chain property returns the correct field value when the field exists with a valid string.
        /// </summary>
        [Test]
        public void Chain_WhenFieldExists_ReturnsFieldValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "chain", "processing-chain-value");
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Chain;

            // Assert
            Assert.That(result, Is.EqualTo("processing-chain-value"));
        }

        /// <summary>
        /// Tests that the Chain property throws FieldNotFoundException when the field is missing from the collection.
        /// </summary>
        [Test]
        public void Chain_WhenFieldMissing_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var chequeFields = new ChequeFields(fieldCollection);

            // Act & Assert
            var ex = Assert.Throws<FieldNotFoundException>(() => { var _ = chequeFields.Chain; });
            Assert.That(ex.Message, Does.Contain("chain"));
        }

        /// <summary>
        /// Tests that the Chain property returns an empty string when the field exists but has a null value.
        /// </summary>
        [Test]
        public void Chain_WhenFieldValueIsNull_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "chain", null);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Chain;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Chain property returns an empty string when the field exists with an empty string value.
        /// </summary>
        [Test]
        public void Chain_WhenFieldValueIsEmpty_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "chain", string.Empty);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Chain;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Chain property returns whitespace when the field exists with whitespace-only value.
        /// </summary>
        [TestCase("   ")]
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase(" \t\n ")]
        public void Chain_WhenFieldValueIsWhitespace_ReturnsWhitespace(string whitespace)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "chain", whitespace);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Chain;

            // Assert
            Assert.That(result, Is.EqualTo(whitespace));
        }

        /// <summary>
        /// Tests that the Chain property correctly handles fields with special characters.
        /// </summary>
        [TestCase("chain-with-dashes")]
        [TestCase("chain_with_underscores")]
        [TestCase("chain.with.dots")]
        [TestCase("chain/with/slashes")]
        [TestCase("chain\\with\\backslashes")]
        [TestCase("chain:with:colons")]
        [TestCase("chain@with@at")]
        [TestCase("chain#with#hash")]
        [TestCase("chain$with$dollar")]
        [TestCase("chain%with%percent")]
        [TestCase("chain&with&ampersand")]
        [TestCase("chain*with*asterisk")]
        [TestCase("chain+with+plus")]
        [TestCase("chain=with=equals")]
        [TestCase("chain with spaces")]
        [TestCase("chain\twith\ttabs")]
        [TestCase("chain<with>brackets")]
        [TestCase("chain[with]square")]
        [TestCase("chain{with}curly")]
        [TestCase("chain|with|pipes")]
        [TestCase("chain~with~tilde")]
        [TestCase("chain`with`backtick")]
        [TestCase("chain'with'quotes")]
        [TestCase("chain\"with\"doublequotes")]
        public void Chain_WhenFieldContainsSpecialCharacters_ReturnsCorrectValue(string value)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "chain", value);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Chain;

            // Assert
            Assert.That(result, Is.EqualTo(value));
        }

        /// <summary>
        /// Tests that the Chain property returns the ToString() representation when the field value is a non-string type.
        /// </summary>
        [TestCase(123, "123")]
        [TestCase(123.45, "123.45")]
        [TestCase(true, "True")]
        [TestCase(false, "False")]
        public void Chain_WhenFieldValueIsNonString_ReturnsToStringRepresentation(object value, string expected)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "chain", value);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Chain;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that the Chain property handles very long string values correctly.
        /// </summary>
        [Test]
        public void Chain_WhenFieldValueIsVeryLong_ReturnsFullValue()
        {
            // Arrange
            var longValue = new string('x', 10000);
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "chain", longValue);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Chain;

            // Assert
            Assert.That(result, Is.EqualTo(longValue));
            Assert.That(result.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that the Chain property handles Unicode and international characters correctly.
        /// </summary>
        [TestCase("chain-français")]
        [TestCase("chain-日本語")]
        [TestCase("chain-中文")]
        [TestCase("chain-العربية")]
        [TestCase("chain-עברית")]
        [TestCase("chain-Ελληνικά")]
        [TestCase("chain-кириллица")]
        [TestCase("chain-emoji-😀🎉")]
        public void Chain_WhenFieldContainsUnicodeCharacters_ReturnsCorrectValue(string value)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "chain", value);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.Chain;

            // Assert
            Assert.That(result, Is.EqualTo(value));
        }

        /// <summary>
        /// Helper method to add a field to a FieldCollection using reflection.
        /// This is necessary because the Add method is internal.
        /// </summary>
        /// <param name="collection">The field collection to add to.</param>
        /// <param name="name">The field name.</param>
        /// <param name="value">The field value.</param>
        private static void AddFieldToCollection(FieldCollection collection, string name, object? value)
        {
            var field = new Field(name, value);
            var addMethod = typeof(FieldCollection).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
            addMethod?.Invoke(collection, new object[] { field });
        }

        /// <summary>
        /// Tests that AccountNumber returns the field value when the field exists with a valid string.
        /// </summary>
        [Test]
        public void AccountNumber_WhenFieldExists_ReturnsFieldValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var expectedValue = "123456789";
            AddFieldToCollection(fieldCollection, "accountNumber", expectedValue);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.AccountNumber;

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Tests that AccountNumber returns an empty string when the field exists with a null value.
        /// Field.StringValue converts null to empty string.
        /// </summary>
        [Test]
        public void AccountNumber_WhenFieldExistsWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "accountNumber", null);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.AccountNumber;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that AccountNumber returns an empty string when the field exists with an empty string value.
        /// </summary>
        [Test]
        public void AccountNumber_WhenFieldExistsWithEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "accountNumber", string.Empty);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.AccountNumber;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that AccountNumber returns a whitespace string when the field exists with whitespace value.
        /// </summary>
        [Test]
        public void AccountNumber_WhenFieldExistsWithWhitespace_ReturnsWhitespace()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var whitespaceValue = "   ";
            AddFieldToCollection(fieldCollection, "accountNumber", whitespaceValue);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.AccountNumber;

            // Assert
            Assert.That(result, Is.EqualTo(whitespaceValue));
        }

        /// <summary>
        /// Tests that AccountNumber returns a very long string when the field exists with a very long value.
        /// </summary>
        [Test]
        public void AccountNumber_WhenFieldExistsWithVeryLongString_ReturnsLongString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var longValue = new string('A', 10000);
            AddFieldToCollection(fieldCollection, "accountNumber", longValue);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.AccountNumber;

            // Assert
            Assert.That(result, Is.EqualTo(longValue));
        }

        /// <summary>
        /// Tests that AccountNumber returns a string with special characters when the field exists with special characters.
        /// </summary>
        [Test]
        public void AccountNumber_WhenFieldExistsWithSpecialCharacters_ReturnsSpecialCharacters()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var specialValue = "!@#$%^&*()_+-=[]{}|;':\",./<>?`~";
            AddFieldToCollection(fieldCollection, "accountNumber", specialValue);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.AccountNumber;

            // Assert
            Assert.That(result, Is.EqualTo(specialValue));
        }

        /// <summary>
        /// Tests that AccountNumber returns a string representation when the field exists with a non-string object.
        /// Field.StringValue calls ToString() on the object.
        /// </summary>
        [Test]
        public void AccountNumber_WhenFieldExistsWithNumericValue_ReturnsStringRepresentation()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var numericValue = 987654321;
            AddFieldToCollection(fieldCollection, "accountNumber", numericValue);
            var chequeFields = new ChequeFields(fieldCollection);

            // Act
            var result = chequeFields.AccountNumber;

            // Assert
            Assert.That(result, Is.EqualTo(numericValue.ToString()));
        }

        /// <summary>
        /// Tests that AccountNumber throws FieldNotFoundException when the field does not exist in the collection.
        /// </summary>
        [Test]
        public void AccountNumber_WhenFieldDoesNotExist_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var chequeFields = new ChequeFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() => _ = chequeFields.AccountNumber);
            Assert.That(exception?.Message, Does.Contain("accountNumber"));
        }

        /// <summary>
        /// Tests that AccountNumber throws FieldNotFoundException with correct message when field is missing.
        /// </summary>
        [Test]
        public void AccountNumber_WhenFieldDoesNotExist_ThrowsExceptionWithCorrectMessage()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, "otherField", "someValue");
            var chequeFields = new ChequeFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() => _ = chequeFields.AccountNumber);
            Assert.That(exception?.Message, Is.EqualTo("The field 'accountNumber' was not found in the collection."));
        }
    }
}