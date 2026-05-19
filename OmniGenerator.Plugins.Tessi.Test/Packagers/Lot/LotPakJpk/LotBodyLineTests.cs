using Moq;
using NUnit.Framework;
using OmniGenerator.Plugins.Tessi.Packagers.Lot;
using OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk;


namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk.UnitTests
{
    public class LotBodyLineTests
    {
        /// <summary>
        /// Tests that the constructor correctly assigns properties from RemittanceFields and RootFields.
        /// Verifies remittance and root level field mappings.
        /// </summary>
        [Test]
        public void Constructor_ValidRemittanceAndRootFields_AssignsPropertiesCorrectly()
        {
            // Arrange
            var docFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            // DocumentFields requires 'encline' as a required field
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection).GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("encline", "1") });
            var docFields = new DocumentFields(docFieldCollection);

            var remFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection).GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(remFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("remittance-id", "REM999") });
            var remFields = new RemittanceFields(remFieldCollection);

            var rootFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection).GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("packet-number", "PKT999") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection).GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("organization-code", "ORG999") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection).GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("process-code", "PROC99") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection).GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("reconciliation", "0") });
            // RootFields also requires 'capture-point-code' and 'organization-unit-code' as required fields
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection).GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("capture-point-code", "CP001") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection).GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("organization-unit-code", "OU001") });
            var rootFields = new RootFields(rootFieldCollection);

            var bwRecto = new OffsetLengthImage();
            var bwVerso = new OffsetLengthImage();
            var gsRecto = new OffsetLengthImage();
            var gsVerso = new OffsetLengthImage();

            // Act
            var line = new LotBodyLine(1, docFields, remFields, rootFields,
                bwRecto, bwVerso, gsRecto, gsVerso);

            // Assert
            Assert.That(line.RemittanceID, Is.EqualTo("REM999"));
            Assert.That(line.LotID, Is.EqualTo("PKT999"));
            Assert.That(line.BankCode, Is.EqualTo("ORG999"));
            Assert.That(line.ProcessCode, Is.EqualTo("PROC99"));
            Assert.That(line.Reconciliation, Is.EqualTo("0"));
        }

        /// <summary>
        /// Tests that RefEndos and NumDoc are correctly set to the string representation of the index parameter.
        /// Verifies that a positive index value is properly converted to string.
        /// </summary>
        [TestCase(1, "1")]
        [TestCase(42, "42")]
        [TestCase(999, "999")]
        public void Constructor_PositiveIndex_SetsRefEndosAndNumDocCorrectly(int index, string expected)
        {
            // Arrange
            var mockDocFields = new Mock<DocumentFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRemFields = new Mock<RemittanceFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRootFields = new Mock<RootFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockBwRecto = new Mock<OffsetLengthImage>();
            mockBwRecto.Setup(x => x.Length).Returns(0);
            var mockBwVerso = new Mock<OffsetLengthImage>();
            mockBwVerso.Setup(x => x.Length).Returns(0);
            var mockGsRecto = new Mock<OffsetLengthImage>();
            mockGsRecto.Setup(x => x.Length).Returns(0);
            var mockGsVerso = new Mock<OffsetLengthImage>();
            mockGsVerso.Setup(x => x.Length).Returns(0);

            // Act
            var line = new LotBodyLine(index, mockDocFields.Object, mockRemFields.Object, mockRootFields.Object,
                mockBwRecto.Object, mockBwVerso.Object, mockGsRecto.Object, mockGsVerso.Object);

            // Assert
            Assert.That(line.RefEndos, Is.EqualTo(expected));
            Assert.That(line.NumDoc, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that RefEndos and NumDoc are correctly set when index is zero.
        /// Verifies edge case of zero index value.
        /// </summary>
        [Test]
        public void Constructor_ZeroIndex_SetsRefEndosAndNumDocToZero()
        {
            // Arrange
            var docFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("encline", "01"));
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("dataread", ""));
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("quality-code", "0"));
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("ref-doc", ""));
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("signature", "---SIGNATURE---"));
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("status", "0"));
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("priority", ""));
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("rib", ""));
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("nb-checks", ""));
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("icr-conf-amount", ""));
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("icr-amount", ""));
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("sort-error", "0"));
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("image-quality", "0"));
            docFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("deleted", "0"));
            var documentFields = new DocumentFields(docFieldCollection);
            
            var remFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            remFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("remittance-id", ""));
            var remittanceFields = new RemittanceFields(remFieldCollection);
            
            var rootFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            rootFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("packet-number", "0001"));
            rootFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("organization-code", "00000"));
            rootFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("process-code", "000"));
            rootFieldCollection.Add(new OmniGenerator.Lib.Hierarchy.Field("reconciliation", "0"));
            var rootFields = new RootFields(rootFieldCollection);
            
            var bwRecto = new OffsetLengthImage();
            var bwVerso = new OffsetLengthImage();
            var gsRecto = new OffsetLengthImage();
            var gsVerso = new OffsetLengthImage();

            // Act
            var line = new LotBodyLine(0, documentFields, remittanceFields, rootFields,
                bwRecto, bwVerso, gsRecto, gsVerso);

            // Assert
            Assert.That(line.RefEndos, Is.EqualTo("0"));
            Assert.That(line.NumDoc, Is.EqualTo("0"));
        }

        /// <summary>
        /// Tests that RefEndos and NumDoc are correctly set when index is negative.
        /// Verifies that negative values are properly converted to string with minus sign.
        /// </summary>
        [TestCase(-1, "-1")]
        [TestCase(-999, "-999")]
        public void Constructor_NegativeIndex_SetsRefEndosAndNumDocCorrectly(int index, string expected)
        {
            // Arrange
            var mockDocFields = new Mock<DocumentFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRemFields = new Mock<RemittanceFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRootFields = new Mock<RootFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockBwRecto = new Mock<OffsetLengthImage>();
            mockBwRecto.Setup(x => x.Length).Returns(0);
            var mockBwVerso = new Mock<OffsetLengthImage>();
            mockBwVerso.Setup(x => x.Length).Returns(0);
            var mockGsRecto = new Mock<OffsetLengthImage>();
            mockGsRecto.Setup(x => x.Length).Returns(0);
            var mockGsVerso = new Mock<OffsetLengthImage>();
            mockGsVerso.Setup(x => x.Length).Returns(0);

            // Act
            var line = new LotBodyLine(index, mockDocFields.Object, mockRemFields.Object, mockRootFields.Object,
                mockBwRecto.Object, mockBwVerso.Object, mockGsRecto.Object, mockGsVerso.Object);

            // Assert
            Assert.That(line.RefEndos, Is.EqualTo(expected));
            Assert.That(line.NumDoc, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that RefEndos and NumDoc handle extreme integer values.
        /// Verifies boundary conditions for int.MaxValue and int.MinValue.
        /// </summary>
        [TestCase(int.MaxValue, "2147483647")]
        [TestCase(int.MinValue, "-2147483648")]
        public void Constructor_ExtremeIndexValues_SetsRefEndosAndNumDocCorrectly(int index, string expected)
        {
            // Arrange
            var mockDocFields = new Mock<DocumentFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRemFields = new Mock<RemittanceFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRootFields = new Mock<RootFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockBwRecto = new Mock<OffsetLengthImage>();
            mockBwRecto.Setup(x => x.Length).Returns(0);
            var mockBwVerso = new Mock<OffsetLengthImage>();
            mockBwVerso.Setup(x => x.Length).Returns(0);
            var mockGsRecto = new Mock<OffsetLengthImage>();
            mockGsRecto.Setup(x => x.Length).Returns(0);
            var mockGsVerso = new Mock<OffsetLengthImage>();
            mockGsVerso.Setup(x => x.Length).Returns(0);

            // Act
            var line = new LotBodyLine(index, mockDocFields.Object, mockRemFields.Object, mockRootFields.Object,
                mockBwRecto.Object, mockBwVerso.Object, mockGsRecto.Object, mockGsVerso.Object);

            // Assert
            Assert.That(line.RefEndos, Is.EqualTo(expected));
            Assert.That(line.NumDoc, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that SendGrayLevel is set to "0" when gsRecto.Length is zero.
        /// Verifies that grayscale recto image is not present when length is zero.
        /// </summary>
        [Test]
        public void Constructor_GsRectoLengthZero_SetsSendGrayLevelToZero()
        {
            // Arrange
            var docFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("encline", "TestEncline") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("dataread", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("quality-code", "0") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("ref-doc", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("signature", "---SIGNATURE---") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("status", "0") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("priority", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("rib", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("nb-checks", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("icr-conf-amount", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("icr-amount", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("sort-error", "0") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("image-quality", "0") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("deleted", "0") });
            var docFields = new DocumentFields(docFieldCollection);

            var remFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(remFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("remittance-id", "") });
            var remFields = new RemittanceFields(remFieldCollection);

            var rootFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("packet-number", "0001") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("organization-code", "ORG123") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("process-code", "000") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("reconciliation", "0") });
            var rootFields = new RootFields(rootFieldCollection);

            var bwRecto = new OffsetLengthImage();
            var bwVerso = new OffsetLengthImage();
            var gsRecto = new OffsetLengthImage();
            var gsVerso = new OffsetLengthImage();

            // Act
            var line = new LotBodyLine(1, docFields, remFields, rootFields,
                bwRecto, bwVerso, gsRecto, gsVerso);

            // Assert
            Assert.That(line.SendGrayLevel, Is.EqualTo("0"));
        }

        /// <summary>
        /// Tests that SendGrayLevel is set to "1" when gsRecto.Length is positive.
        /// Verifies that grayscale recto image is present when length is greater than zero.
        /// </summary>
        [TestCase(1)]
        [TestCase(100)]
        [TestCase(int.MaxValue)]
        public void Constructor_GsRectoLengthPositive_SetsSendGrayLevelToOne(int length)
        {
            // Arrange
            var mockDocFields = new Mock<DocumentFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRemFields = new Mock<RemittanceFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRootFields = new Mock<RootFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockBwRecto = new Mock<OffsetLengthImage>();
            mockBwRecto.Setup(x => x.Length).Returns(0);
            var mockBwVerso = new Mock<OffsetLengthImage>();
            mockBwVerso.Setup(x => x.Length).Returns(0);
            var mockGsRecto = new Mock<OffsetLengthImage>();
            mockGsRecto.Setup(x => x.Length).Returns(length);
            var mockGsVerso = new Mock<OffsetLengthImage>();
            mockGsVerso.Setup(x => x.Length).Returns(0);

            // Act
            var line = new LotBodyLine(1, mockDocFields.Object, mockRemFields.Object, mockRootFields.Object,
                mockBwRecto.Object, mockBwVerso.Object, mockGsRecto.Object, mockGsVerso.Object);

            // Assert
            Assert.That(line.SendGrayLevel, Is.EqualTo("1"));
        }

        /// <summary>
        /// Tests that SendRear is set to "0" when gsVerso.Length is zero.
        /// Verifies that grayscale verso (rear) image is not present when length is zero.
        /// </summary>
        [Test]
        public void Constructor_GsVersoLengthZero_SetsSendRearToZero()
        {
            // Arrange
            var docFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("encline", "TestEncline") });
            var docFields = new DocumentFields(docFieldCollection);

            var remFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            var remFields = new RemittanceFields(remFieldCollection);

            var rootFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("packet-number", "0001") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("organization-code", "ORG") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("process-code", "000") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("reconciliation", "0") });
            var rootFields = new RootFields(rootFieldCollection);

            var bwRecto = new OffsetLengthImage();
            var bwVerso = new OffsetLengthImage();
            var gsRecto = new OffsetLengthImage();
            var gsVerso = new OffsetLengthImage();

            // Act
            var line = new LotBodyLine(1, docFields, remFields, rootFields,
                bwRecto, bwVerso, gsRecto, gsVerso);

            // Assert
            Assert.That(line.SendRear, Is.EqualTo("0"));
        }

        /// <summary>
        /// Tests that SendRear is set to "1" when gsVerso.Length is positive.
        /// Verifies that grayscale verso (rear) image is present when length is greater than zero.
        /// </summary>
        [TestCase(1)]
        [TestCase(100)]
        [TestCase(int.MaxValue)]
        public void Constructor_GsVersoLengthPositive_SetsSendRearToOne(int length)
        {
            // Arrange
            var mockDocFields = new Mock<DocumentFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRemFields = new Mock<RemittanceFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRootFields = new Mock<RootFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockBwRecto = new Mock<OffsetLengthImage>();
            mockBwRecto.Setup(x => x.Length).Returns(0);
            var mockBwVerso = new Mock<OffsetLengthImage>();
            mockBwVerso.Setup(x => x.Length).Returns(0);
            var mockGsRecto = new Mock<OffsetLengthImage>();
            mockGsRecto.Setup(x => x.Length).Returns(0);
            var mockGsVerso = new Mock<OffsetLengthImage>();
            mockGsVerso.Setup(x => x.Length).Returns(length);

            // Act
            var line = new LotBodyLine(1, mockDocFields.Object, mockRemFields.Object, mockRootFields.Object,
                mockBwRecto.Object, mockBwVerso.Object, mockGsRecto.Object, mockGsVerso.Object);

            // Assert
            Assert.That(line.SendRear, Is.EqualTo("1"));
        }

        /// <summary>
        /// Tests the combination of SendGrayLevel and SendRear with different image length scenarios.
        /// Verifies correct flags for all combinations of present/absent grayscale images.
        /// </summary>
        [TestCase(0, 0, "0", "0")]
        [TestCase(100, 0, "1", "0")]
        [TestCase(0, 200, "0", "1")]
        [TestCase(100, 200, "1", "1")]
        public void Constructor_GsImageLengthCombinations_SetsSendFlagsCorrectly(int gsRectoLength, int gsVersoLength, string expectedSendGrayLevel, string expectedSendRear)
        {
            // Arrange
            var mockDocFields = new Mock<DocumentFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRemFields = new Mock<RemittanceFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRootFields = new Mock<RootFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockBwRecto = new Mock<OffsetLengthImage>();
            mockBwRecto.Setup(x => x.Length).Returns(0);
            var mockBwVerso = new Mock<OffsetLengthImage>();
            mockBwVerso.Setup(x => x.Length).Returns(0);
            var mockGsRecto = new Mock<OffsetLengthImage>();
            mockGsRecto.Setup(x => x.Length).Returns(gsRectoLength);
            var mockGsVerso = new Mock<OffsetLengthImage>();
            mockGsVerso.Setup(x => x.Length).Returns(gsVersoLength);

            // Act
            var line = new LotBodyLine(1, mockDocFields.Object, mockRemFields.Object, mockRootFields.Object,
                mockBwRecto.Object, mockBwVerso.Object, mockGsRecto.Object, mockGsVerso.Object);

            // Assert
            Assert.That(line.SendGrayLevel, Is.EqualTo(expectedSendGrayLevel));
            Assert.That(line.SendRear, Is.EqualTo(expectedSendRear));
        }

        /// <summary>
        /// Tests that TimeStamp is set and is not empty.
        /// Verifies that the TimeStamp property is populated with DateTime.Now.TimeOfDay.TotalSeconds.
        /// </summary>
        [Test]
        public void Constructor_AnyInput_SetsTimeStampToNonEmptyValue()
        {
            // Arrange
            var docFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("encline", "TestEncline") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("dataread", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("quality-code", "0") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("ref-doc", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("signature", "---SIGNATURE---") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("status", "0") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("priority", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("rib", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("nb-checks", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("icr-conf-amount", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("icr-amount", "") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("sort-error", "0") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("image-quality", "0") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(docFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("deleted", "0") });
            var mockDocFields = new DocumentFields(docFieldCollection);

            var remFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(remFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("remittance-id", "") });
            var mockRemFields = new RemittanceFields(remFieldCollection);

            var rootFieldCollection = new OmniGenerator.Lib.Hierarchy.FieldCollection();
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("packet-number", "0001") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("organization-code", "ORG123") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("process-code", "000") });
            typeof(OmniGenerator.Lib.Hierarchy.FieldCollection)
                .GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(rootFieldCollection, new object[] { new OmniGenerator.Lib.Hierarchy.Field("reconciliation", "0") });
            var mockRootFields = new RootFields(rootFieldCollection);

            var mockBwRecto = new OffsetLengthImage();
            var mockBwVerso = new OffsetLengthImage();
            var mockGsRecto = new OffsetLengthImage();
            var mockGsVerso = new OffsetLengthImage();

            // Act
            var line = new LotBodyLine(1, mockDocFields, mockRemFields, mockRootFields,
                mockBwRecto, mockBwVerso, mockGsRecto, mockGsVerso);

            // Assert
            Assert.That(line.TimeStamp, Is.Not.Null);
            Assert.That(line.TimeStamp, Is.Not.Empty);
        }

        /// <summary>
        /// Tests that LengthRectoPak is correctly set to the string representation of bwRecto.Length.
        /// Verifies black and white recto image length is properly converted.
        /// </summary>
        [TestCase(0, "0")]
        [TestCase(1000, "1000")]
        [TestCase(int.MaxValue, "2147483647")]
        public void Constructor_BwRectoLength_SetsLengthRectoPakCorrectly(int length, string expected)
        {
            // Arrange
            var mockDocFields = new Mock<DocumentFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRemFields = new Mock<RemittanceFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRootFields = new Mock<RootFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockBwRecto = new Mock<OffsetLengthImage>();
            mockBwRecto.Setup(x => x.Length).Returns(length);
            mockBwRecto.Setup(x => x.Offset).Returns(0);
            var mockBwVerso = new Mock<OffsetLengthImage>();
            mockBwVerso.Setup(x => x.Length).Returns(0);
            mockBwVerso.Setup(x => x.Offset).Returns(0);
            var mockGsRecto = new Mock<OffsetLengthImage>();
            mockGsRecto.Setup(x => x.Length).Returns(0);
            mockGsRecto.Setup(x => x.Offset).Returns(0);
            var mockGsVerso = new Mock<OffsetLengthImage>();
            mockGsVerso.Setup(x => x.Length).Returns(0);
            mockGsVerso.Setup(x => x.Offset).Returns(0);

            // Act
            var line = new LotBodyLine(1, mockDocFields.Object, mockRemFields.Object, mockRootFields.Object,
                mockBwRecto.Object, mockBwVerso.Object, mockGsRecto.Object, mockGsVerso.Object);

            // Assert
            Assert.That(line.LengthRectoPak, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that OffsetRectoPak is correctly set to (bwRecto.Offset + 1).ToString().
        /// Verifies that offset is incremented by one and converted to string.
        /// </summary>
        [TestCase(0, "1")]
        [TestCase(999, "1000")]
        [TestCase(int.MaxValue - 1, "2147483647")]
        public void Constructor_BwRectoOffset_SetsOffsetRectoPakCorrectly(int offset, string expected)
        {
            // Arrange
            var mockDocFields = new Mock<DocumentFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRemFields = new Mock<RemittanceFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRootFields = new Mock<RootFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockBwRecto = new Mock<OffsetLengthImage>();
            mockBwRecto.Setup(x => x.Length).Returns(0);
            mockBwRecto.Setup(x => x.Offset).Returns(offset);
            var mockBwVerso = new Mock<OffsetLengthImage>();
            mockBwVerso.Setup(x => x.Length).Returns(0);
            mockBwVerso.Setup(x => x.Offset).Returns(0);
            var mockGsRecto = new Mock<OffsetLengthImage>();
            mockGsRecto.Setup(x => x.Length).Returns(0);
            mockGsRecto.Setup(x => x.Offset).Returns(0);
            var mockGsVerso = new Mock<OffsetLengthImage>();
            mockGsVerso.Setup(x => x.Length).Returns(0);
            mockGsVerso.Setup(x => x.Offset).Returns(0);

            // Act
            var line = new LotBodyLine(1, mockDocFields.Object, mockRemFields.Object, mockRootFields.Object,
                mockBwRecto.Object, mockBwVerso.Object, mockGsRecto.Object, mockGsVerso.Object);

            // Assert
            Assert.That(line.OffsetRectoPak, Is.EqualTo(expected));
        }

        /// <summary>
        /// Tests that LengthVersoPak and OffsetVersoPak are correctly set from bwVerso.
        /// Verifies black and white verso image length and offset are properly converted.
        /// </summary>
        [TestCase(500, 1000, "500", "1001")]
        [TestCase(0, 0, "0", "1")]
        public void Constructor_BwVersoLengthAndOffset_SetsVersoPakPropertiesCorrectly(int length, int offset, string expectedLength, string expectedOffset)
        {
            // Arrange
            var mockDocFields = new Mock<DocumentFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRemFields = new Mock<RemittanceFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRootFields = new Mock<RootFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockBwRecto = new Mock<OffsetLengthImage>();
            mockBwRecto.Setup(x => x.Length).Returns(0);
            mockBwRecto.Setup(x => x.Offset).Returns(0);
            var mockBwVerso = new Mock<OffsetLengthImage>();
            mockBwVerso.Setup(x => x.Length).Returns(length);
            mockBwVerso.Setup(x => x.Offset).Returns(offset);
            var mockGsRecto = new Mock<OffsetLengthImage>();
            mockGsRecto.Setup(x => x.Length).Returns(0);
            mockGsRecto.Setup(x => x.Offset).Returns(0);
            var mockGsVerso = new Mock<OffsetLengthImage>();
            mockGsVerso.Setup(x => x.Length).Returns(0);
            mockGsVerso.Setup(x => x.Offset).Returns(0);

            // Act
            var line = new LotBodyLine(1, mockDocFields.Object, mockRemFields.Object, mockRootFields.Object,
                mockBwRecto.Object, mockBwVerso.Object, mockGsRecto.Object, mockGsVerso.Object);

            // Assert
            Assert.That(line.LengthVersoPak, Is.EqualTo(expectedLength));
            Assert.That(line.OffsetVersoPak, Is.EqualTo(expectedOffset));
        }

        /// <summary>
        /// Tests that LengthRectoJpk and OffsetRectoJpk are correctly set from gsRecto.
        /// Verifies grayscale recto image length and offset are properly converted for JPK format.
        /// </summary>
        [TestCase(2000, 5000, "2000", "5001")]
        [TestCase(0, 0, "0", "1")]
        public void Constructor_GsRectoLengthAndOffset_SetsRectoJpkPropertiesCorrectly(int length, int offset, string expectedLength, string expectedOffset)
        {
            // Arrange
            var mockDocFields = new Mock<DocumentFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRemFields = new Mock<RemittanceFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRootFields = new Mock<RootFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockBwRecto = new Mock<OffsetLengthImage>();
            mockBwRecto.Setup(x => x.Length).Returns(0);
            mockBwRecto.Setup(x => x.Offset).Returns(0);
            var mockBwVerso = new Mock<OffsetLengthImage>();
            mockBwVerso.Setup(x => x.Length).Returns(0);
            mockBwVerso.Setup(x => x.Offset).Returns(0);
            var mockGsRecto = new Mock<OffsetLengthImage>();
            mockGsRecto.Setup(x => x.Length).Returns(length);
            mockGsRecto.Setup(x => x.Offset).Returns(offset);
            var mockGsVerso = new Mock<OffsetLengthImage>();
            mockGsVerso.Setup(x => x.Length).Returns(0);
            mockGsVerso.Setup(x => x.Offset).Returns(0);

            // Act
            var line = new LotBodyLine(1, mockDocFields.Object, mockRemFields.Object, mockRootFields.Object,
                mockBwRecto.Object, mockBwVerso.Object, mockGsRecto.Object, mockGsVerso.Object);

            // Assert
            Assert.That(line.LengthRectoJpk, Is.EqualTo(expectedLength));
            Assert.That(line.OffsetRectoJpk, Is.EqualTo(expectedOffset));
        }

        /// <summary>
        /// Tests that LengthVersoJpk and OffsetVersoJpk are correctly set from gsVerso.
        /// Verifies grayscale verso image length and offset are properly converted for JPK format.
        /// </summary>
        [TestCase(3000, 8000, "3000", "8001")]
        [TestCase(0, 0, "0", "1")]
        public void Constructor_GsVersoLengthAndOffset_SetsVersoJpkPropertiesCorrectly(int length, int offset, string expectedLength, string expectedOffset)
        {
            // Arrange
            var mockDocFields = new Mock<DocumentFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRemFields = new Mock<RemittanceFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRootFields = new Mock<RootFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockBwRecto = new Mock<OffsetLengthImage>();
            mockBwRecto.Setup(x => x.Length).Returns(0);
            mockBwRecto.Setup(x => x.Offset).Returns(0);
            var mockBwVerso = new Mock<OffsetLengthImage>();
            mockBwVerso.Setup(x => x.Length).Returns(0);
            mockBwVerso.Setup(x => x.Offset).Returns(0);
            var mockGsRecto = new Mock<OffsetLengthImage>();
            mockGsRecto.Setup(x => x.Length).Returns(0);
            mockGsRecto.Setup(x => x.Offset).Returns(0);
            var mockGsVerso = new Mock<OffsetLengthImage>();
            mockGsVerso.Setup(x => x.Length).Returns(length);
            mockGsVerso.Setup(x => x.Offset).Returns(offset);

            // Act
            var line = new LotBodyLine(1, mockDocFields.Object, mockRemFields.Object, mockRootFields.Object,
                mockBwRecto.Object, mockBwVerso.Object, mockGsRecto.Object, mockGsVerso.Object);

            // Assert
            Assert.That(line.LengthVersoJpk, Is.EqualTo(expectedLength));
            Assert.That(line.OffsetVersoJpk, Is.EqualTo(expectedOffset));
        }

        /// <summary>
        /// Tests that negative offset values are handled correctly in offset calculations.
        /// Verifies that negative offsets + 1 are properly converted to string.
        /// </summary>
        [TestCase(-10, "-9")]
        [TestCase(-1, "0")]
        public void Constructor_NegativeOffset_HandlesOffsetCalculationCorrectly(int offset, string expected)
        {
            // Arrange
            var mockDocFields = new Mock<DocumentFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRemFields = new Mock<RemittanceFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockRootFields = new Mock<RootFields>(Mock.Of<OmniGenerator.Lib.Hierarchy.FieldCollection>());
            var mockBwRecto = new Mock<OffsetLengthImage>();
            mockBwRecto.Setup(x => x.Length).Returns(0);
            mockBwRecto.Setup(x => x.Offset).Returns(offset);
            var mockBwVerso = new Mock<OffsetLengthImage>();
            mockBwVerso.Setup(x => x.Length).Returns(0);
            mockBwVerso.Setup(x => x.Offset).Returns(0);
            var mockGsRecto = new Mock<OffsetLengthImage>();
            mockGsRecto.Setup(x => x.Length).Returns(0);
            mockGsRecto.Setup(x => x.Offset).Returns(0);
            var mockGsVerso = new Mock<OffsetLengthImage>();
            mockGsVerso.Setup(x => x.Length).Returns(0);
            mockGsVerso.Setup(x => x.Offset).Returns(0);

            // Act
            var line = new LotBodyLine(1, mockDocFields.Object, mockRemFields.Object, mockRootFields.Object,
                mockBwRecto.Object, mockBwVerso.Object, mockGsRecto.Object, mockGsVerso.Object);

            // Assert
            Assert.That(line.OffsetRectoPak, Is.EqualTo(expected));
        }
    }
}