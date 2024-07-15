using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Exceptions
{
    /// <summary>
    /// The exception thrown when error occurs when reading or validating configuration
    /// </summary>
    internal class ParamException : Exception
    {
        public List<string> Errors { get; set; } = new List<string>();

        public ParamException() { }

        public ParamException(string message) : base(message) { }

        public ParamException(string message, Exception innerException) : base(message, innerException) { }
    }
}
