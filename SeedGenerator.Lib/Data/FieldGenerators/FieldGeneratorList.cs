using SeedGenerator.Lib.Tools;

namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorList : AbstractFieldGenerator<string>
    {
        public string ListPath { get; }

        public FieldGeneratorList(string name, string listpath)
            : base(name)
        {
            ListPath = listpath;
        }

        protected override string GenerateValue()
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
