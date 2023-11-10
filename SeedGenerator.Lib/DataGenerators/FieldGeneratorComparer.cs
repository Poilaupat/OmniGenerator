using System.Linq;

namespace SeedGenerator.Lib.Builders
{
    internal class FieldGeneratorComparer : IComparer<FieldGeneratorBase>
    {
        public int Compare(FieldGeneratorBase? x, FieldGeneratorBase? y)
        {
            if (x is null || y is null)
                throw new ArgumentNullException();

            if (x is FieldGeneratorDependantBase || y is FieldGeneratorDependantBase)
            {
                if (x is FieldGeneratorDependantBase xd && y is FieldGeneratorDependantBase yd)
                {
                    if (xd.Dependances.Keys.Contains(y.Name)
                            && yd.Dependances.Keys.Contains(x.Name))
                        return x.Name.CompareTo(y.Name);
                    else if (xd.Dependances.Keys.Contains(y.Name))
                        return -1;
                    else
                        return 1;
                }
                else if (x is FieldGeneratorDependantBase)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }

            }
            else
            {
                return x.Name.CompareTo(y.Name);
            }
        }
    }
}
