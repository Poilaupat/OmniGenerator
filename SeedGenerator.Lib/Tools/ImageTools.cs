using Svg;
using System.Drawing;
using System.Reflection;

namespace SeedGenerator.Lib.Tools
{
    public static class ImageTools
    {
        public static Bitmap RenderSvg(SvgDocument svg, int hztlResolution, int vrtlResolution)
        {
            int rasterX = (int)(hztlResolution * svg.ViewBox.Width / 25.4f);
            int rasterY = (int)(vrtlResolution * svg.ViewBox.Height / 25.4f);

            var raster = new Bitmap(rasterX, rasterY);
            using ( var g = Graphics.FromImage(raster))
            {
                g.Clear(Color.White);
                var svgBitmap = svg.Draw(rasterX, rasterY);
                g.DrawImage(svgBitmap, 0, 0);
            }
            raster.SetResolution(hztlResolution, vrtlResolution);

            return raster;
        }

        public static Bitmap RenderSvg(SvgDocument svg, int resolution)
        {
            return RenderSvg(svg, resolution, resolution);
        }

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
