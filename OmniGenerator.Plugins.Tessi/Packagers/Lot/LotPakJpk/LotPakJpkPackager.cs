using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    [OmniGeneratorPluginMetadata("packager.tessi.lotpakjpk", "A packager that exports documents in the LOT+PAK+JPK fashion")]
    public class LotPakJpkPackager : OmniGeneratorPluginBase, IPackager
    {
        private int _resolution;

        public async Task ProcessAsync(Root root, string basepath, int imageRenderingResolution)
        {
            _resolution = imageRenderingResolution;

            string packagename = $"todoname_{DateTime.Now:yyyyMMddHHmmss}";

            using var lot = new StreamWriter(new FileStream(Path.Combine(basepath, $"{packagename}.lot"), FileMode.Create));
            using var pak = new BinaryWriter(new FileStream(Path.Combine(basepath, $"{packagename}.pak"), FileMode.Create));
            using var jpk = new BinaryWriter(new FileStream(Path.Combine(basepath, $"{packagename}.jpk"), FileMode.Create));

            await WriteHeaderAsync(root, 1, lot);

            int bwOffset = 0, gsOffset = 0;
            foreach (var doc in root.GetDocuments())
            {
                var (newBwOffset, newGsOffset) = await WriteDocumentAsync(doc, lot, pak, jpk, bwOffset, gsOffset);
                bwOffset = newBwOffset;
                gsOffset = newGsOffset;
            }

            return;
        }

        public async Task WriteHeaderAsync(Root root, int packetNumber, StreamWriter lot)
        {
            LotHeaderLine header = new(root, packetNumber);
            await lot.WriteLineAsync(header.ToFixedLengthString());
            return; 
        }

        public async Task<(int newBwOffset, int newGsOffset)> WriteDocumentAsync(
            Document doc,
            StreamWriter lot,
            BinaryWriter pak,
            BinaryWriter jpk,
            int bwOffset,
            int gsOffset)
        {
            OffsetLengthImage bwRecto = new();
            OffsetLengthImage bwVerso = new();
            OffsetLengthImage gsRecto = new();
            OffsetLengthImage gsVerso = new();

            if (doc.RectoVectorImage is not null)
            {
                var renderer = new SvgRenderer(doc.RectoVectorImage, _resolution);
                bwRecto.Set(renderer.ToTiffGroup4(), ref bwOffset);
                gsRecto.Set(renderer.ToJpeg(), ref gsOffset);

                pak.Write(bwRecto.Image);
                jpk.Write(gsRecto.Image);
            }

            if (doc.VersoVectorImage is not null)
            {
                var renderer = new SvgRenderer(doc.VersoVectorImage, _resolution);
                bwVerso.Set(renderer.ToTiffGroup4(), ref bwOffset);
                gsVerso.Set(renderer.ToJpeg(), ref gsOffset);

                pak.Write(bwRecto.Image);
                jpk.Write(gsRecto.Image);
            }

            await lot.WriteLineAsync($"" +
                $"{bwRecto.Length:000000000} {bwRecto.Offset:00000000000} " +
                $"{bwVerso.Length:000000000} {bwVerso.Offset:00000000000} " +
                $"{gsRecto.Length:000000000} {gsRecto.Offset:00000000000} " +
                $"{gsVerso.Length:000000000} {gsVerso.Offset:00000000000} ");

            return (bwOffset, gsOffset);
        }
    }
}
