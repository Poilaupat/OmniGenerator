using OmniGenerator.Lib.Hierarchy;
using Svg;

namespace OmniGenerator.Lib.Interfaces
{
    /// <summary>
    /// Defines the interface for generating recto and verso images for a given <see cref="Document"/>
    /// </summary>
    public interface IDocumentRenderer : IOmniGeneratorPlugin
    {
        /// <summary>
        /// Generates recto SVG image from the fields of the document
        /// </summary>
        /// <param name="document">The document</param>
        /// <returns>An SVG représentation of the document recto</returns>
        SvgDocument RenderRecto(Document document);

        /// <summary>
        /// Generates verso SVG image from the fields of the document
        /// </summary>
        /// <param name="document">The document</param>
        /// <returns>An SVG représentation of the document verso</returns>
        SvgDocument RenderVerso(Document document);
    }
}