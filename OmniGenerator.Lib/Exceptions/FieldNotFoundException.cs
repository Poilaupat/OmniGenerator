using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Exceptions
{
    internal class FieldNotFoundException : Exception
    {
        public FieldNotFoundException() : base() { }

        public FieldNotFoundException(string message) : base(message) { }

        public FieldNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
