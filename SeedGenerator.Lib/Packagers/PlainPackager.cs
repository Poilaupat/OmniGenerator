using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Tools;

namespace SeedGenerator.Lib.Packagers
{
    /// <summary>
    /// A <see cref="IPackager"/> that writes data and image file in a directory
    /// The directory name is the concatenation of the current date+time with the root numlot
    /// </summary>
    public class PlainPackager : DebugPackagerBase, IPackager
    {
        public async Task ProcessAsync(Root root, string path)
        {
            var seedname = $"{DateTime.Now:yyyyMMddHHmmss}_{root.Fields["numlot"].Value}";
            var seedpath = Path.Combine(path, seedname);

            if (!Directory.Exists(seedpath))
                Directory.CreateDirectory(seedpath);

            var txtfile = Path.Combine(seedpath, $"{seedname}.txt");
            await File.WriteAllLinesAsync(txtfile, GetTxtFileContent(root));

            foreach (var document in root.GetDocuments(true))
            {
                WriteDocumentImages(document, seedpath);
            }
        }

        private void WriteDocumentImages(Document document, string seedpath)
        {
            if (document.RectoImage is not null)
            {
                using (var bitmap = ImageTools.RenderSvg(document.RectoImage, 200))
                {
                    bitmap.Save(
                        Path.Combine(seedpath, $"{document.Id:000000}R.jpg"),
                        System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            if (document.VersoImage is not null)
            {
                using (var bitmap = ImageTools.RenderSvg(document.VersoImage, 200))
                {
                    bitmap.Save(
                        Path.Combine(seedpath, $"{document.Id:000000}V.jpg"),
                        System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }
    }
}

