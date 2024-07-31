using Svg;

namespace OmniGenerator.Lib.Generators
{
    /// <summary>
    /// Modelize a document with its name, fields and images
    /// </summary>
    public class Document : Element
    {
        /// <summary>
        /// The image composer to use when generating images
        /// </summary>
        public string? ImageComposer { get; set; }

        /// <summary>
        /// A SGV representation of the document recto
        /// </summary>
        public SvgDocument? RectoImage { get; set; }

        /// <summary>
        /// A SGV representation of the document verso
        /// </summary>
        public SvgDocument? VersoImage { get; set; }

        /// <summary>
        /// Creates a new Document
        /// </summary>
        /// <param name="id">The id of the document</param>
        /// <param name="name">The name of the document. It can be seen as a document type</param>
        /// <param name="imageComposer">The key of image composer to use when generating image. If null no image will be generated.</param>
        public Document(long id, string name, string? imageComposer)
            :base("document", name, id)
        {
            ImageComposer = imageComposer;
        }
    }
}
