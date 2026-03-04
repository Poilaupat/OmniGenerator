using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Generators.Interfaces
{
    internal interface IFieldGeneratorFromList<TCollection> : IFieldGenerator
        where TCollection : notnull
    {
        /// <summary>
        /// The collection of items where generated values are picked up
        /// </summary>
        IEnumerable<TCollection>? List { get; }
    }
}
