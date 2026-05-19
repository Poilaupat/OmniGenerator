using System;

using NUnit.Framework;
using OmniGenerator.Plugins.Tessi.Packagers.Lot;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="OffsetLengthImage"/> class.
    /// </summary>
    [TestFixture]
    public class OffsetLengthImageTests
    {
        /// <summary>
        /// Tests that Set method correctly assigns the image, stores the offset, and updates the ref offset parameter.
        /// </summary>
        /// <param name="imageLength">The length of the image array to test.</param>
        /// <param name="initialOffset">The initial offset value.</param>
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(10, 0)]
        [TestCase(100, 0)]
        [TestCase(0, 100)]
        [TestCase(10, 100)]
        [TestCase(50, 500)]
        [TestCase(1000, 1000)]
        public void Set_WithValidImageAndOffset_ShouldSetImageAndUpdateOffset(int imageLength, int initialOffset)
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            byte[] testImage = new byte[imageLength];
            int offset = initialOffset;
            int expectedNewOffset = initialOffset + imageLength;

            // Act
            offsetLengthImage.Set(testImage, ref offset);

            // Assert
            Assert.That(offsetLengthImage.Image, Is.EqualTo(testImage));
            Assert.That(offsetLengthImage.Length, Is.EqualTo(imageLength));
            Assert.That(offset, Is.EqualTo(expectedNewOffset));
        }

        /// <summary>
        /// Tests that Set method correctly sets the Offset property based on whether the image is empty or not.
        /// </summary>
        /// <param name="imageLength">The length of the image array to test.</param>
        /// <param name="initialOffset">The initial offset value.</param>
        /// <param name="expectedOffsetProperty">The expected value of the Offset property.</param>
        [TestCase(0, 0, 0)]
        [TestCase(0, 100, 0)]
        [TestCase(1, 0, 0)]
        [TestCase(10, 50, 50)]
        [TestCase(100, 200, 200)]
        public void Set_OffsetProperty_ShouldReturnZeroForEmptyImageOrStoredOffsetForNonEmpty(int imageLength, int initialOffset, int expectedOffsetProperty)
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            byte[] testImage = new byte[imageLength];
            int offset = initialOffset;

            // Act
            offsetLengthImage.Set(testImage, ref offset);

            // Assert
            Assert.That(offsetLengthImage.Offset, Is.EqualTo(expectedOffsetProperty));
        }

        /// <summary>
        /// Tests that Set method works correctly with negative offset values.
        /// </summary>
        [Test]
        public void Set_WithNegativeOffset_ShouldSetOffsetAndCalculateNewOffset()
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            byte[] testImage = new byte[10];
            int offset = -50;
            int expectedNewOffset = -50 + 10;

            // Act
            offsetLengthImage.Set(testImage, ref offset);

            // Assert
            Assert.That(offsetLengthImage.Image, Is.EqualTo(testImage));
            Assert.That(offsetLengthImage.Offset, Is.EqualTo(-50));
            Assert.That(offset, Is.EqualTo(expectedNewOffset));
        }

        /// <summary>
        /// Tests that Set method can be called multiple times and correctly updates state each time.
        /// </summary>
        [Test]
        public void Set_CalledMultipleTimes_ShouldUpdateStateCorrectly()
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            byte[] firstImage = new byte[10];
            byte[] secondImage = new byte[20];
            int offset = 0;

            // Act - First call
            offsetLengthImage.Set(firstImage, ref offset);

            // Assert - After first call
            Assert.That(offsetLengthImage.Image, Is.EqualTo(firstImage));
            Assert.That(offsetLengthImage.Length, Is.EqualTo(10));
            Assert.That(offsetLengthImage.Offset, Is.EqualTo(0));
            Assert.That(offset, Is.EqualTo(10));

            // Act - Second call
            offsetLengthImage.Set(secondImage, ref offset);

            // Assert - After second call
            Assert.That(offsetLengthImage.Image, Is.EqualTo(secondImage));
            Assert.That(offsetLengthImage.Length, Is.EqualTo(20));
            Assert.That(offsetLengthImage.Offset, Is.EqualTo(10));
            Assert.That(offset, Is.EqualTo(30));
        }

        /// <summary>
        /// Tests that Set method works correctly with int.MaxValue as offset (boundary condition).
        /// </summary>
        [Test]
        public void Set_WithMaxIntOffset_ShouldSetOffsetAndCalculateNewOffset()
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            byte[] testImage = new byte[0];
            int offset = int.MaxValue;

            // Act
            offsetLengthImage.Set(testImage, ref offset);

            // Assert
            Assert.That(offsetLengthImage.Image, Is.EqualTo(testImage));
            Assert.That(offsetLengthImage.Offset, Is.EqualTo(0)); // Empty image returns 0
            Assert.That(offset, Is.EqualTo(int.MaxValue)); // MaxValue + 0 = MaxValue
        }

        /// <summary>
        /// Tests that Set method works correctly with int.MinValue as offset (boundary condition).
        /// </summary>
        [Test]
        public void Set_WithMinIntOffset_ShouldSetOffsetAndCalculateNewOffset()
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            byte[] testImage = new byte[10];
            int offset = int.MinValue;
            int expectedNewOffset = int.MinValue + 10;

            // Act
            offsetLengthImage.Set(testImage, ref offset);

            // Assert
            Assert.That(offsetLengthImage.Image, Is.EqualTo(testImage));
            Assert.That(offsetLengthImage.Offset, Is.EqualTo(int.MinValue));
            Assert.That(offset, Is.EqualTo(expectedNewOffset));
        }

        /// <summary>
        /// Tests that Set method correctly handles a large image array.
        /// </summary>
        [Test]
        public void Set_WithLargeImageArray_ShouldSetImageAndUpdateOffset()
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            byte[] largeImage = new byte[1000000];
            int offset = 500;
            int expectedNewOffset = 500 + 1000000;

            // Act
            offsetLengthImage.Set(largeImage, ref offset);

            // Assert
            Assert.That(offsetLengthImage.Image, Is.EqualTo(largeImage));
            Assert.That(offsetLengthImage.Length, Is.EqualTo(1000000));
            Assert.That(offsetLengthImage.Offset, Is.EqualTo(500));
            Assert.That(offset, Is.EqualTo(expectedNewOffset));
        }

        /// <summary>
        /// Tests that the Length property returns the correct value for different image sizes.
        /// Verifies that Length correctly reflects the current state of the Image byte array.
        /// </summary>
        /// <param name="imageSize">The size of the image byte array to test.</param>
        /// <param name="expectedLength">The expected length value.</param>
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(10, 10)]
        [TestCase(100, 100)]
        [TestCase(1000, 1000)]
        [TestCase(int.MaxValue - 1000, int.MaxValue - 1000)]
        public void Length_WithVariousImageSizes_ReturnsCorrectLength(int imageSize, int expectedLength)
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            if (imageSize > 0)
            {
                offsetLengthImage.Image = new byte[imageSize];
            }

            // Act
            var actualLength = offsetLengthImage.Length;

            // Assert
            Assert.That(actualLength, Is.EqualTo(expectedLength));
        }

        /// <summary>
        /// Tests that the Length property returns 0 when Image is set to an empty array.
        /// Verifies the default behavior when the image data is empty.
        /// </summary>
        [Test]
        public void Length_WhenImageIsEmpty_ReturnsZero()
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage
            {
                Image = []
            };

            // Act
            var actualLength = offsetLengthImage.Length;

            // Assert
            Assert.That(actualLength, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that the Length property updates correctly when Image is changed.
        /// Verifies that Length reflects the current state of the Image property dynamically.
        /// </summary>
        [Test]
        public void Length_WhenImageIsUpdated_ReturnsNewLength()
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage
            {
                Image = new byte[5]
            };

            // Act & Assert - Initial length
            Assert.That(offsetLengthImage.Length, Is.EqualTo(5));

            // Act - Update image
            offsetLengthImage.Image = new byte[20];

            // Assert - New length
            Assert.That(offsetLengthImage.Length, Is.EqualTo(20));

            // Act - Set to empty
            offsetLengthImage.Image = [];

            // Assert - Zero length
            Assert.That(offsetLengthImage.Length, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that the Offset property returns 0 when the Image is empty (default state).
        /// This verifies the conditional logic that returns 0 when Image.Length is 0.
        /// </summary>
        [Test]
        public void Offset_WhenImageIsEmptyByDefault_Returns0()
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();

            // Act
            var result = offsetLengthImage.Offset;

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that the Offset property returns 0 when the Image is explicitly set to an empty array,
        /// regardless of the offset value provided to the Set method.
        /// This verifies that the Offset property correctly checks Image.Length before returning _offset.
        /// </summary>
        /// <param name="offsetValue">The offset value to test with an empty image.</param>
        [TestCase(0)]
        [TestCase(100)]
        [TestCase(-50)]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void Offset_WhenImageIsEmptyAfterSet_Returns0(int offsetValue)
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            var emptyImage = Array.Empty<byte>();
            var offset = offsetValue;

            // Act
            offsetLengthImage.Set(emptyImage, ref offset);
            var result = offsetLengthImage.Offset;

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that the Offset property returns the correct offset value when the Image has data.
        /// This verifies that when Image.Length > 0, the Offset property returns the _offset field value.
        /// </summary>
        /// <param name="offsetValue">The offset value to test.</param>
        /// <param name="imageSize">The size of the image array.</param>
        [TestCase(0, 1)]
        [TestCase(100, 5)]
        [TestCase(1000, 10)]
        [TestCase(-50, 3)]
        [TestCase(int.MaxValue, 1)]
        [TestCase(int.MinValue, 100)]
        public void Offset_WhenImageHasData_ReturnsOffsetValue(int offsetValue, int imageSize)
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            var image = new byte[imageSize];
            var offset = offsetValue;

            // Act
            offsetLengthImage.Set(image, ref offset);
            var result = offsetLengthImage.Offset;

            // Assert
            Assert.That(result, Is.EqualTo(offsetValue));
        }

        /// <summary>
        /// Tests that the Offset property returns the updated offset value after multiple Set calls.
        /// This verifies that the Offset property correctly reflects the most recent _offset value
        /// when the Image has data.
        /// </summary>
        [Test]
        public void Offset_AfterMultipleSetCalls_ReturnsLatestOffsetValue()
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            var firstImage = new byte[5];
            var secondImage = new byte[10];
            var firstOffset = 100;
            var secondOffset = 500;

            // Act
            offsetLengthImage.Set(firstImage, ref firstOffset);
            var firstResult = offsetLengthImage.Offset;

            offsetLengthImage.Set(secondImage, ref secondOffset);
            var secondResult = offsetLengthImage.Offset;

            // Assert
            Assert.That(firstResult, Is.EqualTo(100));
            Assert.That(secondResult, Is.EqualTo(500));
        }

        /// <summary>
        /// Tests that the Offset property returns 0 when transitioning from a non-empty to an empty Image.
        /// This verifies the conditional check Image.Length > 0 correctly evaluates to false after
        /// setting an empty array, even if _offset has a non-zero value.
        /// </summary>
        [Test]
        public void Offset_WhenTransitioningFromNonEmptyToEmptyImage_Returns0()
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            var nonEmptyImage = new byte[10];
            var emptyImage = Array.Empty<byte>();
            var offset1 = 100;
            var offset2 = 200;

            // Act
            offsetLengthImage.Set(nonEmptyImage, ref offset1);
            var resultWithData = offsetLengthImage.Offset;

            offsetLengthImage.Set(emptyImage, ref offset2);
            var resultAfterEmpty = offsetLengthImage.Offset;

            // Assert
            Assert.That(resultWithData, Is.EqualTo(100));
            Assert.That(resultAfterEmpty, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that the Offset property correctly handles a single-byte image.
        /// This is a boundary test for the Image.Length > 0 condition.
        /// </summary>
        [Test]
        public void Offset_WithSingleByteImage_ReturnsOffsetValue()
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            var singleByteImage = new byte[1];
            var offset = 42;

            // Act
            offsetLengthImage.Set(singleByteImage, ref offset);
            var result = offsetLengthImage.Offset;

            // Assert
            Assert.That(result, Is.EqualTo(42));
        }

        /// <summary>
        /// Tests that the Offset property correctly handles a very large image array.
        /// This verifies the property works correctly with boundary image sizes.
        /// </summary>
        [Test]
        public void Offset_WithLargeImage_ReturnsOffsetValue()
        {
            // Arrange
            var offsetLengthImage = new OffsetLengthImage();
            var largeImage = new byte[10000];
            var offset = 999;

            // Act
            offsetLengthImage.Set(largeImage, ref offset);
            var result = offsetLengthImage.Offset;

            // Assert
            Assert.That(result, Is.EqualTo(999));
        }
    }
}