using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Interfaces
{
    /// <summary>
    /// Defines the interface of all field generators
    /// </summary>
    internal interface IFieldGenerator
    {
        /// <summary>
        /// The field generator name. It must be unique over all fields in parameters.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The last value generated
        /// </summary>
        /// <returns>The value</returns>
        object LastValue { get; }

        void RefreshValue();
    }
}
