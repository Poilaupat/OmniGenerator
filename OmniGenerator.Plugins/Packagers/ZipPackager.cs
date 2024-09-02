using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Tools;
using OmniGenerator.Plugins.Packagers.Tools;
using System.ComponentModel.Composition;
using System.Drawing.Imaging;
using System.IO.Compression;

namespace OmniGenerator.Plugins.Packagers
{
    /// <summary>
    /// A <see cref="IPackager"/> that writes data and image file in a zip file
    /// The zip name is the concatenation of the current date+time with the root numlot
    /// </summary>
    [Export(typeof(IPackager))]
    [PluginMetadata("packager.omni.zip")]
    public class ZipPackager : IPackager
    {
        public async Task ProcessAsync(Root root, string basepath)
        {
            string packagename = $"{DateTime.Now:yyyyMMddHHmmss}_{root.Fields["numlot"].Value}";

            using (var fs = new FileStream($"{Path.Combine(basepath, string.Concat(packagename, ".zip"))}", FileMode.CreateNew))
            using (var archive = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                var txtfile = archive.CreateEntry($"{packagename}.txt");

                using (var es = txtfile.Open())
                using (var sw = new StreamWriter(es))
                {
                    foreach (var line in PackagerTools.GetDefaultTextFileContent(root))
                    {
                        await sw.WriteLineAsync(line);
                    }
                }
                foreach (var document in root.GetDocuments(true))
                {
                    WriteDocumentImages(document, archive);
                }
            }
        }
        private void WriteDocumentImages(Document document, ZipArchive archive)
        {
            if (document.RectoImage is not null)
                PackagerTools.WriteImage(
                    document.RectoImage,
                    archive,
                    $"{document.Id:000000}R.jpg",
                    200,
                    ImageFormat.Jpeg);

            if (document.VersoImage is not null)
                PackagerTools.WriteImage(
                    document.VersoImage,
                    archive,
                    $"{document.Id:000000}V.jpg",
                    200,
                    ImageFormat.Jpeg);
        }
    }
}
