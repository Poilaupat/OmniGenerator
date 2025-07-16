using NUnit.Framework;
using OmniGenerator.Lib.FixedLengthLine;
using System.Reflection;

namespace OmniGenerator.Test.Lib.Export
{
    [TestFixture]
    public class FixedLengthLineTests
    {
        [Test]
        public void Constructor_SetsAllProperties()
        {
            var attr = new FixedLengthLineFieldAttribute(3, 7, 'X', PadDirection.Left);
            Assert.That(attr.Offset, Is.EqualTo(3));
            Assert.That(attr.Length, Is.EqualTo(7));
            Assert.That(attr.PaddingChar, Is.EqualTo('X'));
            Assert.That(attr.PadDirection, Is.EqualTo(PadDirection.Left));
        }

        [Test]
        public void Constructor_DefaultPaddingCharAndDirection()
        {
            var attr = new FixedLengthLineFieldAttribute(4, 8);
            Assert.That(attr.Offset, Is.EqualTo(4));
            Assert.That(attr.Length, Is.EqualTo(8));
            Assert.That(attr.PaddingChar, Is.EqualTo(' '));
            Assert.That(attr.PadDirection, Is.EqualTo(PadDirection.Right));
        }

        [Test]
        public void AttributeUsage_IsPropertyTargeted()
        {
            var prop = typeof(FixedLengthLineDummyLine).GetProperty(nameof(FixedLengthLineDummyLine.FieldA))!;
            var attr = prop.GetCustomAttribute<FixedLengthLineFieldAttribute>();
            Assert.That(attr, Is.Not.Null);
            Assert.That(attr.Offset, Is.EqualTo(0));
            Assert.That(attr.Length, Is.EqualTo(5));
            Assert.That(attr.PaddingChar, Is.EqualTo('0'));
            Assert.That(attr.PadDirection, Is.EqualTo(PadDirection.Left));
        }

        [Test]
        public void AttributeUsage_DefaultPaddingCharAndDirection()
        {
            var prop = typeof(FixedLengthLineDummyLine).GetProperty(nameof(FixedLengthLineDummyLine.FieldB))!;
            var attr = prop.GetCustomAttribute<FixedLengthLineFieldAttribute>();
            Assert.That(attr, Is.Not.Null);
            Assert.That(attr.Offset, Is.EqualTo(5));
            Assert.That(attr.Length, Is.EqualTo(10));
            Assert.That(attr.PaddingChar, Is.EqualTo(' '));
            Assert.That(attr.PadDirection, Is.EqualTo(PadDirection.Right));
        }

        [Test]
        public void FixecLengthLine_Result()
        {
            FixedLengthLineDummyLine item = new FixedLengthLineDummyLine
            {
                FieldA = "123",
                FieldB = "Hello"
            };

            var line = item.ToFixedLengthString();

            Assert.That(line, Is.EqualTo("00123Hello     ")); // 5 chars for FieldA, 10 for FieldB
        }
    }

    [FixedLengthLine(15)]
    public class FixedLengthLineDummyLine : FixedLengthLineBase
    {
        [FixedLengthLineField(0, 5, '0', PadDirection.Left)]
        public string FieldA { get; set; } = string.Empty;

        [FixedLengthLineField(5, 10)]
        public string FieldB { get; set; } = string.Empty;
    }
}
