using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot
{
    internal class OffsetLengthImage
    {
        private int _offset = 0;

        public byte[] Image { get; set; } = Array.Empty<byte>();
        public int Offset => Image.Length > 0 ? _offset : 0;
        public int Length => Image.Length;

        public void Set(byte[] image, ref int offset) 
        { 
            Image = image;
            _offset = offset;

            offset = _offset + Image.Length;
        }
    }
}
