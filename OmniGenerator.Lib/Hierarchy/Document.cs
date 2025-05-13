using Svg;
using System.Diagnostics;

namespace OmniGenerator.Lib.Hierarchy
{
    /// <summary>
    /// Modelize a document with its name, fields and images
    /// </summary>
    [DebuggerDisplay("Document = {Name}")]
    public class Document : Element
    {
        /// <summary>
        /// The image composer to use when generating images for this <see cref="Document"/>
        /// </summary>
        public string? ImageComposer { get; set; }

        /// <summary>
        /// A SGV representation of the <see cref="Document"/> recto
        /// </summary>
        public SvgDocument? RectoImage { get; set; }

        /// <summary>
        /// A SGV representation of the <see cref="Document"/> verso
        /// </summary>
        public SvgDocument? VersoImage { get; set; }

        /// <summary>
        /// Creates a new Document
        /// </summary>
        /// <param name="name">The name of the <see cref="Document"/>. It can be seen as a document type</param>
        /// <param name="imageComposer">The key of image composer to use when generating image. If null no image will be generated.</param>
        public Document(string name, string? imageComposer)
            : base("document", name)
        {
            ImageComposer = imageComposer;
        }
    }
}
