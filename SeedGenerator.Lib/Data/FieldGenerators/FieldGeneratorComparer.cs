using Microsoft.ProgramSynthesis.Transformation.Formula.Build.RuleNodeTypes;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorComparer : IComparer<AbstractFieldGenerator>
    {
        /// <summary>
        /// Rules for comparison :
        ///     1. AbstractFieldGenerator < AbstractFieldGeneratorDependant
        ///     2. AbstractFieldGeneratorDependant A < AbstractFieldGeneratorDependant B if B is dependant on A
        ///     3. if "equal" order alphabetically against Name property  
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public int Compare(AbstractFieldGenerator? x, AbstractFieldGenerator? y)
        {
            if (x is null || y is null)
                throw new ArgumentNullException($"Cannot compare null {nameof(AbstractFieldGenerator)}");

            var xIsFieldDependant = IsFieldDependantGenerator(x);
            var yIsFieldDependant = IsFieldDependantGenerator(y);

            //Rule 1
            if (!xIsFieldDependant && yIsFieldDependant)
            {
                return -1;
            }

            //Rule 1
            if(xIsFieldDependant && !yIsFieldDependant)
            {
                return 1;
            }

            //Rule 2 (note : there is no circular dependance detection for now)
            if (x is AbstractFieldGeneratorDependant xd && y is AbstractFieldGeneratorDependant yd)
            {
                if (yd.Dependences.Any(x => x.Name ==xd.Name)) 
                {
                    return -1;
                }
                
                if(xd.Dependences.Any(x => x.Name == yd.Name))
                {
                    return 1;
                }
            }

            //Rule 3
            //return x.Name.CompareTo(y.Name);
            return 0;
        }

        private bool IsFieldDependantGenerator(object x)
        {
            return x
                .GetType()
                .IsSubclassOf(typeof(AbstractFieldGeneratorDependant));
        }
    }
}
