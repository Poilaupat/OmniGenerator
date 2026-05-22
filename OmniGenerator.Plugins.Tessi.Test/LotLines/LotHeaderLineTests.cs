using OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Plugins.Tessi.LotLines.UnitTests
{
    [TestFixture]
    public class LotHeaderLineTests
    {
        private static FieldCollection BuildFields(Dictionary<string, Field> dict)
        {
            var fc = new FieldCollection();
            fc.AddRange(dict);
            return fc;
        }

        private static RootFields BuildRootFields() => new(BuildFields(new Dictionary<string, Field>
        {
            ["packet-date"]            = new Field("packet-date", new DateTime(2024, 6, 15)),
            ["capture-point-code"]     = new Field("capture-point-code", "12345"),
            ["scanner-code"]           = new Field("scanner-code", "001"),
            ["organization-unit-code"] = new Field("organization-unit-code", "00042"),
            ["organization-code"]      = new Field("organization-code", "00030"),
            ["scanner-serial-number"]  = new Field("scanner-serial-number", "SN123456789"),
            ["packet-number"]          = new Field("packet-number", "0001"),
            ["packet-name"]            = new Field("packet-name", "TEST"),
        }));

        [Test]
        public void ToFixedLengthString_ShouldReturn435Characters()
        {
            var line = new LotHeaderLine(BuildRootFields());
            Assert.That(line.ToFixedLengthString().Length, Is.EqualTo(435));
        }

        [Test]
        public void ToFixedLengthString_Copyright_ShouldBeAtOffset0()
        {
            var line = new LotHeaderLine(BuildRootFields());
            var result = line.ToFixedLengthString();

            // Copyright = "ATHIC", offset=0, length=5, PadRight with ' '
            Assert.That(result.Substring(0, 5), Is.EqualTo("ATHIC"));
        }

        [Test]
        public void ToFixedLengthString_Date_ShouldBeFormattedDDMMYYYY()
        {
            var line = new LotHeaderLine(BuildRootFields());
            var result = line.ToFixedLengthString();

            // Date = "15062024", offset=19, length=8
            Assert.That(result.Substring(19, 8), Is.EqualTo("15062024"));
        }

        [Test]
        public void ToFixedLengthString_BranchID_ShouldBePaddedLeft()
        {
            var line = new LotHeaderLine(BuildRootFields());
            var result = line.ToFixedLengthString();

            // BranchID = "12345", offset=28, length=5, PadLeft with '0'
            Assert.That(result.Substring(28, 5), Is.EqualTo("12345"));
        }

        [Test]
        public void ToFixedLengthString_ShortBranchID_ShouldBePaddedWithZeroes()
        {
            var rootFields = new RootFields(BuildFields(new Dictionary<string, Field>
            {
                ["packet-date"]            = new Field("packet-date", new DateTime(2024, 6, 15)),
                ["capture-point-code"]     = new Field("capture-point-code", "12"),
                ["scanner-code"]           = new Field("scanner-code", "001"),
                ["organization-unit-code"] = new Field("organization-unit-code", "00042"),
                ["organization-code"]      = new Field("organization-code", "00030"),
                ["scanner-serial-number"]  = new Field("scanner-serial-number", "SN123456789"),
                ["packet-number"]          = new Field("packet-number", "0001"),
                ["packet-name"]            = new Field("packet-name", "TEST"),
            }));
            var result = new LotHeaderLine(rootFields).ToFixedLengthString();

            // BranchID = "12", offset=28, length=5, PadLeft '0' → "00012"
            Assert.That(result.Substring(28, 5), Is.EqualTo("00012"));
        }

        [Test]
        public void ToFixedLengthString_LongBranchID_ShouldBeTruncated()
        {
            var rootFields = new RootFields(BuildFields(new Dictionary<string, Field>
            {
                ["packet-date"]            = new Field("packet-date", new DateTime(2024, 6, 15)),
                ["capture-point-code"]     = new Field("capture-point-code", "TOOLONGVALUE"),
                ["scanner-code"]           = new Field("scanner-code", "001"),
                ["organization-unit-code"] = new Field("organization-unit-code", "00042"),
                ["organization-code"]      = new Field("organization-code", "00030"),
                ["scanner-serial-number"]  = new Field("scanner-serial-number", "SN123456789"),
                ["packet-number"]          = new Field("packet-number", "0001"),
                ["packet-name"]            = new Field("packet-name", "TEST"),
            }));
            var result = new LotHeaderLine(rootFields).ToFixedLengthString();

            // BranchID length=5, value too long → truncated to 5 chars
            Assert.That(result.Substring(28, 5), Is.EqualTo("TOOLO"));
        }
    }
}
