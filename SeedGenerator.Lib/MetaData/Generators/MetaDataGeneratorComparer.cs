namespace SeedGenerator.Lib.MetaData.Generators
{
    internal class MetaDataGeneratorComparer : IComparer<MetaDataGeneratorBase>
    {
        public int Compare(MetaDataGeneratorBase? x, MetaDataGeneratorBase? y)
        {
            if (x is null || y is null)
                throw new ArgumentNullException();

            if (x is MetaDataGeneratorDependantBase || y is MetaDataGeneratorDependantBase)
            {
                if (x is MetaDataGeneratorDependantBase xd && y is MetaDataGeneratorDependantBase yd)
                {
                    if (xd.DependantUpon.Equals(y.Name, StringComparison.InvariantCultureIgnoreCase)
                            && yd.DependantUpon.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase))
                        return x.Name.CompareTo(y.Name);
                    else if (xd.DependantUpon.Equals(y.Name, StringComparison.InvariantCultureIgnoreCase))
                        return 1;
                    else
                        return -1;
                }
                else if (x is MetaDataGeneratorDependantBase)
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
