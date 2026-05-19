using System;
using Moq;
using NUnit.Framework;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk;


namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="LotStatisticLine"/> class.
    /// </summary>
    [TestFixture]
    public class LotStatisticLineTests
    {
        /// <summary>
        /// Tests that the constructor initializes the Encline property with the expected hardcoded value "88".
        /// </summary>
        [Test]
        public void LotStatisticLine_Constructor_ShouldSetEnclineTo88()
        {
            // Arrange
            var mockFieldCollection = new Mock<FieldCollection>();
            var rootFields = new RootFields(mockFieldCollection.Object);

            // Act
            var line = new LotStatisticLine(rootFields);

            // Assert
            Assert.That(line.Encline, Is.EqualTo("88"));
        }

        /// <summary>
        /// Tests that the constructor initializes the MachineName property with the expected hardcoded value "OmniGenerator Scan".
        /// </summary>
        [Test]
        public void LotStatisticLine_Constructor_ShouldSetMachineNameToOmniGeneratorScan()
        {
            // Arrange
            var mockFieldCollection = new Mock<FieldCollection>();
            var rootFields = new RootFields(mockFieldCollection.Object);

            // Act
            var line = new LotStatisticLine(rootFields);

            // Assert
            Assert.That(line.MachineName, Is.EqualTo("OmniGenerator Scan"));
        }

        /// <summary>
        /// Tests that the constructor copies the MachineSerialNumber from the provided RootFields parameter.
        /// Verifies that the property value matches the source RootFields.MachineSerialNumber.
        /// </summary>
        [Test]
        public void LotStatisticLine_Constructor_ShouldCopyMachineSerialNumberFromRootFields()
        {
            // Arrange
            var mockFieldCollection = new Mock<FieldCollection>();
            var rootFields = new RootFields(mockFieldCollection.Object);
            var expectedSerialNumber = rootFields.MachineSerialNumber;

            // Act
            var line = new LotStatisticLine(rootFields);

            // Assert
            Assert.That(line.MachineSerialNumber, Is.EqualTo(expectedSerialNumber));
        }

        /// <summary>
        /// Tests that the constructor throws NullReferenceException when passed a null RootFields parameter.
        /// Verifies proper null handling and prevents invalid object construction.
        /// </summary>
        [Test]
        public void LotStatisticLine_Constructor_WithNullRootFields_ShouldThrowNullReferenceException()
        {
            // Arrange
            RootFields? rootFields = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => new LotStatisticLine(rootFields!));
        }

        /// <summary>
        /// Tests that the constructor initializes default properties with their expected initial values.
        /// Verifies that NbErrors, NbDoubleFeed, NbJam, and NbIntervention are all set to 0.
        /// </summary>
        [Test]
        public void LotStatisticLine_Constructor_ShouldInitializeDefaultPropertiesToZero()
        {
            // Arrange
            var mockFieldCollection = new Mock<FieldCollection>();
            var rootFields = new RootFields(mockFieldCollection.Object);

            // Act
            var line = new LotStatisticLine(rootFields);

            // Assert
            Assert.That(line.NbErrors, Is.EqualTo(0));
            Assert.That(line.NbDoubleFeed, Is.EqualTo(0));
            Assert.That(line.NbJam, Is.EqualTo(0));
            Assert.That(line.NbIntervention, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that the constructor initializes string properties with their expected default values.
        /// Verifies that FirmwareVersion is set to empty string.
        /// </summary>
        [Test]
        public void LotStatisticLine_Constructor_ShouldInitializeStringPropertiesToDefaultValues()
        {
            // Arrange
            var mockFieldCollection = new Mock<FieldCollection>();
            var rootFields = new RootFields(mockFieldCollection.Object);

            // Act
            var line = new LotStatisticLine(rootFields);

            // Assert
            Assert.That(line.FirmwareVersion, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the constructor initializes read-only filler properties with their expected empty string values.
        /// Verifies that Filler1, Filler2, and Signature are all set to empty strings.
        /// </summary>
        [Test]
        public void LotStatisticLine_Constructor_ShouldInitializeReadOnlyPropertiesToEmptyStrings()
        {
            // Arrange
            var mockFieldCollection = new Mock<FieldCollection>();
            var rootFields = new RootFields(mockFieldCollection.Object);

            // Act
            var line = new LotStatisticLine(rootFields);

            // Assert
            Assert.That(line.Filler1, Is.EqualTo(string.Empty));
            Assert.That(line.Filler2, Is.EqualTo(string.Empty));
            Assert.That(line.Signature, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that the constructor properly initializes all properties when given a valid RootFields instance.
        /// Comprehensive test verifying the complete object state after construction.
        /// </summary>
        [Test]
        public void LotStatisticLine_Constructor_WithValidRootFields_ShouldInitializeAllPropertiesCorrectly()
        {
            // Arrange
            var mockFieldCollection = new Mock<FieldCollection>();
            var rootFields = new RootFields(mockFieldCollection.Object);

            // Act
            var line = new LotStatisticLine(rootFields);

            // Assert
            Assert.That(line.Encline, Is.EqualTo("88"));
            Assert.That(line.MachineName, Is.EqualTo("OmniGenerator Scan"));
            Assert.That(line.MachineSerialNumber, Is.EqualTo(rootFields.MachineSerialNumber));
            Assert.That(line.NbErrors, Is.EqualTo(0));
            Assert.That(line.NbDoubleFeed, Is.EqualTo(0));
            Assert.That(line.NbJam, Is.EqualTo(0));
            Assert.That(line.NbIntervention, Is.EqualTo(0));
            Assert.That(line.FirmwareVersion, Is.EqualTo(string.Empty));
            Assert.That(line.Filler1, Is.EqualTo(string.Empty));
            Assert.That(line.Filler2, Is.EqualTo(string.Empty));
            Assert.That(line.Signature, Is.EqualTo(string.Empty));
        }
    }
}