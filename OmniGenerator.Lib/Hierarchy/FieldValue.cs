using System.Globalization;

namespace OmniGenerator.Lib.Hierarchy
{
    /// <summary>
    /// Represents a typed field value with safe access and string conversion capabilities.
    /// Replaces raw <see cref="object"/> storage to preserve type information while
    /// providing runtime-safe conversions and invariant string representations.
    /// </summary>
    public readonly struct FieldValue : IEquatable<FieldValue>
    {
        /// <summary>
        /// The raw value stored in this field.
        /// </summary>
        public object? RawValue { get; }

        /// <summary>
        /// The runtime type of <see cref="RawValue"/>.
        /// </summary>
        public Type ValueType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValue"/> struct with a string value.
        /// </summary>
        /// <param name="value">The string value.</param>
        public FieldValue(string value)
        {
            RawValue = value ?? string.Empty;
            ValueType = typeof(string);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValue"/> struct with an integer value.
        /// </summary>
        /// <param name="value">The integer value.</param>
        public FieldValue(int value)
        {
            RawValue = value;
            ValueType = typeof(int);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValue"/> struct with a long integer value.
        /// </summary>
        /// <param name="value">The long integer value.</param>
        public FieldValue(long value)
        {
            RawValue = value;
            ValueType = typeof(long);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValue"/> struct with a DateTime value.
        /// </summary>
        /// <param name="value">The DateTime value.</param>
        public FieldValue(DateTime value)
        {
            RawValue = value;
            ValueType = typeof(DateTime);
        }

        /// <summary>
        /// Private constructor for creating a FieldValue with a null RawValue.
        /// </summary>
        private FieldValue(object? rawValue, Type valueType)
        {
            RawValue = rawValue;
            ValueType = valueType;
        }

        /// <summary>
        /// Creates a <see cref="FieldValue"/> from an arbitrary <see cref="object"/>.
        /// </summary>
        /// <param name="value">The object to wrap. Can be null.</param>
        /// <returns>A new <see cref="FieldValue"/> instance.</returns>
        public static FieldValue FromObject(object? value)
        {
            return value switch
            {
                null => new FieldValue(null, typeof(object)),
                string s => new FieldValue(s),
                int i => new FieldValue(i),
                long l => new FieldValue(l),
                DateTime dt => new FieldValue(dt),
                _ => new FieldValue(value.ToString() ?? string.Empty)
            };
        }

        /// <summary>
        /// Attempts to retrieve the value as type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="value">When this method returns, contains the value if conversion succeeded; otherwise, the default value.</param>
        /// <returns><c>true</c> if the value could be retrieved as <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public bool TryConvert<T>(out T? value)
        {
            if (RawValue is T typedValue)
            {
                value = typedValue;
                return true;
            }

            if (RawValue is null)
            {
                value = default;
                return typeof(T).IsClass || Nullable.GetUnderlyingType(typeof(T)) != null;
            }

            try
            {
                value = (T)System.Convert.ChangeType(RawValue, typeof(T), CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        /// Retrieves the value as type <typeparamref name="T"/>, or throws if conversion is not possible.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <returns>The value as <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException">Thrown when the value cannot be converted to <typeparamref name="T"/>.</exception>
        public T Convert<T>()
        {
            if (TryConvert<T>(out var value))
                return value!;

            throw new InvalidCastException($"Cannot convert field value of type {ValueType.Name} to {typeof(T).Name}.");
        }

        /// <summary>
        /// Converts the field value to its invariant-culture string representation.
        /// </summary>
        /// <returns>The string representation of the value.</returns>
        public string ToInvariantString()
        {
            return System.Convert.ToString(RawValue, CultureInfo.InvariantCulture) ?? string.Empty;
        }

        /// <summary>
        /// Attempts to coerce a mutated string back into the original value's type.
        /// Used primarily by the error simulation engine to preserve typed reads after string mutation.
        /// </summary>
        /// <param name="mutatedString">The mutated string representation.</param>
        /// <returns>A new <see cref="FieldValue"/> with the coerced value, or the string if coercion fails.</returns>
        public FieldValue CoerceFromString(string mutatedString)
        {
            if (ValueType == typeof(string))
                return new FieldValue(mutatedString);

            try
            {
                var coerced = System.Convert.ChangeType(mutatedString, ValueType, CultureInfo.InvariantCulture);
                return FromObject(coerced);
            }
            catch (Exception ex) when (ex is InvalidCastException or FormatException or OverflowException)
            {
                return new FieldValue(mutatedString);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="FieldValue"/> is equal to the current instance.
        /// </summary>
        public bool Equals(FieldValue other)
        {
            if (ValueType != other.ValueType)
                return false;

            if (RawValue is null && other.RawValue is null)
                return true;

            return RawValue?.Equals(other.RawValue) ?? false;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is FieldValue other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(RawValue, ValueType);

        /// <inheritdoc />
        public override string ToString() => ToInvariantString();

        public static bool operator ==(FieldValue left, FieldValue right) => left.Equals(right);
        public static bool operator !=(FieldValue left, FieldValue right) => !left.Equals(right);
    }
}
