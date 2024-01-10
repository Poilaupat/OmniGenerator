using Svg;

namespace SeedGenerator.Lib.Data
{
    public class Document : Element
    {
        public SvgDocument? RectoImage { get; set; }
        public SvgDocument? VersoImage { get; set; }

        public Document(long id, string name)
            :base("document", name, id)
        {
        }
    }
}
