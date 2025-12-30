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
            var fields = new LotPakJpkPackagerFields(root.Fields);
            _resolution = imageRenderingResolution;

            string packagename = fields.PacketName;

            await using var lot = new StreamWriter(new FileStream(Path.Combine(basepath, $"{packagename}.lot"), FileMode.Create));
            await using var pak = new BinaryWriter(new FileStream(Path.Combine(basepath, $"{packagename}.pak"), FileMode.Create));
            await using var jpk = new BinaryWriter(new FileStream(Path.Combine(basepath, $"{packagename}.jpk"), FileMode.Create));

            await WriteHeaderAsync(root, fields.PacketNumber, lot);

            int bwOffset = 0, gsOffset = 0;
            int index = 1;
            foreach (var document in root.GetDocuments())
            {
                var (newBwOffset, newGsOffset) = await WriteBodyAsync(index++, document, root, lot, pak, jpk, bwOffset, gsOffset);
                bwOffset = newBwOffset;
                gsOffset = newGsOffset;
            }

            return;
        }

        public async Task WriteHeaderAsync(Root root, string packetNumber, StreamWriter lot)
        {
            LotHeaderLine header = new(root, packetNumber);
            await lot.WriteLineAsync(header.ToFixedLengthString());
            return;
        }

        public async Task<(int newBwOffset, int newGsOffset)> WriteBodyAsync(
            int index,
            Document doc,
            Root root,
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

            // Writing recto images
            if (doc.RectoVectorImage is not null)
            {
                var renderer = new SvgRenderer(doc.RectoVectorImage, _resolution);
                bwRecto.Set(renderer.ToTiffGroup4(), ref bwOffset);
                gsRecto.Set(renderer.ToJpeg(), ref gsOffset);

                pak.Write(bwRecto.Image);
                jpk.Write(gsRecto.Image);
            }

            // Writing verso images
            if (doc.VersoVectorImage is not null)
            {
                var renderer = new SvgRenderer(doc.VersoVectorImage, _resolution);
                bwVerso.Set(renderer.ToTiffGroup4(), ref bwOffset);
                gsVerso.Set(renderer.ToJpeg(), ref gsOffset);

                pak.Write(bwVerso.Image);
                jpk.Write(gsVerso.Image);
            }

            // Writing LOT body line
            LotBodyLine body = new(
                index,
                doc,
                root,
                bwRecto,
                bwVerso,
                gsRecto,
                gsVerso);
            await lot.WriteLineAsync(body.ToFixedLengthString());

            return (bwOffset, gsOffset);
        }
    }
}
