using Svg;
using System.Drawing;
using System.Reflection;

namespace SeedGenerator.Lib.Tools
{
    /// <summary>
    /// A set of tools for SVG images
    /// </summary>
    public static class ImageTools
    {
        /// <summary>
        /// Renders the given SVG document to a <see cref="Bitmap"/> at the specified vertical and horizontal resolution
        /// </summary>
        /// <param name="svg">The SVG xml file to render</param>
        /// <param name="hztlResolution">The horizontal resolution in pixel per inch</param>
        /// <param name="vrtlResolution">The vertical resolution in pixel per inch</param>
        /// <returns>The rendered bitmap</returns>
        public static Bitmap RenderSvg(SvgDocument svg, int hztlResolution, int vrtlResolution)
        {
            //Reminders :
            // SVG witdh / heigth are set to physical document measures in mm
            // Resolution is given in pixel per inch
            // One inch is 25.4mm

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

        /// <summary>
        /// Renders the given SVG image to a Bitmap at the specified resolution
        /// </summary>
        /// <param name="svg">The SVG xml document to render</param>
        /// <param name="resolution">The resolution in pixel per inch</param>
        /// <returns>The rendered bitmap</returns>
        public static Bitmap RenderSvg(SvgDocument svg, int resolution)
        {
            return RenderSvg(svg, resolution, resolution);
        }

        /// <summary>
        /// Draws a text in the given SVG document
        /// </summary>
        /// <param name="svg">The svg</param>
        /// <param name="text">The test to insert</param>
        /// <param name="id">The id tag of the text in the svg document</param>
        /// <param name="x">The x position of the text in millimeter</param>
        /// <param name="y">The y position of the text in millimeter</param>
        /// <param name="fontFamilly">The font of the text</param>
        /// <param name="fontSize">The size of the font</param>
        /// <param name="color">The color of the font</param>
        public static void DrawText(this SvgDocument svg, string text, string id, float x, float y, string fontFamilly, float fontSize, Color color)
        {
            var tag = new SvgText() { ID = id, FontFamily = fontFamilly, FontSize = new SvgUnit(fontSize), Fill = new SvgColourServer(color) };
            tag.X.Add(new SvgUnit(x));
            tag.Y.Add(new SvgUnit(y));
            tag.Nodes.Add(new SvgContentNode { Content = text });
            svg.Children.Add(tag);
        }

        /// <summary>
        /// Creates a new blank SVG document a specified size
        /// </summary>
        /// <param name="width">The width of the document in millimeter</param>
        /// <param name="height">The height of the document in millimeter</param>
        /// <returns>The new SVG document</returns>
        public static SvgDocument NewBlankSvg(int width, int height)
        {
            var svg = new SvgDocument();
            svg.ViewBox = new SvgViewBox(0, 0, width, height);
            svg.Fill = new SvgColourServer(Color.White);
            return svg;
        }

        /// <summary>
        /// Loads a font from the dll resources
        /// If no embedded font is found, nothing is loaded and a default font will be used
        /// </summary>
        /// <param name="fontName">The font name</param>
        /// <returns></returns>
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
