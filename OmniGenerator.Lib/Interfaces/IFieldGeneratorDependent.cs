using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniGenerator.Lib.Exceptions;

namespace OmniGenerator.Lib.Interfaces
{
    internal interface IFieldGeneratorDependent : IFieldGenerator
    {
        /// <summary>
        /// The list of all the generator names this <see cref="IFieldGeneratorDependent"/> is dependent upon
        /// </summary>
        List<string> DependenceNames { get; }

        /// <summary>
        /// The list of all the other generators this <see cref="IFieldGeneratorDependent"/> is dependent upon
        /// </summary>
        List<IFieldGenerator> Dependences { get; }

        /// <summary>
        /// Indicates if the <see cref="IFieldGeneratorDependent"/> is dependant on the generator specified in the paramters
        /// This method must check dependencies transitively. This means that :
        ///     - If A is dependant on B
        ///     - If B is dependant on C
        ///     - Then A.IsDependentUpon(C) must return true
        /// </summary>
        /// <param name="generator">The generator to check dependency upon</param>
        /// <returns>True if instance is dependent on generator, false if not</returns>
        /// <exception cref="ConfigurationException">Happens if a circular dependency is detected in field generators</exception>
        bool IsDependentUpon(IFieldGenerator generator);
    }
}
