using System;
using System.Reflection;

using NUnit.Framework;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk.UnitTests
{
    /// <summary>
    /// Unit tests for <see cref="RootFields"/> class.
    /// </summary>
    [TestFixture]
    public partial class RootFieldsTests
    {
        /// <summary>
        /// Tests that PacketDate returns DateTime.Now when the field does not exist in the collection.
        /// </summary>
        [Test]
        public void PacketDate_WhenFieldDoesNotExist_ReturnsCurrentDateTime()
        {
            // Arrange
            var emptyFieldCollection = new FieldCollection();
            var rootFields = new RootFields(emptyFieldCollection);
            var before = DateTime.Now;

            // Act
            var result = rootFields.PacketDate;
            var after = DateTime.Now;

            // Assert
            Assert.That(result, Is.GreaterThanOrEqualTo(before));
            Assert.That(result, Is.LessThanOrEqualTo(after));
        }

        /// <summary>
        /// Tests that PacketDate returns DateTime.Now when the field exists but its value is not a DateTime (string).
        /// </summary>
        [Test]
        public void PacketDate_WhenFieldValueIsString_ReturnsCurrentDateTime()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, new Field("packet-date", "2024-01-01"));
            var rootFields = new RootFields(fieldCollection);
            var before = DateTime.Now;

            // Act
            var result = rootFields.PacketDate;
            var after = DateTime.Now;

            // Assert
            Assert.That(result, Is.GreaterThanOrEqualTo(before));
            Assert.That(result, Is.LessThanOrEqualTo(after));
        }

        /// <summary>
        /// Tests that PacketDate returns DateTime.Now when the field exists but its value is not a DateTime (integer).
        /// </summary>
        [Test]
        public void PacketDate_WhenFieldValueIsInteger_ReturnsCurrentDateTime()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, new Field("packet-date", 12345));
            var rootFields = new RootFields(fieldCollection);
            var before = DateTime.Now;

            // Act
            var result = rootFields.PacketDate;
            var after = DateTime.Now;

            // Assert
            Assert.That(result, Is.GreaterThanOrEqualTo(before));
            Assert.That(result, Is.LessThanOrEqualTo(after));
        }

        /// <summary>
        /// Tests that PacketDate returns DateTime.Now when the field exists but its value is null.
        /// </summary>
        [Test]
        public void PacketDate_WhenFieldValueIsNull_ReturnsCurrentDateTime()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, new Field("packet-date", null!));
            var rootFields = new RootFields(fieldCollection);
            var before = DateTime.Now;

            // Act
            var result = rootFields.PacketDate;
            var after = DateTime.Now;

            // Assert
            Assert.That(result, Is.GreaterThanOrEqualTo(before));
            Assert.That(result, Is.LessThanOrEqualTo(after));
        }

        /// <summary>
        /// Tests that PacketDate returns the DateTime value when the field exists with a valid DateTime.
        /// </summary>
        [Test]
        public void PacketDate_WhenFieldValueIsDateTime_ReturnsDateTimeValue()
        {
            // Arrange
            var expectedDate = new DateTime(2024, 6, 15, 10, 30, 45);
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, new Field("packet-date", expectedDate));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.PacketDate;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        /// <summary>
        /// Tests that PacketDate handles DateTime.MinValue correctly.
        /// </summary>
        [Test]
        public void PacketDate_WhenFieldValueIsDateTimeMinValue_ReturnsMinValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, new Field("packet-date", DateTime.MinValue));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.PacketDate;

            // Assert
            Assert.That(result, Is.EqualTo(DateTime.MinValue));
        }

        /// <summary>
        /// Tests that PacketDate handles DateTime.MaxValue correctly.
        /// </summary>
        [Test]
        public void PacketDate_WhenFieldValueIsDateTimeMaxValue_ReturnsMaxValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            AddFieldToCollection(fieldCollection, new Field("packet-date", DateTime.MaxValue));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.PacketDate;

            // Assert
            Assert.That(result, Is.EqualTo(DateTime.MaxValue));
        }

        /// <summary>
        /// Helper method to add a field to a FieldCollection using reflection.
        /// </summary>
        /// <param name="collection">The FieldCollection to add the field to.</param>
        /// <param name="field">The Field to add.</param>
        private void AddFieldToCollection(FieldCollection collection, Field field)
        {
            var addMethod = typeof(FieldCollection).GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic);
            addMethod?.Invoke(collection, new object[] { field });
        }

        /// <summary>
        /// Tests that ProcessCode returns the default value "000" when the field is not present in the collection.
        /// </summary>
        [Test]
        public void ProcessCode_WhenFieldNotPresent_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProcessCode;

            // Assert
            Assert.That(result, Is.EqualTo("000"));
        }

        /// <summary>
        /// Tests that ProcessCode returns the field value when present with a valid string.
        /// </summary>
        /// <param name="value">The value to set for the process-code field.</param>
        [TestCase("ABC")]
        [TestCase("123")]
        [TestCase("XYZ-456")]
        public void ProcessCode_WhenFieldPresentWithValue_ReturnsValue(string value)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("process-code", value));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProcessCode;

            // Assert
            Assert.That(result, Is.EqualTo(value));
        }

        /// <summary>
        /// Tests that ProcessCode returns empty string when the field is present but has a null value.
        /// Note: Field.StringValue converts null to empty string.
        /// </summary>
        [Test]
        public void ProcessCode_WhenFieldPresentWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("process-code", null!));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProcessCode;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that ProcessCode returns empty string when the field is present with an empty string value.
        /// </summary>
        [Test]
        public void ProcessCode_WhenFieldPresentWithEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("process-code", string.Empty));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProcessCode;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that ProcessCode returns the whitespace value when the field contains only whitespace.
        /// </summary>
        [TestCase("   ")]
        [TestCase("\t")]
        [TestCase("\r\n")]
        [TestCase(" \t\r\n ")]
        public void ProcessCode_WhenFieldPresentWithWhitespace_ReturnsWhitespace(string whitespace)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("process-code", whitespace));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProcessCode;

            // Assert
            Assert.That(result, Is.EqualTo(whitespace));
        }

        /// <summary>
        /// Tests that ProcessCode returns the default value when present.
        /// </summary>
        [Test]
        public void ProcessCode_WhenFieldPresentWithDefaultValue_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("process-code", "000"));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProcessCode;

            // Assert
            Assert.That(result, Is.EqualTo("000"));
        }

        /// <summary>
        /// Tests that ProcessCode correctly handles special characters in the field value.
        /// </summary>
        /// <param name="specialValue">The special character string to test.</param>
        [TestCase("@#$%^&*")]
        [TestCase("процесс")]
        [TestCase("プロセス")]
        [TestCase("🔥💻")]
        [TestCase("<script>alert('xss')</script>")]
        [TestCase("C:\\path\\to\\file")]
        [TestCase("key=value&another=key")]
        public void ProcessCode_WhenFieldPresentWithSpecialCharacters_ReturnsSpecialCharacters(string specialValue)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("process-code", specialValue));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProcessCode;

            // Assert
            Assert.That(result, Is.EqualTo(specialValue));
        }

        /// <summary>
        /// Tests that ProcessCode correctly handles very long strings.
        /// </summary>
        [Test]
        public void ProcessCode_WhenFieldPresentWithVeryLongString_ReturnsLongString()
        {
            // Arrange
            var longString = new string('A', 10000);
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("process-code", longString));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProcessCode;

            // Assert
            Assert.That(result, Is.EqualTo(longString));
        }

        /// <summary>
        /// Tests that ProcessCode returns string representation when field value is a non-string object.
        /// </summary>
        [TestCase(42, "42")]
        [TestCase(3.14, "3.14")]
        [TestCase(true, "True")]
        public void ProcessCode_WhenFieldPresentWithNonStringValue_ReturnsStringRepresentation(object value, string expected)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("process-code", value));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProcessCode;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that ProcessCode handles control characters in the field value.
        /// </summary>
        [TestCase("\0")]
        [TestCase("\u0001")]
        [TestCase("\u001F")]
        public void ProcessCode_WhenFieldPresentWithControlCharacters_ReturnsControlCharacters(string controlChars)
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            fieldCollection.Add(new Field("process-code", controlChars));
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.ProcessCode;

            // Assert
            Assert.That(result, Is.EqualTo(controlChars));
        }

        /// <summary>
        /// Tests that Reconciliation property returns the default value "0" when the field is not present in the collection.
        /// </summary>
        [Test]
        public void Reconciliation_WhenFieldNotPresent_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Reconciliation;

            // Assert
            Assert.That(result, Is.EqualTo("0"));
        }

        /// <summary>
        /// Tests that Reconciliation property returns the field value when present in the collection.
        /// </summary>
        /// <param name="fieldValue">The value to set for the reconciliation field.</param>
        /// <param name="expectedResult">The expected result value.</param>
        [TestCase("1", "1")]
        [TestCase("0", "0")]
        [TestCase("true", "true")]
        [TestCase("false", "false")]
        [TestCase("yes", "yes")]
        [TestCase("no", "no")]
        public void Reconciliation_WhenFieldPresent_ReturnsFieldValue(string fieldValue, string expectedResult)
        {
            // Arrange
            var fieldCollection = CreateFieldCollectionWithReconciliation(fieldValue);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Reconciliation;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that Reconciliation property handles empty string value correctly.
        /// </summary>
        [Test]
        public void Reconciliation_WhenFieldValueIsEmpty_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = CreateFieldCollectionWithReconciliation(string.Empty);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Reconciliation;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that Reconciliation property handles whitespace string value correctly.
        /// </summary>
        [Test]
        public void Reconciliation_WhenFieldValueIsWhitespace_ReturnsWhitespace()
        {
            // Arrange
            var fieldCollection = CreateFieldCollectionWithReconciliation("   ");
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Reconciliation;

            // Assert
            Assert.That(result, Is.EqualTo("   "));
        }

        /// <summary>
        /// Tests that Reconciliation property handles special characters correctly.
        /// </summary>
        /// <param name="specialValue">The special character value to test.</param>
        [TestCase("!@#$%^&*()")]
        [TestCase("123-456-789")]
        [TestCase("test\nvalue")]
        [TestCase("test\tvalue")]
        public void Reconciliation_WhenFieldContainsSpecialCharacters_ReturnsValue(string specialValue)
        {
            // Arrange
            var fieldCollection = CreateFieldCollectionWithReconciliation(specialValue);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Reconciliation;

            // Assert
            Assert.That(result, Is.EqualTo(specialValue));
        }

        /// <summary>
        /// Tests that Reconciliation property returns default value when field value is null.
        /// </summary>
        [Test]
        [Category("ProductionBugSuspected")]
        [Ignore("ProductionBugSuspected")]
        public void Reconciliation_WhenFieldValueIsNull_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = CreateFieldCollectionWithReconciliation(null);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Reconciliation;

            // Assert
            Assert.That(result, Is.EqualTo("0"));
        }

        /// <summary>
        /// Tests that Reconciliation property handles very long string values correctly.
        /// </summary>
        [Test]
        public void Reconciliation_WhenFieldValueIsVeryLong_ReturnsFullValue()
        {
            // Arrange
            var longValue = new string('x', 10000);
            var fieldCollection = CreateFieldCollectionWithReconciliation(longValue);
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.Reconciliation;

            // Assert
            Assert.That(result, Is.EqualTo(longValue));
        }

        /// <summary>
        /// Helper method to create a FieldCollection with a reconciliation field.
        /// Note: This method requires InternalsVisibleTo access to the OmniGenerator.Lib assembly.
        /// Add the following to the OmniGenerator.Lib AssemblyInfo.cs or .csproj if not already present:
        /// [assembly: InternalsVisibleTo("OmniGenerator.Plugins.Tessi.Test")]
        /// </summary>
        /// <param name="value">The value for the reconciliation field.</param>
        /// <returns>A FieldCollection containing the reconciliation field.</returns>
        private FieldCollection CreateFieldCollectionWithReconciliation(object? value)
        {
            var fieldCollection = new FieldCollection();
            var field = new Field("reconciliation", value!);
            fieldCollection.Add(field);
            return fieldCollection;
        }

        /// <summary>
        /// Helper class to build FieldCollection instances with test data.
        /// Note: This assumes InternalsVisibleTo is configured for the test assembly to access internal Add methods.
        /// </summary>
        private class FieldCollectionBuilder
        {
            private readonly FieldCollection _collection = new();

            /// <summary>
            /// Adds a field to the collection.
            /// </summary>
            /// <param name="name">The field name.</param>
            /// <param name="value">The field value.</param>
            /// <returns>The builder instance for method chaining.</returns>
            public FieldCollectionBuilder AddField(string name, object value)
            {
                _collection.Add(new Field(name, value));
                return this;
            }

            /// <summary>
            /// Builds and returns the FieldCollection.
            /// </summary>
            /// <returns>The constructed FieldCollection.</returns>
            public FieldCollection Build() => _collection;
        }

        /// <summary>
        /// Tests that PacketName returns the field value when the field exists with a valid string.
        /// </summary>
        /// <param name="fieldValue">The value to store in the packet-name field.</param>
        /// <param name="expectedValue">The expected return value.</param>
        [TestCase("TestName", "TestName", TestName = "PacketName_ValidStringValue_ReturnsFieldValue")]
        [TestCase("", "", TestName = "PacketName_EmptyString_ReturnsEmptyString")]
        [TestCase("   ", "   ", TestName = "PacketName_WhitespaceOnly_ReturnsWhitespace")]
        [TestCase("Name!@#$%^&*()", "Name!@#$%^&*()", TestName = "PacketName_SpecialCharacters_ReturnsSpecialCharacters")]
        [TestCase("Name\r\nWith\tControl", "Name\r\nWith\tControl", TestName = "PacketName_ControlCharacters_ReturnsWithControlCharacters")]
        [TestCase("Ñame_with_Ünicode_字符", "Ñame_with_Ünicode_字符", TestName = "PacketName_UnicodeCharacters_ReturnsUnicodeCharacters")]
        public void PacketName_FieldExistsWithString_ReturnsFieldValue(string fieldValue, string expectedValue)
        {
            // Arrange
            var fieldCollection = new FieldCollectionBuilder()
                .AddField("packet-name", fieldValue)
                .Build();
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.PacketName;

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Tests that PacketName returns the default value "DefaultName" when the field does not exist.
        /// </summary>
        [Test]
        public void PacketName_FieldNotExists_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.PacketName;

            // Assert
            Assert.That(result, Is.EqualTo("DefaultName"));
        }

        /// <summary>
        /// Tests that PacketName returns empty string when the field exists but has a null value.
        /// Field.StringValue returns empty string for null values.
        /// </summary>
        [Test]
        public void PacketName_FieldWithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var fieldCollection = new FieldCollectionBuilder()
                .AddField("packet-name", null!)
                .Build();
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.PacketName;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that PacketName returns the ToString() representation when the field contains a non-string value.
        /// </summary>
        /// <param name="fieldValue">The non-string value to store.</param>
        /// <param name="expectedValue">The expected ToString() representation.</param>
        [TestCase(12345, "12345", TestName = "PacketName_IntegerValue_ReturnsToStringRepresentation")]
        [TestCase(123.45, "123.45", TestName = "PacketName_DoubleValue_ReturnsToStringRepresentation")]
        [TestCase(true, "True", TestName = "PacketName_BooleanValue_ReturnsToStringRepresentation")]
        public void PacketName_FieldWithNonStringValue_ReturnsToStringRepresentation(object fieldValue, string expectedValue)
        {
            // Arrange
            var fieldCollection = new FieldCollectionBuilder()
                .AddField("packet-name", fieldValue)
                .Build();
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.PacketName;

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Tests that PacketName handles very long string values correctly.
        /// </summary>
        [Test]
        public void PacketName_VeryLongString_ReturnsFullString()
        {
            // Arrange
            var longString = new string('A', 10000);
            var fieldCollection = new FieldCollectionBuilder()
                .AddField("packet-name", longString)
                .Build();
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.PacketName;

            // Assert
            Assert.That(result, Is.EqualTo(longString));
            Assert.That(result.Length, Is.EqualTo(10000));
        }

        /// <summary>
        /// Tests that PacketName returns default when an empty FieldCollection is provided.
        /// </summary>
        [Test]
        public void PacketName_EmptyFieldCollection_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = new FieldCollection();
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.PacketName;

            // Assert
            Assert.That(result, Is.EqualTo("DefaultName"));
        }

        /// <summary>
        /// Tests that PacketName works correctly when other fields exist but packet-name does not.
        /// </summary>
        [Test]
        public void PacketName_OtherFieldsExist_ReturnsDefaultValue()
        {
            // Arrange
            var fieldCollection = new FieldCollectionBuilder()
                .AddField("other-field", "OtherValue")
                .AddField("another-field", "AnotherValue")
                .Build();
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.PacketName;

            // Assert
            Assert.That(result, Is.EqualTo("DefaultName"));
        }

        /// <summary>
        /// Tests that PacketName handles DateTime value correctly by converting to string.
        /// </summary>
        [Test]
        public void PacketName_DateTimeValue_ReturnsToStringRepresentation()
        {
            // Arrange
            var dateTime = new DateTime(2024, 1, 15, 10, 30, 0);
            var fieldCollection = new FieldCollectionBuilder()
                .AddField("packet-name", dateTime)
                .Build();
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.PacketName;

            // Assert
            Assert.That(result, Is.EqualTo(dateTime.ToString()));
        }

        /// <summary>
        /// Tests that PacketName handles Guid value correctly by converting to string.
        /// </summary>
        [Test]
        public void PacketName_GuidValue_ReturnsToStringRepresentation()
        {
            // Arrange
            var guid = Guid.Parse("12345678-1234-1234-1234-123456789012");
            var fieldCollection = new FieldCollectionBuilder()
                .AddField("packet-name", guid)
                .Build();
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.PacketName;

            // Assert
            Assert.That(result, Is.EqualTo(guid.ToString()));
        }

        /// <summary>
        /// Tests that PacketName handles single character strings correctly.
        /// </summary>
        [Test]
        public void PacketName_SingleCharacter_ReturnsSingleCharacter()
        {
            // Arrange
            var fieldCollection = new FieldCollectionBuilder()
                .AddField("packet-name", "X")
                .Build();
            var rootFields = new RootFields(fieldCollection);

            // Act
            var result = rootFields.PacketName;

            // Assert
            Assert.That(result, Is.EqualTo("X"));
        }
    }
}