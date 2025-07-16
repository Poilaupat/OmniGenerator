using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot
{
    /// <summary>
    /// Represents an image with its offset and length information for fixed-length packaging.
    /// </summary>
    internal class OffsetLengthImage
    {
        private int _offset = 0;

        /// <summary>
        /// Gets or sets the image data as a byte array.
        /// </summary>
        public byte[] Image { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// Gets the offset of the image in the package.
        /// Returns zero if the image is empty.
        /// </summary>
        public int Offset => Image.Length > 0 ? _offset : 0;

        /// <summary>
        /// Gets the length of the image data in bytes.
        /// </summary>
        public int Length => Image.Length;

        /// <summary>
        /// Sets the image data and updates the offset reference.
        /// </summary>
        /// <param name="image">The image data to set.</param>
        /// <param name="offset">
        /// The reference offset to set for this image. 
        /// After setting, the offset is updated to point to the next position after the image.
        /// </param>
        public void Set(byte[] image, ref int offset)
        {
            Image = image;
            _offset = offset;
            offset = _offset + Image.Length;
        }
    }
}
