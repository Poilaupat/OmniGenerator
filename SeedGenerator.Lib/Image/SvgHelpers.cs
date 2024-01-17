using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Image
{
    public static class SvgHelpers
    {
        public static void DrawText(this SvgDocument svg, string text, string id, float x, float y, string fontFamilly, float fontSize, Color color)
        {
            var tag = new SvgText() { ID = id, FontFamily = fontFamilly, FontSize = new SvgUnit(fontSize), Fill = new SvgColourServer(color) };
            tag.X.Add(new SvgUnit(x));
            tag.Y.Add(new SvgUnit(y));
            tag.Nodes.Add(new SvgContentNode { Content = text });
            svg.Children.Add(tag);
        }

        public static SvgDocument NewBlankSvg(int width, int height)
        {
            var svg = new SvgDocument();
            svg.ViewBox = new SvgViewBox(0, 0, width, height);
            svg.Fill = new SvgColourServer(Color.White);
            return svg;
        }

        public static byte[] GetFontBytes(string fontName)
        {
            string fontPath = $"SeedGenerator.Lib.Resources.Fonts.{fontName}";

            using (var fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fontPath))
            using (var ms = new MemoryStream())
            {
                if (fontStream is not null)
                {
                    fontStream.CopyTo(ms);
                }
                return ms.ToArray();
            }
        }
    }
}
