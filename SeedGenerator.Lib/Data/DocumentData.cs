using Svg;

namespace SeedGenerator.Lib.Data
{
    public class DocumentData
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public FieldCollection Fields { get; set; } = new FieldCollection();

        public SvgDocument? RectoImage { get; set; }

        public DocumentData(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
