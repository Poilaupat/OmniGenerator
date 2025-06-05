using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.Interfaces.FieldGenerators
{
    /// <summary>  
    /// Represents a field generator that depends on other field generators to provide values.  
    /// Instances must provides mechanisms to manage and check dependencies (including transitive dependencies and circular dependencies).  
    /// </summary>  
    internal interface IFieldGeneratorDependent : IFieldGenerator
    {
        /// <summary>  
        /// Gets the list of names of all generators this <see cref="IFieldGeneratorDependent"/> is dependent upon.  
        /// </summary>  
        List<string> DependenceNames { get; }

        /// <summary>  
        /// Gets the list of all generator instances this <see cref="IFieldGeneratorDependent"/> is dependent upon.  
        /// </summary>  
        List<IFieldGenerator> GeneratorDependencies { get; }

        /// <summary>  
        /// Determines whether this <see cref="IFieldGeneratorDependent"/> is dependent on the specified generator.  
        /// This method checks dependencies transitively. For example:  
        /// - If A is dependent on B  
        /// - If B is dependent on C  
        /// - Then A.IsDependentUpon(C) returns true.  
        /// </summary>  
        /// <param name="generator">The generator to check dependency upon.</param>  
        /// <returns>True if this instance is dependent on the specified generator; otherwise, false.</returns>  
        /// <exception cref="ConfigurationException">Thrown if a circular dependency is detected among field generators.</exception>  
        bool IsDependentUpon(IFieldGenerator generator);
    }
}
