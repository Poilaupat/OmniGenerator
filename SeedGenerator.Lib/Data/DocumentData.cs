using Svg;

namespace SeedGenerator.Lib.Data
{
    public class DocumentData
    {
        public string Name { get; set; }

        public FieldCollection Fields { get; set; } = new FieldCollection();

        public SvgDocument? Image { get; set; }

        public DocumentData(string name)
        {
            Name = name;
        }
    }
}
