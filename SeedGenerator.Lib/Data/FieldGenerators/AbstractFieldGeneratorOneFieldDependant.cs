namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal abstract class AbstractFieldGeneratorOneFieldDependant : AbstractFieldGeneratorDependant
    {
        protected AbstractFieldGeneratorOneFieldDependant(string name, string dependantUpon) 
            : base(name, dependantUpon)
        {
            if (Dependances.Count > 1)
            {
                throw new ArgumentException($"{name} generator support only one dependance");
            }
        }
    }
}
