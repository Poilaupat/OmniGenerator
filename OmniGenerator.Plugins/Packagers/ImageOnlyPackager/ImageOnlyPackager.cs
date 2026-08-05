using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Renderers;

namespace OmniGenerator.Plugins.Packagers.ImageOnlyPackager
{
    /// <summary>
    /// A <see cref="IPackager"/> implementation that writes only image files for each document in a directory.
    /// The directory name is the concatenation of the current date and time with the root's "numlot" field.
    /// Each document's recto and verso images are exported as JPEG and TIFF (Group 4) files.
    /// </summary>
    [OmniGeneratorPluginMetadata("packager.omni.imageonly", "A packager that only exports images of documents")]
    public class ImageOnlyPackager : OmniGeneratorPluginBase, IPackager
    {
        private readonly IFileSystem _fileSystem;

        public ImageOnlyPackager() : this(new PhysicalFileSystem()) { }

        public ImageOnlyPackager(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        /// <summary>
        /// Processes the specified <see cref="Root"/> hierarchy and generates image files for each document.
        /// Images are saved in a directory named with the current date/time and the root's "numlot" field.
        /// </summary>
        /// <param name="root">The <see cref="Root"/> object representing the top-level document hierarchy to be packaged.</param>
        /// <param name="basepath">The base directory where the generated files should be written.</param>
        /// <param name="imageRenderingResolution">The resolution (in DPI) to use when rendering images.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous packaging operation.</returns>
        public async Task ProcessAsync(Root root, string basepath, int imageRenderingResolution)
        {
            var packagename = $"{DateTime.Now:yyyyMMddHHmmss}";
            var packagepath = Path.Combine(basepath, packagename);

            if (!Directory.Exists(packagepath))
                Directory.CreateDirectory(packagepath);

            var documents = root
                .GetAllDocuments()
                .ToArray();

            for (var i = 0; i < documents.Count(); i++)
            {
                await WriteDocumentImagesAsync(i + 1, documents[i], packagepath, imageRenderingResolution);
            }

            return;
        }

        /// <summary>
        /// Writes the recto and verso images of a document as JPEG and TIFF (Group 4) files to the specified path.
        /// </summary>
        /// <param name="i">The index of the document (used for file naming).</param>
        /// <param name="document">The <see cref="Document"/> whose images are to be written.</param>
        /// <param name="path">The directory path where the images will be saved.</param>
        /// <param name="imageRenderingResolution">The resolution (in DPI) to use when rendering images.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous file writing operation.</returns>
        private async Task WriteDocumentImagesAsync(int i, Document document, string path, int imageRenderingResolution)
        {
            if (document.RectoVectorImage is not null)
            {
                document.RectoVectorImage.Write(Path.Combine(path, $"{i:000000}R.svg"));
                var renderer = new SvgRenderer(document.RectoVectorImage, imageRenderingResolution);
                await _fileSystem.WriteAllBytesAsync(Path.Combine(path, $"{i:000000}R.jpg"), renderer.ToJpeg());
                await _fileSystem.WriteAllBytesAsync(Path.Combine(path, $"{i:000000}R.tiff"), renderer.ToTiffGroup4());
            }

            if (document.VersoVectorImage is not null)
            {
                document.VersoVectorImage.Write(Path.Combine(path, $"{i:000000}V.svg"));
                var renderer = new SvgRenderer(document.VersoVectorImage, imageRenderingResolution);
                await _fileSystem.WriteAllBytesAsync(Path.Combine(path, $"{i:000000}V.jpg"), renderer.ToJpeg());
                await _fileSystem.WriteAllBytesAsync(Path.Combine(path, $"{i:000000}V.tiff"), renderer.ToTiffGroup4());
            }
        }
    }
}
