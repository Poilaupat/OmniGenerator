using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Tools;
using Svg;
using System.Drawing;

namespace SeedGenerator.Lib.Image.Composer
{
    public abstract class ImageComposerBase : IImageComposer
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public virtual SvgDocument ComposeImageRecto(Document document)
        {
            return ImageTools.NewBlankSvg(Width, Height);
        }

        public virtual SvgDocument ComposeImageVerso(Document document)
        {
            return ImageTools.NewBlankSvg(Width, Height);
        }
    }
}
