using NUnit.Framework;
using OmniGenerator.Lib.Hierarchy;
using System;

namespace OmniGenerator.Test.Lib.Hierarchy
{
    [TestFixture]
    public class FieldValueTests
    {
        #region Construction Tests

        [Test]
        public void Constructor_WithString_StoresStringValue()
        {
            var fieldValue = new FieldValue("test");
            Assert.That(fieldValue.RawValue, Is.EqualTo("test"));
            Assert.That(fieldValue.ValueType, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void Constructor_WithInt_StoresIntValue()
        {
            var fieldValue = new FieldValue(42);
            Assert.That(fieldValue.RawValue, Is.EqualTo(42));
            Assert.That(fieldValue.ValueType, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void Constructor_WithLong_StoresLongValue()
        {
            var fieldValue = new FieldValue(123456789L);
            Assert.That(fieldValue.RawValue, Is.EqualTo(123456789L));
            Assert.That(fieldValue.ValueType, Is.EqualTo(typeof(long)));
        }

        [Test]
        public void Constructor_WithDateTime_StoresDateTimeValue()
        {
            var date = new DateTime(2024, 1, 15);
            var fieldValue = new FieldValue(date);
            Assert.That(fieldValue.RawValue, Is.EqualTo(date));
            Assert.That(fieldValue.ValueType, Is.EqualTo(typeof(DateTime)));
        }

        [Test]
        public void FromObject_WithNull_CreatesNullValue()
        {
            var fieldValue = FieldValue.FromObject(null);
            Assert.That(fieldValue.RawValue, Is.Null);
            Assert.That(fieldValue.ValueType, Is.EqualTo(typeof(object)));
        }

        [Test]
        public void FromObject_WithString_CreatesStringValue()
        {
            var fieldValue = FieldValue.FromObject("hello");
            Assert.That(fieldValue.RawValue, Is.EqualTo("hello"));
            Assert.That(fieldValue.ValueType, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void FromObject_WithInt_CreatesIntValue()
        {
            var fieldValue = FieldValue.FromObject(100);
            Assert.That(fieldValue.RawValue, Is.EqualTo(100));
            Assert.That(fieldValue.ValueType, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void FromObject_WithUnsupportedType_ConvertsToString()
        {
            var obj = new { Name = "Test", Value = 42 };
            var fieldValue = FieldValue.FromObject(obj);
            Assert.That(fieldValue.ValueType, Is.EqualTo(typeof(string)));
            Assert.That(fieldValue.RawValue, Does.Contain("Name"));
        }

        #endregion

        #region TryGet Tests

        [Test]
        public void TryGet_WithMatchingType_ReturnsTrue()
        {
            var fieldValue = new FieldValue(42);
            var success = fieldValue.TryConvert<int>(out var value);
            Assert.That(success, Is.True);
            Assert.That(value, Is.EqualTo(42));
        }

        [Test]
        public void TryGet_WithConvertibleType_ReturnsTrue()
        {
            var fieldValue = new FieldValue(42);
            var success = fieldValue.TryConvert<long>(out var value);
            Assert.That(success, Is.True);
            Assert.That(value, Is.EqualTo(42L));
        }

        [Test]
        public void TryGet_WithIncompatibleType_ReturnsFalse()
        {
            var fieldValue = new FieldValue("not a number");
            var success = fieldValue.TryConvert<int>(out var value);
            Assert.That(success, Is.False);
            Assert.That(value, Is.EqualTo(default(int)));
        }

        [Test]
        public void TryGet_StringFromInt_ReturnsTrue()
        {
            var fieldValue = new FieldValue(123);
            var success = fieldValue.TryConvert<string>(out var value);
            Assert.That(success, Is.True);
            Assert.That(value, Is.EqualTo("123"));
        }

        #endregion

        #region GetRequired Tests

        [Test]
        public void GetRequired_WithMatchingType_ReturnsValue()
        {
            var fieldValue = new FieldValue(999);
            var value = fieldValue.Convert<int>();
            Assert.That(value, Is.EqualTo(999));
        }

        [Test]
        public void GetRequired_WithIncompatibleType_ThrowsInvalidCastException()
        {
            var fieldValue = new FieldValue("text");
            Assert.Throws<InvalidCastException>(() => fieldValue.Convert<int>());
        }

        #endregion

        #region ToInvariantString Tests

        [Test]
        public void ToInvariantString_WithString_ReturnsString()
        {
            var fieldValue = new FieldValue("hello");
            var result = fieldValue.ToInvariantString();
            Assert.That(result, Is.EqualTo("hello"));
        }

        [Test]
        public void ToInvariantString_WithInt_ReturnsStringRepresentation()
        {
            var fieldValue = new FieldValue(12345);
            var result = fieldValue.ToInvariantString();
            Assert.That(result, Is.EqualTo("12345"));
        }

        [Test]
        public void ToInvariantString_WithDateTime_ReturnsInvariantFormat()
        {
            var date = new DateTime(2024, 1, 15, 14, 30, 0);
            var fieldValue = new FieldValue(date);
            var result = fieldValue.ToInvariantString();
            Assert.That(result, Does.Contain("2024"));
            Assert.That(result, Does.Contain("01"));
            Assert.That(result, Does.Contain("15"));
        }

        #endregion

        #region CoerceFromString Tests

        [Test]
        public void CoerceFromString_WithStringType_ReturnsStringValue()
        {
            var original = new FieldValue("original");
            var coerced = original.CoerceFromString("mutated");
            Assert.That(coerced.ValueType, Is.EqualTo(typeof(string)));
            Assert.That(coerced.RawValue, Is.EqualTo("mutated"));
        }

        [Test]
        public void CoerceFromString_WithIntType_ConvertsValidString()
        {
            var original = new FieldValue(123);
            var coerced = original.CoerceFromString("456");
            Assert.That(coerced.ValueType, Is.EqualTo(typeof(int)));
            Assert.That(coerced.RawValue, Is.EqualTo(456));
        }

        [Test]
        public void CoerceFromString_WithIntType_FallbacksToStringOnInvalidInput()
        {
            var original = new FieldValue(123);
            var coerced = original.CoerceFromString("12?45");
            Assert.That(coerced.ValueType, Is.EqualTo(typeof(string)));
            Assert.That(coerced.RawValue, Is.EqualTo("12?45"));
        }

        [Test]
        public void CoerceFromString_WithLongType_ConvertsValidString()
        {
            var original = new FieldValue(123456789L);
            var coerced = original.CoerceFromString("987654321");
            Assert.That(coerced.ValueType, Is.EqualTo(typeof(long)));
            Assert.That(coerced.RawValue, Is.EqualTo(987654321L));
        }

        [Test]
        public void CoerceFromString_WithDateTimeType_FallbacksToStringOnInvalidInput()
        {
            var original = new FieldValue(DateTime.Now);
            var coerced = original.CoerceFromString("not a date");
            Assert.That(coerced.ValueType, Is.EqualTo(typeof(string)));
            Assert.That(coerced.RawValue, Is.EqualTo("not a date"));
        }

        #endregion

        #region Equality Tests

        [Test]
        public void Equals_WithSameValue_ReturnsTrue()
        {
            var value1 = new FieldValue(42);
            var value2 = new FieldValue(42);
            Assert.That(value1.Equals(value2), Is.True);
            Assert.That(value1 == value2, Is.True);
        }

        [Test]
        public void Equals_WithDifferentValue_ReturnsFalse()
        {
            var value1 = new FieldValue(42);
            var value2 = new FieldValue(99);
            Assert.That(value1.Equals(value2), Is.False);
            Assert.That(value1 != value2, Is.True);
        }

        [Test]
        public void Equals_WithDifferentType_ReturnsFalse()
        {
            var value1 = new FieldValue(42);
            var value2 = new FieldValue("42");
            Assert.That(value1.Equals(value2), Is.False);
        }

        #endregion

        #region ToString Tests

        [Test]
        public void ToString_ReturnsInvariantString()
        {
            var fieldValue = new FieldValue(123);
            var result = fieldValue.ToString();
            Assert.That(result, Is.EqualTo("123"));
        }

        #endregion
    }
}
