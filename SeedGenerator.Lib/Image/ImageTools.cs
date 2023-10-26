using SkiaSharp;

namespace SeedGenerator.Lib.Image
{
    public static class ImageTools
    {
        /// <summary>
        /// Creates a new Bitmap object from its physical properties
        /// </summary>
        /// <param name="resolution">The image resolution in pixel per inch</param>
        /// <param name="width">The image physical width in millimeters</param>
        /// <param name="height">The image physical heigth in millimeters</param>
        public static SKSurface CreateSurface(int resolution, int width, int height)
        {
            int pixelHeigth = (int)(height / 2.54 * resolution);
            int pixelWidth = (int)(width / 2.54 * resolution);

            SKImageInfo ii = new SKImageInfo(pixelWidth, pixelHeigth, SKColorType.Gray8);
            return SKSurface.Create(ii);
        }
    }
}
