using NUnit.Framework;
using OmniGenerator.Lib.Renderers;
using System;

namespace OmniGenerator.Test.Lib.Renderers
{
    [TestFixture]
    public class NumberToWordsTests
    {
        #region Basic Cases - Zero and Simple Digits

        [Test]
        public void Convert_Zero_ReturnsZero()
        {
            var result = NumberToWords.Convert(0);
            Assert.That(result, Is.EqualTo("zéro"));
        }

        [Test]
        [TestCase(1, "un")]
        [TestCase(2, "deux")]
        [TestCase(3, "trois")]
        [TestCase(4, "quatre")]
        [TestCase(5, "cinq")]
        [TestCase(6, "six")]
        [TestCase(7, "sept")]
        [TestCase(8, "huit")]
        [TestCase(9, "neuf")]
        public void Convert_SingleDigit_ReturnsCorrectWord(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        #endregion

        #region Teens (10-19)

        [Test]
        [TestCase(10, "dix")]
        [TestCase(11, "onze")]
        [TestCase(12, "douze")]
        [TestCase(13, "treize")]
        [TestCase(14, "quatorze")]
        [TestCase(15, "quinze")]
        [TestCase(16, "seize")]
        [TestCase(17, "dix-sept")]
        [TestCase(18, "dix-huit")]
        [TestCase(19, "dix-neuf")]
        public void Convert_Teens_ReturnsCorrectWord(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        #endregion

        #region Tens (20-99)

        [Test]
        [TestCase(20, "vingt")]
        [TestCase(30, "trente")]
        [TestCase(40, "quarante")]
        [TestCase(50, "cinquante")]
        [TestCase(60, "soixante")]
        [TestCase(70, "soixante-dix")]
        [TestCase(80, "quatre-vingts")]  // Note: 80 takes an 's'
        [TestCase(90, "quatre-vingt-dix")]
        public void Convert_RoundTens_ReturnsCorrectWord(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(21, "vingt-et-un")]
        [TestCase(31, "trente-et-un")]
        [TestCase(41, "quarante-et-un")]
        [TestCase(51, "cinquante-et-un")]
        [TestCase(61, "soixante-et-un")]
        public void Convert_TensWithOneBelow70_HasEtPrefix(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(22, "vingt-deux")]
        [TestCase(35, "trente-cinq")]
        [TestCase(48, "quarante-huit")]
        [TestCase(59, "cinquante-neuf")]
        [TestCase(66, "soixante-six")]
        public void Convert_TensWithOtherDigits_ReturnsCorrectFormat(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(71, "soixante-et-onze")]
        [TestCase(72, "soixante-douze")]
        [TestCase(75, "soixante-quinze")]
        [TestCase(79, "soixante-dix-neuf")]
        public void Convert_SeventyRange_UsesSpecialFormat(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(81, "quatre-vingt-un")]
        [TestCase(85, "quatre-vingt-cinq")]
        [TestCase(89, "quatre-vingt-neuf")]
        public void Convert_EightyRange_UsesSpecialFormat(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(91, "quatre-vingt-onze")]
        [TestCase(95, "quatre-vingt-quinze")]
        [TestCase(99, "quatre-vingt-dix-neuf")]
        public void Convert_NinetyRange_UsesSpecialFormat(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        #endregion

        #region Hundreds (100-999)

        [Test]
        public void Convert_OneHundred_ReturnsCent()
        {
            var result = NumberToWords.Convert(100);
            Assert.That(result, Is.EqualTo("cent"));
        }

        [Test]
        [TestCase(200, "deux cents")]
        [TestCase(300, "trois cents")]
        [TestCase(500, "cinq cents")]
        [TestCase(900, "neuf cents")]
        public void Convert_RoundHundreds_ReturnsPluralForm(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(101, "cent un")]
        [TestCase(215, "deux cent quinze")]
        [TestCase(347, "trois cent quarante-sept")]
        [TestCase(580, "cinq cent quatre-vingts")]
        [TestCase(999, "neuf cent quatre-vingt-dix-neuf")]
        public void Convert_HundredsWithRemainder_ReturnsCorrectFormat(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        #endregion

        #region Thousands (1000-999999)

        [Test]
        public void Convert_OneThousand_ReturnsMille()
        {
            var result = NumberToWords.Convert(1000);
            Assert.That(result, Is.EqualTo("mille"));
        }

        [Test]
        [TestCase(2000, "deux milles")]
        [TestCase(5000, "cinq milles")]
        [TestCase(10000, "dix milles")]
        public void Convert_RoundThousands_ReturnsCorrectFormat(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(1001, "mille un")]
        [TestCase(1234, "mille deux cent trente-quatre")]
        [TestCase(2580, "deux mille cinq cent quatre-vingts")]
        [TestCase(99999, "quatre-vingt-dix-neuf mille neuf cent quatre-vingt-dix-neuf")]
        public void Convert_ThousandsWithRemainder_ReturnsCorrectFormat(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        #endregion

        #region Millions (1000000-999999999)

        [Test]
        public void Convert_OneMillion_ReturnsUnMillion()
        {
            var result = NumberToWords.Convert(1000000);
            Assert.That(result, Is.EqualTo("un million"));
        }

        [Test]
        [TestCase(2000000, "deux millions")]
        [TestCase(5000000, "cinq millions")]
        [TestCase(10000000, "dix millions")]
        public void Convert_RoundMillions_ReturnsPluralForm(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(1000001, "un million un")]
        [TestCase(2345678, "deux million trois cent quarante-cinq mille six cent soixante-dix-huit")]
        public void Convert_MillionsWithRemainder_ReturnsCorrectFormat(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        #endregion

        #region Edge Cases and Boundaries

        [Test]
        [TestCase(99)]
        [TestCase(999)]
        [TestCase(9999)]
        [TestCase(99999)]
        [TestCase(999999)]
        [TestCase(9999999)]
        public void Convert_MaxValuesForEachRange_DoesNotThrow(int value)
        {
            Assert.DoesNotThrow(() => NumberToWords.Convert(value));
        }

        [Test]
        public void Convert_NegativeNumber_ThrowsNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => NumberToWords.Convert(-1));
        }

        [Test]
        public void Convert_NumberTooLarge_ThrowsNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => NumberToWords.Convert(1000000000));
        }

        #endregion

        #region Additional Real-World Examples

        [Test]
        [TestCase(1789, "mille sept cent quatre-vingt-neuf")]
        [TestCase(1999, "mille neuf cent quatre-vingt-dix-neuf")]
        [TestCase(2024, "deux mille vingt-quatre")]
        public void Convert_HistoricalYears_ReturnsCorrectFormat(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(42, "quarante-deux")]  // Answer to everything
        [TestCase(69, "soixante-neuf")]
        [TestCase(420, "quatre cent vingt")]
        public void Convert_CommonNumbers_ReturnsCorrectFormat(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        #endregion

        #region Special French Grammar Rules

        [Test]
        [TestCase(180, "cent quatre-vingts")]
        [TestCase(280, "deux cent quatre-vingts")]
        [TestCase(380, "trois cent quatre-vingts")]
        public void Convert_EightyInHundreds_EndsWithS(int value, string expected)
        {
            var result = NumberToWords.Convert(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Convert_ResultsEndingInQuatreVingt_ShouldHaveS()
        {
            var result80 = NumberToWords.Convert(80);
            var result180 = NumberToWords.Convert(180);
            var result280 = NumberToWords.Convert(280);

            Assert.That(result80, Does.EndWith("s"), "80 should end with 's'");
            Assert.That(result180, Does.EndWith("s"), "180 should end with 's'");
            Assert.That(result280, Does.EndWith("s"), "280 should end with 's'");
        }

        #endregion
    }
}
