using OmniGenerator.Lib.Generators;
using Svg;

namespace OmniGenerator.Lib.Interfaces
{
    /// <summary>
    /// Defines the interface for generating recto and verso images for a given <see cref="Document"/>
    /// There must be an implementation of this interface for each document type
    /// The methods should check the type of the document that is passed to them corresponds to the type of document they are designed for to avoid configuration mismatch
    /// </summary>
    public interface IImageComposer
    {
        /// <summary>
        /// Generates recto SVG image from the fields of the document
        /// </summary>
        /// <param name="document">The document</param>
        /// <returns>An SVG représentation of the document recto</returns>
        SvgDocument ComposeImageRecto(Document document);

        /// <summary>
        /// Generates verso SVG image from the fields of the document
        /// </summary>
        /// <param name="document">The document</param>
        /// <returns>An SVG représentation of the document verso</returns>
        SvgDocument ComposeImageVerso(Document document);
    }
}