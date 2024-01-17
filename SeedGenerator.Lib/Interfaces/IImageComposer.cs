using SeedGenerator.Lib.Data;
using Svg;

namespace SeedGenerator.Lib.Interfaces
{
    public interface IImageComposer
    {
        SvgDocument ComposeImageRecto(Document document);
        SvgDocument ComposeImageVerso(Document document);
    }
}