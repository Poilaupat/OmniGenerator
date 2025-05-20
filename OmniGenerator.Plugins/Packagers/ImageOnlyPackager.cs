using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Plugins.Packagers.Tools;
using System.Drawing.Imaging;

namespace OmniGenerator.Plugins.Packagers
{
    /// <summary>
    /// A <see cref="IPackager"/> that writes only image files in a directory
    /// The directory name is the concatenation of the current date+time with the root numlot
    /// </summary>
    [OmniGeneratorPluginMetadata("packager.omni.imageonly", "A packager that only exports images of documents")]
    public class ImageOnlyPackager : OmniGeneratorPluginBase, IPackager
    {
        public async Task ProcessAsync(Root root, string basepath)
        {
            var packagename = $"{DateTime.Now:yyyyMMddHHmmss}_{root.Fields["numlot"].Value}";
            var packagepath = Path.Combine(basepath, packagename);

            if (!Directory.Exists(packagepath))
                Directory.CreateDirectory(packagepath);

            var documents = root
                .GetDocuments()
                .ToArray();

            for (var i = 0; i < documents.Count(); i++)
            {
                WriteDocumentImages(i, documents[i], packagepath);
            }

            await Task.CompletedTask;
        }

        private void WriteDocumentImages(int i, Document document, string path)
        {
            if (document.RectoImage is not null)
                PackagerTools.WriteImage(
                    document.RectoImage,
                    Path.Combine(path, $"{i:000000}R.jpg"),
                    200,
                    ImageFormat.Jpeg);

            if (document.VersoImage is not null)
                PackagerTools.WriteImage(
                    document.VersoImage,
                    Path.Combine(path, $"{i:000000}V.jpg"),
                    200,
                    ImageFormat.Jpeg);
        }
    }
}
