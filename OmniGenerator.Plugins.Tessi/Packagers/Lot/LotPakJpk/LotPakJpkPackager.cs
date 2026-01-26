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

            var rootFields = new RootFields(root.Fields);

            await using var lot = new StreamWriter(new FileStream(Path.Combine(basepath, $"{rootFields.PacketName}.lot"), FileMode.Create));
            await using var pak = new BinaryWriter(new FileStream(Path.Combine(basepath, $"{rootFields.PacketName}.pak"), FileMode.Create));
            await using var jpk = new BinaryWriter(new FileStream(Path.Combine(basepath, $"{rootFields.PacketName}.jpk"), FileMode.Create));

            await WriteHeaderAsync(rootFields, lot);

            int bwOffset = 0, gsOffset = 0;
            int index = 1;
            foreach (var document in root.GetDocuments())
            {
                var (newBwOffset, newGsOffset) = await WriteBodyAsync(index++, document, rootFields, lot, pak, jpk, bwOffset, gsOffset);
                bwOffset = newBwOffset;
                gsOffset = newGsOffset;
            }

            //End of packet
            await WritePacketEndLine(rootFields, lot);

            //End of LOT file
            await WriteStatisticsLine(rootFields, lot);
            await WriteScannerStatisticsLine(lot);
            await WriteNavetteLine(lot);
            
            return;
        }

        public async Task WriteHeaderAsync(RootFields rootFields, StreamWriter lot)
        {
            LotHeaderLine line = new(rootFields);
            await lot.WriteLineAsync(line.ToFixedLengthString());
        }

        public async Task<(int newBwOffset, int newGsOffset)> WriteBodyAsync(
            int index,
            Document document,
            RootFields rootFields,
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
            if (document.RectoVectorImage is not null)
            {
                var renderer = new SvgRenderer(document.RectoVectorImage, _resolution);
                bwRecto.Set(renderer.ToTiffGroup4(), ref bwOffset);
                gsRecto.Set(renderer.ToJpeg(), ref gsOffset);

                pak.Write(bwRecto.Image);
                jpk.Write(gsRecto.Image);
            }

            // Writing verso images
            if (document.VersoVectorImage is not null)
            {
                var renderer = new SvgRenderer(document.VersoVectorImage, _resolution);
                bwVerso.Set(renderer.ToTiffGroup4(), ref bwOffset);
                gsVerso.Set(renderer.ToJpeg(), ref gsOffset);

                pak.Write(bwVerso.Image);
                jpk.Write(gsVerso.Image);
            }

            DocumentFields documentFields = new(document.Fields);
            RemittanceFields remittanceFields = new(document.Parent.Fields);

            // Writing LOT body line
            LotBodyLine line = new(
                index,
                documentFields,
                remittanceFields,
                rootFields,
                bwRecto,
                bwVerso,
                gsRecto,
                gsVerso);
            await lot.WriteLineAsync(line.ToFixedLengthString());

            return (bwOffset, gsOffset);
        }

        public async Task WritePacketEndLine(RootFields rootFields, StreamWriter lot)
        {
            LotPacketEnd line = new(rootFields);
            await lot.WriteLineAsync(line.ToFixedLengthString());
        }

        public async Task WriteStatisticsLine(RootFields rootFields, StreamWriter lot)
        {
            LotStatisticLine line = new(rootFields);
            await lot.WriteLineAsync(line.ToFixedLengthString());
        }

        public async Task WriteScannerStatisticsLine(StreamWriter lot)
        {
            LotScannerStatisticsLine line = new();
            await lot.WriteLineAsync(line.ToFixedLengthString());
        }
        public async Task WriteNavetteLine(StreamWriter lot)
        {
            LotNavetteLine line = new();
            await lot.WriteLineAsync(line.ToFixedLengthString());
        }
    }
}
