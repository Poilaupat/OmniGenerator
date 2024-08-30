using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Tools;
using OmniGenerator.Plugins.Packagers.Tools;
using System.ComponentModel.Composition;
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
        public async Task ProcessAsync(Root root, string path)
        {
            string packagename = $"{DateTime.Now:yyyyMMddHHmmss}_{root.Fields["numlot"].Value}";

            using (var fs = new FileStream($"{Path.Combine(path, string.Concat(packagename, ".zip"))}", FileMode.CreateNew))
            using (var archive = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                var txtfile = archive.CreateEntry($"{packagename}.txt");

                using (var es = txtfile.Open())
                using (var sw = new StreamWriter(es))
                {
                    foreach (var line in TextExportFileTools.GetDefaultTextFileContent(root))
                    {
                        await sw.WriteLineAsync(line);
                    }
                }
                foreach (var document in root.GetDocuments(true))
                {
                    WriteImages(document, archive);
                }
            }
        }
        private void WriteImages(Document document, ZipArchive archive)
        {
            if (document.RectoImage is not null)
            {
                using (var bitmap = ImageTools.RenderSvg(document.RectoImage, 200))
                {
                    var entry = archive.CreateEntry($"R{document.Id:000000}.jpg");
                    using (var es = entry.Open())
                    {
                        bitmap.Save(es, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }

                }
            }
            if (document.VersoImage is not null)
            {
                using (var bitmap = ImageTools.RenderSvg(document.VersoImage, 200))
                {
                    var entry = archive.CreateEntry($"V{document.Id:000000}.jpg");
                    using (var es = entry.Open())
                    {
                        bitmap.Save(es, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
            }
        }
    }
}
