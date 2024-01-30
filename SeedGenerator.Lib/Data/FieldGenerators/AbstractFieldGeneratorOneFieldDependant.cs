namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal abstract class AbstractFieldGeneratorOneFieldDependant : AbstractFieldGeneratorDependant
    {
        protected AbstractFieldGeneratorOneFieldDependant(string name, string dependentUpon) 
            : base(name, dependentUpon)
        {
            if (DependenceNames.Count > 1)
            {
                throw new ArgumentException($"{name} generator support only one dependence");
            }
        }
    }
}
