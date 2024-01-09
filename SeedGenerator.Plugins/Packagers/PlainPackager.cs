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
        public async Task GenerateFilesAsync(PacketData packet, string path)
        {
            var seedname = $"{DateTime.Now:yyyyMMddHHmmss}_{packet.Fields["numlot"].Value}";
            var seedpath = Path.Combine(path, seedname);

            if (!Directory.Exists(seedpath))
                Directory.CreateDirectory(seedpath);

            var txtfile = Path.Combine(seedpath, $"{seedname}.txt");
            File.WriteAllLines(txtfile, GetTxtFileContent(packet));


            foreach (var doc in packet.Documents)
            {
                if (doc.RectoImage is not null && doc.Fields is not null)
                {
                    if (doc.RectoImage is not null && doc.Fields is not null)
                    {
                        using (var bitmap = ImageTools.RenderSvg(doc.RectoImage, 200))
                        {
                            var jpgFilePath = Path.Combine(seedpath, $"{doc.Id:000000}.jpg");
                            bitmap.Save(jpgFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }
                }
            }

            await Task.CompletedTask;
        }
    }
}

