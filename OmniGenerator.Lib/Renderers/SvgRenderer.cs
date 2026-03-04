using ImageMagick;
using ImageMagick.Formats;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Renderers
{
    /// <summary>
    /// Provides functionality to render SVG documents to bitmap images and convert them to various formats.
    /// </summary>
    public class SvgRenderer
    {
        private Bitmap _image;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgRenderer"/> class and renders the specified SVG document to a bitmap at the given resolution.
        /// </summary>
        /// <param name="recto">The SVG document to render.</param>
        /// <param name="resolution">The resolution in pixels per inch (DPI) for rendering the image.</param>
        public SvgRenderer(SvgDocument recto, int resolution) =>
            //Renders the vector image to a bitmap at the specified resolution
            _image = RenderSvg(recto, resolution);

        /// <summary>
        /// Converts the rendered SVG image to a TIFF file using Group 4 compression.
        /// </summary>
        /// <returns>A byte array containing the TIFF image data.</returns>
        public byte[] ToTiffGroup4()
        {
            using (var bitmapStream = new MemoryStream())
            {
                _image.Save(bitmapStream, ImageFormat.Bmp);
                bitmapStream.Position = 0;

                using (var image = new MagickImage(bitmapStream))
                {
                    image.Format = MagickFormat.Tiff;
                    image.Settings.Compression = CompressionMethod.Group4;
                    image.ColorType = ColorType.Grayscale; // First converts to grayscale
                    image.NegateGrayscale();
                    image.AdaptiveThreshold(10, 10); // Then Apply adaptive binarization
                    image.Negate();

                    using var outputStream = new MemoryStream();
                    image.Write(outputStream);
                    return outputStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts the rendered SVG image to a JPEG file with the specified quality.
        /// </summary>
        /// <param name="quality">The JPEG quality (0-100). Defaults to 75.</param>
        /// <returns>A byte array containing the JPEG image data.</returns>
        public byte[] ToJpeg(uint quality = 75)
        {
            using (var bitmapStream = new MemoryStream())
            {
                _image.Save(bitmapStream, ImageFormat.Bmp);
                bitmapStream.Position = 0;

                using (var image = new MagickImage(bitmapStream))
                {
                    image.Format = MagickFormat.Jpg;
                    image.Quality = quality;

                    using var outputStream = new MemoryStream();
                    image.Write(outputStream);
                    return outputStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Renders the given SVG image to a <see cref="Bitmap"/> at the specified resolution.
        /// </summary>
        /// <param name="svg">The SVG XML document to render.</param>
        /// <param name="resolution">The resolution in pixels per inch (DPI).</param>
        /// <returns>The rendered <see cref="Bitmap"/>.</returns>
        private Bitmap RenderSvg(SvgDocument svg, int resolution)
        {
            return RenderSvg(svg, resolution, resolution);
        }

        /// <summary>
        /// Renders the given SVG document to a <see cref="Bitmap"/> at the specified horizontal and vertical resolutions.
        /// </summary>
        /// <param name="svg">The SVG XML file to render.</param>
        /// <param name="hztlResolution">The horizontal resolution in pixels per inch (DPI).</param>
        /// <param name="vrtlResolution">The vertical resolution in pixels per inch (DPI).</param>
        /// <returns>The rendered <see cref="Bitmap"/>.</returns>
        private Bitmap RenderSvg(SvgDocument svg, int hztlResolution, int vrtlResolution)
        {
            //Reminders :
            // SVG witdh / heigth are set to physical document measures in mm
            // Resolution is given in pixel per inch
            // One inch is 25.4mm

            int rasterX = (int)(hztlResolution * svg.ViewBox.Width / 25.4f);
            int rasterY = (int)(vrtlResolution * svg.ViewBox.Height / 25.4f);

            var raster = new Bitmap(rasterX, rasterY);
            using (var g = Graphics.FromImage(raster))
            {
                g.Clear(Color.White);
                var svgBitmap = svg.Draw(rasterX, rasterY);
                g.DrawImage(svgBitmap, 0, 0);
            }
            raster.SetResolution(hztlResolution, vrtlResolution);

            return raster;
        }

    }
}
