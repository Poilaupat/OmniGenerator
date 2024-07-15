using OmniGenerator.Lib.Data;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Tools;
using Svg;
using System.Drawing;

namespace OmniGenerator.Lib.Image.Composer
{
    /// <summary>
    /// The base class for all <see cref="IImageComposer"/>
    /// </summary>
    public abstract class ImageComposerBase : IImageComposer
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
        static ImageComposerBase()
        {
            LoadFonts();
        }

        /// <summary>
        /// Composes the recto
        /// </summary>
        /// <param name="document">The data of the document</param>
        /// <returns>A SVG representation of the recto</returns>
        public virtual SvgDocument ComposeImageRecto(Document document)
        {
            return ImageTools.NewBlankSvg(Width, Height);
        }

        /// <summary>
        /// Composes the verso
        /// </summary>
        /// <param name="document">The data of the document</param>
        /// <returns>A SVG representation of the verso</returns>
        public virtual SvgDocument ComposeImageVerso(Document document)
        {
            return ImageTools.NewBlankSvg(Width, Height);
        }

        /// <summary>
        /// Loads embedded fonts
        /// </summary>
        private static void LoadFonts()
        {
            SvgFontManager.PrivateFontDataList.Add(ImageTools.GetFontBytes("Cmc7.ttf"));
            SvgFontManager.PrivateFontDataList.Add(ImageTools.GetFontBytes("OcrbRegular.ttf"));
        }
    }
}
