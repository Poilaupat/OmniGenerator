using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Tools;
using System.ComponentModel.Composition;

namespace OmniGenerator.Lib.Packagers.Default
{
    /// <summary>
    /// A <see cref="IPackager"/> that writes data and image file in a directory
    /// The directory name is the concatenation of the current date+time with the root numlot
    /// </summary>
    [Export(typeof(IPackager))]
    [PluginMetadata("plain-packager")]
    public class PlainPackager : DebugPackagerBase, IPackager
    {
        public async Task ProcessAsync(Root root, string path)
        {
            var packagename = $"{DateTime.Now:yyyyMMddHHmmss}_{root.Fields["numlot"].Value}";
            var packagepath = Path.Combine(path, packagename);

            if (!Directory.Exists(packagepath))
                Directory.CreateDirectory(packagepath);

            var txtfile = Path.Combine(packagepath, $"{packagename}.txt");
            await File.WriteAllLinesAsync(txtfile, GetTxtFileContent(root));

            foreach (var document in root.GetDocuments(true))
            {
                WriteDocumentImages(document, packagepath);
            }
        }

        private void WriteDocumentImages(Document document, string packagepath)
        {
            if (document.RectoImage is not null)
            {
                using (var bitmap = ImageTools.RenderSvg(document.RectoImage, 200))
                {
                    bitmap.Save(
                        Path.Combine(packagepath, $"{document.Id:000000}R.jpg"),
                        System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            if (document.VersoImage is not null)
            {
                using (var bitmap = ImageTools.RenderSvg(document.VersoImage, 200))
                {
                    bitmap.Save(
                        Path.Combine(packagepath, $"{document.Id:000000}V.jpg"),
                        System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }
    }
}

