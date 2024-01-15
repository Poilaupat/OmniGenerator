using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.ImageComposers;
using Svg;
using System.Drawing;

namespace SeedGenerator.Plugins.Image.Composers
{
    public class CouponSepaComposer : ImageComposerBase
    {
        public CouponSepaComposer()
        {
            Width = 175;
            Height = 80;
        }

        public override void ComposeDocumentImages(Document document)
        {
            ComposeDocumentImageRecto(document);
        }

        private void ComposeDocumentImageRecto(Document document)
        {
            var svg = new SvgDocument();
            svg.ViewBox = new SvgViewBox(0, 0, Width, Height);

            DrawRectoBackground(svg);

            document.RectoImage = svg;
        }

        private void DrawRectoBackground(SvgDocument svg)
        {
            svg.Fill = new SvgColourServer(Color.White);

            DrawText(svg, "Coupon S€PA", "title", 6.5f, 16f, "Arial", 20, Color.Black);
        }
    }
}
