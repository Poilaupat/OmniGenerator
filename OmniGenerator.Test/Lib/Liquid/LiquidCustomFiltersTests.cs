using DotLiquid;
using NUnit.Framework;
using OmniGenerator.Lib.Liquid;

namespace OmniGenerator.Test.Lib.Liquid
{
    [TestFixture]
    public class LiquidCustomFiltersTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Register custom filters
            Template.RegisterFilter(typeof(LiquidCustomFilters));
        }

        #region Format Filter Tests

        [Test]
        public void Format_WithInteger_FormatsCorrectly()
        {
            // Arrange
            var template = Template.Parse("{{value | format:'D4'}}");

            // Act
            var result = template.Render(Hash.FromAnonymousObject(new { value = 42 }));

            // Assert
            Assert.That(result, Is.EqualTo("0042"));
        }

        [Test]
        public void Format_WithDecimal_FormatsCorrectly()
        {
            // Arrange
            var template = Template.Parse("{{value | format:'F2'}}");

            // Act
            var result = template.Render(Hash.FromAnonymousObject(new { value = 123.456 }));

            // Assert
            // Note: Result depends on current culture (. or , as decimal separator)
            Assert.That(result, Does.Contain("123"));
            Assert.That(result, Does.Contain("46"));
        }

        [Test]
        public void Format_WithNullValue_ReturnsEmptyString()
        {
            // Arrange
            var template = Template.Parse("{{value | format:'D4'}}");

            // Act
            var result = template.Render(Hash.FromAnonymousObject(new { value = (object?)null }));

            // Assert
            Assert.That(result, Is.Empty);
        }

        #endregion

        #region PadLeft Filter Tests

        [Test]
        public void PadLeft_WithInteger_PadsWithZeros()
        {
            // Arrange
            var template = Template.Parse("{{value | pad_left:6}}");

            // Act
            var result = template.Render(Hash.FromAnonymousObject(new { value = 123 }));

            // Assert
            Assert.That(result, Is.EqualTo("000123"));
        }

        [Test]
        public void PadLeft_WithString_PadsWithZeros()
        {
            // Arrange
            var template = Template.Parse("{{value | pad_left:4}}");

            // Act
            var result = template.Render(Hash.FromAnonymousObject(new { value = "42" }));

            // Assert
            Assert.That(result, Is.EqualTo("0042"));
        }

        [Test]
        public void PadLeft_WithCustomChar_PadsWithSpecifiedChar()
        {
            // Arrange
            var template = Template.Parse("{{value | pad_left:5,'X'}}");

            // Act
            var result = template.Render(Hash.FromAnonymousObject(new { value = "42" }));

            // Assert
            Assert.That(result, Is.EqualTo("XXX42"));
        }

        [Test]
        public void PadLeft_WithLongerStringThanPadding_ReturnsOriginalString()
        {
            // Arrange
            var template = Template.Parse("{{value | pad_left:3}}");

            // Act
            var result = template.Render(Hash.FromAnonymousObject(new { value = "12345" }));

            // Assert
            Assert.That(result, Is.EqualTo("12345"));
        }

        [Test]
        public void PadLeft_WithNullValue_ReturnsPaddedEmptyString()
        {
            // Arrange
            var template = Template.Parse("{{value | pad_left:4}}");

            // Act
            var result = template.Render(Hash.FromAnonymousObject(new { value = (object?)null }));

            // Assert
            Assert.That(result, Is.EqualTo("0000"));
        }

        #endregion

        #region PadRight Filter Tests

        [Test]
        public void PadRight_WithString_PadsWithSpaces()
        {
            // Arrange
            var template = Template.Parse("{{value | pad_right:10}}");

            // Act
            var result = template.Render(Hash.FromAnonymousObject(new { value = "John" }));

            // Assert
            Assert.That(result, Is.EqualTo("John      "));
        }

        [Test]
        public void PadRight_WithCustomChar_PadsWithSpecifiedChar()
        {
            // Arrange
            var template = Template.Parse("{{value | pad_right:10,'0'}}");

            // Act
            var result = template.Render(Hash.FromAnonymousObject(new { value = "ABC" }));

            // Assert
            Assert.That(result, Is.EqualTo("ABC0000000"));
        }

        [Test]
        public void PadRight_WithNullValue_ReturnsPaddedSpaces()
        {
            // Arrange
            var template = Template.Parse("{{value | pad_right:5}}");

            // Act
            var result = template.Render(Hash.FromAnonymousObject(new { value = (object?)null }));

            // Assert
            Assert.That(result, Is.EqualTo("     "));
        }

        #endregion

        #region Integration Tests

        [Test]
        public void ComplexTemplate_WithMultipleFilters_FormatsCorrectly()
        {
            // Arrange
            var template = Template.Parse("{{date | date:'yyMMdd'}}{{bank | pad_left:5}}{{agency | pad_left:5}}{{number | pad_left:4}}");

            // Act
            var result = template.Render(Hash.FromAnonymousObject(new
            {
                date = new DateTime(2024, 1, 15),
                bank = "16038",
                agency = "16001",
                number = 42
            }));

            // Assert
            Assert.That(result, Is.EqualTo("24011516038160010042"));
        }

        [Test]
        public void ChainingFilters_WorksCorrectly()
        {
            // Arrange
            var template = Template.Parse("{{value | format:'D4' | prepend:'ID-'}}");

            // Act
            var result = template.Render(Hash.FromAnonymousObject(new { value = 42 }));

            // Assert
            Assert.That(result, Is.EqualTo("ID-0042"));
        }

        #endregion
    }
}
