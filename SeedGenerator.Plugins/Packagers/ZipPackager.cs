using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;
using System.Diagnostics.Metrics;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace SeedGenerator.Plugins.Packagers
{
    public class ZipPackager : IPackager
    {
        public async Task GenerateFilesAsync(PacketData packet, string path)
        {
            string seedname = $"{DateTime.Now:yyyyMMddHHmmss}_{packet.Fields["numlot"].Value}";

            using (var fs = new FileStream($"{Path.Combine(path, string.Concat(seedname, ".zip"))}", FileMode.CreateNew))
            using (var archive = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                var txtfile = archive.CreateEntry($"{seedname}.txt");

                using (var es = txtfile.Open())
                using (var sw = new StreamWriter(es))
                {
                    foreach (var line in GetTxtFileContent(packet))
                    {
                        await sw.WriteLineAsync(line);
                    }
                }

                foreach (var doc in packet.Documents)
                {
                    if (doc.Name == "cheque" && doc.Image is not null && doc.Fields is not null)
                    {
                        string svgxml;
                        using (var ms = new MemoryStream())
                        {
                            doc.Image.Write(ms);
                            svgxml = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
                        }

                        var svgfile = archive.CreateEntry($"{doc.Fields["cmc7"].Value}.svg");
                        using (var es = svgfile.Open())
                        using (var sw = new StreamWriter(es))
                        {
                            await sw.WriteLineAsync(svgxml);
                        }
                    }
                }
            }
        }

        private IEnumerable<string> GetTxtFileContent(PacketData packet)
        {
            yield return $"00 {DateTime.Now:yyyyMMddHHmmss} {packet.Fields["numlot"].Value}";

            foreach (var document in packet.Documents)
            {
                if (document.Fields is not null)
                {
                    yield return document.Name switch
                    {
                        "slip" => $"{document.Fields["encline"].Value} {document.Fields["numremise"].Value}",
                        "coupon" => $"{document.Fields["encline"].Value} {document.Fields["numcoupon"].Value}",
                        "cheque" => $"{document.Fields["encline"].Value} {document.Fields["cmc7"].Value}",
                        _ => throw new Exception("Unexpected document type"),
                    };
                }
            }
        }
    }
}