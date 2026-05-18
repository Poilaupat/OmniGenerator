using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using Svg;
using System.Drawing;

namespace OmniGenerator.Lib.Renderers
{
    /// <summary>
    /// The base class for all <see cref="IDocumentRenderer"/>
    /// </summary>
    public abstract class DocumentRendererBase : OmniGeneratorPluginBase, IDocumentRenderer
    {
        /// <summary>
        /// The recto and verso image Width in millimeter
        /// </summary>
        public int Width { get; init; }

        /// <summary>
        /// The recto and verso image Height in millimeter
        /// </summary>
        public int Height { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentRendererBase"/> class.
        /// </summary>
        /// <param name="width">The width of the document in millimeters.</param>
        /// <param name="height">The height of the document in millimeters.</param>
        protected DocumentRendererBase(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Load special fonts needed for the image generation. Special fonts are embedded in application.
        /// The special font available are :
        ///  - CMC7
        ///  - OCRB
        ///  - Caveat
        ///
        /// Other system fonts can be used without having to load them
        /// </summary>
        static DocumentRendererBase() => LoadFonts();

        /// <summary>
        /// Renders the recto
        /// </summary>
        /// <param name="document">The data of the document</param>
        /// <returns>A SVG representation of the recto</returns>
        public virtual SvgDocument RenderRecto(Document document)
        {
            return SvgExtensions.NewBlankSvg(Width, Height);
        }

        /// <summary>
        /// Renders the verso
        /// </summary>
        /// <param name="document">The data of the document</param>
        /// <returns>A SVG representation of the verso</returns>
        public virtual SvgDocument RenderVerso(Document document)
        {
            return SvgExtensions.NewBlankSvg(Width, Height);
        }

        /// <summary>
        /// Loads embedded fonts
        /// </summary>
        private static void LoadFonts()
        {
            RegisterFont("Cmc7.ttf");
            RegisterFont("OcrbRegular.ttf");
            RegisterFont("CaveatRegular.ttf");
        }

        /// <summary>
        /// Extracts an embedded font to a persistent temp file and registers it via
        /// <see cref="SvgFontManager.PrivateFontPathList"/> so svg.net reloads it from
        /// disk on every render call, avoiding GDI+ invalidation caused by PrivateFontCollection.Dispose().
        /// </summary>
        private static void RegisterFont(string fontFileName)
        {
            byte[] data = SvgExtensions.GetFontBytes(fontFileName);
            if (data.Length == 0)
                return;

            string tempPath = Path.Combine(Path.GetTempPath(), fontFileName);
            File.WriteAllBytes(tempPath, data);
            SvgFontManager.PrivateFontPathList.Add(tempPath);
        }
    }
}
