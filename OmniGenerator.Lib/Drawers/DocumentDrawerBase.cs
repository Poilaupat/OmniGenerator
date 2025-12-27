using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using Svg;
using System.Drawing;

namespace OmniGenerator.Lib.Drawers
{
    /// <summary>
    /// The base class for all <see cref="IDocumentDrawer"/>
    /// </summary>
    public abstract class DocumentDrawerBase : OmniGeneratorPluginBase, IDocumentDrawer
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
        /// Initializes a new instance of the <see cref="DocumentDrawerBase"/> class.
        /// </summary>
        /// <param name="width">The width of the document in millimeters.</param>
        /// <param name="height">The height of the document in millimeters.</param>
        protected DocumentDrawerBase(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Load special fonts needed for the image generation. Special fonts are embedded in application.
        /// The special font available are :
        ///  - CMC7
        ///  - OCRB
        ///
        /// Other system fonts can be used without having to load them
        /// </summary>
        static DocumentDrawerBase() => LoadFonts();

        /// <summary>
        /// Composes the recto
        /// </summary>
        /// <param name="document">The data of the document</param>
        /// <returns>A SVG representation of the recto</returns>
        public virtual SvgDocument DrawRecto(Document document)
        {
            return SvgExtensions.NewBlankSvg(Width, Height);
        }

        /// <summary>
        /// Composes the verso
        /// </summary>
        /// <param name="document">The data of the document</param>
        /// <returns>A SVG representation of the verso</returns>
        public virtual SvgDocument DrawVerso(Document document)
        {
            return SvgExtensions.NewBlankSvg(Width, Height);
        }

        /// <summary>
        /// Loads embedded fonts
        /// </summary>
        private static void LoadFonts()
        {
            SvgFontManager.PrivateFontDataList.Add(SvgExtensions.GetFontBytes("Cmc7.ttf"));
            SvgFontManager.PrivateFontDataList.Add(SvgExtensions.GetFontBytes("OcrbRegular.ttf"));
        }
    }
}
