using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Tools;
using OmniGenerator.Plugins.Packagers.Tools;
using System.ComponentModel.Composition;
using System.Drawing.Imaging;

namespace OmniGenerator.Plugins.Packagers
{
    /// <summary>
    /// A <see cref="IPackager"/> that writes data and image file in a directory
    /// The directory name is the concatenation of the current date+time with the root numlot
    /// </summary>
    [Export(typeof(IPackager))]
    [PluginMetadata("packager.omni.plain")]
    public class PlainPackager : IPackager
    {
        public async Task ProcessAsync(Root root, string basepath)
        {
            var packagename = $"{DateTime.Now:yyyyMMddHHmmss}_{root.Fields["numlot"].Value}";
            var packagepath = Path.Combine(basepath, packagename);

            if (!Directory.Exists(packagepath))
                Directory.CreateDirectory(packagepath);

            var txtfile = Path.Combine(packagepath, $"{packagename}.txt");
            await File.WriteAllLinesAsync(txtfile, PackagerTools.GetDefaultTextFileContent(root));

            var documents = root
                .GetDocuments(true)
                .ToArray();

            for (var i = 0; i < documents.Count(); i++)
            {
                WriteDocumentImages(i, documents[i], packagepath);
            }
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

