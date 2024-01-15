namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorComparer : IComparer<AbstractFieldGenerator>
    {
        public int Compare(AbstractFieldGenerator? x, AbstractFieldGenerator? y)
        {
            if (x is null || y is null)
                throw new ArgumentNullException();

            if (x is AbstractFieldGeneratorDependant || y is AbstractFieldGeneratorDependant)
            {
                if (x is AbstractFieldGeneratorDependant xd && y is AbstractFieldGeneratorDependant yd)
                {
                    if (xd.Dependances.Keys.Contains(y.Name)
                            && yd.Dependances.Keys.Contains(x.Name))
                        return x.Name.CompareTo(y.Name);
                    else if (xd.Dependances.Keys.Contains(y.Name))
                        return -1;
                    else
                        return 1;
                }
                else if (x is AbstractFieldGeneratorDependant)
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
