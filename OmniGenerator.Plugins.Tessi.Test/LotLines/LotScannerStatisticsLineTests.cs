using System;
using System.Collections;
using System.Threading;

using NUnit.Framework;
using OmniGenerator.Lib;
using OmniGenerator.Lib.FixedLengthLine;
using OmniGenerator.Plugins.Tessi.Packagers.Lot;
using OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk;

namespace OmniGenerator.Plugins.Tessi.LotLines.UnitTests
{
    [TestFixture]
    public class LotScannerStatisticsLineTests
    {
        [Test]
        public void ToFixedLengthString_ShouldReturn114Characters()
        {
            var line = new LotScannerStatisticsLine();
            Assert.That(line.ToFixedLengthString().Length, Is.EqualTo(114));
        }

        [Test]
        public void ToFixedLengthString_Encline_ShouldBe89AtOffset0()
        {
            var line = new LotScannerStatisticsLine();
            var result = line.ToFixedLengthString();

            // Encline = "89", offset=0, length=2
            Assert.That(result.Substring(0, 2), Is.EqualTo("89"));
        }

        /// <summary>
        /// Tests that the parameterless constructor successfully creates an instance
        /// without throwing any exceptions.
        /// </summary>
        [Test]
        public void Constructor_Default_CreatesInstance()
        {
            // Act
            var line = new LotScannerStatisticsLine();

            // Assert
            Assert.That(line, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the parameterless constructor initializes Encline property to "89".
        /// Expected: Encline should be "89" after instantiation.
        /// </summary>
        [Test]
        public void Constructor_Default_InitializesEnclineTo89()
        {
            // Act
            var line = new LotScannerStatisticsLine();

            // Assert
            Assert.That(line.Encline, Is.EqualTo("89"));
        }

        /// <summary>
        /// Tests that the parameterless constructor initializes all integer statistical properties to zero.
        /// Expected: All integer properties should be 0 after instantiation.
        /// </summary>
        /// <param name="propertyName">The name of the property to check.</param>
        [TestCase("PowerOnHours")]
        [TestCase("PowerOnHoursSinceReset")]
        [TestCase("DocCount")]
        [TestCase("NumDocSinceReset")]
        [TestCase("NumDoubleDoc")]
        [TestCase("NumJamDoc")]
        [TestCase("NumFeederJams")]
        [TestCase("NulSkewedDocs")]
        [TestCase("NumIncompleteImage")]
        [TestCase("EndorseDotCount")]
        public void Constructor_Default_InitializesIntegerPropertiesToZero(string propertyName)
        {
            // Act
            var line = new LotScannerStatisticsLine();
            var propertyInfo = typeof(LotScannerStatisticsLine).GetProperty(propertyName);
            var value = (int)propertyInfo!.GetValue(line)!;

            // Assert
            Assert.That(value, Is.EqualTo(0));
        }
    }
}