using Svg;
using System.Drawing;

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
    }
}
