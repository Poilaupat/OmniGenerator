using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Interfaces;
using System.Text.RegularExpressions;

namespace OmniGenerator.Lib.Data.FieldGenerators
{
    /// <summary>
    /// The base type for all dependent generators
    /// Those generator depends on the value of other generators to generates its own values
    /// </summary>
    /// <typeparam name="T">The type of the values produced by the <see cref="AbstractFieldGeneratorDependant"/></typeparam>
    internal abstract class AbstractFieldGeneratorDependant<T> : AbstractFieldGenerator<T>, IFieldGeneratorDependent
        where T : notnull
    {
        /// <summary>
        /// The names of this <see cref="AbstractFieldGeneratorDependant{T}"/> dependencies
        /// </summary>
        public List<string> DependenceNames { get; private set; } = new List<string>();

        /// <summary>
        /// The dependencies of this <see cref="AbstractFieldGeneratorDependant{T}"/>
        /// </summary>
        public List<IFieldGenerator> Dependences { get; } = new List<IFieldGenerator>();

        /// <summary>
        /// Creates a new <see cref="AbstractFieldGeneratorDependant{T}"/>
        /// </summary>
        /// <param name="name">The name of the generator</param>
        /// <param name="dependentUpon">The names of the generator dependencies separated by comas, semicolons or pipes</param>
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
        /// Indicates if this <see cref="AbstractFieldGeneratorDependant{T}"/> depends on the specified generator
        /// Dependence can be either direct or transitive
        /// </summary>
        /// <param name="generator">The generator to check the dependency against</param>
        /// <returns>True is this instance of <see cref="AbstractFieldGeneratorDependant{T}"/> is dependent on the specied generator. False if not</returns>
        public bool IsDependentUpon(IFieldGenerator generator)
        {
            return IsDependentUpon(this, generator, this.Name);
        }

        private bool IsDependentUpon(IFieldGeneratorDependent x, IFieldGenerator y, string fieldname)
        {
            foreach (var dependence in x.Dependences)
            {
                //Circular dependency
                if (fieldname == dependence.Name)
                    throw new ParamException($"Field generator circular dependency detected for field {dependence.Name} !");

                //Direct dependance
                if (y.Name == dependence.Name)
                    return true;

                //Transitive dependance
                if (dependence is IFieldGeneratorDependent dependentDependence 
                    && IsDependentUpon(dependentDependence, y, fieldname))
                    return true;
            }

            return false;
        }
    }
}
