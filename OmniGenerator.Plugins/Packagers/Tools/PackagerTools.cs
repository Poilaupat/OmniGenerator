using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Tools;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OmniGenerator.Plugins.Packagers.Tools
{
    public static class PackagerTools
    {
        /// <summary>
        /// Renders an image from a SvgDocument and writes it to a file
        /// </summary>
        /// <param name="svg">The SvgDocument</param>
        /// <param name="imagefullpath">The target file path</param>
        /// <param name="resolution">The desired resolution of the image</param>
        /// <param name="format">The image format</param>
        public static void WriteImage(SvgDocument svg, string imagefullpath, int resolution, ImageFormat format)
        {
            using (var bitmap = RenderSvg(svg, resolution))
            {
                bitmap.Save(imagefullpath, format);
            }
        }

        /// <summary>
        /// Renders an image from a SvgDocument and writes it to a zip
        /// </summary>
        /// <param name="svg">The SvgDocument</param>
        /// <param name="archive">The target zip file</param>
        /// <param name="imageName">The image file name</param>
        /// <param name="resolution">The desired resolution of the image</param>
        /// <param name="format">The image format</param>
        public static void WriteImage(SvgDocument svg, ZipArchive archive, string imageName,int resolution, ImageFormat format)
        {
            using (var bitmap = RenderSvg(svg, resolution))
            {
                var entry = archive.CreateEntry(imageName);
                using (var es = entry.Open())
                {
                    bitmap.Save(es, format);
                }
            }
        }

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
            using (var g = Graphics.FromImage(raster))
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
    }
}
