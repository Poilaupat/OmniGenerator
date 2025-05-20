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
        public int Width { get; protected set; }

        /// <summary>
        /// The recto and verso image Height in millimeter
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// Load special fonts needed for the image generation. Special fonts are embedded in application.
        /// The special font available are :
        ///  - CMC7
        ///  - OCRB
        ///  
        /// Other system fonts can be used without having to load them
        /// </summary>
        static DocumentDrawerBase()
        {
            LoadFonts();
        }

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
