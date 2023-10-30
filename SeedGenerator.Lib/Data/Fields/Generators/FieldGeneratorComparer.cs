namespace SeedGenerator.Lib.Data.Fields.Generators
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
                    if (xd.DependantUpon.Equals(y.Name, StringComparison.InvariantCultureIgnoreCase)
                            && yd.DependantUpon.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase))
                        return x.Name.CompareTo(y.Name);
                    else if (xd.DependantUpon.Equals(y.Name, StringComparison.InvariantCultureIgnoreCase))
                        return 1;
                    else
                        return -1;
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
