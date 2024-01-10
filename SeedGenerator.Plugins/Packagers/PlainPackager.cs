using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Tools;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Plugins.Packagers
{
    public class PlainPackager : DebugPackagerBase, IPackager
    {
        public async Task ProcessAsync(Root root, string path)
        {
            var seedname = $"{DateTime.Now:yyyyMMddHHmmss}_{root.Fields["numlot"].Value}";
            var seedpath = Path.Combine(path, seedname);

            if (!Directory.Exists(seedpath))
                Directory.CreateDirectory(seedpath);

            var txtfile = Path.Combine(seedpath, $"{seedname}.txt");
            File.WriteAllLines(txtfile, GetTxtFileContent(root));

            foreach (var document in root.GetDocuments(true))
            {
                WriteDocumentImages(document, seedpath);
            }

            await Task.CompletedTask;
        }

        private void WriteDocumentImages(Document document, string seedpath)
        {
            if (document.RectoImage is not null)
            {
                using (var bitmap = ImageTools.RenderSvg(document.RectoImage, 200))
                {
                    bitmap.Save(
                        Path.Combine(seedpath, $"R{document.Id:000000}.jpg"),
                        System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            if (document.VersoImage is not null)
            {
                using (var bitmap = ImageTools.RenderSvg(document.VersoImage, 200))
                {
                    bitmap.Save(
                        Path.Combine(seedpath, $"V{document.Id:000000}.jpg"),
                        System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }
    }
}

