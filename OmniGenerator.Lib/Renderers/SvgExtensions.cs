using Svg;
using Svg.Transforms;
using System.Drawing;
using System.Reflection;

namespace OmniGenerator.Lib.Renderers
{
    /// <summary>
    /// Provides extension methods and utility functions for working with SVG documents.
    /// Includes methods for drawing text, barcodes, and managing SVG elements.
    /// </summary>
    public static class SvgExtensions
    {
        /// <summary>
        /// Draws text in the given SVG document at the specified position.
        /// </summary>
        /// <param name="svg">The SVG document to draw on.</param>
        /// <param name="text">The text content to insert.</param>
        /// <param name="id">The unique ID tag for the text element in the SVG document.</param>
        /// <param name="x">The x position of the text in millimeters.</param>
        /// <param name="y">The y position of the text in millimeters.</param>
        /// <param name="fontFamilly">The font family name (e.g., "Arial", "Calibri", "OCRB").</param>
        /// <param name="fontSize">The font size in millimeters.</param>
        /// <param name="color">The color of the text.</param>
        /// <param name="fontWeight">The font weight (default: Normal).</param>
        /// <param name="anchor">The text anchor alignment (default: Start).</param>
        public static void DrawText(this SvgDocument svg, string text, string id, float x, float y, string fontFamilly, float fontSize, Color color, SvgFontWeight fontWeight = SvgFontWeight.Normal, SvgTextAnchor anchor = SvgTextAnchor.Start)
        {
            var tag = new SvgText() { ID = id, FontFamily = fontFamilly, FontSize = new SvgUnit(fontSize), Fill = new SvgColourServer(color), FontWeight = fontWeight, TextAnchor = anchor };
            tag.X.Add(new SvgUnit(x));
            tag.Y.Add(new SvgUnit(y));
            tag.Nodes.Add(new SvgContentNode { Content = text });
            svg.Children.Add(tag);
        }

        /// <summary>
        /// Creates a new blank SVG document with the specified dimensions.
        /// The document is initialized with a white background.
        /// </summary>
        /// <param name="width">The width of the document in millimeters.</param>
        /// <param name="height">The height of the document in millimeters.</param>
        /// <returns>A new <see cref="SvgDocument"/> with the specified dimensions.</returns>
        public static SvgDocument NewBlankSvg(int width, int height)
        {
            var svg = new SvgDocument();
            svg.ViewBox = new SvgViewBox(0, 0, width, height);
            svg.Fill = new SvgColourServer(Color.White);
            return svg;
        }

        /// <summary>
        /// Draws a barcode in the given SVG document at the specified position.
        /// Supports both 1D linear barcodes (Code128, EAN, UPC) and 2D matrix codes (QR Code, DataMatrix, Aztec).
        /// </summary>
        /// <param name="svg">The SVG document to draw on.</param>
        /// <param name="content">The data to encode in the barcode.</param>
        /// <param name="type">The type of barcode to generate (e.g., Code128, QRCode, EAN).</param>
        /// <param name="x">The x position in millimeters where the barcode will be placed.</param>
        /// <param name="y">The y position in millimeters where the barcode will be placed.</param>
        /// <param name="scale">The scale factor to apply to the barcode size.</param>
        /// <exception cref="ArgumentException">Thrown if the content is invalid for the specified barcode type.</exception>
        public static void DrawBarCode(this SvgDocument svg, string content, EBarCodeType type, float x, float y, float scale)
        {
            string barcodeStr = BarCodeRenderHelper.Generate(type, content);

            SvgDocument qrCodeSvg = SvgDocument.FromSvg<SvgDocument>(barcodeStr);
            SvgGroup group = new();
            group.CopyStyleAttributes(qrCodeSvg); //Barcoder generates SVG with styles in root tag, we need to copy them to the group to preserve the look of the barcode
            foreach (var child in qrCodeSvg.Children)
            {
                group.Children.Add(child);
            }
            group.Transforms = new SvgTransformCollection
            {
                new SvgTranslate(x, y),
                new SvgScale(scale)
            };

            svg.Children.Add(group);
        }

        /// <summary>
        /// Loads a font from the embedded DLL resources.
        /// If the specified font is not found in the resources, an empty byte array is returned,
        /// and the system will use a default font.
        /// </summary>
        /// <param name="fontName">The name of the font file to load (without path prefix).</param>
        /// <returns>A byte array containing the font data, or an empty array if the font is not found.</returns>
        /// <remarks>
        /// Fonts must be embedded as resources in the format: OmniGenerator.Lib.Resources.Fonts.{fontName}
        /// </remarks>
        public static byte[] GetFontBytes(string fontName)
        {
            string fontPath = $"OmniGenerator.Lib.Resources.Fonts.{fontName}";

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

        /// <summary>
        /// Copies style attributes from a source SVG element to a target SVG element.
        /// Includes fill, stroke, opacity, and other visual style properties.
        /// </summary>
        /// <param name="target">The target SVG element to receive the style attributes.</param>
        /// <param name="source">The source SVG element from which to copy style attributes.</param>
        /// <remarks>
        /// This method is useful when embedding generated SVG elements (like barcodes) into a parent document
        /// while preserving their original styling.
        /// </remarks>
        private static void CopyStyleAttributes(this SvgElement target, SvgElement source)
        {
            target.Fill = source.Fill;
            target.Stroke = source.Stroke;
            target.StrokeWidth = source.StrokeWidth;
            target.StrokeLineCap = source.StrokeLineCap;
            target.Opacity = source.Opacity;
            target.FillOpacity = source.FillOpacity;
            target.StrokeOpacity = source.StrokeOpacity;
        }
    }
}
