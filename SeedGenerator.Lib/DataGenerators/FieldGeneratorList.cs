using SeedGenerator.Lib.Tools;

namespace SeedGenerator.Lib.DataGenerators
{
    internal class FieldGeneratorList : AbstractFieldGenerator
    {
        public string ListPath { get; }

        public FieldGeneratorList(string name, string listpath)
            : base(name)
        {
            ListPath = listpath;
        }

        public override string NextValue()
        {
            var list = ListCache.GetList(ListPath);

            if (list.Length > 0)
            {
                return list[new Random().Next(0, list.Length)];
            }
            else
            {
                return string.Empty;
            }
        }




    }
}
