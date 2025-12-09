using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Interfaces.FieldGenerators;
using OmniGenerator.Lib.Tools;
using System.Text.RegularExpressions;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The base type for all dependent generators.
    /// These generators depend on the values of other generators to generate their own values.
    /// </summary>
    /// <typeparam name="T">The type of the values produced by the <see cref="AbstractFieldGeneratorDependant{T}"/>.</typeparam>
    internal abstract class AbstractFieldGeneratorDependant<T> : AbstractFieldGenerator<T>, IFieldGeneratorDependent
        where T : notnull
    {
        /// <summary>
        /// Gets the names of the dependencies for this <see cref="AbstractFieldGeneratorDependant{T}"/>.
        /// </summary>
        public List<string> DependenceNames { get; private set; } = new List<string>();

        /// <summary>
        /// Gets the generator dependencies for this <see cref="AbstractFieldGeneratorDependant{T}"/>.
        /// </summary>
        public List<IFieldGenerator> GeneratorDependencies { get; } = new List<IFieldGenerator>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractFieldGeneratorDependant{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the generator.</param>
        /// <param name="dependentUpon">The names of the generator dependencies, separated by commas, semicolons, or pipes.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="name"/> is null, empty, or whitespace.</exception>
        protected AbstractFieldGeneratorDependant(string name, string dependentUpon)
            : base(name)
        {
            string[] dependenceNames = Regex.Replace(dependentUpon, @"\s", string.Empty).Split(new[] { ',', ';', '|' });

            foreach (var dependenceName in dependenceNames)
            {
                DependenceNames.Add(dependenceName);
            }
        }

        /// <summary>
        /// Determines whether this <see cref="AbstractFieldGeneratorDependant{T}"/> depends on the specified generator.
        /// The dependency can be either direct or transitive.
        /// </summary>
        /// <param name="generator">The generator to check the dependency against.</param>
        /// <returns>True if this instance is dependent on the specified generator; otherwise, false.</returns>
        /// <exception cref="ConfigurationException">Thrown if a circular dependency is detected among field generators.</exception>
        public bool IsDependentUpon(IFieldGenerator generator)
        {
            return IsDependentUpon(this, generator, this.Name);
        }

        /// <summary>
        /// Recursively checks whether the specified generator is a dependency of this instance.
        /// </summary>
        /// <param name="x">The dependent generator to check.</param>
        /// <param name="y">The generator to check the dependency against.</param>
        /// <param name="fieldname">The name of the field being checked for circular dependencies.</param>
        /// <returns>True if the specified generator is a dependency; otherwise, false.</returns>
        /// <exception cref="ConfigurationException">Thrown if a circular dependency is detected.</exception>
        private bool IsDependentUpon(IFieldGeneratorDependent x, IFieldGenerator y, string fieldname)
        {
            foreach (var dependence in x.GeneratorDependencies)
            {
                // Circular dependency  
                if (fieldname == dependence.Name)
                    throw new ConfigurationException($"Field generator circular dependency detected for field {dependence.Name} !");

                // Direct dependency  
                if (y.Name == dependence.Name)
                    return true;

                // Transitive dependency  
                if (dependence is IFieldGeneratorDependent dependentDependence
                    && IsDependentUpon(dependentDependence, y, fieldname))
                    return true;
            }

            return false;
        }
    }
}
