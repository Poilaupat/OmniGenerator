using NUnit.Framework;
using OmniGenerator.Lib.ErrorSimulation.Mutators;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Test.Lib.ErrorSimulation
{
    [TestFixture]
    public class FieldErrorMutatorTests
    {
        [Test]
        public void MisreadMutator_ShouldMutateDataChannelOnly_WhenFieldHasValue()
        {
            // Arrange
            var field = new Field("Amount", "123456");
            var mutator = new MisreadMutator();

            // Act
            mutator.Mutate(field);

            // Assert
            Assert.That(field.StringValue, Is.EqualTo("123456"), "Canonical value must never be mutated.");
            Assert.That(field.ImageStringValue, Is.EqualTo("123456"), "Image channel must be untouched.");
            Assert.That(field.DataStringValue, Does.Contain("?"), "Data channel should contain misread characters.");
            Assert.That(field.DataStringValue, Has.Length.EqualTo(6));
        }

        [Test]
        public void MisreadMutator_ShouldDoNothing_WhenFieldValueIsEmpty()
        {
            // Arrange
            var field = new Field("Empty", string.Empty);
            var mutator = new MisreadMutator();

            // Act
            mutator.Mutate(field);

            // Assert
            Assert.That(field.DataStringValue, Is.EqualTo(string.Empty));
        }

        [Test]
        public void SubstitutionMutator_ShouldSubstituteSingleCharacterInDataChannel_WhenConfusableCharPresent()
        {
            // Arrange
            var field = new Field("Number", "000000");
            var mutator = new SubstitutionMutator();

            // Act
            mutator.Mutate(field);

            // Assert
            Assert.That(field.StringValue, Is.EqualTo("000000"), "Canonical value must never be mutated.");
            Assert.That(field.ImageStringValue, Is.EqualTo("000000"), "Image channel must be untouched.");
            Assert.That(field.DataStringValue, Has.Length.EqualTo(6));
            Assert.That(field.DataStringValue.Count(c => c == '8'), Is.EqualTo(1), "Exactly one '0' should become '8'.");
        }

        [Test]
        public void SubstitutionMutator_ShouldLeaveDataChannelUnchanged_WhenNoConfusableCharacter()
        {
            // Arrange
            var field = new Field("Text", "abc def");
            var mutator = new SubstitutionMutator();

            // Act
            mutator.Mutate(field);

            // Assert
            Assert.That(field.DataStringValue, Is.EqualTo("abc def"));
        }

        [Test]
        public void InconsistencyMutator_ShouldMutateImageChannelOnly_WhenFieldHasDigits()
        {
            // Arrange
            var field = new Field("Amount", "123456");
            var mutator = new InconsistencyMutator();

            // Act
            mutator.Mutate(field);

            // Assert
            Assert.That(field.StringValue, Is.EqualTo("123456"), "Canonical value must never be mutated.");
            Assert.That(field.DataStringValue, Is.EqualTo("123456"), "Data channel must be untouched.");
            Assert.That(field.ImageStringValue, Is.Not.EqualTo("123456"), "Image channel should diverge.");
            Assert.That(field.ImageStringValue, Has.Length.EqualTo(6));
        }

        [Test]
        public void InconsistencyMutator_ShouldDoNothing_WhenFieldValueIsEmpty()
        {
            // Arrange
            var field = new Field("Empty", string.Empty);
            var mutator = new InconsistencyMutator();

            // Act
            mutator.Mutate(field);

            // Assert
            Assert.That(field.ImageStringValue, Is.EqualTo(string.Empty));
        }
    }
}
