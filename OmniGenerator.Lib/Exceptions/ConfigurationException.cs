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
    internal class ConfigurationException : Exception
    {
        public List<string> Errors { get; set; } = new List<string>();

        public ConfigurationException() { }

        public ConfigurationException(string message) : base(message) { }

        public ConfigurationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
