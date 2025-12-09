using OmniGenerator.Lib.Interfaces.FieldGenerators;

namespace OmniGenerator.Lib.Generators
{
    /// <summary>
    /// Orders the <see cref="IFieldGenerator"/>.
    /// Ensure that <see cref="IFieldGeneratorDependent"/> are enumerated after the <see cref="IFieldGenerator"/> they depends upon
    /// </summary>
    internal class FieldGeneratorComparer : IComparer<IFieldGenerator>
    {
        /// <summary>
        /// Rules for comparison :
        ///     1. IFieldGenerator < IFieldGeneratorDependant
        ///     2. IFieldGeneratorDependant A < IFieldGeneratorDependant B if B is dependant on A
        ///     3. if "equal", order alphabetically against Name property
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public int Compare(IFieldGenerator? x, IFieldGenerator? y)
        {
            if (x is null || y is null)
                throw new ArgumentNullException($"Cannot compare null {nameof(IFieldGenerator)}");


            //Rule 1
            if (x is not IFieldGeneratorDependent && y is IFieldGeneratorDependent)
            {
                return -1;
            }

            //Rule 1
            if (x is IFieldGeneratorDependent && y is not IFieldGeneratorDependent)
            {
                return 1;
            }

            //Rule 2
            if (x is IFieldGeneratorDependent xd && y is IFieldGeneratorDependent yd)
            {
                if (yd.IsDependentUpon(xd))
                {
                    return -1;
                }

                if (xd.IsDependentUpon(yd))
                {
                    return 1;
                }
            }

            //Rule 3
            return x.Name.CompareTo(y.Name);
        }
    }
}
