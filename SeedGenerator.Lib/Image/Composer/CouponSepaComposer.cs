using SeedGenerator.Lib.Data;
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
            var svg = SvgHelpers.NewBlankSvg(Width, Height);

            DrawRectoBackground(svg);

            return svg;
        }

        private void DrawRectoBackground(SvgDocument svg)
        {
            svg.DrawText("Coupon S€PA", "title", 6.5f, 16f, "Arial", 20, Color.Black);
        }
    }
}
