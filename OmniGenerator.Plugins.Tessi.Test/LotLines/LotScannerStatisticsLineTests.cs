using OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk;

namespace OmniGenerator.Plugins.Tessi.Test.LotLines
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
    }
}
