using System;
using System.Reflection;

using NUnit.Framework;
using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Plugins.Tessi.Packagers.Compliance;

namespace OmniGenerator.Plugins.Tessi.Packagers.Compliance.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="RootFields"/> class.
    /// </summary>
    [TestFixture]
    public class RootFieldsTests
    {
        /// <summary>
        /// Tests that the Numlot property returns the correct field when the "numlot" field exists in the collection.
        /// Input: FieldCollection containing a field named "numlot" with a test value.
        /// Expected: The property returns a Field with the correct name and value.
        /// </summary>
        [Test]
        public void Numlot_FieldExists_ReturnsField()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var expectedField = new Field("numlot", "12345");
            fieldCollection.Add(expectedField);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Numlot;

            // Assert
            Assert.That(result.Name, Is.EqualTo("numlot"));
            Assert.That(result.Value, Is.EqualTo("12345"));
        }

        /// <summary>
        /// Tests that the Numlot property throws FieldNotFoundException when the "numlot" field does not exist.
        /// Input: Empty FieldCollection without the "numlot" field.
        /// Expected: FieldNotFoundException is thrown with appropriate message.
        /// </summary>
        [Test]
        public void Numlot_FieldDoesNotExist_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var rootFields = new RootFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() => { var _ = rootFields.Numlot; });
            Assert.That(exception.Message, Does.Contain("numlot"));
        }

        /// <summary>
        /// Tests that the Numlot property returns consistent results across multiple accesses.
        /// Input: FieldCollection containing a field named "numlot".
        /// Expected: Multiple accesses to the property return fields with identical name and value.
        /// </summary>
        [Test]
        public void Numlot_MultipleAccesses_ReturnsConsistentField()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var expectedField = new Field("numlot", "LOT-2024-001");
            fieldCollection.Add(expectedField);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var firstAccess = rootFields.Numlot;
            var secondAccess = rootFields.Numlot;

            // Assert
            Assert.That(firstAccess.Name, Is.EqualTo(secondAccess.Name));
            Assert.That(firstAccess.Value, Is.EqualTo(secondAccess.Value));
            Assert.That(firstAccess.Name, Is.EqualTo("numlot"));
            Assert.That(firstAccess.Value, Is.EqualTo("LOT-2024-001"));
        }

        /// <summary>
        /// Tests that the Numlot property correctly handles different value types stored in the field.
        /// Input: FieldCollection containing a "numlot" field with various object types.
        /// Expected: The property returns the Field with the correct value regardless of type.
        /// </summary>
        [TestCase(12345)]
        [TestCase("LOT-STRING")]
        [TestCase(null)]
        public void Numlot_DifferentValueTypes_ReturnsFieldWithCorrectValue(object? value)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("numlot", value!);
            fieldCollection.Add(field);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Numlot;

            // Assert
            Assert.That(result.Name, Is.EqualTo("numlot"));
            Assert.That(result.Value, Is.EqualTo(value));
        }

        /// <summary>
        /// Tests that the Numlot property throws FieldNotFoundException with the correct field name in the message
        /// when accessing a missing field.
        /// Input: FieldCollection containing other fields but not "numlot".
        /// Expected: FieldNotFoundException is thrown with "numlot" in the error message.
        /// </summary>
        [Test]
        public void Numlot_FieldCollectionHasOtherFieldsButNotNumlot_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("bankCode", "BANK001"));
            fieldCollection.Add(new Field("culture", "fr-FR"));
            var rootFields = new RootFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() => { var _ = rootFields.Numlot; });
            Assert.That(exception.Message, Does.Contain("'numlot'"));
        }

        /// <summary>
        /// Helper method to create a FieldCollection with specified fields.
        /// Uses reflection to access the internal Add method.
        /// </summary>
        /// <param name="fields">Tuples of field name and value.</param>
        /// <returns>A populated FieldCollection.</returns>
        private static FieldCollection CreateFieldCollection(params (string name, object? value)[] fields)
        {
            var fieldCollection = new FieldCollection();
            var addMethod = typeof(FieldCollection).GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic);

            if (addMethod == null)
            {
                throw new InvalidOperationException("Could not find Add method on FieldCollection");
            }

            foreach (var (name, value) in fields)
            {
                var field = new Field(name, value!);
                addMethod.Invoke(fieldCollection, new object[] { field });
            }

            return fieldCollection;
        }

        /// <summary>
        /// Tests that ProviderCode returns the correct string value when the field exists in the collection.
        /// Input: FieldCollection with "providerCode" field containing a valid string value.
        /// Expected: Returns the string value of the field.
        /// </summary>
        [TestCase("PROVIDER123")]
        [TestCase("ABC")]
        [TestCase("Provider-Code_2024")]
        [TestCase("A")]
        [TestCase("VERY_LONG_PROVIDER_CODE_WITH_MANY_CHARACTERS_TO_TEST_LONG_STRING_HANDLING_1234567890")]
        public void ProviderCode_WhenFieldExistsWithValidValue_ReturnsFieldValue(string providerCodeValue)
        {
            // Arrange
            var fieldCollection = CreateFieldCollection("providerCode", providerCodeValue);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProviderCode;

            // Assert
            Assert.That(result, Is.EqualTo(providerCodeValue));
        }

        /// <summary>
        /// Tests that ProviderCode returns empty string when the field exists but its value is null.
        /// Input: FieldCollection with "providerCode" field containing null value.
        /// Expected: Returns empty string (per Field.StringValue behavior: Value?.ToString() ?? string.Empty).
        /// </summary>
        [Test]
        public void ProviderCode_WhenFieldExistsWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = CreateFieldCollection("providerCode", null);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProviderCode;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that ProviderCode returns empty string when the field exists with an empty string value.
        /// Input: FieldCollection with "providerCode" field containing empty string.
        /// Expected: Returns empty string.
        /// </summary>
        [Test]
        public void ProviderCode_WhenFieldExistsWithEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = CreateFieldCollection("providerCode", string.Empty);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProviderCode;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that ProviderCode returns the whitespace string when the field exists with whitespace-only value.
        /// Input: FieldCollection with "providerCode" field containing whitespace characters.
        /// Expected: Returns the whitespace string as-is.
        /// </summary>
        [TestCase("   ")]
        [TestCase("\t")]
        [TestCase("\r\n")]
        [TestCase(" \t\r\n ")]
        public void ProviderCode_WhenFieldExistsWithWhitespace_ReturnsWhitespace(string whitespaceValue)
        {
            // Arrange
            var fieldCollection = CreateFieldCollection("providerCode", whitespaceValue);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProviderCode;

            // Assert
            Assert.That(result, Is.EqualTo(whitespaceValue));
        }

        /// <summary>
        /// Tests that ProviderCode returns strings with special characters correctly.
        /// Input: FieldCollection with "providerCode" field containing special characters.
        /// Expected: Returns the string with special characters.
        /// </summary>
        [TestCase("Provider@Code#2024")]
        [TestCase("Provider$%^&*()")]
        [TestCase("Provider\nCode")]
        [TestCase("Provider\tCode")]
        [TestCase("Provider\\Code")]
        [TestCase("Provider'Code\"Test")]
        [TestCase("Provider<>Code")]
        [TestCase("Provider{}[]Code")]
        public void ProviderCode_WhenFieldExistsWithSpecialCharacters_ReturnsSpecialCharacters(string specialValue)
        {
            // Arrange
            var fieldCollection = CreateFieldCollection("providerCode", specialValue);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProviderCode;

            // Assert
            Assert.That(result, Is.EqualTo(specialValue));
        }

        /// <summary>
        /// Tests that ProviderCode throws FieldNotFoundException when the field does not exist in the collection.
        /// Input: FieldCollection without "providerCode" field.
        /// Expected: Throws FieldNotFoundException with appropriate message.
        /// </summary>
        [Test]
        public void ProviderCode_WhenFieldDoesNotExist_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var rootFields = new RootFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() => { var _ = rootFields.ProviderCode; });
            Assert.That(exception.Message, Does.Contain("providerCode"));
        }

        /// <summary>
        /// Tests that ProviderCode throws FieldNotFoundException when a different field exists but "providerCode" is missing.
        /// Input: FieldCollection with other fields but not "providerCode".
        /// Expected: Throws FieldNotFoundException.
        /// </summary>
        [Test]
        public void ProviderCode_WhenOtherFieldsExistButProviderCodeMissing_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = CreateFieldCollection("bankCode", "BANK123");
            var rootFields = new RootFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() => { var _ = rootFields.ProviderCode; });
            Assert.That(exception.Message, Does.Contain("providerCode"));
        }

        /// <summary>
        /// Tests that ProviderCode correctly handles non-string object values by calling ToString().
        /// Input: FieldCollection with "providerCode" field containing a non-string object (e.g., integer).
        /// Expected: Returns the string representation of the object.
        /// </summary>
        [TestCase(12345, "12345")]
        [TestCase(true, "True")]
        [TestCase(3.14159, "3.14159")]
        public void ProviderCode_WhenFieldContainsNonStringObject_ReturnsToStringRepresentation(object value, string expectedString)
        {
            // Arrange
            var fieldCollection = CreateFieldCollection("providerCode", value);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProviderCode;

            // Assert
            Assert.That(result, Is.EqualTo(expectedString));
        }

        /// <summary>
        /// Helper method to create a FieldCollection with a single field.
        /// Note: This method uses the internal Add method of FieldCollection.
        /// The test assembly must have InternalsVisibleTo access to OmniGenerator.Lib assembly.
        /// </summary>
        /// <param name="fieldName">The name of the field to add.</param>
        /// <param name="fieldValue">The value of the field to add.</param>
        /// <returns>A FieldCollection containing the specified field.</returns>
        private FieldCollection CreateFieldCollection(string fieldName, object? fieldValue)
        {
            var collection = new FieldCollection();
            var field = new Field(fieldName, fieldValue!);

            // Note: This requires InternalsVisibleTo attribute in OmniGenerator.Lib assembly
            // to expose internal members to this test assembly.
            // Add the following to OmniGenerator.Lib's AssemblyInfo.cs or .csproj:
            // [assembly: InternalsVisibleTo("OmniGenerator.Plugins.Tessi.Test")]
            collection.Add(field);

            return collection;
        }

        /// <summary>
        /// Tests that the BankCode property returns the expected string value
        /// when the field exists in the collection with various valid values.
        /// </summary>
        /// <param name="value">The value to store in the bankCode field.</param>
        [TestCase("ABC123")]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("Bank-Code_With.Special!Chars@#$%^&*()")]
        [TestCase("0")]
        [TestCase("12345")]
        [TestCase("\t\n\r")]
        public void BankCode_WithVariousValues_ReturnsExpectedValue(string value)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("bankCode", value);
            fieldCollection.Add(field);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.BankCode;

            // Assert
            Assert.That(result, Is.EqualTo(value));
        }

        /// <summary>
        /// Tests that the BankCode property returns an empty string
        /// when the field exists but has a null value.
        /// </summary>
        [Test]
        public void BankCode_WithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var field = new Field("bankCode", null!);
            fieldCollection.Add(field);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.BankCode;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the BankCode property returns the full string value
        /// when the field contains a very long string.
        /// </summary>
        [Test]
        public void BankCode_WithVeryLongString_ReturnsFullValue()
        {
            // Arrange
            var longValue = new string('A', 10000);
            var fieldCollection = new FieldCollection();
            var field = new Field("bankCode", longValue);
            fieldCollection.Add(field);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.BankCode;

            // Assert
            Assert.That(result, Is.EqualTo(longValue));
        }

        /// <summary>
        /// Tests that the BankCode property throws a FieldNotFoundException
        /// when the bankCode field does not exist in the collection.
        /// </summary>
        [Test]
        public void BankCode_WhenFieldNotFound_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var rootFields = new RootFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() =>
            {
                var _ = rootFields.BankCode;
            });

            Assert.That(exception.Message, Does.Contain("bankCode"));
        }

        /// <summary>
        /// Tests that the BankCode property returns the correct string representation
        /// when the field contains a non-string object value.
        /// </summary>
        [Test]
        public void BankCode_WithNonStringObjectValue_ReturnsToStringResult()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var numericValue = 12345;
            var field = new Field("bankCode", numericValue);
            fieldCollection.Add(field);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.BankCode;

            // Assert
            Assert.That(result, Is.EqualTo("12345"));
        }

        /// <summary>
        /// Tests that the BankCode property handles Unicode characters correctly.
        /// </summary>
        [Test]
        public void BankCode_WithUnicodeCharacters_ReturnsValue()
        {
            // Arrange
            var unicodeValue = "Bänk-Côde-日本-🏦";
            var fieldCollection = new FieldCollection();
            var field = new Field("bankCode", unicodeValue);
            fieldCollection.Add(field);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.BankCode;

            // Assert
            Assert.That(result, Is.EqualTo(unicodeValue));
        }

        /// <summary>
        /// Tests that the BankCode property handles control characters correctly.
        /// </summary>
        [TestCase("\0")]
        [TestCase("\b")]
        [TestCase("\f")]
        public void BankCode_WithControlCharacters_ReturnsValue(string controlChar)
        {
            // Arrange
            var valueWithControlChar = $"Bank{controlChar}Code";
            var fieldCollection = new FieldCollection();
            var field = new Field("bankCode", valueWithControlChar);
            fieldCollection.Add(field);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.BankCode;

            // Assert
            Assert.That(result, Is.EqualTo(valueWithControlChar));
        }

        /// <summary>
        /// Tests that BankUnitCode returns the correct value when the field exists with a valid string value.
        /// </summary>
        [Test]
        public void BankUnitCode_ValidFieldExists_ReturnsFieldValue()
        {
            // Arrange
            var fieldCollection = CreateFieldCollection("bankUnitCode", "UNIT123");
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.BankUnitCode;

            // Assert
            Assert.That(result, Is.EqualTo("UNIT123"));
        }

        /// <summary>
        /// Tests that BankUnitCode throws FieldNotFoundException when the field does not exist.
        /// </summary>
        [Test]
        public void BankUnitCode_FieldDoesNotExist_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var rootFields = new RootFields(fieldCollection);

            // Act & Assert
            var ex = Assert.Throws<FieldNotFoundException>(() => _ = rootFields.BankUnitCode);
            Assert.That(ex?.Message, Does.Contain("bankUnitCode"));
        }

        /// <summary>
        /// Tests that BankUnitCode returns empty string when the field exists with a null value.
        /// Per Field.StringValue implementation, null values are converted to empty string.
        /// </summary>
        [Test]
        public void BankUnitCode_FieldExistsWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = CreateFieldCollection("bankUnitCode", null);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.BankUnitCode;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that BankUnitCode returns the value correctly for various edge case string values.
        /// </summary>
        /// <param name="value">The field value to test.</param>
        /// <param name="expected">The expected return value.</param>
        [TestCase("", "", Description = "Empty string")]
        [TestCase("   ", "   ", Description = "Whitespace only")]
        [TestCase("A", "A", Description = "Single character")]
        [TestCase("UNIT-CODE_123.ABC", "UNIT-CODE_123.ABC", Description = "Special characters")]
        [TestCase("Unit\nCode\tWith\rControl", "Unit\nCode\tWith\rControl", Description = "Control characters")]
        public void BankUnitCode_VariousStringValues_ReturnsExpectedValue(string value, string expected)
        {
            // Arrange
            var fieldCollection = CreateFieldCollection("bankUnitCode", value);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.BankUnitCode;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that BankUnitCode correctly handles very long string values.
        /// </summary>
        [Test]
        public void BankUnitCode_VeryLongString_ReturnsFullValue()
        {
            // Arrange
            var longValue = new string('X', 10000);
            var fieldCollection = CreateFieldCollection("bankUnitCode", longValue);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.BankUnitCode;

            // Assert
            Assert.That(result, Is.EqualTo(longValue));
            Assert.That(result.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that BankUnitCode correctly converts non-string object values to string using ToString.
        /// </summary>
        [Test]
        public void BankUnitCode_NumericValue_ReturnsStringRepresentation()
        {
            // Arrange
            var fieldCollection = CreateFieldCollection("bankUnitCode", 12345);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.BankUnitCode;

            // Assert
            Assert.That(result, Is.EqualTo("12345"));
        }

        /// <summary>
        /// Tests that the RootFields constructor successfully creates an instance when provided with a valid FieldCollection.
        /// </summary>
        [Test]
        public void Constructor_WithValidFieldCollection_CreatesInstance()
        {
            // Arrange
            var fieldCollection = new FieldCollection();

            // Act
            var rootFields = new RootFields(fieldCollection);

            // Assert
            Assert.That(rootFields, Is.Not.Null);
            Assert.That(rootFields, Is.InstanceOf<RootFields>());
        }

        /// <summary>
        /// Tests that the RootFields constructor successfully creates an instance when provided with an empty FieldCollection.
        /// Verifies that an empty collection is a valid input for construction.
        /// </summary>
        [Test]
        public void Constructor_WithEmptyFieldCollection_CreatesInstance()
        {
            // Arrange
            var emptyFieldCollection = new FieldCollection();

            // Act
            var rootFields = new RootFields(emptyFieldCollection);

            // Assert
            Assert.That(rootFields, Is.Not.Null);
            Assert.That(emptyFieldCollection.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that the RootFields constructor does not throw an exception when provided with a null FieldCollection.
        /// Note: While the parameter is non-nullable, null can still be passed at runtime. The constructor completes,
        /// but any subsequent property access will fail with NullReferenceException.
        /// </summary>
        [Test]
        public void Constructor_WithNullFieldCollection_DoesNotThrowDuringConstruction()
        {
            // Arrange
            FieldCollection? nullFieldCollection = null;

            // Act & Assert
            Assert.DoesNotThrow(() => new RootFields(nullFieldCollection!));
        }

        /// <summary>
        /// Tests that the Purpose property returns the expected string value when the field exists with a valid value.
        /// </summary>
        [Test]
        public void Purpose_ValidValue_ReturnsExpectedString()
        {
            // Arrange
            var fieldCollection = CreateFieldCollectionWithField("purpose", "Transaction Purpose");
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Purpose;

            // Assert
            Assert.That(result, Is.EqualTo("Transaction Purpose"));
        }

        /// <summary>
        /// Tests that the Purpose property returns an empty string when the field exists but contains an empty value.
        /// </summary>
        [Test]
        public void Purpose_EmptyString_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = CreateFieldCollectionWithField("purpose", string.Empty);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Purpose;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Purpose property returns whitespace when the field contains only whitespace characters.
        /// </summary>
        [Test]
        public void Purpose_WhitespaceString_ReturnsWhitespace()
        {
            // Arrange
            var fieldCollection = CreateFieldCollectionWithField("purpose", "   \t\n  ");
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Purpose;

            // Assert
            Assert.That(result, Is.EqualTo("   \t\n  "));
        }

        /// <summary>
        /// Tests that the Purpose property correctly handles and returns strings containing special characters.
        /// </summary>
        [Test]
        public void Purpose_SpecialCharacters_ReturnsSpecialCharacters()
        {
            // Arrange
            var fieldCollection = CreateFieldCollectionWithField("purpose", "Purpose: <Test> & \"Value\" with 'quotes' | symbols!");
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Purpose;

            // Assert
            Assert.That(result, Is.EqualTo("Purpose: <Test> & \"Value\" with 'quotes' | symbols!"));
        }

        /// <summary>
        /// Tests that the Purpose property correctly handles very long strings.
        /// </summary>
        [Test]
        public void Purpose_VeryLongString_ReturnsLongString()
        {
            // Arrange
            var longString = new string('A', 10000);
            var fieldCollection = CreateFieldCollectionWithField("purpose", longString);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Purpose;

            // Assert
            Assert.That(result, Is.EqualTo(longString));
            Assert.That(result.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that the Purpose property throws FieldNotFoundException when the "purpose" field does not exist in the collection.
        /// </summary>
        [Test]
        public void Purpose_FieldNotFound_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection(); // Empty collection
            var rootFields = new RootFields(fieldCollection);

            // Act & Assert
            var ex = Assert.Throws<FieldNotFoundException>(() => { var _ = rootFields.Purpose; });
            Assert.That(ex?.Message, Does.Contain("purpose"));
        }

        /// <summary>
        /// Tests that the Purpose property returns an empty string when the field exists but has a null value.
        /// Field.StringValue converts null values to empty strings.
        /// </summary>
        [Test]
        public void Purpose_NullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = CreateFieldCollectionWithField("purpose", null);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Purpose;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Purpose property correctly handles unicode characters.
        /// </summary>
        [Test]
        public void Purpose_UnicodeCharacters_ReturnsUnicodeString()
        {
            // Arrange
            var unicodeString = "目的: Транзакция Çü€";
            var fieldCollection = CreateFieldCollectionWithField("purpose", unicodeString);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Purpose;

            // Assert
            Assert.That(result, Is.EqualTo(unicodeString));
        }

        /// <summary>
        /// Tests that the Purpose property correctly handles numeric values by converting them to strings.
        /// </summary>
        [Test]
        public void Purpose_NumericValue_ReturnsStringRepresentation()
        {
            // Arrange
            var fieldCollection = CreateFieldCollectionWithField("purpose", 12345);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Purpose;

            // Assert
            Assert.That(result, Is.EqualTo("12345"));
        }

        /// <summary>
        /// Helper method to create a FieldCollection with a single field.
        /// NOTE: This method uses the internal Add method of FieldCollection.
        /// If compilation fails, ensure InternalsVisibleTo is configured for the test assembly,
        /// or that the test assembly has appropriate access to internal members.
        /// </summary>
        /// <param name="fieldName">The name of the field to add.</param>
        /// <param name="fieldValue">The value of the field to add.</param>
        /// <returns>A FieldCollection containing the specified field.</returns>
        private FieldCollection CreateFieldCollectionWithField(string fieldName, object? fieldValue)
        {
            var fieldCollection = new FieldCollection();
            var field = new Field(fieldName, fieldValue!);

            // Using reflection to access internal Add method if InternalsVisibleTo is not configured
            var addMethod = typeof(FieldCollection).GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (addMethod != null)
            {
                addMethod.Invoke(fieldCollection, new object[] { field });
            }
            else
            {
                // If InternalsVisibleTo is configured, this direct call should work:
                // fieldCollection.Add(field);

                // Fallback: If neither works, tests will fail with clear error messages
                throw new InvalidOperationException(
                    "Cannot access FieldCollection.Add method. " +
                    "Please ensure InternalsVisibleTo attribute is configured in OmniGenerator.Lib assembly " +
                    "to grant access to this test assembly, or modify the test approach.");
            }

            return fieldCollection;
        }

        /// <summary>
        /// Tests that the Culture property returns the string value of the "culture" field
        /// when the field exists in the collection with various valid and edge-case values.
        /// </summary>
        /// <param name="cultureValue">The culture value to test.</param>
        [TestCase("fr-FR")]
        [TestCase("en-US")]
        [TestCase("de-DE")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("\t")]
        [TestCase("a very long culture string that exceeds typical length expectations for culture codes but should still be handled correctly by the system without any issues")]
        [TestCase("@#$%^&*()")]
        [TestCase("日本語")]
        [TestCase("Ελληνικά")]
        public void Culture_FieldExistsWithValue_ReturnsStringValue(string cultureValue)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("culture", cultureValue));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Culture;

            // Assert
            Assert.That(result, Is.EqualTo(cultureValue));
        }

        /// <summary>
        /// Tests that the Culture property returns an empty string when the "culture" field
        /// exists but has a null value, as per Field.StringValue implementation.
        /// </summary>
        [Test]
        public void Culture_FieldExistsWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("culture", null!));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Culture;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the Culture property throws FieldNotFoundException when the "culture"
        /// field does not exist in the collection.
        /// </summary>
        [Test]
        public void Culture_FieldDoesNotExist_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var rootFields = new RootFields(fieldCollection);

            // Act & Assert
            var exception = Assert.Throws<FieldNotFoundException>(() => { var _ = rootFields.Culture; });
            Assert.That(exception!.Message, Does.Contain("culture"));
        }

        /// <summary>
        /// Creates an empty FieldCollection.
        /// </summary>
        /// <returns>An empty FieldCollection.</returns>
        private static FieldCollection CreateEmptyFieldCollection()
        {
            return new FieldCollection();
        }

        /// <summary>
        /// Tests that BankFlow returns the correct value when the field exists with a valid string.
        /// </summary>
        [Test]
        public void BankFlow_WhenFieldExistsWithValidValue_ReturnsValue()
        {
            // Arrange
            var expectedValue = "BANK_FLOW_123";
            var fields = CreateFieldCollection("bankFlow", expectedValue);
            var rootFields = new RootFields(fields);

            // Act
            var result = rootFields.BankFlow;

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Tests that BankFlow throws FieldNotFoundException when the field does not exist in the collection.
        /// </summary>
        [Test]
        public void BankFlow_WhenFieldDoesNotExist_ThrowsFieldNotFoundException()
        {
            // Arrange
            var fields = CreateEmptyFieldCollection();
            var rootFields = new RootFields(fields);

            // Act & Assert
            var ex = Assert.Throws<FieldNotFoundException>(() => { var _ = rootFields.BankFlow; });
            Assert.That(ex.Message, Does.Contain("bankFlow"));
        }

        /// <summary>
        /// Tests that BankFlow returns an empty string when the field value is an empty string.
        /// </summary>
        [Test]
        public void BankFlow_WhenFieldValueIsEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var fields = CreateFieldCollection("bankFlow", string.Empty);
            var rootFields = new RootFields(fields);

            // Act
            var result = rootFields.BankFlow;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that BankFlow returns whitespace when the field value contains only whitespace.
        /// </summary>
        [Test]
        public void BankFlow_WhenFieldValueIsWhitespace_ReturnsWhitespace()
        {
            // Arrange
            var whitespaceValue = "   \t\n  ";
            var fields = CreateFieldCollection("bankFlow", whitespaceValue);
            var rootFields = new RootFields(fields);

            // Act
            var result = rootFields.BankFlow;

            // Assert
            Assert.That(result, Is.EqualTo(whitespaceValue));
        }

        /// <summary>
        /// Tests that BankFlow returns an empty string when the field value is null.
        /// This tests the Field.StringValue behavior which converts null to empty string.
        /// </summary>
        [Test]
        public void BankFlow_WhenFieldValueIsNull_ReturnsEmptyString()
        {
            // Arrange
            var fields = CreateFieldCollection("bankFlow", null);
            var rootFields = new RootFields(fields);

            // Act
            var result = rootFields.BankFlow;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that BankFlow correctly handles very long string values.
        /// </summary>
        [Test]
        public void BankFlow_WhenFieldValueIsVeryLongString_ReturnsValue()
        {
            // Arrange
            var longValue = new string('A', 10000);
            var fields = CreateFieldCollection("bankFlow", longValue);
            var rootFields = new RootFields(fields);

            // Act
            var result = rootFields.BankFlow;

            // Assert
            Assert.That(result, Is.EqualTo(longValue));
            Assert.That(result.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that BankFlow correctly handles string values with special characters.
        /// </summary>
        [TestCase("bank!@#$%^&*()")]
        [TestCase("bank<>?:\"{}|")]
        [TestCase("банк流程")]
        [TestCase("🏦💰")]
        public void BankFlow_WhenFieldValueContainsSpecialCharacters_ReturnsValue(string specialValue)
        {
            // Arrange
            var fields = CreateFieldCollection("bankFlow", specialValue);
            var rootFields = new RootFields(fields);

            // Act
            var result = rootFields.BankFlow;

            // Assert
            Assert.That(result, Is.EqualTo(specialValue));
        }

        /// <summary>
        /// Tests that BankFlow correctly handles string values with control characters.
        /// </summary>
        [Test]
        public void BankFlow_WhenFieldValueContainsControlCharacters_ReturnsValue()
        {
            // Arrange
            var controlValue = "bank\0flow\r\n\t";
            var fields = CreateFieldCollection("bankFlow", controlValue);
            var rootFields = new RootFields(fields);

            // Act
            var result = rootFields.BankFlow;

            // Assert
            Assert.That(result, Is.EqualTo(controlValue));
        }

        /// <summary>
        /// Tests that BankFlow correctly handles non-string objects by converting them to string.
        /// Field.StringValue calls ToString() on the value.
        /// </summary>
        [TestCase(123, "123")]
        [TestCase(45.67, "45.67")]
        [TestCase(true, "True")]
        public void BankFlow_WhenFieldValueIsNonString_ReturnsStringRepresentation(object value, string expectedString)
        {
            // Arrange
            var fields = CreateFieldCollection("bankFlow", value);
            var rootFields = new RootFields(fields);

            // Act
            var result = rootFields.BankFlow;

            // Assert
            Assert.That(result, Is.EqualTo(expectedString));
        }

        /// <summary>
        /// Tests that BankFlow property can be accessed multiple times and returns consistent results.
        /// </summary>
        [Test]
        public void BankFlow_WhenAccessedMultipleTimes_ReturnsConsistentValue()
        {
            // Arrange
            var expectedValue = "CONSISTENT_FLOW";
            var fields = CreateFieldCollection("bankFlow", expectedValue);
            var rootFields = new RootFields(fields);

            // Act
            var result1 = rootFields.BankFlow;
            var result2 = rootFields.BankFlow;
            var result3 = rootFields.BankFlow;

            // Assert
            Assert.That(result1, Is.EqualTo(expectedValue));
            Assert.That(result2, Is.EqualTo(expectedValue));
            Assert.That(result3, Is.EqualTo(expectedValue));
            Assert.That(result1, Is.SameAs(result2));
            Assert.That(result2, Is.SameAs(result3));
        }

    }
}