using Svg;
using System.Diagnostics;

namespace OmniGenerator.Lib.Hierarchy
{
    /// <summary>
    /// Represents a document with its name, fields, and associated images.
    /// </summary>
    [DebuggerDisplay("Document = {Name}")]
    public class Document : Element
    {
        /// <summary>
        /// Gets or sets the image composer to use when generating images for this <see cref="Document"/>.
        /// </summary>
        public string? ImageComposer { get; set; }

        /// <summary>
        /// Gets or sets the SVG representation of the <see cref="Document"/> recto (front side).
        /// </summary>
        public SvgDocument? RectoVectorImage { get; set; }

        /// <summary>
        /// Gets or sets the SVG representation of the <see cref="Document"/> verso (back side).
        /// </summary>
        public SvgDocument? VersoVectorImage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class with the specified name and image composer.
        /// </summary>
        /// <param name="name">The name of the <see cref="Document"/>. It can be seen as a document type.</param>
        /// <param name="imageComposer">The key of the image composer to use when generating images. If null, no image will be generated.</param>
        public Document(string name, string? imageComposer)
            : base("document", name)
        {
            ImageComposer = imageComposer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class with the specified name, image composer, and fields.
        /// </summary>
        /// <param name="name">The name of the <see cref="Document"/>. It can be seen as a document type.</param>
        /// <param name="imageComposer">The key of the image composer to use when generating images. If null, no image will be generated.</param>
        /// <param name="fields">The fields to associate with the document.</param>
        public Document(string name, string? imageComposer, IDictionary<string, Field> fields)
            : base("document", name, fields)
        {
            ImageComposer = imageComposer;
        }
    }
}
