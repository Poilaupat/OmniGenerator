using System.Reflection;

namespace SeedGenerator.Plugins.Image.Fonts
{
    internal class FontUtility
    {
        public static byte[] GetFontBytes(string fontName)
        {
            string fontPath = $"SeedGenerator.Plugins.Resources.{fontName}";

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
