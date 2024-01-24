using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Tools;
using Svg;
using System.Drawing;

namespace SeedGenerator.Lib.Image.Composer
{
    public class CouponSepaComposer : ImageComposerBase
    {
        public CouponSepaComposer()
        {
            Width = 175;
            Height = 80;
        }

        public override SvgDocument ComposeImageRecto(Document document)
        {
            var svg = ImageTools.NewBlankSvg(Width, Height);

            DrawRectoBackground(svg);

            return svg;
        }

        private void DrawRectoBackground(SvgDocument svg)
        {
            svg.DrawText("TIP S€PA", "title", 79f, 16f, "Arial", 12, Color.Black);
        }
    }
}
