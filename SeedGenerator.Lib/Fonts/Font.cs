using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                fontStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
