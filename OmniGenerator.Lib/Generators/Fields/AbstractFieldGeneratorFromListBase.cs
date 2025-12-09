using OmniGenerator.Lib.Interfaces.FieldGenerators;
using System.Collections.Generic;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The base class for all generators that pick random values from a list
    /// </summary>
    /// <typeparam name="TGenerator">The generator type</typeparam>
    /// <typeparam name="TCollection">The type of collection</typeparam>
    internal abstract class AbstractFieldGeneratorFromListBase<TGenerator, TCollection>
        : AbstractFieldGenerator<TGenerator>, IFieldGeneratorFromList<TCollection>
        where TGenerator : notnull
        where TCollection : notnull
    {
        public virtual IEnumerable<TCollection> List { get; set; }

        /// <summary>
        /// Creates a new <see cref="AbstractFieldGeneratorFromListBase{TGenerator, TCollection}"/>
        /// </summary>
        /// <param name="name">The name of the generator</param>
        /// <param name="list">The resolved list (from file or configuration)</param>
        public AbstractFieldGeneratorFromListBase(string name, IEnumerable<TCollection> list)
            : base(name)
        {
            List = list;
        }

        protected override abstract TGenerator GenerateValue();
    }
}
