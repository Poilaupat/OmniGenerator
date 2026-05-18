using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Renderers;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.LotPakJpk
{
    /// <summary>
    /// A packager that exports documents in the LOT+PAK+JPK format used by Tessi systems.
    /// This format consists of three files:
    /// - LOT: A text file containing document metadata and image references in fixed-length format.
    /// - PAK: A binary file containing black and white TIFF Group 4 compressed images.
    /// - JPK: A binary file containing grayscale JPEG compressed images.
    /// </summary>
    [OmniGeneratorPluginMetadata("packager.tessi.lotpakjpk", "A packager that exports documents in the LOT+PAK+JPK fashion")]
    public class LotPakJpkPackager : OmniGeneratorPluginBase, IPackager
    {
        private int _resolution;

        /// <summary>
        /// Processes the document hierarchy and exports it to LOT, PAK, and JPK files.
        /// </summary>
        /// <param name="root">The root of the document hierarchy containing all documents to export.</param>
        /// <param name="basepath">The base directory path where the output files will be created.</param>
        /// <param name="imageRenderingResolution">The DPI resolution for rendering images (e.g., 300 for 300 DPI).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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
            foreach (var document in root.GetAllDocuments())
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

        /// <summary>
        /// Writes the header line to the LOT file.
        /// The header contains batch-level metadata required by the Tessi system.
        /// </summary>
        /// <param name="rootFields">The root-level fields containing batch metadata.</param>
        /// <param name="lot">The LOT file stream writer.</param>
        /// <returns>A task representing the asynchronous write operation.</returns>
        private async Task WriteHeaderAsync(RootFields rootFields, StreamWriter lot)
        {
            LotHeaderLine line = new(rootFields);
            await lot.WriteLineAsync(line.ToFixedLengthString());
        }

        /// <summary>
        /// Writes a document's data to the LOT, PAK, and JPK files.
        /// Renders the document's recto and verso images, compresses them, and writes metadata to the LOT file.
        /// </summary>
        /// <param name="index">The sequential index of the document within the batch.</param>
        /// <param name="document">The document to process.</param>
        /// <param name="rootFields">The root-level fields containing batch metadata.</param>
        /// <param name="lot">The LOT file stream writer.</param>
        /// <param name="pak">The PAK file binary writer for black and white images.</param>
        /// <param name="jpk">The JPK file binary writer for grayscale images.</param>
        /// <param name="bwOffset">The current byte offset in the PAK file.</param>
        /// <param name="gsOffset">The current byte offset in the JPK file.</param>
        /// <returns>A tuple containing the updated PAK and JPK byte offsets after writing the document.</returns>
        private async Task<(int newBwOffset, int newGsOffset)> WriteBodyAsync(
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

        /// <summary>
        /// Writes the packet end marker line to the LOT file.
        /// This line signals the end of the document batch in the LOT format.
        /// </summary>
        /// <param name="rootFields">The root-level fields containing batch metadata.</param>
        /// <param name="lot">The LOT file stream writer.</param>
        /// <returns>A task representing the asynchronous write operation.</returns>
        private async Task WritePacketEndLine(RootFields rootFields, StreamWriter lot)
        {
            LotPacketEnd line = new(rootFields);
            await lot.WriteLineAsync(line.ToFixedLengthString());
        }

        /// <summary>
        /// Writes the batch statistics line to the LOT file.
        /// This line contains aggregated statistics about the processed documents in the batch.
        /// </summary>
        /// <param name="rootFields">The root-level fields containing batch metadata.</param>
        /// <param name="lot">The LOT file stream writer.</param>
        /// <returns>A task representing the asynchronous write operation.</returns>
        private async Task WriteStatisticsLine(RootFields rootFields, StreamWriter lot)
        {
            LotStatisticLine line = new(rootFields);
            await lot.WriteLineAsync(line.ToFixedLengthString());
        }

        /// <summary>
        /// Writes the scanner statistics line to the LOT file.
        /// This line contains metadata about the scanning process (even for generated documents).
        /// </summary>
        /// <param name="lot">The LOT file stream writer.</param>
        /// <returns>A task representing the asynchronous write operation.</returns>
        private async Task WriteScannerStatisticsLine(StreamWriter lot)
        {
            LotScannerStatisticsLine line = new();
            await lot.WriteLineAsync(line.ToFixedLengthString());
        }

        /// <summary>
        /// Writes the navette (shuttle) line to the LOT file.
        /// This is the final line in the LOT file format, marking the complete end of the file.
        /// </summary>
        /// <param name="lot">The LOT file stream writer.</param>
        /// <returns>A task representing the asynchronous write operation.</returns>
        private async Task WriteNavetteLine(StreamWriter lot)
        {
            LotNavetteLine line = new();
            await lot.WriteLineAsync(line.ToFixedLengthString());
        }
    }
}
