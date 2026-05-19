using Moq;
using NUnit.Framework;
using OmniGenerator.Lib;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk;
using System;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref = "LotPacketEnd"/> class.
    /// </summary>
    [TestFixture]
    public class LotPacketEndTests
    {
        /// <summary>
        /// Tests that the constructor throws <see cref = "ArgumentNullException"/> or <see cref = "NullReferenceException"/>
        /// when a null RootFields parameter is provided.
        /// </summary>
        [Test]
        public void Constructor_WithNullRootFields_ThrowsException()
        {
            // Arrange
            RootFields? rootFields = null;
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => new LotPacketEnd(rootFields!));
        }

        /// <summary>
        /// Tests that the constructor correctly initializes all properties when provided with a valid RootFields instance.
        /// Verifies that Encline is set to "04", MachineName is set to "OmniGenerator Scan",
        /// and MachineSerialNumber is copied from the RootFields parameter.
        /// </summary>
        [Test]
        public void Constructor_WithValidRootFields_SetsPropertiesCorrectly()
        {
            // Arrange
            // Note: RootFields requires a FieldCollection. For this test to compile and run,
            // you may need to create a proper FieldCollection instance or use a test helper.
            // The following approach attempts to create a minimal valid instance.
            var fieldCollectionMock = new Mock<FieldCollection>();
            var rootFields = new RootFields(fieldCollectionMock.Object);
            // Act
            var lotPacketEnd = new LotPacketEnd(rootFields);
            // Assert
            Assert.That(lotPacketEnd.Encline, Is.EqualTo("04"));
            Assert.That(lotPacketEnd.MachineName, Is.EqualTo("OmniGenerator Scan"));
            Assert.That(lotPacketEnd.MachineSerialNumber, Is.EqualTo(rootFields.MachineSerialNumber));
        }

        /// <summary>
        /// Tests that the constructor always sets the Encline property to the constant value "04"
        /// regardless of the RootFields content.
        /// </summary>
        [Test]
        public void Constructor_Always_SetsEnclineTo04()
        {
            // Arrange
            var fieldCollectionMock = new Mock<FieldCollection>();
            var rootFields = new RootFields(fieldCollectionMock.Object);
            // Act
            var lotPacketEnd = new LotPacketEnd(rootFields);
            // Assert
            Assert.That(lotPacketEnd.Encline, Is.EqualTo("04"));
        }

        /// <summary>
        /// Tests that the constructor always sets the MachineName property to "OmniGenerator Scan"
        /// regardless of the RootFields content.
        /// </summary>
        [Test]
        public void Constructor_Always_SetsMachineNameToOmniGeneratorScan()
        {
            // Arrange
            var fieldCollectionMock = new Mock<FieldCollection>();
            var rootFields = new RootFields(fieldCollectionMock.Object);
            // Act
            var lotPacketEnd = new LotPacketEnd(rootFields);
            // Assert
            Assert.That(lotPacketEnd.MachineName, Is.EqualTo("OmniGenerator Scan"));
        }

        /// <summary>
        /// Tests that the constructor correctly copies the MachineSerialNumber from the RootFields parameter
        /// to the LotPacketEnd instance, verifying the dependency injection of this configuration value.
        /// </summary>
        [Test]
        public void Constructor_WithRootFields_CopiesMachineSerialNumberFromRootFields()
        {
            // Arrange
            var fieldCollectionMock = new Mock<FieldCollection>();
            var rootFields = new RootFields(fieldCollectionMock.Object);
            var expectedSerialNumber = rootFields.MachineSerialNumber;
            // Act
            var lotPacketEnd = new LotPacketEnd(rootFields);
            // Assert
            Assert.That(lotPacketEnd.MachineSerialNumber, Is.EqualTo(expectedSerialNumber));
        }

        /// <summary>
        /// Tests that other properties maintain their default initialized values after constructor execution.
        /// Verifies that Field1, Field2, Field3, and Field4 remain at their default value of 0.
        /// </summary>
        [Test]
        public void Constructor_WithValidRootFields_MaintainsDefaultValuesForOtherProperties()
        {
            // Arrange
            var fieldCollectionMock = new Mock<FieldCollection>();
            var rootFields = new RootFields(fieldCollectionMock.Object);
            // Act
            var lotPacketEnd = new LotPacketEnd(rootFields);
            // Assert
            Assert.That(lotPacketEnd.Field1, Is.EqualTo(0));
            Assert.That(lotPacketEnd.Field2, Is.EqualTo(0));
            Assert.That(lotPacketEnd.Field3, Is.EqualTo(0));
            Assert.That(lotPacketEnd.Field4, Is.EqualTo(0));
            Assert.That(lotPacketEnd.Field5, Is.EqualTo(string.Empty));
            Assert.That(lotPacketEnd.Filler1, Is.EqualTo(string.Empty));
            Assert.That(lotPacketEnd.Field6, Is.EqualTo(string.Empty));
            Assert.That(lotPacketEnd.Filler2, Is.EqualTo(string.Empty));
        }
    }
}