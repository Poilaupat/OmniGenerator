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
    public class ConfigurationException : Exception
    {
        public List<string> Errors { get; set; } = new();

        public ConfigurationException() { }

        public ConfigurationException(string message) : base(message) { }

        public ConfigurationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
