using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;
using Svg;
using System.Drawing;

namespace SeedGenerator.Lib.ImageComposers
{
    public abstract class ImageComposerBase : IImageComposer
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public abstract void ComposeDocumentImages(Document document);

        //Helpers
        protected void DrawText(SvgDocument svg, string text, string id, float x, float y, string fontFamilly, float fontSize, Color color)
        {
            var tag = new SvgText() { ID = id, FontFamily = fontFamilly, FontSize = new SvgUnit(fontSize), Fill = new SvgColourServer(color) };
            tag.X.Add(new SvgUnit(x));
            tag.Y.Add(new SvgUnit(y));
            tag.Nodes.Add(new SvgContentNode { Content = text });
            svg.Children.Add(tag);
        }
    }
}
