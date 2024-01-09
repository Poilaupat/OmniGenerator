using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.ImageComposers;
using SeedGenerator.Lib.Interfaces;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Plugins.Image.Composers
{
    public class CouponSepaComposer : ImageComposerBase
    {
        public CouponSepaComposer()
        {
            Width = 175;
            Height = 80;
        }

        public override void ComposeDocumentImages(DocumentData document)
        {
            ComposeDocumentImageRecto(document);
        }

        private void ComposeDocumentImageRecto(DocumentData document)
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
