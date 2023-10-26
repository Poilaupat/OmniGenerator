using SkiaSharp;
using System.Reflection;

namespace SeedGenerator.Lib.Fonts
{
    internal class Font
    {
        public static SKTypeface GetFontFromResource(string fontName)
        {
            string fontPath = $"SeedGenerator.Lib.Resources.Fonts.{fontName}";
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(fontPath))
            {
                var font = SKTypeface.FromStream(resource);
                return font;
            }
        }

        public static byte[] GetFont(string fontName)
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
