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
        ///     2. IFieldGeneratorDependant A < IFieldGeneratorDependant B if B is dependant upon A
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
                Console.WriteLine($"{x.Name} < {y.Name} (x is not dependent < y is dependent)");
                return -1;
            }

            //Rule 1
            if (x is IFieldGeneratorDependent && y is not IFieldGeneratorDependent)
            {
                Console.WriteLine($"{x.Name} > {y.Name} (x is dependent > y is not dependent)");
                return 1;
            }

            //Rule 2
            if (x is IFieldGeneratorDependent xd && y is IFieldGeneratorDependent yd)
            {
                if (yd.IsDependentUpon(xd))
                {
                    Console.WriteLine($"{xd.Name} < {yd.Name} (y is dependent upon x)");
                    return -1;
                }

                if (xd.IsDependentUpon(yd))
                {
                    Console.WriteLine($"{xd.Name} > {yd.Name} (x is dependent upon y)");
                    return 1;
                }
            }

            //Rule 3
            Console.WriteLine($"{x.Name} == {y.Name} (alphabetical order)");
            return x.Name.CompareTo(y.Name);
        }
    }
}
