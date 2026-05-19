using OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk;

namespace OmniGenerator.Plugins.Tessi.Test.LotLines
{
    [TestFixture]
    public class LotNavetteLineTests
    {
        [Test]
        public void ToFixedLengthString_ShouldReturn190Characters()
        {
            var line = new LotNavetteLine();
            Assert.That(line.ToFixedLengthString().Length, Is.EqualTo(190));
        }

        [Test]
        public void ToFixedLengthString_Encline_ShouldBe99AtOffset0()
        {
            var line = new LotNavetteLine();
            var result = line.ToFixedLengthString();

            // Encline = "99", offset=0, length=2
            Assert.That(result.Substring(0, 2), Is.EqualTo("99"));
        }

        [Test]
        public void ToFixedLengthString_PickupHour_ShouldBe0000AtOffset3()
        {
            var line = new LotNavetteLine();
            var result = line.ToFixedLengthString();

            // PickupHour = "00:00", offset=3, length=5
            Assert.That(result.Substring(3, 5), Is.EqualTo("00:00"));
        }
    }
}
