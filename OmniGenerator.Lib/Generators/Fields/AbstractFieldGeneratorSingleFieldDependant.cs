using OmniGenerator.Lib.Interfaces;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The base type for all generators that takes one and only one generator as dependence
    /// </summary>
    /// <typeparam name="T">The type of the values produced by the <see cref="AbstractFieldGeneratorOneFieldDependant"/></typeparam>
    internal abstract class AbstractFieldGeneratorSingleFieldDependant<T> : AbstractFieldGeneratorDependant<T>
        where T : notnull
    {
        /// <summary>
        /// Creates a new <see cref="AbstractFieldGeneratorSingleFieldDependant{T}"/>
        /// If the dependence string contains more than one dependence, throw a <see cref="ArgumentException"/>
        /// </summary>
        /// <param name="name">The generator name</param>
        /// <param name="dependentUpon">The generator dependence</param>
        /// <exception cref="ArgumentException"></exception>
        protected AbstractFieldGeneratorSingleFieldDependant(string name, string dependentUpon)
            : base(name, dependentUpon)
        {
            if (DependenceNames.Count > 1)
            {
                throw new ArgumentException($"{name} generator support only one dependence");
            }
        }
    }
}
